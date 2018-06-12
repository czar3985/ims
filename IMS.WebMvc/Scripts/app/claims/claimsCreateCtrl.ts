module claimsCreateCtrl {
    export interface IScope {
        ctrlInit(claimVm: ClaimItemViewModel): void;
        clientChanged(): void;
        claimVm: ClaimItemViewModel;
        policyList: ClientPoliciesListModel[];
        policyChanged(): void;
        submit(form: any): void;
    }
}

(function () {
    imsApp.controller("claimsCreateCtrl", claimsCreateCtrl);

    claimsCreateCtrl.$inject = ["$scope", "$resource"];

    function claimsCreateCtrl(
        $scope: claimsCreateCtrl.IScope,
        $resource: any
    ) {
        var selectedPolicy: ClientPoliciesListModel;
        var policyList: ClientPoliciesListModel[] = new Array<ClientPoliciesListModel>();
        var claimVm: ClaimItemViewModel = {
            ClientId: "0",
            PolicyId: "0",
            StatusId: "0",
            CompanyName: "",
            SumInsured: 0,
            ExpiryDateString: ""
        };

        function init() {
            $(document).ready(function () {
                $("#menu-claim").addClass('active');
                $('.datepicker').datepicker();
            });
        }

        function ctrlInit(_claimVm: ClaimItemViewModel) {
            $scope.claimVm = claimVm = _claimVm;
            claimVm.ClientId = claimVm.ClientId.toString();
            claimVm.PolicyId = claimVm.PolicyId.toString();
            claimVm.StatusId = claimVm.StatusId.toString();

            if (_claimVm.Policies !== undefined && _claimVm.Policies !== null) {
                if (_claimVm.Policies.length > 0) {
                    _claimVm.Policies.forEach(function (item: ClientPoliciesListModel) {
                        policyList.push(item);
                    });
                }
            }
        }

        function clientChanged(): void {
            $resource(util.getPrePath() + '/api/PolicyApi').query({ id: $scope.claimVm.ClientId },
                function (data: ClientPoliciesListModel[] ) {
                    policyList.splice(0, policyList.length);
                    data.forEach(function (item: ClientPoliciesListModel) {
                        policyList.push(item);
                    });

                    if (policyList.length > 0) {
                        selectedPolicy = policyList[0];
                        claimVm.PolicyId = selectedPolicy.Id.toString();
                        updateOthers();
                    }
                    else {
                        resetOthers();
                    }
                },
                function () {
                    alert("Error!");
                });
        }

        function policyChanged(): void {
            policyList.forEach(function (item: ClientPoliciesListModel) {
                if (item.Id == claimVm.PolicyId)
                    selectedPolicy = item;
            });
            updateOthers();
        }

        function updateOthers() {
            var date:Date = new Date(selectedPolicy.ExpiryDate.toString());
            claimVm.CompanyName = selectedPolicy.CompanyName;
            claimVm.PolicyTypeName = selectedPolicy.PolicyTypeName;
            claimVm.ExpiryDateString =
                    (date.getMonth() + 1).toString() + "/" + 
                    date.getDate().toString() + "/" + 
                    date.getFullYear().toString();
            claimVm.SumInsured = selectedPolicy.SumInsured;
        }

        function resetOthers() {
            claimVm.CompanyName = "";
            claimVm.PolicyTypeName = "";
            claimVm.ExpiryDateString = undefined;
            claimVm.SumInsured = 0;
        }

        function createExpiryDate() : Date {
            var date = new Date;
            date.setDate(date.getDate() + 1);
            return date;
        }

        function submit(form): void {
            if (form.$valid) {
                if (claimVm.ClientId == 0 ||
                    claimVm.ClaimAmount <= 0) {
                    return;
                }
                
                $resource(util.getPrePath() + '/api/claimApi').save(claimVm,
                    function (data: any) {
                        location.replace(data.returnUrl);
                    },
                    function () {
                        alert("Error!");
                    });
            }
        }

        init();
        $scope.ctrlInit = ctrlInit;
        $scope.clientChanged = clientChanged;
        $scope.claimVm = claimVm;
        $scope.policyList = policyList;
        $scope.policyChanged = policyChanged;
        $scope.submit = submit;
    }

})();