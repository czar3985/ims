declare module server {
	interface ClientViewModel {
		clientsList: server.ClientsListModel[];
	}
	interface ClientsListModel extends ClientModel {
		clientName: string;
		totalBusiness: number;
		totalClaim: number;
		balance: number;
	}
	interface ClientModel {
		id: number;
		accountNumber: string;
		isOrganization: boolean;
		organizationName: string;
		lastName: string;
		firstName: string;
		address: string;
		contactNumber: string;
		email: string;
		returnUrl: string;
		remarks: string;
	}
	interface ClientSimple {
		id: number;
		clientName: string;
		email: string;
		accountNumber: string;
	}
}
