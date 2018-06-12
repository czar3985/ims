module invoicesCreateCtrl {
    export interface IScope {
        ctrlInit(invoiceVm: InvoiceItemViewModel): void;
        clientChanged(): void;
        invoiceVm: InvoiceItemViewModel;
        policyList: ClientPoliciesListModel[];
        clientList: ClientSimple[];
        policyChanged(): void;

        errorVars: any;

        premium: ParticularModel;
        vat: ParticularModel;
        pList: ParticularModel[];
        addParticular(): void;
        removeParticular(idx: number): void
        particularTypeList: ParticularType[];
        totalAmountDue: number;
        amountChanged(): void;
        rebateChanged(): void;

        submit(form): void;

        changeLocation(): void;
        changeState(val: number): void;
        isReadOnly(): boolean;

        $apply: any;

        hidePremium: boolean;
        hasVAT: boolean;

        emailApprovedInvoice(id: number): void;
        isSending: boolean;
        markAsPaid(): void;

        hasEWTChanged(): void;
        hasVATChanged(): void;
    }
}

(function () {
    imsApp.controller("invoicesCreateCtrl", invoicesCreateCtrl);

    invoicesCreateCtrl.$inject = ["$scope", "$resource", "$location", "$timeout"];

    function invoicesCreateCtrl(
        $scope: invoicesCreateCtrl.IScope,
        $resource: any,
        $location: ng.ILocationService,
        $timeout: ng.ITimeoutService
    ) {
        var selectedPolicy: ClientPoliciesListModel;
        var policyList: ClientPoliciesListModel[] = new Array<ClientPoliciesListModel>();
        var clientList: ClientSimple[];
        var invoiceVm: InvoiceItemViewModel = {
            ClientId: 0,
            PolicyId: 0,
            CompanyName: "",
            EndorsementNumber: "",
            AccountNumber: "",
            SumInsured: 0,
            Premium: 0
        };
        var particularTypeList: ParticularType[];
        var premium: ParticularModel = {
            Id: 0,
            ParticularAmount: 0,
            Remarks: "",
            InvoiceId: 0,
            ParticularTypeId: 0,
            ParticularTypeName: "Premium"
        };
        var vat: ParticularModel = {
            Id: 0,
            ParticularAmount: 0,
            Remarks: "",
            InvoiceId: 0,
            ParticularTypeId: 0,
            ParticularTypeName: "(vat)"
        };
        var tmpVat: ParticularModel = {
            Id: 0,
            ParticularAmount: 0,
            Remarks: "",
            InvoiceId: 0,
            ParticularTypeId: 0,
            ParticularTypeName: ""
        }
        var pList: ParticularModel[] = new Array<ParticularModel>();
        var errorVars: any = {
            InvoiceNumber: false
        }

        function init() {
            $(document).ready(function () {
                $("#menu-invoice").addClass('active');
                $('.datepicker').datepicker();
            });
            
        };
        var isReadOnlyState: boolean = true;
        var returnUrl: string = util.getPrePath() +  "/Invoice/Index";

        function ctrlInit(_invoiceVM: InvoiceItemViewModel) {
            $scope.invoiceVm = invoiceVm = _invoiceVM;
            clientList = _invoiceVM.Clients;
            $scope.particularTypeList = particularTypeList = _invoiceVM.ParticularTypesList;

            invoiceVm.ClientId = _invoiceVM.ClientId.toString();
            invoiceVm.PolicyId = _invoiceVM.PolicyId.toString();

            _invoiceVM.Policies.forEach(function (item: ClientPoliciesListModel) {
                policyList.push(item);
            });
            premium.ParticularAmount = invoiceVm.Premium;

            if (_invoiceVM.Particulars.length > 0) {
                var _premium = undefined;
                var _vat = undefined;
                var idx: number = 0;
                _invoiceVM.Particulars.forEach(function (item: ParticularModel) {
                    if (item.ParticularTypeName.toLowerCase() === "premium") {
                        _premium = item;
                        _invoiceVM.Particulars.splice(idx, 1);
                    }
                    
                    idx++;
                });

                idx = 0;
                _invoiceVM.Particulars.forEach(function (item: ParticularModel) {
                    if (item.ParticularTypeName.toLowerCase().indexOf("(vat)") !== -1) {
                        _vat = item;
                        _invoiceVM.Particulars.splice(idx, 1);
                    }
                    idx++;
                });

                $scope.hasVAT = true;
                if (_premium !== undefined) {
                    premium.Id = _premium.Id;
                    premium.Remarks = _premium.Remarks;
                    premium.ParticularAmount = _premium.ParticularAmount;
                    premium.InvoiceId = _premium.InvoiceId;

                    // edit mode: initially set VAT to none. If it exist, the if below will handle it
                    $scope.hasVAT = false;
                }

                if (_vat !== undefined) {
                    vat.Id = _vat.Id;
                    vat.Remarks = _vat.Remarks;
                    vat.ParticularAmount = _vat.ParticularAmount;
                    vat.InvoiceId = _vat.InvoiceId;
                    $scope.hasVAT = true;
                }

                _invoiceVM.Particulars.forEach(function (item: ParticularModel) {
                    item.ParticularTypeId = item.ParticularTypeId.toString();
                    pList.push(item);
                });
            }

            if (invoiceVm.StatusName.toLowerCase() === "new" ||
                invoiceVm.StatusName.toLowerCase() === "pending" ||
                invoiceVm.StatusName.toLowerCase() === "rejected") {

                isReadOnlyState = false;
            }

            if (!(invoiceVm.ReturnUrl === undefined || invoiceVm.ReturnUrl === null || invoiceVm.ReturnUrl === ""))
                returnUrl = invoiceVm.ReturnUrl;

            amountChanged();
        }

        function clientChanged(): void {
            clientList.forEach(function (item: ClientSimple) {
                if (item.Id == invoiceVm.ClientId)
                    invoiceVm.AccountNumber = item.AccountNumber;
            });

            $resource(util.getPrePath() + '/api/PolicyApi').query({ id: $scope.invoiceVm.ClientId },
                function (data: ClientPoliciesListModel[]) {
                    policyList.splice(0, policyList.length);
                    data.forEach(function (item: ClientPoliciesListModel) {
                        policyList.push(item);
                    });

                    if (policyList.length > 0) {
                        selectedPolicy = policyList[0];
                        invoiceVm.PolicyId = selectedPolicy.Id.toString();
                        updateOthers();
                    }
                },
                function () {
                    alert("Error!");
                });
        }

        function policyChanged(): void {
            policyList.forEach(function (item: ClientPoliciesListModel) {
                if (item.Id == invoiceVm.PolicyId)
                    selectedPolicy = item;
            });
            updateOthers();
        }

        function updateOthers() {
            invoiceVm.CompanyName = selectedPolicy.CompanyName;
            invoiceVm.EndorsementNumber = selectedPolicy.LatestEndorsementNumber;
            invoiceVm.SumInsured = selectedPolicy.SumInsured;
            invoiceVm.Premium = selectedPolicy.Premium;
            invoiceVm.Rebate = selectedPolicy.Rebate;
            rebateChanged();
            amountChanged();
        }

        function resetOthers() {
            invoiceVm.CompanyName = "";
            invoiceVm.EndorsementNumber = "";
            invoiceVm.SumInsured = 0;
            invoiceVm.Premium = 0;
        }

        function addParticular(): void {
            var newP: ParticularModel = {
                Id: 0,
                ParticularAmount: 0,
                Remarks: "",
                InvoiceId: 0,
                ParticularTypeId: "2",
                ParticularTypeName: ""
            };

            pList.push(newP);
        }

        function removeParticular(idx: number): void {
            pList.splice(idx, 1);
            amountChanged();
        }

        function amountChanged(): void {
            var amount: number = 0;

            tmpVat.ParticularAmount = premium.ParticularAmount * 0.12;
            if($scope.hasVAT == true)
                vat.ParticularAmount = tmpVat.ParticularAmount;

            pList.forEach(function (item: ParticularModel) {
                amount += item.ParticularAmount;
            });

            $scope.totalAmountDue = amount + premium.ParticularAmount + vat.ParticularAmount;
        }

        function rebateChanged(): void {
            premium.ParticularAmount = invoiceVm.Premium - (invoiceVm.Premium * invoiceVm.Rebate);
            amountChanged();
        }

        function submit(form): void {
            if (form.$valid) {

                if (invoiceVm.ClientId == 0)
                    return;

                if (invoiceVm.PolicyId == undefined || invoiceVm.PolicyId == 0)
                    return;

                switch (invoiceVm.StatusName.toLowerCase()) {
                    case "new":
                    case "pending":
                    case "rejected":
                        createNewOrSaveChanges();
                        break;
                }
            }
        }

        function createNewOrSaveChanges() {
            invoiceVm.Particulars = pList;
            invoiceVm.TotalAmountDue = $scope.totalAmountDue;

            if ($scope.hasVAT == true)
                pList.unshift(vat);

            pList.unshift(premium);
            $scope.hidePremium = true;

            $resource(util.getPrePath() + '/api/InvoiceApi').save(invoiceVm,
                function (data: InvoiceItemViewModel) {
                    location.replace(returnUrl);
                },
                function (data: any) {
                    if (data.data.error === "InvoiceNumber") {
                        errorVars.InvoiceNumber = true;
                        pList.splice(0, 2);
                        $scope.hidePremium = false;
                    }
                });
        }

        function changeLocation(): void {
            location.replace(util.getPrePath() + "/Invoice/Index");
        }

        function changeState(val: number): void {
            invoiceVm.InvoiceAction = val;
            $resource(util.getPrePath() + '/api/InvoiceApi/ChangeState', null,
                {
                    'patch': { method: 'PATCH' }
                }
            ).patch(invoiceVm,
                function () {
                    location.replace(util.getPrePath() + "/Invoice/Index");
                },
                function () {
                    alert("Error!");
                });
        }

        function markAsPaid(): void {
            invoiceVm.InvoiceAction = 4;

            $resource(util.getPrePath() + '/api/InvoiceApi').save(invoiceVm,
                function (data: InvoiceItemViewModel) {
                    location.replace(returnUrl);
                },
                function () {
                    alert("Error!");
                });
        }

        function emailApprovedInvoice(id: number): void {
            if ($scope.isSending)
                return;
            $scope.isSending = true;
            $resource(util.getPrePath() + '/api/InvoiceApi').get({ id: id },
                function () {
                    changeState(3);
                    location.replace(util.getPrePath() + "/Invoice/Index");
                    $scope.isSending = false;
                },
                function () {
                    toastr.error("Something went wrong with renewal notice send!");
                    $scope.isSending = false;
                });
        }

        function isReadOnly(): boolean {
            return isReadOnlyState;
        }

        function hasEWTChanged(): void {
            var AmountPaid: number = 0;
            if (invoiceVm.HasEWT == true)
                AmountPaid = $scope.totalAmountDue - ($scope.totalAmountDue * 0.02);
            else
                AmountPaid = $scope.totalAmountDue;

            invoiceVm.AmountPaid = AmountPaid;
        }

        function hasVATChanged(): void {
            $scope.hasVAT = !$scope.hasVAT;

            if ($scope.hasVAT == true) {
                vat.Remarks = tmpVat.Remarks;
            } else {
                tmpVat.ParticularAmount = vat.ParticularAmount;
                tmpVat.Remarks = vat.Remarks;

                vat.ParticularAmount = 0;
                vat.Remarks = "";
            }

            amountChanged();
        }

        init();

        $scope.ctrlInit = ctrlInit;
        $scope.clientChanged = clientChanged;
        $scope.invoiceVm = invoiceVm;
        $scope.policyList = policyList;
        $scope.clientList = clientList;
        $scope.policyChanged = policyChanged;

        $scope.premium = premium;
        $scope.vat = vat;
        $scope.pList = pList;
        $scope.addParticular = addParticular;
        $scope.removeParticular = removeParticular;
        $scope.amountChanged = amountChanged;
        $scope.rebateChanged = rebateChanged;
        $scope.totalAmountDue = 0;
        $scope.submit = submit;

        $scope.changeLocation = changeLocation;
        $scope.changeState = changeState;
        $scope.isReadOnly = isReadOnly;

        $scope.hidePremium = false;

        $scope.hasEWTChanged = hasEWTChanged;

        $scope.emailApprovedInvoice = emailApprovedInvoice;
        $scope.isSending = false;
        $scope.markAsPaid = markAsPaid;

        $scope.errorVars = errorVars;

        $scope.hasVAT = true;
        $scope.hasVATChanged = hasVATChanged;
    }

})();