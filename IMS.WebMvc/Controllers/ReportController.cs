using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            var vm = new ReportIndexModel
            {
                Years = ListProviderSvc.GetInvoicesPaidYears(),
                InsuranceProviders = ListProviderSvc.GetInsuranceProviders(),
                PolicyTypes = ListProviderSvc.GetPolicyTypes()
            };

            return View(vm);
        }

        public ActionResult ShowReport(ReportIndexModel vm)
        {
            vm.InsuranceProviders = ListProviderSvc.GetInsuranceProviders();
            vm.PolicyTypes = ListProviderSvc.GetPolicyTypes();
            vm.Months = new string[] { "January", "February", "March", "April",
                    "May", "June", "July", "August", "September", "October", "November", "December"};

            if (vm.IsMonthlyProdType)
            {
                vm = MonthlyProdReport(vm);
            }
            else
            {
                vm = CompTypeYearReport(vm);
            }

            return View(vm);
        }

        private ReportIndexModel CompTypeYearReport(ReportIndexModel vm)
        {
            vm.CompanyName = AttributeProviderSvc.GetCompanyNameFromId(vm.InsuranceProviderId);
            vm.PolicyTypeName = AttributeProviderSvc.GetPolicyTypeNameFromId(vm.PolicyTypeId);
            vm.ReportSums = new ReportSumsModel { };

            var list = new List<ReportGroupedViewModel>();
            var monthQuery = Uow.Invoices.GetAll()
                        .Include(i => i.Particulars.Select(p => p.ParticularType))
                        .Include(i => i.Policy.Client)
                        .Include(i => i.Policy.Agent)
                        .Where(i => i.Policy.InsuranceProviderId == vm.InsuranceProviderId &&
                                i.Policy.PolicyTypeId == vm.PolicyTypeId &&
                                i.Status.Name.ToUpper() == "PAID" &&
                                (i.PaidDate).Year == vm.Year)
                        .ToList()
                        .GroupBy(i => (i.PaidDate).Month);
            foreach (var invoiceGroup in monthQuery)
            {
                var reportList = new List<ReportViewModel>();
                var invoiceList = invoiceGroup.ToList();
                foreach (var invoice in invoiceList)
                {
                    var reportRow = AutoMapper.Mapper.Map<ReportViewModel>(invoice);

                    if (reportRow.IsOrganization)
                        reportRow.ClientName = reportRow.OrganizationName;

                    reportRow.PremiumComponents = new PremiumComponents
                    {
                        Total = invoice.Particulars
                                .Where(p => p.ParticularType.Name == "Premium")
                                .FirstOrDefault()
                                .ParticularAmount
                    };
                    reportRow.TaxComponents = new TaxComponents
                    {
                        Total = invoice.Particulars
                                .Where(p => p.ParticularType.Name != "Premium")
                                .Select(p => p.ParticularAmount)
                                .Sum()
                    };
                    var dstItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Documentary Stamp Tax (DST)")
                            .FirstOrDefault();
                    if (dstItem != null)
                        reportRow.TaxComponents.Dst = dstItem.ParticularAmount;
                    var vatItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Value-Added Tax (VAT)")
                            .FirstOrDefault();
                    if (vatItem != null)
                        reportRow.TaxComponents.Vat = vatItem.ParticularAmount;
                    var lgtItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Local Government Tax (LGT)")
                            .FirstOrDefault();
                    if (lgtItem != null)
                        reportRow.TaxComponents.Lgt = lgtItem.ParticularAmount;
                    var axItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "AX")
                            .FirstOrDefault();
                    if (axItem != null)
                        reportRow.TaxComponents.Ax = axItem.ParticularAmount;
                    var rcharItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "R CHAR")
                            .FirstOrDefault();
                    if (rcharItem != null)
                        reportRow.TaxComponents.Rchar = rcharItem.ParticularAmount;
                    var notarialFeeItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Notarial Fee")
                            .FirstOrDefault();
                    if (notarialFeeItem != null)
                        reportRow.TaxComponents.NotarialFee = notarialFeeItem.ParticularAmount;
                    var otherFeesItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Other Fees")
                            .FirstOrDefault();
                    if (otherFeesItem != null)
                        reportRow.TaxComponents.OtherFees = otherFeesItem.ParticularAmount;
                    var fstItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Fire Service Tax (FST)")
                            .FirstOrDefault();
                    if (fstItem != null)
                        reportRow.TaxComponents.Fst = fstItem.ParticularAmount;
                    var premiumTaxItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Premium Tax")
                            .FirstOrDefault();
                    if (premiumTaxItem != null)
                        reportRow.TaxComponents.PremiumTax = premiumTaxItem.ParticularAmount;
                    var municipalTaxItem = invoice.Particulars
                            .Where(p => p.ParticularType.Name == "Municipal Tax")
                            .FirstOrDefault();
                    if (municipalTaxItem != null)
                        reportRow.TaxComponents.MunicipalTax = municipalTaxItem.ParticularAmount;

                    reportRow.SubAgentComponents = new SubAgentComponents
                    {
                        RebateAmount = invoice.Policy.Premium * (decimal)invoice.Rebate
                    };
                    reportRow.PaymentComponents = new PaymentComponents
                    {
                        AmountPaid = invoice.AmountPaid,
                        PaidDate = invoice.PaidDate,
                        OfficialReceiptNumber = invoice.ORNumber
                    };
                    if (invoice.HasEWT)
                    {
                        reportRow.PaymentComponents.PrepaidTaxEwt = (invoice.AmountPaid * 0.02M);
                        reportRow.PaymentComponents.Difference = reportRow.PaymentComponents.PrepaidTaxEwt;
                    }
                    reportRow.CommissionComponents = new CommissionComponents
                    {
                        TotalCommission = invoice.Policy.Commission
                    };

                    reportList.Add(reportRow);

                    //For bottom-row totals
                    vm.ReportSums.SumPremiumOdExcess += reportRow.PremiumComponents.OdExcess;
                    vm.ReportSums.SumPremiumComplTpl += reportRow.PremiumComponents.ComplTpl;
                    vm.ReportSums.SumPremiumCgl += reportRow.PremiumComponents.Cgl;
                    vm.ReportSums.SumPremiumPersonalAccident += reportRow.PremiumComponents.PersonalAccident;
                    vm.ReportSums.SumPremiumAog += reportRow.PremiumComponents.Aog;
                    vm.ReportSums.SumPremiumContractorsAllRisk += reportRow.PremiumComponents.ContractorsAllRisk;
                    vm.ReportSums.SumPremiumFire += reportRow.PremiumComponents.Fire;
                    vm.ReportSums.SumPremiumAlliedPeril += reportRow.PremiumComponents.AlliedPeril;
                    vm.ReportSums.SumPremiumBurgMoneySec += reportRow.PremiumComponents.BurgMoneySec;
                    vm.ReportSums.SumPremiumPlateGlass += reportRow.PremiumComponents.PlateGlass;
                    vm.ReportSums.SumPremiumRsmd += reportRow.PremiumComponents.Rsmd;
                    vm.ReportSums.SumPremiumTotal += reportRow.PremiumComponents.Total;
                    vm.ReportSums.SumTaxDst += reportRow.TaxComponents.Dst;
                    vm.ReportSums.SumTaxVat += reportRow.TaxComponents.Vat;
                    vm.ReportSums.SumTaxLgt += reportRow.TaxComponents.Lgt;
                    vm.ReportSums.SumTaxAx += reportRow.TaxComponents.Ax;
                    vm.ReportSums.SumTaxRchar += reportRow.TaxComponents.Rchar;
                    vm.ReportSums.SumTaxNotarialFee += reportRow.TaxComponents.NotarialFee;
                    vm.ReportSums.SumTaxOtherFees += reportRow.TaxComponents.OtherFees;
                    vm.ReportSums.SumTaxFst += reportRow.TaxComponents.Fst;
                    vm.ReportSums.SumTaxPremiumTax += reportRow.TaxComponents.PremiumTax;
                    vm.ReportSums.SumTaxMunicipalTax += reportRow.TaxComponents.MunicipalTax;
                    vm.ReportSums.SumTaxTotal += reportRow.TaxComponents.Total;
                    vm.ReportSums.SumGrandTotal += reportRow.GrandTotal;
                    vm.ReportSums.SumAgentRebate += reportRow.SubAgentComponents.RebateAmount;
                }

                list.Add(new ReportGroupedViewModel
                {
                    Month = invoiceGroup.Key,
                    ReportList = reportList.OrderBy(i => i.PaidDate).ToList()
                });
            }
            vm.InvoiceByMonthList = list;

            return vm;
        }

        private ReportIndexModel MonthlyProdReport(ReportIndexModel vm)
        {
            vm.CellsTotalProd = vm.PolicyTypes.Count() - 1;
            vm.CellsTotalAnnualProd = (vm.PolicyTypes.Count() *
                    vm.InsuranceProviders.Count()) + 1;
            vm.TotalAnnualProduction = 0;
            var listByComp = new List<MonthlyProdGroupedViewModel>();
            foreach (var comp in vm.InsuranceProviders)
            {
                var listByType = new List<MonthlyProdByTypeModel>();
                decimal companyTotal = 0;
                foreach (var type in vm.PolicyTypes)
                {
                    var monthQuery = Uow.Invoices.GetAll()
                        .Include(i => i.Particulars.Select(p => p.ParticularType))
                        .Where(i => i.Policy.InsuranceProviderId == comp.Id &&
                                i.Policy.PolicyTypeId == type.Id &&
                                i.Status.Name.ToUpper() == "PAID" &&
                                (i.PaidDate).Year == vm.Year)
                        .ToList()
                        .GroupBy(i => (i.PaidDate).Month);

                    decimal[] monthlyProd = new decimal[12];
                    decimal yearTotal = 0;
                    foreach (var invoiceGroup in monthQuery)
                    {
                        monthlyProd[invoiceGroup.Key - 1] = 0;
                        foreach (var invoice in invoiceGroup)
                        {
                            foreach (var particular in invoice.Particulars)
                            {
                                if (particular.ParticularType.Name.ToLower() == "premium")
                                {
                                    monthlyProd[invoiceGroup.Key - 1] += particular.ParticularAmount;
                                }
                            }
                        }

                        yearTotal += monthlyProd[invoiceGroup.Key - 1];
                    }

                    string[] formattedMonthlyProd = new string[12];
                    for (var month=0; month < 12; month++)
                    {
                        formattedMonthlyProd[month] = monthlyProd[month].ToString("N");
                    }

                    listByType.Add(new MonthlyProdByTypeModel
                    {
                        MonthlyProd = monthlyProd,
                        FormattedMonthlyProd = formattedMonthlyProd,
                        Total = yearTotal,
                        FormattedTotal = yearTotal.ToString("N")
                    });
                    companyTotal += yearTotal;
                }

                listByComp.Add(new MonthlyProdGroupedViewModel
                {
                    MonthlyProdList = listByType,
                    TotalProduction = companyTotal,
                    FormattedTotalProduction = companyTotal.ToString("N")
                });
                vm.TotalAnnualProduction += companyTotal;
            }

            vm.TotalMonthlyAllLines = new TotalMonthlyAllLinesModel[12];
            for (var month = 0; month < 12; month++) {
                vm.TotalMonthlyAllLines[month] = new TotalMonthlyAllLinesModel { };
            }
            for (var index = 0; index < 12; index++)
            {
                foreach (var comp in listByComp)
                {
                    foreach (var type in comp.MonthlyProdList)
                    {
                        vm.TotalMonthlyAllLines[index].Amount += type.MonthlyProd[index];
                    }
                }
            }
            vm.MonthlyProdByCompanyList = listByComp;

            return vm;
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
