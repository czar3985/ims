using AutoMapper.QueryableExtensions;
using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        private const string FileLocationFolder = "~/Content/Files/";
        //private const string FileLocationFolderDbSave = "./Content/Files/";
        // GET: Form
        public ActionResult Index()
        {
            var formsList = Uow.Forms.GetAll()
                .ProjectTo<FormModel>()
                .ToList();
            var documentTypeList = ListProviderSvc.GetDocumentTypes();
            var insuranceProviderList = ListProviderSvc.GetInsuranceProviders();
            var clientList = ListProviderSvc.GetClientSimpleList();

            var formViewModel = new FormViewModel
            {
                NewForm = new FormModel(),
                FormsList = formsList,
                DocumentTypes = documentTypeList,
                InsuranceProviders = insuranceProviderList,
                Clients = clientList
            };

            return View(formViewModel);
        }

        // GET: Form/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Form/Create
        [HttpPost]
        public ActionResult Create(FormViewModel model, HttpPostedFileBase fileUpload)
        {
            try
            {

                SaveFile(model.NewForm, fileUpload);

                Form entity = AutoMapper.Mapper.Map<Form>(model.NewForm);
                Uow.Forms.Add(entity);
                Uow.SaveChanges();

                //LogAdd(entity);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LoggingSvc.LogError(ex);
                return RedirectToAction("Index");
            }
        }

        private void SaveFile(FormModel model, HttpPostedFileBase fileUpload)
        {
            var fileName = Path.GetFileName(fileUpload.FileName);
            var serverFolderAppPath = FileLocationFolder + "Company" + model.InsuranceProviderId.ToString();
            
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
            //var serverFolder = FileLocationFolderDbSave + "Company" + model.InsuranceProviderId.ToString();
            model.FileUrl = serverFolderAppPath + "/" + fileName;
        }

        [HttpPost]
        public ActionResult CreateDocumentType(DocumentType model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<DocumentType>(model);
                Uow.DocumentTypes.Add(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Form/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Form/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Form/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Form/Delete/5
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
        private void LogAdd(Form entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.AddForm);
            creator.AddForm(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Form entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifyForm);
            creator.AddForm(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Form entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.DeleteForm);
            creator.AddForm(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}
