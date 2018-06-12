using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Directory
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumbers { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Department { get; set; }
        public string Remarks { get; set; }
    }
}
