using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class ReportIndexModel
    {
        public int Year { get; set; }
        public bool IsMonthlyProdType { get; set; }
        public string[] Months { get; set; }
        public List<int> Years { get; set; }

        //For Monthly Production Report
        public int CellsTotalProd { get; set; }
        public int CellsTotalAnnualProd { get; set; }
        public List<MonthlyProdGroupedViewModel> MonthlyProdByCompanyList { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalAnnualProduction { get; set; }

        public TotalMonthlyAllLinesModel[] TotalMonthlyAllLines { get; set; }

        //For Report By Company, Policy Type and Year
        [Display(Name = "Company")]
        public int InsuranceProviderId { get; set; }
        public string CompanyName { get; set; }

        [Display(Name = "Policy Type")]
        public int PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; }

        public List<InsuranceProvider> InsuranceProviders { get; set; }
        public List<PolicyType> PolicyTypes { get; set; }
        public List<ReportGroupedViewModel> InvoiceByMonthList { get; set; }
        public ReportSumsModel ReportSums { get; set; }
    }

    //For Monthly Production Report
    public class MonthlyProdGroupedViewModel
    {
        public List<MonthlyProdByTypeModel> MonthlyProdList { get; set; }
        public decimal? TotalProduction { get; set; }
        public string FormattedTotalProduction { get; set; }
    }

    public class MonthlyProdByTypeModel
    {
        public decimal[] MonthlyProd { get; set; }
        public string[] FormattedMonthlyProd { get; set; }
        public decimal? Total { get; set; }
        public string FormattedTotal { get; set; }
    }

    public class TotalMonthlyAllLinesModel
    {
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
    }

    //For Report By Company, Policy Type and Year
    public class ReportGroupedViewModel
    {
        public int Month { get; set; }
        public List<ReportViewModel> ReportList { get; set; }
    }

    public class ReportSumsModel
    {
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumOdExcess { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumComplTpl { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumCgl { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumPersonalAccident { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumAog { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumContractorsAllRisk { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumFire { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumAlliedPeril { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumBurgMoneySec { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumPlateGlass { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumRsmd { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumPremiumTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxDst { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxVat { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxLgt { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxAx { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxRchar { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxNotarialFee { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxOtherFees { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxFst { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxPremiumTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxMunicipalTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumTaxTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumGrandTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumAgentRebate { get; set; }
    }

    public class ReportViewModel
    {
        [Display(Name = "Date")]
        public DateTime PaidDate { get; set; }

        [Display(Name = "Assured")]
        public string ClientName { get; set; }

        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }

        [Display(Name = "Policy No.")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Agent's Code")]
        public string AgentName { get; set; }

        [Display(Name = "Endt. No.")]
        public string EndorsementNumber { get; set; }

        public string Obligee { get; set; }

        public int ClientId { get; set; }
        public int PolicyId { get; set; }
        public int InvoiceId { get; set; }
        public int AgentId { get; set; }

        public PremiumComponents PremiumComponents { get; set; }
        public TaxComponents TaxComponents { get; set; }

        [Display(Name = "GRAND TOTAL")]
        public decimal GrandTotal { get; set; }

        public SubAgentComponents SubAgentComponents { get; set; }
        public PaymentComponents PaymentComponents { get; set; }
        public CommissionComponents CommissionComponents { get; set; }
    }

    public class PremiumComponents
    {
        [Display(Name = "OD EXCESS")]
        public decimal OdExcess { get; set; }

        [Display(Name = "COMPL TPL")]
        public decimal ComplTpl { get; set; }

        [Display(Name = "CGL")]
        public decimal Cgl { get; set; }

        [Display(Name = "Personal Accident")]
        public decimal PersonalAccident { get; set; }

        [Display(Name = "AOG")]
        public decimal Aog { get; set; }

        [Display(Name = "Contractors All Risk")]
        public decimal ContractorsAllRisk { get; set; }

        public decimal Fire { get; set; }

        [Display(Name = "Allied Peril")]
        public decimal AlliedPeril { get; set; }

        [Display(Name = "Burg./Money Sec.")]
        public decimal BurgMoneySec { get; set; }

        [Display(Name = "Plate Glass")]
        public decimal PlateGlass { get; set; }

        [Display(Name = "RSMD")]
        public decimal Rsmd { get; set; }

        [Display(Name = "TOTAL")]
        public decimal Total { get; set; }
    }

    public class TaxComponents
    {
        [Display(Name = "DST")]
        public decimal Dst { get; set; }

        [Display(Name = "VAT(12%)")]
        public decimal Vat { get; set; }

        [Display(Name = "LGT")]
        public decimal Lgt { get; set; }

        [Display(Name = "AX-(.121%)")]
        public decimal Ax { get; set; }

        [Display(Name = "R CHAR")]
        public decimal Rchar { get; set; }

        [Display(Name = "Notarial Fee")]
        public decimal NotarialFee { get; set; }

        [Display(Name = "Other Fees")]
        public decimal OtherFees { get; set; }

        [Display(Name = "FST(2%)")]
        public decimal Fst { get; set; }

        [Display(Name = "Premium Tax")]
        public decimal PremiumTax { get; set; }

        [Display(Name = "Municipal Tax")]
        public decimal MunicipalTax { get; set; }

        [Display(Name = "TOTAL")]
        public decimal Total { get; set; }
    }


    public class PaymentComponents
    {
        [Display(Name = "Amt Pd.")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Prepaid Tax[2% EWT")]
        public decimal PrepaidTaxEwt { get; set; }

        [Display(Name = "DIF.")]
        public decimal Difference { get; set; }

        [Display(Name = "Date Pd.")]
        public DateTime PaidDate { get; set; }

        [Display(Name = "O.R. No.")]
        public string OfficialReceiptNumber { get; set; }
    }

    public class SubAgentComponents
    {
        [Display(Name = "AGENT")]
        public string AgentName { get; set; }

        [Display(Name = "REBATE")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal RebateAmount { get; set; }

        [Display(Name = "DATE GIVE")]
        public DateTime? GiveDate { get; set; }
    }

    public class CommissionComponents
    {
        [Display(Name = "Personal Accident")]
        public decimal PersonalAccident { get; set; }

        [Display(Name = "Gen. Liab.")]
        public decimal GeneralLiability { get; set; }

        [Display(Name = "Contractors All Risk")]
        public decimal ContractorsAllRisk { get; set; }

        [Display(Name = "Elec. Eqpt. Ins.")]
        public decimal ElectricalEquipmentIns { get; set; }

        [Display(Name = "Boiler & Pressure Ven.")]
        public decimal BoilerAndPressureVen { get; set; }

        [Display(Name = "OD EXCESS")]
        public decimal OdExcess { get; set; }

        [Display(Name = "COMPL TPL")]
        public decimal ComplTpl { get; set; }

        [Display(Name = "AOG")]
        public decimal Aog { get; set; }

        public decimal Fire { get; set; }

        [Display(Name = "Allied Peril")]
        public decimal AlliedPeril { get; set; }

        [Display(Name = "Burg./Money Sec.")]
        public decimal BurgMoneySec { get; set; }

        [Display(Name = "CGL")]
        public decimal Cgl { get; set; }

        [Display(Name = "Plate Glass")]
        public decimal PlateGlass { get; set; }

        [Display(Name = "RSMD")]
        public decimal Rsmd { get; set; }

        public decimal Marine { get; set; }

        [Display(Name = "Total Com.")]
        public decimal TotalCommission { get; set; }

        [Display(Name = "Date Rec'd.")]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "O.R. No.")]
        public string OfficialReceiptNumber { get; set; }
    }
}