using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/SoaApi")]
    public class SoaApiController : BaseApiController
    {
        // GET: api/SoaApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SoaApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SoaApi
        public HttpResponseMessage Post([FromBody]SoaModel model)
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
                    //Uow.SaveChanges();
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

                if(model.TotalAmountDue <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { errorMessage = "Unable to create SOA record; Invoice not yet approved or sent to client" });
                }
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

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT: api/SoaApi/5
        public HttpResponseMessage Patch(SoaModel model)
        {
            try
            {
                var soaEntity = Uow.Soas.GetById(model.Id);

                var invoiceList = Uow.Invoices.GetAll()
                        .Where(i => i.Policy.ClientId == soaEntity.ClientId)
                        .Where(i => i.Policy.InsuranceProviderId == soaEntity.InsuranceProviderId)
                        .Where(i => i.Status.Name.ToLower() == "unpaid")
                        .Where(i => i.IssueDate < soaEntity.IssueDate)
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
                entity.IssueDate = tmpEntity.IssueDate;
                entity.DueDate = tmpEntity.DueDate;
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Something went wrong!" });
            }
        }

        // DELETE: api/SoaApi/5
        public void Delete(int id)
        {
        }

        #region Logs
        private void LogAdd(Soa entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddSoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Soa entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifySoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogInvoiceEdit(Invoice entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyInvoice);
            creator.AddInvoice(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Soa entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteSoa);
            creator.AddSoa(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}
