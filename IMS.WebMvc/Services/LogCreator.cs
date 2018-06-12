using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IMS.WebMvc.Controllers;

namespace IMS.WebMvc.Services
{
    public class LogCreator : LogCreatorBase
    {
        private BaseController _controller;
        
        public LogCreator(BaseController controller, ActivityLogTypeEnum type)
            :base(type)
        {
            _controller = controller;
            _entity.UserId = controller.User.Identity.GetUserId();
        }

        public void SaveToLog(bool callSaveChanges = true)
        {
            _controller.Uow.ActivityLogs.Add(_entity);
            if(callSaveChanges)
                _controller.Uow.SaveChanges();
        }
    }
}