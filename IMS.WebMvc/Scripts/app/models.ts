interface ClientViewModel {
    ClientsList: ClientsListModel[];
}

interface ClientsListModel extends ClientModel {
    ClientName?: string;
    TotalBusiness?: number;
    TotalClaim?: number;
    Balance?: number;
}

interface ClientModel {
    Id?: number;
    AccountNumber?: string;
    IsOrganization?: boolean;
    OrganizationName?: string;
    LastName?: string;
    FirstName?: string;
    Address?: string;
    ContactNumber?: string;
    Email?: string;
    Remarks?: string;
     
    ReturnUrl?: string;
}

interface ClientSimple {
    Id?: number;
    ClientName?: string;
    Email?: string;
    AccountNumber?: string;
    //Rebate?: number;
}

interface ClaimItemViewModel extends ClaimModel {
    PolicyNumber?: string;
    PolicyTypeName?: string;
    InsuranceProviderId?: number;
    StatusName?: string;
    Clients?: ExistingClientsViewModel[];
    CompanyName?: string;
    ExpiryDate?: Date;
    ExpiryDateString?: string;
    SumInsured?: number;

    Policies?: ClientPoliciesListModel[];
}

interface ClaimModel {
    Id?: number;
    ClaimDate?: Date;
    ClaimAmount?: number;
    ClaimNumber?: string;
    Remarks?: string;
    ClientId?: any;
    PolicyId?: any;
    StatusId?: any;
}

interface ExistingClientsViewModel {
    Id?: number;
    IsOrganization?: boolean;
    OrganizationName?: string;
    AccountNumber?: string;
}

interface ClientPoliciesListModel {
    Id?: number;
    CompanyName?: string;
    PolicyNumber: string;
    StatusName: string;
    ExpiryDate?: Date;
    PolicyTypeName: string;
    SumInsured: number;
    Rate: number;
    Premium: number;
    LatestEndorsementNumber?: string;
    Rebate?: number;
}

interface InvoiceItemViewModel extends InvoiceModel {
    IsOrganization?: boolean;
    OrganizationName?: string;
    ClientName?: string;
    InsuranceProviderId?: number;
    PolicyNumber?: string;
    SumInsured?: number;
    StatusName?: string;
    AccountNumber?: string;
    CompanyName?: string;
    Premium?: number;
    Rebate?: number;
    Policies?: ClientPoliciesListModel[];
    Particulars?: ParticularModel[];
    ParticularTypesList?: ParticularType[];
    Clients?: ClientSimple[];
}

interface InvoiceModel {
    Id?: number;
    IssueDate?: Date;
    PaidDate?: Date;
    DueDate?: Date;
    InvoiceNumber?: string;
    TotalAmountDue?: number;
    Remarks?: string;
    ClientId?: any;
    PolicyId?: any;
    StatusId?: number;
    EndorsementNumber?: string;
    Rebate?: number;
    ReturnUrl?: string;
    ORNumber?: string;
    HasEWT?: boolean;
    AmountPaid?: number;
    InvoiceAction?: number;
}

interface ExistingPolicyModel {
    Id?: number;
    PolicyNumber?: string;
}
interface ParticularModel {
    Id?: number;
    ParticularAmount?: number;
    Remarks?: string;
    InvoiceId?: number;
    ParticularTypeId?: any;
    ParticularTypeName?: string;
}

interface ParticularType {
    Id?: number;
    Name?: string;
}


// Policies 

interface PolicyType {
    Id?: number;
    Name?: number;
}

interface PolicyDetailModel extends PolicyModel {
    Companies?: any[];
    PolicyTypes?: any[];
    Agents?: Agent[];
    Clients?: ClientSimple[];
    DefaultRebates?: DefaultRebate[];
    Statuses?: any[];
    Policy?: PolicyModel;
    CompanyName?: string;
    PolicyTypeName?: string;
    IsOrganization?: boolean;
    OrganizationName?: string;
    ClientName?: string;
    AgentName?: string;
}
interface PolicyModel {
    Id?: number;
    SumInsured?: number;
    Remarks?: string;
    InceptionDate?: Date;
    DateIssued?: Date;
    ExpiryDate?: Date;
    Premium?: number;
    Rate?: number;
    PolicyNumber?: string;
    RenewalPolicyNumber?: string;
    Rebate?: number;
    Address?: string;
    Commission?: number;
    ClientId?: any;
    AgentId?: any;
    InsuranceProviderId?: any;
    PolicyTypeId?: any;
    StatusId?: any;
    StatusName?: string;
    ReturnUrl?: string;
    Endt?: EndorsementModel;
    Risks?: RiskModel[];
    Endorsements?: EndorsementModel[];
}
interface RiskModel {
    Id?: number;
    AmountInsured?: number;
    Rate?: number;
    Remarks?: string;
    Premium?: number;
    PolicyId?: number;
    Address?: string;
}

interface EndorsementModel {
    Id?: number;
    IssueDate?: Date;
    EffectiveDate?: Date;
    EndorsementNumber?: string;
    EndorsementAmount?: number;
    IsRet?: boolean;
    TotalEndorsementAmount?: number;
    Remarks?: string;
    Mortgagee?: string;
    Mt?: number;
    Pt?: number;
    Dst?: number;
    Fst?: number;
    Vat?: number;
    PolicyId?: number;
}

interface DefaultRebate {
    Id?: number;
    Rate?: number;
    InsuranceProviderId?: number;
    PolicyTypeId?: number;
}
// Forms ###############################

interface EmailFormModel {
    FormId?: number;
    ClientId?: any;
    Email?: string;
    OtherEmail?: string;
    Subject?: string;
    Body?: string;

    FileName?: string;
    FileUrl?: string;
}

interface FormModel {
    Id: number;
    Remarks: string;
    FileName: string;
    FileUrl: string;
    InsuranceProviderId: number;
    DocumentTypeId: number;
    DocumentTypeName: string;
    CompanyName: string;
}

interface FormViewModel {
    NewForm: FormModel;
    FormsList: FormModel[];
    DocumentTypes: DocumentType[];
    InsuranceProviders: any[];
    Clients: ClientSimple[];
}

// SOA #############################

interface SoaIndexModel extends SoaModel {
    SoaList: SoaModel[];
    Clients: ExistingClientsViewModel[];
    Companies: InsuranceProvider[];
}

interface SoaModel {
    Id?: number;
    DueDate?: Date;
    DueDateFormatted?: string;
    IssueDate?: Date;
    Remarks?: string;
    StatusId?: any;
    ClientId?: any;
    InsuranceProviderId?: any;
    ClientName?: string;
    TotalAmountDue?: number;
    StatusName?: string;
}

// Insurance Providers ##################

interface InsuranceProvider {
    Id?: number;
    Name?: string;
}

// Agent

interface Agent {
    Id?: number;
    Name?: string;
}