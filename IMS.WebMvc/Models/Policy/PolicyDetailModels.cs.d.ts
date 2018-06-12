/// <reference path="../Client/ClientViewModels.cs.d.ts" />

declare module server {
	interface PolicyDetailModel extends PolicyModel {
		companies: any[];
		policyTypes: any[];
		agents: any[];
		clients: server.ClientSimple[];
		defaultRebates: any[];
		statuses: any[];
		policy: server.PolicyModel;
		newAttachment: server.PolicyAttachmentModel;
		companyName: string;
		policyTypeName: string;
		isOrganization: boolean;
		organizationName: string;
		clientName: string;
		agentName: string;
		address: string;
		policyInvoices: server.PolicyInvoiceModel[];
		policyClaims: server.PolicyClaimModel[];
		policyAttachments: server.PolicyAttachmentModel[];
	}
	interface PolicyModel {
		id: number;
		sumInsured: number;
		remarks: string;
		address: string;
		inceptionDate: Date;
		dateIssued: Date;
		expiryDate: Date;
		premium: number;
		rate: number;
		policyNumber: string;
		renewalPolicyNumber: string;
		rebate: number;
		commission: number;
		clientId: number;
		accountNumber: string;
		agentId: number;
		insuranceProviderId: number;
		policyTypeId: number;
		statusId: number;
		statusName: string;
		returnUrl: string;
		endt: server.EndorsementModel;
		risk: server.RiskModel;
		risks: server.RiskModel[];
		endorsements: server.EndorsementModel[];
	}
	interface RiskModel {
		id: number;
		amountInsured: number;
		rate: number;
		premium: number;
		remarks: string;
		policyId: number;
		address: string;
	}
	interface ExistingClientsViewModel {
		id: number;
		isOrganization: boolean;
		organizationName: string;
		accountNumber: string;
		clientName: string;
	}
	interface PolicyInvoiceModel {
		issueDate: Date;
		invoiceNumber: string;
		totalAmountDue: number;
		statusName: string;
		remarks: string;
	}
	interface PolicyClaimModel {
		claimDate: Date;
		claimAmount: number;
		claimNumber: string;
		remarks: string;
		statusName: string;
	}
	interface PolicyAttachmentModel {
		id: number;
		postedDate: Date;
		fileName: string;
		fileUrl: string;
		remarks: string;
		policyId: number;
	}
	interface EndorsementModel {
		id: number;
		isRet: boolean;
		remarks: string;
		mortgagee: string;
		policyId: number;
		issueDate: Date;
		effectiveDate: Date;
		endorsementNumber: string;
		endorsementAmount: number;
		totalEndorsementAmount: number;
		mt: number;
		pt: number;
		dst: number;
		fst: number;
		vat: number;
	}
	interface DefaultRebateModel {
		id: number;
		rate: number;
		companyName: string;
		policyTypeName: string;
		insuranceProviderId: number;
		policyTypeId: number;
	}
	interface DefaultRebateViewModel {
		defaultRebatesList: server.DefaultRebateModel[];
	}
}
