/// <reference path="../Client/ClientDetailModels.cs.d.ts" />
/// <reference path="../Client/ClientViewModels.cs.d.ts" />

declare module server {
	const enum InvoiceActionEnum {
		Approve,
		Reject,
		Cancel,
		GenerateAndEmail,
		Paid,
		UpdatePostPaidProperties,
	}
	interface InvoiceGroupedViewModel {
		companyName: string;
		invoiceViewModelList: server.InvoiceItemViewModel[];
	}
	interface InvoiceItemViewModel extends InvoiceModel {
		isOrganization: boolean;
		organizationName: string;
		clientName: string;
		insuranceProviderId: number;
		policyNumber: string;
		sumInsured: number;
		statusName: string;
		accountNumber: string;
		companyName: string;
		premium: number;
		policies: server.ClientPoliciesListModel[];
		particularTypesList: any[];
		clients: server.ClientSimple[];
		isFromPolicy: boolean;
		isReadOnlyState: boolean;
		clientEmail: string;
	}
	interface InvoiceActionViewModel {
		isApprove: boolean;
		isSelected: boolean;
		invoiceId: number;
	}
	interface InvoiceModel {
		id: number;
		issueDate: Date;
		paidDate: Date;
		dueDate: Date;
		invoiceNumber: string;
		totalAmountDue: number;
		remarks: string;
		clientId: number;
		policyId: number;
		statusId: number;
		endorsementNumber: string;
		rebate: number;
		particulars: server.ParticularModel[];
		invoiceAction: server.InvoiceActionEnum;
		returnUrl: string;
		oRNumber: string;
		hasEWT: boolean;
		amountPaid: number;
	}
	interface ExistingPolicyModel {
		id: number;
		policyNumber: string;
	}
	interface ParticularModel {
		id: number;
		particularAmount: number;
		remarks: string;
		invoiceId: number;
		particularTypeId: number;
		particularTypeName: string;
	}
}
