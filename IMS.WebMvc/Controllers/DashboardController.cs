using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using IMS.WebMvc.Models;
using Rotativa;
using IMS.Data;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            var dashboardModel = new DashboardModel();

            var expiryDate = DateTime.Now.AddDays(DataContext.ExpiringPolicyDays);
            var expiringPolicies = Uow.Policies.GetAll()
                .Where(p => p.ExpiryDate <= expiryDate && p.Status.Name == "Active")
                .ProjectTo<ExpiringPoliciesModel>()
                .ToList();
            foreach (var item in expiringPolicies)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            dashboardModel.ExpiringPolicies = expiringPolicies;

            var outstandingInvoices = Uow.Invoices.GetAll()
                .Where(i => i.Status.Name.ToLower() == "unpaid")
                .ProjectTo<OutstandingInvoicesModel>()
                .ToList();
            foreach (var item in outstandingInvoices)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            dashboardModel.OutstandingInvoices = outstandingInvoices;

            var soas = Uow.Soas.GetAll()
                .Where(s => s.Status.Name.ToLower() == "unpaid")
                .ProjectTo<SoaModel>()
                .ToList();
            foreach (var item in soas)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            dashboardModel.Soas = soas;

            return View(dashboardModel);
        }

        public ActionResult RenewalNoticeAsPdf(int id, string clientName)
        {
            return new ActionAsPdf("RenewalNoticePdfView", new { id = id })
            {
                FileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + clientName + "-RenewalNotice.pdf"
            };
        }

        [AllowAnonymous]
        public ActionResult RenewalNoticePdfView(int id)
        {
            var model = Uow.Policies.GetAll()
                .Where(p => p.Id == id)
                .ProjectTo<ExpiringPoliciesModel>()
                .FirstOrDefault();
            if (model.IsOrganization)
                model.ClientName = model.OrganizationName;

            return View(model);
        }

        public ActionResult RenewalNoticeSendEmail(int id)
        {
            return View();
        }
    }
}