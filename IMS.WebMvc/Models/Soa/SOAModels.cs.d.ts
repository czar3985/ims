/// <reference path="../Policy/PolicyDetailModels.cs.d.ts" />

declare module server {
	interface SoaIndexModel extends SoaModel {
		soaList: server.SoaModel[];
		clients: server.ExistingClientsViewModel[];
		companies: any[];
		hasError: boolean;
		error: string;
		statuses: any[];
	}
	interface SoaModel {
		id: number;
		issueDate: Date;
		dueDate: Date;
		remarks: string;
		totalAmountDue: number;
		clientId: number;
		statusId: number;
		insuranceProviderId: number;
		clientName: string;
		companyName: string;
		totalAmountDueWithEwt: number;
		statusName: string;
		address: string;
		isOrganization: boolean;
		organizationName: string;
		soaGroups: server.SoaGroupedByType[];
	}
	interface SoaGroupedByType {
		policyTypeName: string;
		soaTableEntries: server.SoaTableEntry[];
		premiumSum: number;
		taxSum: number;
		totalSum: number;
		ewtSum: number;
		amountDueSum: number;
	}
	interface SoaTableEntry {
		issueDate: Date;
		policyTypeId: number;
		policyNumber: string;
		premium: number;
		tax: number;
		total: number;
		ewt: number;
		amountDue: number;
	}
}
