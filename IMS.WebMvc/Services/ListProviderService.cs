using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Controllers;

namespace IMS.WebMvc.Services
{
    public class ListProviderService : BaseService
    {
        public ListProviderService(BaseController controller)
            :base(controller)
        {
        }

        public List<InsuranceProvider> GetInsuranceProviders()
        {
            var list = _uow.InsuranceProviders.GetAll()
                .OrderBy(i => i.Name)
                .ToList();
            return list;
        }

        public List<DocumentType> GetDocumentTypes()
        {
            var list = _uow.DocumentTypes.GetAll().ToList();

            return list;
        }

        public List<ClientSimple> GetClientSimpleList()
        {
            var clients = _uow.Clients.GetAll().ToList();
            return ConvertToClientSimpleList(clients);
        }

        public List<ClientSimple> GetClientsWithPoliciesSimpleList()
        {
            var clients = _uow.Clients.GetAll()
                    .Where(c => c.Policies.Count() != 0)
                    .ToList();
            return ConvertToClientSimpleList(clients);
        }

        private List<ClientSimple> ConvertToClientSimpleList(List<Client> clients)
        {
            var list = new List<ClientSimple>();
            ClientSimple clientSimple = null;

            foreach (var item in clients)
            {
                clientSimple = new ClientSimple
                {
                    Id = item.Id,
                    Email = item.Email,
                    AccountNumber = item.AccountNumber
                    //Rebate = item.Rebate
                };

                if (item.IsOrganization)
                    clientSimple.ClientName = item.OrganizationName;
                else
                    clientSimple.ClientName = item.LastName + ", " + item.FirstName;
                list.Add(clientSimple);
            }

            return list.OrderBy(c => c.ClientName).ToList();
        }

        public List<ParticularType> GetParticularTypes()
        {
            var list = _uow.ParticularTypes.GetAll()
                .OrderBy(p => p.Id)
                .ToList();

            var itemToRemove = list.Find(p => p.Name.ToLower() == "premium");
            list.Remove(itemToRemove);

            itemToRemove = list.Find(p => p.Name.ToLower().Contains("(vat)"));
            list.Remove(itemToRemove);

            return list;
        }

        public List<PolicyType> GetPolicyTypes()
        {
            var list = _uow.PolicyTypes.GetAll()
                .OrderBy(p => p.Name).ToList();

            return list;
        }

        public List<PolicyStatus> GetPolicyStatuses()
        {
            var list = _uow.PolicyStatuses.GetAll()
                .OrderBy(p => p.Name).ToList();

            return list;
        }

        public List<Agent> GetAgents()
        {
            var list = _uow.Agents.GetAll()
                .OrderBy(a => a.Name).ToList();

            return list;
        }

        public List<int> GetInvoicesPaidYears()
        {
            var list = _uow.Invoices.GetAll()
                .Where(i => i.Status.Name.ToUpper() == "PAID")
                .Select(i => (i.PaidDate).Year)
                .Distinct().ToList();

            return list;
        }

        public List<DefaultRebate> GetDefaultRebates()
        {
            var list = _uow.DefaultRebates.GetAll().ToList();

            return list;
        }

        public List<OfferStatus> GetOfferStatuses()
        {
            var list = _uow.OfferStatuses.GetAll().ToList();

            return list;
        }
    }
}