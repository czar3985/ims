using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMS.Entities;
using IMS.WebMvc.Models;
using AutoMapper.QueryableExtensions;
using System.Net;
using Rotativa;
using IMS.WebMvc.Services;
using System.Data.Entity;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class SoaController : BaseController
    {
        // GET: Soa
        public ActionResult Index(string error)
        {
            var unpaidStatusId = AttributeProviderSvc.GetInvoiceStatusIdFromName("unpaid");

            var list = Uow.Soas.GetAll()
                .OrderBy(s => s.StatusId)
                .ProjectTo<SoaModel>(new { invoiceStatus= unpaidStatusId })
                .ToList();
            foreach (var item in list)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }

            List<ExistingClientsViewModel> clients;
            clients = Uow.Clients.GetAll()
                    .Where(c => c.Policies.Count() != 0)
                    .ProjectTo<ExistingClientsViewModel>().ToList();
            foreach (var item in clients)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            List<SoaStatus> statuses;
            statuses = Uow.SoaStatuses.GetAll().ToList();

            var vm = new SoaIndexModel
            {
                SoaList = list,
                HasError = string.IsNullOrEmpty(error) == false,
                Error = error,
                Clients = clients.OrderBy(c => c.ClientName).ToList(),
                Companies = ListProviderSvc.GetInsuranceProviders(),
                Statuses = statuses
            };
            return View(vm);
        }

        // GET: Soa/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vm = Uow.Soas.GetAll()
                .Where(s => s.Id == id)
                .ProjectTo<SoaModel>()
                .FirstOrDefault();
            if (vm == null)
                return HttpNotFound();

            var soaModel = PrepareViewModel(vm);

            return View(soaModel);
        }

        private SoaModel PrepareViewModel (SoaModel vm)
        {
            // EDWIN
            var soaEntity = Uow.Soas.GetAll()
                .Where(s => s.Id == vm.Id)
                .Include(s => s.Status)
                .FirstOrDefault();
            List<SoaTableEntry> entriesAll;
            DateTime startDate = soaEntity.StartDate.AddMinutes(-1);
            // EDWIN END
            if (soaEntity.Status.Name.ToLower() == "paid")
            {
                entriesAll = Uow.Invoices.GetAll()
                    .Where(i => i.Policy.ClientId == vm.ClientId)
                    .Where(i => i.Policy.InsuranceProviderId == vm.InsuranceProviderId)
                    .Where(i => i.Status.Name.ToLower() == "paid")
                    .Where(i => i.IssueDate >= startDate && i.IssueDate < soaEntity.IssueDate)
                    .ProjectTo<SoaTableEntry>()
                    .ToList();
            }
            else
            {
                entriesAll = Uow.Invoices.GetAll()
                    .Where(i => i.Policy.ClientId == vm.ClientId)
                    .Where(i => i.Policy.InsuranceProviderId == vm.InsuranceProviderId)
                    .Where(i => i.Status.Name.ToLower() == "unpaid")
                    .Where(i => i.IssueDate >= startDate && i.IssueDate < soaEntity.IssueDate)
                    .ProjectTo<SoaTableEntry>()
                    .ToList();
            }
            foreach (var entry in entriesAll)
            {
                entry.Ewt = entry.Total * 0.02M;
                entry.AmountDue = entry.Total + entry.Ewt;
            }

            var policyTypeList = Uow.PolicyTypes.GetAll().ToList();
            var soaGroupList = new List<SoaGroupedByType>();
            foreach (var policyType in policyTypeList)
            {
                var entriesPerType = entriesAll
                        .Where(sEntry => sEntry.PolicyTypeId == policyType.Id)
                        .ToList();
                soaGroupList.Add(new SoaGroupedByType
                {
                    PolicyTypeName = policyType.Name,
                    SoaTableEntries = entriesPerType,
                    PremiumSum = entriesPerType.Select(e => e.Premium).Sum(),
                    TaxSum = entriesPerType.Select(e => e.Tax).Sum(),
                    TotalSum = entriesPerType.Select(e => e.Total).Sum(),
                    EwtSum = entriesPerType.Select(e => e.Ewt).Sum(),
                    AmountDueSum = entriesPerType.Select(e => e.AmountDue).Sum()
                });
            }

            vm.SoaGroups = soaGroupList;
            vm.TotalAmountDueWithEwt = entriesAll.Select(a => a.AmountDue).Sum();
            if (vm.IsOrganization)
                vm.ClientName = vm.OrganizationName;

            return vm;
        }

        // GET: Soa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Soa/Create
        [HttpPost]
        public ActionResult Create(SoaModel model)
        {
            try
            {
                var prevSoa = Uow.Soas.GetAll()
                    .Where(s => s.ClientId == model.ClientId &&
                            s.InsuranceProviderId == model.InsuranceProviderId &&
                            s.Status.Name.ToLower() == "unpaid")
                    .FirstOrDefault();
                if (prevSoa != null)
                {
                    prevSoa.StatusId = AttributeProviderSvc.GetSoaStatusIdFromName("closed");
                    Uow.Soas.Update(prevSoa);
                    Uow.SaveChanges();
                    LogEdit(prevSoa);
                }

                // To include time in Issue Date for checking against Invoice list
                DateTime dateTimeNow = DateTime.Now;
                if ((model.IssueDate.Year == dateTimeNow.Year) &&
                    (model.IssueDate.Month == dateTimeNow.Month) &&
                    (model.IssueDate.Day == dateTimeNow.Day))
                {
                    model.IssueDate = dateTimeNow;
                }

                // EDWIN
                var list = Uow.Invoices.GetAll()
                    .Where(i => i.Policy.ClientId == model.ClientId &&
                                i.Policy.InsuranceProviderId == model.InsuranceProviderId &&
                                i.Status.Name.ToLower() == "unpaid" &&
                                i.IssueDate < model.IssueDate)
                    .OrderBy(i => i.IssueDate)
                    .Select(i => new { TotalAmountDue = i.TotalAmountDue, InvoiceIssueDate = i.IssueDate })
                    .ToList();
                model.TotalAmountDue = list.Sum(i => i.TotalAmountDue);
                // EDWIN END

                model.StatusId = AttributeProviderSvc.GetSoaStatusIdFromName("unpaid");
                    
                var entity = AutoMapper.Mapper.Map<Soa>(model);
                // EDWIN
                var firstInvoice = list.FirstOrDefault();
                if (firstInvoice != null)
                    entity.StartDate = firstInvoice.InvoiceIssueDate;
                else
                    entity.StartDate = DateTime.Now;
                // EDWIN END

                Uow.Soas.Add(entity);
                Uow.SaveChanges();

                LogAdd(entity);
                     
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        // GET: Soa/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Soa/Edit/5
        [HttpPost]
        public ActionResult Edit(SoaModel model)
        {
            try
            {
                var invoiceList = Uow.Invoices.GetAll()
                        .Where(i => i.Policy.ClientId == model.ClientId)
                        .Where(i => i.Policy.InsuranceProviderId == model.InsuranceProviderId)
                        .Where(i => i.Status.Name.ToLower() == "unpaid")
                        .Where(i => i.IssueDate < model.IssueDate)
                        .ToList();

                var tmpEntity = Uow.Soas.GetById(model.Id);

                // amount can only be recomputed if unpaid
                model.StatusName = AttributeProviderSvc.GetSoaStatusNameFromId(model.StatusId);

                if (model.StatusName.ToLower() == "unpaid")
                    model.TotalAmountDue = invoiceList.Select(i => i.TotalAmountDue).Sum();
                else
                    model.TotalAmountDue = tmpEntity.TotalAmountDue;
                
                Uow.Soas.Detach(tmpEntity);

                var entity = AutoMapper.Mapper.Map<Soa>(model);

                // EDWIN
                entity.StartDate = tmpEntity.StartDate;
                // EDWIN END

                Uow.Soas.Update(entity);

                LogEdit(entity);

                // if SOA is marked as paid, mark corresponding invoices as paid as well
                if (model.StatusName.ToLower() == "paid")
                {
                    foreach (var invoice in invoiceList)
                    {
                        invoice.StatusId = AttributeProviderSvc.GetInvoiceStatusIdFromName("paid");
                        invoice.PaidDate = DateTime.Now;
                        Uow.Invoices.Update(invoice);
                        LogInvoiceEdit(invoice);
                    }
                }

                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = "Something went wrong!" });
            }
        }

        public ActionResult GeneratePdf(int id, string clientName)
        {
            return new ActionAsPdf("GenerateSoaPdfView", new { id = id })
            {
                FileName = "SOA-" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + clientName + ".pdf"
            };
        }

        [AllowAnonymous]
        public ActionResult GenerateSoaPdfView(int id)
        {
            var vm = Uow.Soas.GetAll()
                .Where(s => s.Id == id)
                .ProjectTo<SoaModel>()
                .FirstOrDefault();
            if (vm == null)
                return HttpNotFound();

            var soaModel = PrepareViewModel(vm);

            return View(soaModel);
        }

        // GET: Soa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Soa/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region Logs
        private void LogAdd(Soa entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.AddSoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Soa entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifySoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogInvoiceEdit(Invoice entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifyInvoice);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Soa entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.DeleteSoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}
