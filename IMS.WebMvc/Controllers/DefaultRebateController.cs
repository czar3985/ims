using AutoMapper.QueryableExtensions;
using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System.Linq;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class DefaultRebateController : BaseController
    {
        // GET: DefaultRebate
        public ActionResult Index()
        {
            var defaultRebatesCount = Uow.DefaultRebates.GetAll().Count();

            //// START: ADD ENTRIES  ////
            // Default Rebates added later on in the project and 
            // Companies already exist but without Default Rebate entries
            if (defaultRebatesCount == 0)
            {
                var companies = Uow.InsuranceProviders.GetAll().ToList();
                var policyTypes = Uow.PolicyTypes.GetAll().ToList();
                foreach (var company in companies)
                {
                    foreach (var policyType in policyTypes)
                    {
                        var entity = new DefaultRebate
                        {
                            InsuranceProviderId = company.Id,
                            PolicyTypeId = policyType.Id,
                            Rate = 0
                        };
                        Uow.DefaultRebates.Add(entity);
                    }
                }
                Uow.SaveChanges();    
            }
            //// END: ADD ENTRIES   ////

            var defaultRebates = Uow.DefaultRebates.GetAll()
                .ProjectTo<DefaultRebateModel>()
                .OrderBy(r => r.InsuranceProviderId)
                .ThenBy(r => r.PolicyTypeId)
                .ToList();

            var defaultRebatesVM = new DefaultRebateViewModel
            {
                DefaultRebatesList = defaultRebates
            };
            return View(defaultRebatesVM);
        }

        // GET: DefaultRebate/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = Uow.DefaultRebates.GetAll()
                            .Where(d => d.Id == id)
                            .ProjectTo<DefaultRebateModel>()
                            .FirstOrDefault();

            vm.CompanyName = AttributeProviderSvc.GetCompanyNameFromId(vm.InsuranceProviderId);
            vm.PolicyTypeName = AttributeProviderSvc.GetPolicyTypeNameFromId(vm.PolicyTypeId);

            return View(vm);
        }

        // POST: DefaultRebate/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DefaultRebateModel model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<DefaultRebate>(model);
                Uow.DefaultRebates.Update(entity);
                Uow.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                var vm = Uow.DefaultRebates.GetAll()
                            .Where(d => d.Id == id)
                            .ProjectTo<DefaultRebateModel>()
                            .FirstOrDefault();

                vm.CompanyName = AttributeProviderSvc.GetCompanyNameFromId(vm.InsuranceProviderId);
                vm.PolicyTypeName = AttributeProviderSvc.GetPolicyTypeNameFromId(vm.PolicyTypeId);

                return View(vm);
            }
        }

        #region Logs
        private void LogAdd(DefaultRebate entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.AddDefaultRebate);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Policy entity)
        {
            var creator = new LogCreator(this, ActivityLogTypeEnum.ModifyDefaultRebate);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}