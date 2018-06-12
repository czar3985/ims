using IMS.Data;
using IMS.WebMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Services
{
    public class BaseService
    {
        private BaseController _controller;
        public BaseService(BaseController controller)
        {
            _controller = controller;
            __uow = controller.Uow;
        }

        public BaseService(BaseApiController controller)
        {
            __uow = controller.Uow;
        }

        private ApplicationUnit __uow;
        public ApplicationUnit _uow
        {
            get {
                return __uow;
            }
        }
    }
}