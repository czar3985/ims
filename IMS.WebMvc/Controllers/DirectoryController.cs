using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using IMS.WebMvc.Models;
using IMS.Entities;
using System.Net;

namespace IMS.WebMvc.Controllers
{
    public class DirectoryController : BaseController
    {
        // GET: Directory
        public ActionResult Index()
        {
            var list = Uow.Directories.GetAll()
                .ProjectTo<DirectoryViewModel>()
                .ToList();

            return View(list);
        }

        // GET: Directory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = Uow.Directories.GetAll()
                .Where(d => d.Id == id)
                .ProjectTo<DirectoryViewModel>()
                .FirstOrDefault();

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        // GET: Directory/Create
        public ActionResult Create()
        {
            var model = new DirectoryViewModel();
            return View(model);
        }

        // POST: Directory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DirectoryViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var entity = AutoMapper.Mapper.Map<Directory>(model);
                    Uow.Directories.Add(entity);
                    Uow.Directories.Add(entity);
                    Uow.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Directory/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 
            var model = Uow.Directories.GetAll()
                .Where(d => d.Id == id)
                .ProjectTo<DirectoryViewModel>()
                .FirstOrDefault();

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        // POST: Directory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DirectoryViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var entity = AutoMapper.Mapper.Map<Directory>(model);
                    Uow.Directories.Update(entity);
                    Uow.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Directory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = Uow.Directories.GetAll()
                .Where(d => d.Id == id)
                .ProjectTo<DirectoryViewModel>()
                .FirstOrDefault();

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        // POST: Directory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Uow.Directories.Delete(id);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
