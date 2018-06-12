using AutoMapper.QueryableExtensions;
using IMS.Data;
using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/InvoiceApi")]
    public class InvoiceApiController : BaseApiController
    {
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = Uow.Invoices.GetAll()
                       .Where(i => i.Id == (int)id)
                       .Include(i => i.Particulars)
                       .ProjectTo<InvoiceItemViewModel>()
                       .FirstOrDefault();
                if (model.IsOrganization)
                    model.ClientName = model.OrganizationName;

                _emailSvc.SendApprovedInvoice(model);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage Post(InvoiceModel model)
        {
            try
            {
                model.StatusId = GetInvoiceStatusId("Pending");
                var particularPremium = model.Particulars.First();
                particularPremium.ParticularTypeId = GetParticularTypeId("Premium");

                if (model.Particulars.Count > 1)
                {
                    var particularVAT = model.Particulars[1];
                    if (particularVAT.ParticularTypeId == 0)
                        particularVAT.ParticularTypeId = GetParticularTypeId("Value-Added Tax (VAT)");
                }

                var entity = AutoMapper.Mapper.Map<Invoice>(model);

                if (entity.Id == 0)
                {
                    entity.PaidDate = new DateTime(1900, 01, 01);
                    if (CreateNewInvoice(entity) == false)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "InvoiceNumber" });
                }
                else
                {
                    if (SaveModifications(entity) == false)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "InvoiceNumber" });
                }


                string tmpApproveUrl = Url.Link("Default", new { controller = "Invoice", action="CreateEdit", id = entity.Id});
                Regex regex = new Regex(@"\d+\.\d+\.\d+\.\d+");
                string approveUrl = regex.Replace(tmpApproveUrl, DataContext.ServerAddress);
                var client = Uow.Clients.GetById(model.ClientId);
                var clientName = client.LastName + ", " + client.FirstName;
                var policyNumber = Uow.Policies.GetById(model.PolicyId).PolicyNumber;

                _emailSvc.SendNeedApprovalInvoice(model, clientName, policyNumber, approveUrl);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private bool CreateNewInvoice(Invoice entity)
        {
            if (CheckIfInvoiceNumberExists(entity))
                return false;

            Uow.Invoices.Add(entity);
            Uow.SaveChanges();

            LogAdd(entity);

            return true;
        }

        private bool CheckIfInvoiceNumberExists(Invoice entity)
        {
            var policy = Uow.Policies.GetById(entity.PolicyId);
            var count = Uow.Invoices.GetAll()
                .Where(i => i.InvoiceNumber == entity.InvoiceNumber &&
                        i.Policy.InsuranceProviderId == policy.InsuranceProviderId)
                .Count();
            Uow.Policies.Detach(policy);
            return count != 0;
        }

        private bool CheckIfDuplicateInvoiceNumber(Invoice entity)
        {
            bool ret = false;
            var policy = Uow.Policies.GetById(entity.PolicyId);
            var invoices = Uow.Invoices.GetAll()
                .Where(i => i.InvoiceNumber == entity.InvoiceNumber &&
                        i.Policy.InsuranceProviderId == policy.InsuranceProviderId)
                .ToList();

            if(invoices.Count() != 0)
            {
                if (invoices[0].Id != entity.Id)
                    ret = true;
            }

            Uow.Policies.Detach(policy);
            foreach (var item in invoices)
            {
                Uow.Invoices.Detach(item);
            }
            return ret;
        }

        private bool SaveModifications(Invoice entity)
        {
            if (CheckIfDuplicateInvoiceNumber(entity))
                return false;

            var pIdList = Uow.Particulars.GetAll()
                .Where(p => p.InvoiceId == entity.Id)
                .Select(p => p.Id)
                .ToList();

            foreach (var item in entity.Particulars)
            {
                if (item.Id == 0)
                {
                    item.InvoiceId = entity.Id;
                    Uow.Particulars.Add(item);
                }
                else
                {
                    Uow.Particulars.Update(item);
                    pIdList.Remove(item.Id);
                }
            }

            foreach (var item in pIdList)
            {
                Uow.Particulars.Delete(item);
            }

            Uow.Invoices.Update(entity);
            Uow.SaveChanges();

            LogEdit(entity);

            return true;
        }

        private int GetInvoiceStatusId(string value)
        {
            var invoiceStatuses = Uow.InvoiceStatuses.GetAll().ToList();
            foreach (var item in invoiceStatuses)
            {
                if (item.Name == value)
                {
                    return item.Id;
                }
            }

            return -1;
        }

        private int GetParticularTypeId(string value)
        {
            int ret = Uow.ParticularTypes.GetAll()
                .Where(p => p.Name == value)
                .Select(p => p.Id)
                .FirstOrDefault();
            return ret;
        }

        [HttpPatch]
        [Route("ChangeState")]
        public HttpResponseMessage ChangeState(InvoiceModel model)
        {
            try
            {
                var entity = Uow.Invoices.GetById(model.Id);

                entity.AmountPaid = model.TotalAmountDue;

                switch (model.InvoiceAction)
                {
                    case InvoiceActionEnum.Approve:
                        entity.StatusId = GetInvoiceStatusId("Approved");
                        break;
                    case InvoiceActionEnum.Reject:
                        entity.StatusId = GetInvoiceStatusId("Rejected");
                        break;
                    case InvoiceActionEnum.Cancel:
                        entity.StatusId = GetInvoiceStatusId("Cancelled");
                        break;
                    case InvoiceActionEnum.GenerateAndEmail:
                        entity.StatusId = GetInvoiceStatusId("Unpaid");
                        break;
                    case InvoiceActionEnum.Paid:
                        entity.StatusId = GetInvoiceStatusId("Paid");
                        entity.PaidDate = model.PaidDate;
                        ReflectPostPaidInvoiceProperties(model, entity);
                        break;
                    case InvoiceActionEnum.UpdatePostPaidProperties:
                        ReflectPostPaidInvoiceProperties(model, entity);
                        break;
                    default:
                        break;
                }

                Uow.Invoices.Update(entity);
                Uow.SaveChanges();

                LogChangeState(entity);

                // Mark SOA as paid if corresponding invoices have been paid
                if (model.InvoiceAction == InvoiceActionEnum.Paid)
                    UpdateSoa(model.Id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private void ReflectPostPaidInvoiceProperties(InvoiceModel model, Invoice entity)
        {
            decimal AmountPaid = 0;
            if (model.HasEWT == true)
                AmountPaid = model.TotalAmountDue - model.TotalAmountDue * 0.02M;
            else
                AmountPaid = model.TotalAmountDue;

            entity.AmountPaid = AmountPaid;
            entity.HasEWT = model.HasEWT;
            entity.ORNumber = model.ORNumber;
        }

        private void UpdateSoa(int id)
        {
            try
            {
                var invoice = Uow.Invoices.GetAll()
                    .Where(i => i.Id == id)
                    .Include(i => i.Policy.Client)
                    .FirstOrDefault();

                //check if soa exists. if yes, check if all corresponding invoices have been paid
                //if yes, change soa to paid
                var soa = Uow.Soas.GetAll()
                    .Where(s => s.ClientId == invoice.Policy.Client.Id)
                    .Where(s => s.InsuranceProviderId == invoice.Policy.InsuranceProviderId)
                    .Where(s => s.Status.Name.ToLower() == "unpaid")
                    .FirstOrDefault();
                if (soa == null)
                    return;

                var invoiceList = Uow.Invoices.GetAll()
                    .Where(i => i.Policy.ClientId == soa.ClientId)
                    .Where(i => i.Policy.InsuranceProviderId == soa.InsuranceProviderId)
                    .Where(i => i.Status.Name.ToLower() == "unpaid")
                    .Where(i => i.IssueDate < soa.IssueDate)
                    .ToList();
                if (invoiceList.Count() == 0)
                {
                    soa.StatusId = GetSoaStatusId("Paid");
                    Uow.Soas.Update(soa);
                    Uow.SaveChanges();
                    LogSoaEdit(soa);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private int GetSoaStatusId(string value)
        {
            int ret = Uow.SoaStatuses.GetAll()
                .Where(s => s.Name == value)
                .Select(s => s.Id)
                .FirstOrDefault();
            return ret;
        }

        private IMS.WebMvc.Services.EmailService __emailSvc;
        public IMS.WebMvc.Services.EmailService _emailSvc
        {
            get
            {
                if (__emailSvc == null)
                    __emailSvc = new IMS.WebMvc.Services.EmailService();
                return __emailSvc;
            }
        }

        #region Logs
        private void LogAdd(Invoice entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddInvoice);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Invoice entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyInvoice);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogChangeState(Invoice entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ChangeStatePolicy);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Invoice entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteInvoice);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog(false);
        }
        
        private void LogSoaEdit(Soa entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifySoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}