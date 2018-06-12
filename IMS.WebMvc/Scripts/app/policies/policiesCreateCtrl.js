(function () {
    imsApp.controller("policiesCreateCtrl", policiesCreateCtrl);
    policiesCreateCtrl.$inject = ["$scope", "$resource", "$timeout"];
    function policiesCreateCtrl($scope, $resource, $timeout) {
        var policyVm = {};
        var insuranceProviderVm = {};
        var policyTypeVm = {};
        var clientVm = {};
        var agentVm = {};
        var insuranceProviders = new Array();
        var policyTypes = new Array();
        var clients = new Array();
        var agents = new Array();
        var rList = new Array();
        function init() {
            $(document).ready(function () {
                $("#menu-policy").addClass('active');
                $('.datepicker').datepicker();
            });
        }
        function ctrlInit(_policyVm) {
            $scope.policyVm = policyVm = _policyVm;
            policyVm.Companies.forEach(function (item) {
                insuranceProviders.push(item);
            });
            policyVm.PolicyTypes.forEach(function (item) {
                policyTypes.push(item);
            });
            policyVm.Clients.forEach(function (item) {
                clients.push(item);
            });
            policyVm.Agents.forEach(function (item) {
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
                policyVm.Risks.forEach(function (item) {
                    rList.push(item);
                });
                amountChanged();
            }
            clientVm.IsOrganization = false;
        }
        function addRisk() {
            var newP = {
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
        function removeRisk(idx) {
            rList.splice(idx, 1);
            amountChanged();
            premiumChanged();
        }
        function amountChanged() {
            var amount = 0;
            rList.forEach(function (item) {
                if (item.AmountInsured !== undefined)
                    amount += item.AmountInsured;
            });
            $scope.sumInsured = amount;
            riskItemChanged();
        }
        function submit(form) {
            if (form.$valid) {
                policyVm.Risks = rList;
                policyVm.SumInsured = $scope.sumInsured;
                $resource(util.getPrePath() + '/api/PolicyApi').save(policyVm, function (data) {
                    location.replace(util.getPrePath() + "/policy/Index");
                }, function () {
                    alert("Error: Unable to create policy!");
                });
            }
        }
        function submitInsuranceProvider(formInsuranceProvider) {
            if (formInsuranceProvider.$valid) {
                $resource(util.getPrePath() + '/api/CompanyApi').save(insuranceProviderVm, function (data) {
                    insuranceProviders.push(data);
                    policyVm.InsuranceProviderId = data.Id;
                    policyVm.Rebate = 0;
                    $('#add-company-modal').modal('hide');
                }, function () {
                    alert("Error: Unable to create new company!");
                });
            }
        }
        function submitPolicyType(formPolicyType) {
            if (formPolicyType.$valid) {
                $resource(util.getPrePath() + '/api/PolicyTypeApi').save(policyTypeVm, function (data) {
                    policyTypes.push(data);
                    policyVm.PolicyTypeId = data.Id;
                    policyVm.Rebate = 0;
                    $('#add-policy-type-modal').modal('hide');
                }, function () {
                    alert("Error: Unable to create policy type!");
                });
            }
        }
        function submitClient(formClient) {
            if (formClient.$valid) {
                $resource(util.getPrePath() + '/api/ClientApi').save(clientVm, function (data) {
                    clients.push(data);
                    policyVm.ClientId = data.Id;
                    //// DELETED: Rebate will now come from Company/Policy Type defaults, not Client defaults
                    //policyVm.Rebate = data.Rebate;
                    $('#addClientFromPolicyModal').modal('hide');
                }, function () {
                    alert("Error: Unable to create client!");
                });
            }
        }
        function submitAgent(formAgent) {
            if (formAgent.$valid) {
                $resource(util.getPrePath() + '/api/AgentApi').save(agentVm, function (data) {
                    agents.push(data);
                    policyVm.AgentId = data.Id;
                    $('#add-agent-modal').modal('hide');
                }, function () {
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
        function updateRebate() {
            policyVm.Rebate = 0;
            if ((policyVm.InsuranceProviderId != 0) && (policyVm.PolicyTypeId != 0)) {
                policyVm.DefaultRebates.forEach(function (item) {
                    if ((item.InsuranceProviderId == policyVm.InsuranceProviderId)
                        && (item.PolicyTypeId == policyVm.PolicyTypeId))
                        policyVm.Rebate = item.Rate;
                });
            }
        }
        function companyChanged() {
            updateRebate();
        }
        function policyTypeChanged() {
            updateRebate();
        }
        function riskItemChanged() {
            var newRate = 0;
            //var totalRate: number = 0;
            var totalCommission = 0;
            var totalPremium = 0;
            rList.forEach(function (item) {
                if (item.AmountInsured !== undefined && item.Rate !== undefined) {
                    var commision = item.AmountInsured * item.Rate;
                    totalCommission += commision;
                    totalPremium += item.AmountInsured;
                }
            });
            //newRate = totalRate / rList.length;
            newRate = totalCommission / totalPremium;
            policyVm.Rate = newRate;
            policyVm.Commission = totalCommission;
        }
        function premiumChanged() {
            var newPremium = 0;
            rList.forEach(function (item) {
                if (item.Premium !== undefined)
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
//# sourceMappingURL=policiesCreateCtrl.js.map