using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IMS.WebMvc.Models;
using AutoMapper.QueryableExtensions;
using IMS.Entities;
using System;
using System.Net;
using IMS.WebMvc.Services;
using System.Web;
using System.IO;
using System.Data.Entity;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class PolicyController : BaseController
    {
        private const string FileLocationFolder = "~/Content/Files/Policies/";
        //private const string FileLocationFolderDbSave = "./Content/Files/Policies/";

        // GET: Policy
        public ActionResult Index()
        {
            var policyViewModelList = Uow.Policies.GetAll()
                .ProjectTo<PolicyItemViewModel>()
                .ToList();
            foreach (var policy in policyViewModelList)
            {
                if (policy.IsOrganization)
                    policy.ClientName = policy.OrganizationName;
            }

            var list = new List<PolicyGroupedViewModel>();
            list.Add(new PolicyGroupedViewModel
            {
                CompanyName = "All",
                PolicyViewModelList = policyViewModelList
            });

            var companyIdNames = Uow.InsuranceProviders.GetAll()
                .Select(c => new { Id = c.Id, Name = c.Name })
                .ToList();

            foreach (var item in companyIdNames)
            {
                list.Add(new PolicyGroupedViewModel
                {
                    CompanyName = item.Name,
                    PolicyViewModelList = policyViewModelList
                        .Where(pvm => pvm.InsuranceProviderId == item.Id)
                        .ToList()
                });
            }

            return View(list);
        }

        // GET: Policy/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var policyDetailModel = Uow.Policies.GetAll()
                .Where(c => c.Id == id)
                .ProjectTo<PolicyDetailModel>()
                .FirstOrDefault();

            if (policyDetailModel == null)
                return HttpNotFound();

            if (policyDetailModel.IsOrganization)
                policyDetailModel.ClientName = policyDetailModel.OrganizationName;

            var claimsList = Uow.Claims.GetAll()
                .Where(c => c.PolicyId == id)
                .ProjectTo<PolicyClaimModel>()
                .ToList();
            var invoicesList = Uow.Invoices.GetAll()
                .Where(i => i.PolicyId == id)
                .ProjectTo<PolicyInvoiceModel>()
                .ToList();
            var attachmentsList = Uow.PolicyAttachments.GetAll()
                .Where(i => i.PolicyId == id)
                .ProjectTo<PolicyAttachmentModel>()
                .ToList();
            var endt = new EndorsementModel
            {
                PolicyId = (int)id,
                IssueDate = DateTime.Now,
                EffectiveDate = DateTime.Now
            };
            var endorsementsList = Uow.Endorsements.GetAll()
                .Where(i => i.PolicyId == id)
                .ProjectTo<EndorsementModel>()
                .ToList();

            policyDetailModel.PolicyClaims = claimsList;
            policyDetailModel.PolicyInvoices = invoicesList;
            policyDetailModel.PolicyAttachments = attachmentsList;
            policyDetailModel.Endt = endt;
            policyDetailModel.Endorsements = endorsementsList;

            return View(policyDetailModel);
        }

        // GET: Policy/Create
        public ActionResult Create(int? ClientId, string returnUrl)
        {
            if (ClientId.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PolicyDetailModel vm = new PolicyDetailModel()
            {
                Companies = ListProviderSvc.GetInsuranceProviders(),
                PolicyTypes = ListProviderSvc.GetPolicyTypes(),
                Statuses = ListProviderSvc.GetPolicyStatuses(),
                Agents = ListProviderSvc.GetAgents(),
                InceptionDate = DateTime.Now,
                DateIssued = DateTime.Now,
                ExpiryDate = DateTime.Now,
                StatusName = "New",
                ReturnUrl = returnUrl,
                ClientId = (int)ClientId
            };

            List<ClientSimple> clients = null;
            if (ClientId == 0)
            {
                clients = ListProviderSvc.GetClientSimpleList();
            }
            else
            {
                clients = new List<ClientSimple>();
                var clientEntity = Uow.Clients.GetById((int)ClientId);
                if (clientEntity == null)
                {
                    return HttpNotFound();
                }
                ClientSimple client = new ClientSimple
                {
                    Id = clientEntity.Id,
                };
                if (clientEntity.IsOrganization)
                    client.ClientName = clientEntity.OrganizationName;
                else
                    client.ClientName = clientEntity.LastName + ", " + clientEntity.FirstName;

                clients.Add(client);
            }
            vm.Clients = clients;

            List<DefaultRebate> defaultRebates = null;
            defaultRebates = ListProviderSvc.GetDefaultRebates();
            vm.DefaultRebates = defaultRebates;

            return View(vm);
        }

        // GET: Policy/Edit/5
        public ActionResult Edit(int? id, string returnUrl)
        {
            if (id.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PolicyDetailModel policyDetailModel = GetPolicyDetailModel(id);
            policyDetailModel.ReturnUrl = returnUrl;

            if (policyDetailModel.StatusName.ToLower() != "active")
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(policyDetailModel);
        }

        private PolicyDetailModel GetPolicyDetailModel(int? id)
        {
            var policyDetailModel = Uow.Policies.GetAll()
                            .Where(c => c.Id == id)
                            .ProjectTo<PolicyDetailModel>()
                            .FirstOrDefault();
            if (policyDetailModel.IsOrganization)
                policyDetailModel.ClientName = policyDetailModel.OrganizationName;

            policyDetailModel.Companies = ListProviderSvc.GetInsuranceProviders();
            policyDetailModel.PolicyTypes = ListProviderSvc.GetPolicyTypes();
            policyDetailModel.Statuses = ListProviderSvc.GetPolicyStatuses();
            policyDetailModel.Agents = ListProviderSvc.GetAgents();
            policyDetailModel.Clients = ListProviderSvc.GetClientSimpleList();

            return policyDetailModel;
        }

        // POST: Policy/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PolicyModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    PolicyDetailModel policyDetailModel = GetPolicyDetailModel(id);
                    return View(policyDetailModel);
                }

                model.StatusId = AttributeProviderSvc.GetPolicyStatusIdFromName(model.StatusName);

                var entity = AutoMapper.Mapper.Map<Policy>(model);
                Uow.Policies.Update(entity);
                Uow.SaveChanges();

                if (string.IsNullOrEmpty(model.ReturnUrl) == false)
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                PolicyDetailModel policyDetailModel = GetPolicyDetailModel(id);
                return View(policyDetailModel);
            }
        }

        // GET: Policy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Policy/Delete/5
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
        public ActionResult CreateCompany(InsuranceProvider model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<InsuranceProvider>(model);
                Uow.InsuranceProviders.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Create", new { ClientId = 0 });
            }
            catch
            {
                return RedirectToAction("Create", new { ClientId = 0 });
            }
        }

        [HttpPost]
        public ActionResult CreatePolicyType(PolicyType model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<PolicyType>(model);
                Uow.PolicyTypes.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Create", new { ClientId = 0 });
            }
            catch
            {
                return RedirectToAction("Create", new { ClientId = 0 });
            }
        }

        [HttpPost]
        public ActionResult CreateAgent(Agent model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<Agent>(model);
                Uow.Agents.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Create", new { ClientId = 0 });
            }
            catch
            {
                return RedirectToAction("Create", new { ClientId = 0 });
            }
        }

        [HttpPost]
        public ActionResult CreateEndorsement(PolicyDetailModel policyDetailModel)
        {
            try
            {
                var model = policyDetailModel.Endt;
                if (model.IsRet)
                    model.EndorsementAmount *= -1;
                var entity = AutoMapper.Mapper.Map<Endorsement>(model);
                Uow.Endorsements.Add(entity);

                UpdatePolicyAndInvoices(model);
                Uow.SaveChanges();

                return RedirectToAction("Details", new { id = model.PolicyId });
            }
            catch (Exception ex)
            {
                var model = policyDetailModel.Endt;
                return RedirectToAction("Details", new { id = model.PolicyId });
            }
        }

        private void UpdatePolicyAndInvoices(EndorsementModel model)
        {
            // Update (1)policy premium and for invoices that are not yet approved,
            // the (2)premium particular and (3)total amount due
            // only if the created or edited endorsement is the latest for the policy  
            var latestEndt = Uow.Endorsements.GetAll()
                .Where(e => e.PolicyId == model.PolicyId)
                .OrderByDescending(e => e.EffectiveDate)
                .FirstOrDefault();
            if (latestEndt != null)
            {
                if (model.EffectiveDate >= latestEndt.EffectiveDate)
                {
                    var policyEntity = Uow.Policies.GetAll()
                        .Where(p => p.Id == model.PolicyId)
                        .FirstOrDefault();
                    policyEntity.Premium += model.TotalEndorsementAmount;
                    Uow.Policies.Update(policyEntity);

                    var invoices = Uow.Invoices.GetAll()
                        .Where(i => i.PolicyId == model.PolicyId)
                        .Include(i => i.Particulars.Select(p => p.ParticularType))
                        .Include(i => i.Status)
                        .ToList();
                    if (invoices != null)
                    {
                        foreach (var item in invoices)
                        {
                            if (item.Status.Name.ToLower() == "pending" || item.Status.Name.ToLower() == "rejected")
                            {
                                if (item.Particulars != null)
                                {
                                    var premiumItem = item.Particulars
                                        .Where(p => p.ParticularType.Name.ToLower() == "premium")
                                        .FirstOrDefault();
                                    if (premiumItem != null)
                                    {
                                        premiumItem.ParticularAmount += model.TotalEndorsementAmount;
                                        item.TotalAmountDue = item.Particulars
                                            .Sum(p => p.ParticularAmount);
                                        Uow.Particulars.Update(premiumItem);
                                        Uow.Invoices.Update(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }       
        }

        [HttpPost]
        public ActionResult EditEndorsement(PolicyDetailModel policyDetailModel, int id)
        {
            try
            {
                var model = policyDetailModel.Endt;
                if (model.IsRet)
                    model.EndorsementAmount *= -1;
                var entity = AutoMapper.Mapper.Map<Endorsement>(model);
                Uow.Endorsements.Update(entity);

                UpdatePolicyAndInvoices(model);
                Uow.SaveChanges();

                return RedirectToAction("Details", new { id = model.PolicyId });
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                var model = policyDetailModel.Endt;
                return RedirectToAction("Details", new { id = model.PolicyId });
            }
        }

        [HttpPost]
        public ActionResult RenewPolicy(PolicyModel model)
        {
            try
            {
                var origEntity = Uow.Policies.GetById(model.Id);
                var entity = AutoMapper.Mapper.Map<Policy>(model);

                entity.Id = 0;
                entity.RenewalPolicyNumber = origEntity.PolicyNumber;
                entity.DateIssued = DateTime.Now;

                entity.StatusId = AttributeProviderSvc.GetPolicyStatusIdFromName("active");
                origEntity.StatusId = AttributeProviderSvc.GetPolicyStatusIdFromName("expired");

                Uow.Policies.Add(entity);
                Uow.Policies.Update(origEntity);
                Uow.SaveChanges();

                return RedirectToAction("Details", new { id = entity.Id });
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                return RedirectToAction("Details", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult CancelPolicy(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = Uow.Policies.GetById((int)id);
                if (entity == null)
                    return HttpNotFound();

                entity.StatusId = AttributeProviderSvc.GetPolicyStatusIdFromName("cancelled");
                Uow.Policies.Update(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpPost]
        public ActionResult CreateAttachment(PolicyDetailModel model, HttpPostedFileBase fileUpload)
        {
            try
            {
                model.NewAttachment.PolicyId = model.Id;
                SaveFile(model.NewAttachment, fileUpload);

                PolicyAttachment entity = AutoMapper.Mapper.Map<PolicyAttachment>(model.NewAttachment);
                entity.PolicyId = model.Id;
                entity.PostedDate = DateTime.Now;
                Uow.PolicyAttachments.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Details", "Policy", new { id = model.Id });
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                return RedirectToAction("Details", "Policy", new { id = model.Id });
            }
        }

        private void SaveFile(PolicyAttachmentModel model, HttpPostedFileBase fileUpload)
        {
            var fileName = Path.GetFileName(fileUpload.FileName);
            var serverFolderAppPath = FileLocationFolder + model.PolicyId.ToString();
            var localSaveFolder = Server.MapPath(serverFolderAppPath);

            if (System.IO.Directory.Exists(localSaveFolder) == false)
                System.IO.Directory.CreateDirectory(localSaveFolder);

            string localSaveFile = localSaveFolder + "/" + fileName; ;

            if (System.IO.File.Exists(localSaveFile))
            {
                int count = 1;
                var fileExtension = Path.GetExtension(fileName);
                var newFileName = Path.GetFileNameWithoutExtension(fileName);
                do
                {
                    fileName = newFileName + "-" + count.ToString() + fileExtension;
                    localSaveFile = localSaveFolder + "/" + fileName; ;
                    count++;
                } while (System.IO.File.Exists(localSaveFile));
            }

            using (FileStream fs = new FileStream(localSaveFile, FileMode.CreateNew))
            {
                fileUpload.InputStream.CopyTo(fs);
            }

            model.FileName = fileName;
            //var serverFolder = FileLocationFolderDbSave + model.PolicyId.ToString();
            model.FileUrl = serverFolderAppPath + "/" + fileName;
        }

        #region Logs
        private void LogAdd(Policy entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.AddPolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Policy entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifyPolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Policy entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.DeletePolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}
