using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }

        // Navigation Properties
        public virtual IList<Policy> Policies { get; set; }
        public virtual IList<Offer> Offers { get; set; }
    }

    public class Offer
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Remarks { get; set; }
        // Foreign Keys
        public int ClientId { get; set; }
        public int StatusId { get; set; }
        // Navigation Properties
        public virtual Client Client { get; set; }
        public virtual OfferStatus Status { get; set; }
    }

    public class OfferStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
