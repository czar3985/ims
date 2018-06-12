module policiesCreateCtrl {
    export interface IScope {
        ctrlInit(policyVm: PolicyDetailModel): void;
        policyVm: PolicyDetailModel;
        insuranceProviderVm: InsuranceProvider;
        policyTypeVm: PolicyType;
        clientVm: ClientModel;
        agentVm: Agent;
        //clientChanged(): void;
        companyChanged(): void;
        policyTypeChanged(): void;
        insuranceProviders: InsuranceProvider[];
        policyTypes: PolicyType[];
        clients: ClientSimple[];
        agents: Agent[];
        rList: RiskModel[];
        addRisk(): void;
        removeRisk(idx: number): void
        sumInsured: number;
        amountChanged(): void;
        submit(form): void;
        submitInsuranceProvider(formInsuranceProvider): void;
        submitPolicyType(formPolicyType): void;
        submitClient(formClient): void;
        submitAgent(formAgent): void;
        clientReturnUrl: string;
        riskItemChanged(): void;
        premiumChanged(): void;
    }
}

(function () {
    imsApp.controller("policiesCreateCtrl", policiesCreateCtrl);

    policiesCreateCtrl.$inject = ["$scope", "$resource", "$timeout"];

    function policiesCreateCtrl(
        $scope: policiesCreateCtrl.IScope,
        $resource: any,
        $timeout: ng.ITimeoutService
    ) {
        var policyVm: PolicyDetailModel = {};
        var insuranceProviderVm: InsuranceProvider = {};
        var policyTypeVm: PolicyType = {};
        var clientVm: ClientModel = {};
        var agentVm: Agent = {};
        var insuranceProviders: InsuranceProvider[] = new Array<InsuranceProvider>();
        var policyTypes: PolicyType[] = new Array<PolicyType>();
        var clients: ClientSimple[] = new Array<ClientSimple>();
        var agents: Agent[] = new Array<Agent>();
        var rList: RiskModel[] = new Array<RiskModel>();

        function init() {
            $(document).ready(function () {
                $("#menu-policy").addClass('active');
                $('.datepicker').datepicker();
            });
        }

        function ctrlInit(_policyVm: PolicyDetailModel) {
            $scope.policyVm = policyVm = _policyVm;

            policyVm.Companies.forEach(function (item: InsuranceProvider) {
                insuranceProviders.push(item);
            });
            policyVm.PolicyTypes.forEach(function (item: PolicyType) {
                policyTypes.push(item);
            });
            policyVm.Clients.forEach(function (item: ClientSimple) {
                clients.push(item);
            });
            policyVm.Agents.forEach(function (item: Agent) {
                agents.push(item);
            });

            if (policyVm.Id === 0) {
                if (insuranceProviders.length > 0)
                    policyVm.InsuranceProviderId = insuranceProviders[0].Id.toString();

                policyVm.PolicyTypeId = policyTypes[0].Id.toString();

                if (clients.length > 0)
                    policyVm.ClientId = clients[0].Id.toString();

                if (agents.length > 0)
                    policyVm.AgentId = agents[0].Id;
            }
            else {
                policyVm.Risks.forEach(function (item: RiskModel) {
                    rList.push(item);
                });
                amountChanged();
            }
            clientVm.IsOrganization = false;
        }

        function addRisk(): void {
            var newP: RiskModel = {
                Id: 0,
                AmountInsured: 0,
                Remarks: "",
                Rate: 0,
                Premium: 0,
                PolicyId: 0
            };

            rList.push(newP);
            premiumChanged();
        }

        function removeRisk(idx: number): void {
            rList.splice(idx, 1);
            amountChanged();
            premiumChanged();
        }

        function amountChanged(): void {
            var amount: number = 0;
            rList.forEach(function (item: RiskModel) {
                if(item.AmountInsured !== undefined)
                    amount += item.AmountInsured;
            });
            $scope.sumInsured = amount;

            riskItemChanged();
        }

        function submit(form): void {
            if (form.$valid) {
                policyVm.Risks = rList;
                policyVm.SumInsured = $scope.sumInsured;

                $resource(util.getPrePath() + '/api/PolicyApi').save(policyVm,
                    function (data: PolicyDetailModel) {
                        location.replace(util.getPrePath() + "/policy/Index");
                    },
                    function () {
                        alert("Error: Unable to create policy!");
                    });
            }
        }

        function submitInsuranceProvider(formInsuranceProvider): void {
            if (formInsuranceProvider.$valid) {
                $resource(util.getPrePath() + '/api/CompanyApi').save(insuranceProviderVm,
                    function (data: InsuranceProvider) {
                        insuranceProviders.push(data);
                        policyVm.InsuranceProviderId = data.Id;
                        policyVm.Rebate = 0;
                        $('#add-company-modal').modal('hide');
                    },
                    function () {
                        alert("Error: Unable to create new company!");
                    });
            }
        }

        function submitPolicyType(formPolicyType): void {
            if (formPolicyType.$valid) {
                $resource(util.getPrePath() + '/api/PolicyTypeApi').save(policyTypeVm,
                    function (data: PolicyType) {
                        policyTypes.push(data);
                        policyVm.PolicyTypeId = data.Id;
                        policyVm.Rebate = 0;
                        $('#add-policy-type-modal').modal('hide');
                    },
                    function () {
                        alert("Error: Unable to create policy type!");
                    });
            }
        }

        function submitClient(formClient): void {
            if (formClient.$valid) {
                $resource(util.getPrePath() + '/api/ClientApi').save(clientVm,
                    function (data: ClientSimple) {
                        clients.push(data);
                        policyVm.ClientId = data.Id;
                        //// DELETED: Rebate will now come from Company/Policy Type defaults, not Client defaults
                        //policyVm.Rebate = data.Rebate;
                        $('#addClientFromPolicyModal').modal('hide');
                    },
                    function () {
                        alert("Error: Unable to create client!");
                    });
            }
        }

        function submitAgent(formAgent): void {
            if (formAgent.$valid) {
                $resource(util.getPrePath() + '/api/AgentApi').save(agentVm,
                    function (data: Agent) {
                        agents.push(data);
                        policyVm.AgentId = data.Id;
                        $('#add-agent-modal').modal('hide');
                    },
                    function () {
                        alert("Error: Unable to create Agent!");
                    });
            }
        }

        //// DELETED: DELETED: Rebate will now come from Company/Policy Type defaults, not Client defaults
        //function clientChanged(): void {
        //    clients.forEach(function (item: ClientSimple) {
        //        if (item.Id.toString() == policyVm.ClientId)
        //            policyVm.Rebate = item.Rebate;
        //    });
        //}

        function updateRebate(): void {
            policyVm.Rebate = 0;
            if ((policyVm.InsuranceProviderId != 0) && (policyVm.PolicyTypeId != 0)) {
                policyVm.DefaultRebates.forEach(function (item: DefaultRebate) {
                    if ((item.InsuranceProviderId == policyVm.InsuranceProviderId)
                        && (item.PolicyTypeId == policyVm.PolicyTypeId))
                        policyVm.Rebate = item.Rate;
                });
            }
        }

        function companyChanged(): void {
            updateRebate();
        }

        function policyTypeChanged(): void {
            updateRebate();
        }

        function riskItemChanged(): void {
            var newRate: number = 0;
            //var totalRate: number = 0;
            var totalCommission: number = 0;
            var totalPremium: number = 0;

            rList.forEach(function (item: RiskModel) {
                if (item.AmountInsured !== undefined && item.Rate !== undefined) {
                    var commision: number = item.AmountInsured * item.Rate;
                    totalCommission += commision;
                    totalPremium += item.AmountInsured;
                    //totalRate += item.Rate;
                }
            });

            //newRate = totalRate / rList.length;
            newRate = totalCommission / totalPremium;
            policyVm.Rate = newRate;
            policyVm.Commission = totalCommission;
        }

        function premiumChanged(): void {
            var newPremium: number = 0;
            rList.forEach(function (item: RiskModel) {
                if(item.Premium !== undefined)
                    newPremium += item.Premium;
            });

            policyVm.Premium = newPremium;
        }

        init();

        $scope.ctrlInit = ctrlInit;
        $scope.policyVm = policyVm;
        $scope.insuranceProviderVm = insuranceProviderVm;
        $scope.policyTypeVm = policyTypeVm;
        $scope.clientVm = clientVm;
        $scope.agentVm = agentVm;
        $scope.insuranceProviders = insuranceProviders;
        $scope.policyTypes = policyTypes;
        $scope.clients = clients;
        $scope.agents = agents;
        $scope.rList = rList;
        $scope.addRisk = addRisk;
        $scope.removeRisk = removeRisk;
        $scope.amountChanged = amountChanged;
        $scope.sumInsured = 0;
        $scope.submit = submit;
        $scope.submitInsuranceProvider = submitInsuranceProvider;
        $scope.submitPolicyType = submitPolicyType;
        $scope.submitClient = submitClient;
        $scope.submitAgent = submitAgent;
        //$scope.clientChanged = clientChanged;
        $scope.companyChanged = companyChanged;
        $scope.policyTypeChanged = policyTypeChanged;

        $scope.clientReturnUrl = util.getPrePath() + "/Policy/Create?ClientId=0";
        $scope.riskItemChanged = riskItemChanged;
        $scope.premiumChanged = premiumChanged;
    }

})();