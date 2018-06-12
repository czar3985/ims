/// <reference path="../Policy/PolicyDetailModels.cs.d.ts" />
/// <reference path="../Client/ClientDetailModels.cs.d.ts" />

declare module server {
	interface ClaimGroupedViewModel {
		companyName: string;
		claimViewModelList: server.ClaimItemViewModel[];
	}
	interface ClaimItemViewModel extends ClaimModel {
		policyNumber: string;
		policyTypeName: string;
		insuranceProviderId: number;
		statusName: string;
		clients: server.ExistingClientsViewModel[];
		companyName: string;
		expiryDate: Date;
		expiryDateString: string;
		sumInsured: number;
		statuses: any[];
		policies: server.ClientPoliciesListModel[];
		clientName: string;
		isOrganization: boolean;
		organizationName: string;
	}
	interface ClaimModel {
		id: number;
		claimDate: Date;
		claimAmount: number;
		claimNumber: string;
		remarks: string;
		clientId: number;
		policyId: number;
		statusId: number;
		returnUrl: string;
	}
}
