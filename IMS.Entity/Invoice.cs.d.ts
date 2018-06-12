declare module server {
	interface Invoice {
		id: number;
		issueDate: Date;
		paidDate: Date;
		invoiceNumber: string;
		totalAmountDue: number;
		endorsementNumber: string;
		rebate: number;
		remarks: string;
		oRNumber: string;
		hasEWT: boolean;
		amountPaid: number;
		dueDate: Date;
		policyId: number;
		statusId: number;
		policy: {
			id: number;
			sumInsured: number;
			remarks: string;
			inceptionDate: Date;
			dateIssued: Date;
			expiryDate: Date;
			premium: number;
			rate: number;
			policyNumber: string;
			renewalPolicyNumber: string;
			rebate: number;
			commission: number;
			address: string;
			clientId: number;
			agentId: number;
			insuranceProviderId: number;
			policyTypeId: number;
			statusId: number;
			client: {
				id: number;
				accountNumber: string;
				isOrganization: boolean;
				organizationName: string;
				lastName: string;
				firstName: string;
				address: string;
				contactNumber: string;
				email: string;
				remarks: string;
				policies: any[];
			};
			agent: {
				id: number;
				name: string;
				policies: any[];
			};
			insuranceProvider: {
				id: number;
				name: string;
				policies: any[];
				forms: any[];
			};
			policyType: {
				id: number;
				name: string;
			};
			status: {
				id: number;
				name: string;
			};
			policyAttachments: any[];
			risks: any[];
			endorsements: any[];
			invoices: server.Invoice[];
			claims: any[];
		};
		status: server.InvoiceStatus;
		particulars: server.Particular[];
	}
	interface InvoiceStatus {
		id: number;
		name: string;
	}
	interface Particular {
		id: number;
		particularAmount: number;
		remarks: string;
		invoiceId: number;
		particularTypeId: number;
		invoice: server.Invoice;
		particularType: server.ParticularType;
	}
	interface ParticularType {
		id: number;
		name: string;
	}
}
