declare module server {
	interface ClientDetailModel extends ClientsListModel {
		clientPoliciesList: server.ClientPoliciesListModel[];
		clientClaimsList: server.ClientClaimsListModel[];
	}
	interface ClientPoliciesListModel {
		id: number;
		insuranceProviderId: number;
		companyName: string;
		policyNumber: string;
		statusName: string;
		expiryDate: Date;
		policyTypeId: number;
		policyTypeName: string;
		latestEndorsementNumber: string;
		sumInsured: number;
		rate: number;
		premium: number;
		rebate: number;
	}
	interface ClientClaimsListModel {
		id: number;
		policyNumber: string;
		policyTypeName: string;
		claimDate: Date;
		claimAmount: number;
		reference: string;
	}
}
