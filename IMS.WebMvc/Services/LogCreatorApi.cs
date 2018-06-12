using IMS.Entities;
using IMS.WebMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;


namespace IMS.WebMvc.Services
{
    public class LogCreatorApi : LogCreatorBase
    {
        BaseApiController _controller;
        public LogCreatorApi(BaseApiController controller, ActivityLogTypeEnum type)
            :base(type)
        {
            _controller = controller;
            _entity.UserId = controller.User.Identity.GetUserId();
        }

        public void SaveToLog(bool callSaveChanges = true)
        {
            _controller.Uow.ActivityLogs.Add(_entity);
            if (callSaveChanges)
                _controller.Uow.SaveChanges();
        }
    }
}