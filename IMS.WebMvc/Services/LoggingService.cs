using IMS.WebMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Services
{
    public class LoggingService : BaseService
    {
        public LoggingService(BaseController controller)
            : base(controller)
        {
        }

        public void LogError(Exception ex)
        {

        }


    }
}