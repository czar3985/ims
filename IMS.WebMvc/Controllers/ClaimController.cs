using AutoMapper.QueryableExtensions;
using IMS.Entities;
using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class ClaimController : BaseController
    {
        // GET: Claim
        public ActionResult Index()
        {
            var list = new List<ClaimGroupedViewModel>();

            var claimViewModelList = Uow.Claims.GetAll()
                .ProjectTo<ClaimItemViewModel>()
                .ToList();
            foreach (var item in claimViewModelList)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            list.Add(new ClaimGroupedViewModel
            {
                CompanyName = "All",
                ClaimViewModelList = claimViewModelList
            });

            var companyIdNames = Uow.InsuranceProviders.GetAll()
                .Select(c => new { Id = c.Id, Name = c.Name })
                .ToList();

            foreach (var item in companyIdNames)
            {
                list.Add(new ClaimGroupedViewModel
                {
                    CompanyName = item.Name,
                    ClaimViewModelList = claimViewModelList
                        .Where(pvm => pvm.InsuranceProviderId == item.Id)
                        .ToList()
                });
            }

            return View(list);
        }

        // GET: Claim/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vm = Uow.Claims.GetAll()
                            .Where(c => c.Id == id)
                            .ProjectTo<ClaimItemViewModel>()
                            .FirstOrDefault();

            if (vm == null)
                return HttpNotFound();

            if (vm.IsOrganization)
                vm.ClientName = vm.OrganizationName;

            return View(vm);
        }

        // GET: Claim/Create
        public ActionResult Create(string returnUrl)
        {
            List<ExistingClientsViewModel> clients;
            clients = Uow.Clients.GetAll()
                    .Where(c => c.Policies.Count() != 0)
                    .ProjectTo<ExistingClientsViewModel>().ToList();
            foreach (var item in clients)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }

            var vm = new ClaimItemViewModel
            {
                Clients = clients.OrderBy(c => c.ClientName).ToList(),
                StatusName = "New",
                ClaimDate = DateTime.Now,
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        // POST: Claim/Create
        [HttpPost]
        public ActionResult Create(ClaimModel model)
        {
            try
            {
                model.StatusId = AttributeProviderSvc.GetClaimStatusIdFromName("new");

                var entity = AutoMapper.Mapper.Map<Claim>(model);
                Uow.Claims.Add(entity);
                Uow.SaveChanges();

                if (string.IsNullOrEmpty(model.ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(model.ReturnUrl);
            }
            catch
            {
                return View();
            }
        }

        // GET: Claim/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = Uow.Claims.GetAll()
                            .Where(c => c.Id == id)
                            .ProjectTo<ClaimItemViewModel>()
                            .FirstOrDefault();

            List<ExistingClientsViewModel> clients;
            clients = Uow.Clients.GetAll()
                    .Where(c => c.Policies.Count() != 0)
                    .ProjectTo<ExistingClientsViewModel>().ToList();
            foreach (var item in clients)
            {
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            vm.Clients = clients.OrderBy(c => c.ClientName).ToList();

            List<ClaimStatus> statuses;
            statuses = Uow.ClaimStatuses.GetAll().ToList();
            vm.Statuses = statuses;

            var policies = Uow.Policies.GetAll()
                .Where(p => p.ClientId == vm.ClientId)
                .ProjectTo<ClientPoliciesListModel>()
                .ToList();

            vm.Policies = policies;

            return View(vm);
        }

        // POST: Claim/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ClaimModel model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<Claim>(model);
                Uow.Claims.Update(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                var vm = Uow.Claims.GetAll()
                            .Where(c => c.Id == id)
                            .ProjectTo<ClaimItemViewModel>()
                            .FirstOrDefault();

                List<ExistingClientsViewModel> clients;
                clients = Uow.Clients.GetAll()
                        .ProjectTo<ExistingClientsViewModel>().ToList();
                foreach (var item in clients)
                {
                    if (item.IsOrganization)
                        item.ClientName = item.OrganizationName;
                }
                vm.Clients = clients.OrderBy(c => c.ClientName).ToList();

                List<ClaimStatus> statuses;
                statuses = Uow.ClaimStatuses.GetAll().ToList();
                vm.Statuses = statuses;

                return View(vm);
            }
        }

        // GET: Claim/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Claim/Delete/5
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
    }
}
