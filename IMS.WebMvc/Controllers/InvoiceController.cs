using AutoMapper.QueryableExtensions;
using IMS.Entities;
using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Rotativa;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index()
        {
            var invoiceViewModelList = Uow.Invoices.GetAll()
                .ProjectTo<InvoiceItemViewModel>()
                .ToList();
            foreach (var invoice in invoiceViewModelList)
            {
                if (invoice.IsOrganization)
                    invoice.ClientName = invoice.OrganizationName;
            }

            var list = new List<InvoiceGroupedViewModel>();
            list.Add(new InvoiceGroupedViewModel
            {
                CompanyName = "All",
                InvoiceViewModelList = invoiceViewModelList
            });

            var companyIdNames = Uow.InsuranceProviders.GetAll()
                .Select(c => new { Id = c.Id, Name = c.Name })
                .ToList();

            foreach (var item in companyIdNames)
            {
                list.Add(new InvoiceGroupedViewModel
                {
                    CompanyName = item.Name,
                    InvoiceViewModelList = invoiceViewModelList
                        .Where(ivm => ivm.InsuranceProviderId == item.Id)
                        .ToList()
                });
            }

            return View(list);
        }

        public ActionResult CreateEdit(int? id, int? policyId, string returnUrl)
        {
            InvoiceItemViewModel vm;

            if (id != null) //Edit
            {
                vm = Uow.Invoices.GetAll()
                    .Where(i => i.Id == (int)id)
                    .Include(i => i.Particulars)
                    .ProjectTo<InvoiceItemViewModel>()
                    .FirstOrDefault();

                if(vm.DueDate.Year < 1980)
                {
                    vm.DueDate = DateTime.Now;
                }

                if(!(vm.StatusName.ToLower() == "pending" || vm.StatusName.ToLower() == "rejected"))
                {
                    //Invoice has been approved. Premium and Endorsement Number are fixed at this point.
                    var premiumFromParticular = vm.Particulars.Where(p => p.ParticularTypeName.ToLower() == "premium")
                        .FirstOrDefault();
                    if (premiumFromParticular != null)
                        vm.Premium = premiumFromParticular.ParticularAmount;

                    //For Particular Readonly Values
                    vm.IsReadOnlyState = true;
                }
                else
                {
                    //Invoice not yet approved. Endorsement Number can still be modified.
                    var policy = Uow.Policies.GetAll()
                           .Where(p => p.Id == vm.PolicyId)
                           .Include(p => p.Endorsements)
                           .FirstOrDefault();
                    if (policy != null && policy.Endorsements != null && policy.Endorsements.Count > 0)
                    {
                        var latestEndorsementNumber = policy.Endorsements
                            .OrderByDescending(e => e.EffectiveDate)
                            .FirstOrDefault()
                            .EndorsementNumber;
                        vm.EndorsementNumber = latestEndorsementNumber;
                    }

                    //For Particular Readonly Values
                    vm.IsReadOnlyState = false;
                }

                if (vm.StatusName.ToLower() == "unpaid")
                {
                    vm.PaidDate = DateTime.Now;
                }

                var policies = Uow.Policies.GetAll()
                    .Where(p => p.ClientId == vm.ClientId)
                    .ProjectTo<ClientPoliciesListModel>()
                    .ToList();

                vm.Policies = policies;

                vm.Clients = ListProviderSvc.GetClientsWithPoliciesSimpleList();
                vm.IsFromPolicy = false;
            }
            else //Create
            {
                vm = new InvoiceItemViewModel
                {
                    StatusName = "New",
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(1),
                    IsFromPolicy = false
                };

                if(policyId != null)
                {
                    vm.IsFromPolicy = true;
                    vm.Policies = Uow.Policies.GetAll()
                        .Where(p => p.Id == policyId)
                        .ProjectTo<ClientPoliciesListModel>()
                        .ToList();

                    vm.PolicyId = vm.Policies[0].Id;
                    vm.CompanyName = vm.Policies[0].CompanyName;
                    vm.EndorsementNumber = vm.Policies[0].LatestEndorsementNumber;
                    vm.Premium = vm.Policies[0].Premium;
                    vm.Rebate = vm.Policies[0].Rebate;

                    var item = Uow.Policies.GetAll()
                        .Where(p => p.Id == policyId)
                        .Select(p => p.Client)
                        .FirstOrDefault();

                    var client = new ClientSimple
                    {
                        Id = item.Id,
                        Email = item.Email,
                        AccountNumber = item.AccountNumber
                    };

                    if (item.IsOrganization)
                        client.ClientName = item.OrganizationName;
                    else
                        client.ClientName = item.LastName + ", " + item.FirstName;

                    vm.Clients = new List<ClientSimple>();
                    vm.Clients.Add(client);
                    vm.ClientId = client.Id;
                    vm.ClientName = client.ClientName;
                    vm.AccountNumber = client.AccountNumber;
                }
                else
                {
                    vm.Clients = ListProviderSvc.GetClientsWithPoliciesSimpleList();
                }
            }

            vm.ReturnUrl = returnUrl;
            vm.ParticularTypesList = ListProviderSvc.GetParticularTypes();
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                model.StatusId = AttributeProviderSvc.GetInvoiceStatusIdFromName("pending"); ;

                var entity = AutoMapper.Mapper.Map<Invoice>(model);
                Uow.Invoices.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                return View();
            }
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vm = Uow.Invoices.GetAll()
                            .Where(i => i.Id == id)
                            .ProjectTo<InvoiceItemViewModel>()
                            .FirstOrDefault();
            if (vm.IsOrganization)
                vm.ClientName = vm.OrganizationName;

            return View(vm);
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, InvoiceModel model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<Invoice>(model);
                Uow.Invoices.Update(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var vm = Uow.Invoices.GetAll()
                .Where(i => i.Id == (int)id)
                .Include(i => i.Particulars)
                .ProjectTo<InvoiceItemViewModel>()
                .FirstOrDefault();

            if (vm.IsOrganization)
                vm.ClientName = vm.OrganizationName;

            return View(vm);
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoice/Delete/5
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

        [HttpPost]
        public ActionResult InvoiceAction(IList<InvoiceActionViewModel> list)
        {
            int statusId;
            if (list == null || list.Count <= 0)
                return RedirectToAction("Index", "Invoice");

            if (list[0].IsApprove)
            {
                statusId = AttributeProviderSvc.GetInvoiceStatusIdFromName("approved");
            }
            else
            {
                statusId = AttributeProviderSvc.GetInvoiceStatusIdFromName("rejected");
            }


            foreach (var item in list)
            {
                if (item.IsSelected)
                {
                    var entity = Uow.Invoices.GetById(item.InvoiceId);
                    if (entity != null)
                    {
                        entity.StatusId = statusId;
                        Uow.Invoices.Update(entity);
                    }
                }
            }

            Uow.SaveChanges();

            return RedirectToAction("Index", "Invoice");
        }

        public ActionResult SaveInvoice(int id, string clientName)
        {
            return new ActionAsPdf("SaveInvoicePdfView", new { id = id })
            {
                FileName = DateTime.Now.ToString("yyyy-MM-dd") + "-Invoice-" + clientName + ".pdf"
            };
        }

        [AllowAnonymous]
        public ActionResult SaveInvoicePdfView(int id)
        {
            var vm = Uow.Invoices.GetAll()
                 .Where(i => i.Id == (int)id)
                 .Include(i => i.Particulars)
                 .ProjectTo<InvoiceItemViewModel>()
                 .FirstOrDefault();
            if (vm.IsOrganization)
                vm.ClientName = vm.OrganizationName;

            return View(vm);
        }
    }
}
