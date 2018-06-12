using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using System.Web.Mvc;
using IMS.Entities;
using Newtonsoft.Json;
using System.Net;
using IMS.WebMvc.Services;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        // GET: Client
        public ActionResult Index()
        {
            var clientViewModel = new ClientViewModel();

            var clientsList = Uow.Clients.GetAll()
                .ProjectTo<ClientsListModel>()
                .ToList();
            foreach (var item in clientsList)
            {
                if (item.TotalBusiness == null)
                    item.TotalBusiness = 0M;
                if (item.TotalClaim == null)
                    item.TotalClaim = 0M;
                if (item.Balance == null)
                    item.Balance = 0M;
                if (item.IsOrganization)
                    item.ClientName = item.OrganizationName;
            }
            clientViewModel.ClientsList = clientsList;

            return View(clientViewModel);
        }

        // GET: Client/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientDetailModel = Uow.Clients.GetAll()
                .Where(c => c.Id == id)
                .ProjectTo<ClientDetailModel>()
                .FirstOrDefault();

            if (clientDetailModel == null)
                return HttpNotFound();

            if (clientDetailModel.IsOrganization)
                clientDetailModel.ClientName = clientDetailModel.OrganizationName;

            var clientPoliciesList = Uow.Policies.GetAll()
                .Where(p => p.ClientId == id)
                .ProjectTo<ClientPoliciesListModel>()
                .ToList();
            clientDetailModel.ClientPoliciesList = clientPoliciesList;

            var clientClaimsList = Uow.Policies.GetAll()
                .Where(p => p.ClientId == id)
                .SelectMany(p => p.Claims)
                .ProjectTo<ClientClaimsListModel>()
                .ToList();
            clientDetailModel.ClientClaimsList = clientClaimsList;

            var clientOffersList = Uow.Offers.GetAll()
                .Where(p => p.ClientId == id)
                .ProjectTo<ClientOffersListModel>()
                .ToList();
            clientDetailModel.ClientOffersList = clientOffersList;

            var offerStatuses = ListProviderSvc.GetOfferStatuses();
            clientDetailModel.OfferStatuses = offerStatuses;

            return View(clientDetailModel);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            return RedirectToAction("Index");
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult Create(ClientModel model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<Client>(model);
                Uow.Clients.Add(entity);
                Uow.SaveChanges();

                LogAdd(entity);

                if (string.IsNullOrEmpty(model.ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(model.ReturnUrl);
            }
            catch(Exception ex)
            {
                LoggingSvc.LogError(ex);
                return View(model);
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(ClientModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var entity = AutoMapper.Mapper.Map<Client>(model);
                Uow.Clients.Update(entity);
                LogEdit(entity);
                Uow.SaveChanges();

                return RedirectToLocal(model.ReturnUrl);
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
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
        private void LogAdd(Client entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.AddClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Client entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifyClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Client entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.DeleteClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}
