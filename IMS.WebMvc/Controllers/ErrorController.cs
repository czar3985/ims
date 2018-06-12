using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    public class ErrorModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ErrorController : BaseController
    {
        public ActionResult Error(string title, string message, string returnUrl)
        {
            var vm = new ErrorModel
            {
                Title = title,
                Message = message,
                ReturnUrl = returnUrl
            };

            return View(vm);
        }
    }
}