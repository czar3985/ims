using IMS.Data;
using IMS.Entities;
using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IMS.WebMvc.Services
{
    public class EmailService
    {
        private const string _emailAccount = "manilaoverseasims";
        public bool Send(
            string[] destinations, 
            string subject, 
            string message, 
            List<Attachment> attachments)
        {
            try
            {
                MailMessage email = new MailMessage();
                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential(_emailAccount, "mocimoci"),
                    EnableSsl = true
                };

                email.From = new MailAddress(_emailAccount + "@gmail.com");
                foreach (var item in destinations)
                {
                    email.To.Add(item);
                }
                
                email.Subject = subject;
                email.Body = message;
                email.IsBodyHtml = true;

                if(attachments != null && attachments.Count > 0)
                {
                    foreach (var item in attachments)
                    {
                        email.Attachments.Add(item);
                    }
                }

                mailClient.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendRenewalNotice(ExpiringPoliciesModel model)
        {
            try
            {
                var subject = "Reminder for Renewal of Policy";
                var message =
                    "Dear " + @model.ClientName + "," +
                    "<br /><br /><br />" +
                    "<strong>Re: REMINDER FOR RENEWAL OF POLICY</strong><br />" +
                    "<p>" +
                    "Policy Number: " + @model.PolicyNumber + "<br />" +
                    "Policy Type: " + @model.PolicyTypeName + "<br />" +
                    "Company: " + @model.CompanyName +
                    "</p>" +
                    "<p>We wish to remind you that your policy expires on " + 
                    @model.ExpiryDate.ToString("MMMM dd") + ".</p>" +
                    "<p>Please let us know whether you would like to renew the policy under the same terms.</p>" +
                    "<p>Thank you.</p>" +
                    "<br />MANILA OVERSEAS COMMERCIAL, INC." +
                    "<p>201 Capt. Roja St., Addition Hills<br />" +
                    "San Juan City<br />" +
                    "Email: moci310@gmail.com<br />" +
                    "Tels. 570-4578; 570-4579 & 570-4580; 463-7871<br />" +
                    "Fax No. 570-5191" +
                    "</p>";

                var destinations = GetEmailAddresses(model.ClientEmail);
                Send(destinations, subject, message, null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendNeedApprovalInvoice(InvoiceModel model, string clientName, string policyNumber, string url)
        {
            var subject = "IMS: Invoice requesting your approval";
            var message = "<strong>A new invoice needs your approval: </strong > " +
                        "<br />< br /> " +
                        "<strong>Insured: </strong>" + clientName +
                        "<br />" + 
                        "<strong>Policy Number: </strong>" + policyNumber +
                        "<br />" +
                        "<strong>Invoice Number: </strong>" + model.InvoiceNumber +
                        "<br />" +
                        "<strong>Total Amount Due: </strong>" + model.TotalAmountDue + 
                        "<br /><br />" +
                        "Click here to approve or reject: <a href=\"" +  url +"\">here</a>";

            var destinations = GetEmailAddresses(DataContext.AdminEmailAdd);
            Send(destinations, subject, message, null);

            return true;
        }

        public bool SendApprovedInvoice(InvoiceItemViewModel model)
        {
            try
            {
                var subject = "Reminder for Payment of Insurance Policy Balance";
                var message =
                   "Dear " + @model.ClientName + "," +
                    "<br /><br /><br />" +
                    "<strong>Re: REMINDER FOR PAYMENT OF INSURANCE POLICY BALANCE</strong><br />" +
                    "<p>We are contacting you with regards to a new invoice that has been created "+
                    "on your account.</p>"+
                    "<p>Please pay the balance of Php " + @model.TotalAmountDue + 
                    " as soon as you receive this notice.</p>" +
                    "<p>The details of the invoice are as follows:<br /><br />" +
                    "<strong>Invoice Issue Date: </strong>" + @model.IssueDate.ToString("MMMM dd, yyyy") + 
                    "<br />" +
                    "<strong>Invoice Number: </strong>" + @model.InvoiceNumber + "<br />" +
                    "<strong>Policy Number: </strong>" + @model.PolicyNumber + "<br />" +
                    "<strong>Company: </strong>" + @model.CompanyName + "</p>" +
                    "<strong>Particulars</strong><br />";
                foreach(var item in model.Particulars)
                {
                    message += (@item.ParticularTypeName + ": Php" + @item.ParticularAmount + "<br />");
                }
                message += "<br /><strong>TOTAL AMOUNT DUE: </strong> Php" + @model.TotalAmountDue + "<br />" +
                    "<p>If you have sent your payment, please ignore this letter.</p>" +
                    "<p>We look forward to conducting future business with you.</p>" +
                    "<p>Thank you.</p>" +
                    "<br />MANILA OVERSEAS COMMERCIAL, INC." +
                    "<p>201 Capt. Roja St., Addition Hills<br />" +
                    "San Juan City<br />" +
                    "Email: moci310@gmail.com<br />" +
                    "Tels. 570-4578; 570-4579 & 570-4580; 463-7871<br />" +
                    "Fax No. 570-5191" +
                    "</p>";

                var destinations = GetEmailAddresses(model.ClientEmail);
                Send(destinations, subject, message, null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendForm(List<string> destinations, string subject, string body, string attachmentServerLocation)
        {
            List<Attachment> list = new List<Attachment>();
            var attachment = new Attachment(attachmentServerLocation);
            list.Add(attachment);

            Send(destinations.ToArray(), subject, body, list);
            return true;
        }

        protected string[] GetEmailAddresses(string commaSeparatedEmailAdds)
        {
            var emailAddsArray = commaSeparatedEmailAdds.Split(',');

            return emailAddsArray;
        }
    }
}