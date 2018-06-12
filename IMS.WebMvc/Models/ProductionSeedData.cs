using IMS.Data;
using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class ProductionSeedData : CreateDatabaseIfNotExists<DataContext>
    {
        string[] InvoiceStatusList = { "New", "Pending", "Approved", "Rejected", "Generated", "Unpaid", "Paid", "Cancelled" };
        string[] PolicyStatusList = { "New", "Active", "Expired", "Cancelled" };
        string[] ClaimStatusList = { "New", "Pending", "Cancelled", "Rejected", "Released" };
        string[] PolicyTypeList = { "Fire", "Marine", "Accident", "Bonds" };
        string[] ParticularTypeList = {"Premium","Documentary Stamp Tax (DST)","Fire Service Tax (FST)",
                                        "Premium Tax","Municipal Tax", "Local Government Tax (LGT)",
                                        "Value-Added Tax (VAT)", "Notarial Fee", "AX", "R CHAR", "Other Fees"};
        string[] SoaStatusList = { "Unpaid", "Paid", "Closed" };
        string[] DocumentTypeList = { "Endorsement Form", "Accident Form" };
        string[] OfferStatusList = { "New", "Accepted", "Declined" };

        protected override void Seed(DataContext context)
        {
            base.Seed(context);
            AddInvoiceStatuses(context);
            AddPolicyStatuses(context);
            AddClaimStatuses(context);
            AddPolicyTypes(context);
            AddParticularTypes(context);
            AddSoaStatuses(context);
            AddDocumentTypes(context);
            AddOfferStatuses(context);
        }

        private void AddInvoiceStatuses(DataContext context)
        {
            foreach (var item in InvoiceStatusList)
            {
                var invoiceStatus = new InvoiceStatus { Name = item };
                context.InvoiceStatuses.Add(invoiceStatus);
            }
        }

        private void AddPolicyStatuses(DataContext context)
        {
            foreach (var item in PolicyStatusList)
            {
                var policyStatus = new PolicyStatus { Name = item };
                context.PolicyStatuses.Add(policyStatus);
            }
        }

        private void AddClaimStatuses(DataContext context)
        {
            foreach (var item in ClaimStatusList)
            {
                var claimStatus = new ClaimStatus { Name = item };
                context.ClaimStatuses.Add(claimStatus);
            }
        }

        private void AddPolicyTypes(DataContext context)
        {
            foreach (var item in PolicyTypeList)
            {
                var policyType = new PolicyType { Name = item };
                context.PolicyTypes.Add(policyType);
            }
        }

        private void AddParticularTypes(DataContext context)
        {
            foreach (var item in ParticularTypeList)
            {
                var particularType = new ParticularType { Name = item };
                context.ParticularTypes.Add(particularType);
            }
        }

        private void AddSoaStatuses(DataContext context)
        {
            foreach (var item in SoaStatusList)
            {
                var soaStatus = new SoaStatus { Name = item };
                context.SoaStatuses.Add(soaStatus);
            }
        }

        private void AddDocumentTypes(DataContext context)
        {
            foreach (var item in DocumentTypeList)
            {
                var documentType = new DocumentType { Name = item };
                context.DocumentTypes.Add(documentType);
            }
        }

        private void AddOfferStatuses(DataContext context)
        {
            foreach (var item in OfferStatusList)
            {
                var offerStatus = new OfferStatus { Name = item };
                context.OfferStatuses.Add(offerStatus);
            }
        }
    }
}