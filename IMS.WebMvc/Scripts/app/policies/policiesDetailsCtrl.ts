module policiesDetailsCtrl {
    export interface IScope {
        ctrlInit(pvm: PolicyDetailModel): void;
        newEndt: EndorsementModel;
        emailRenewalNotice(id: number): void;
        generatePDF(): void;
        cancelPolicy(): void;
        //totalEndorsementAmount: number;
        amountChanged(): void;
        getEditActionUrl(): string;
        edit(id: number): void;
        deleteAttachment(id: number): void;
        confirmDelete(): void;
    }
}

(function () {
    imsApp.controller("policiesDetailsCtrl", policiesDetailsCtrl);

    policiesDetailsCtrl.$inject = ["$scope", "$resource"];

    function policiesDetailsCtrl(
        $scope: policiesDetailsCtrl.IScope,
        $resource: any
    ) {
        var newEndt: EndorsementModel = {};
        var endtList: EndorsementModel[];

        function init() {
        }

        function ctrlInit(_pvm: PolicyDetailModel) {
            $scope.newEndt = newEndt = _pvm.Endt;
            endtList = _pvm.Endorsements;
        }

        function emailRenewalNotice(id: number): void {
            $resource(util.getPrePath() + '/api/DashboardApi').get({ id: id },
                function () {
                    toastr.success("Renewal notice successfully sent!");
                },
                function () {
                    toastr.error("Something went wrong with renewal notice send!");
                });
        }

        function generatePDF(): void {
            $(".modal").modal('hide');
        }

        function cancelPolicy(): void {
            bootbox.confirm("Are you sure you want to cancel the policy?", function (result: boolean) {
                if (result)
                    $("#cancel-policy").submit();
            }).find("div.modal-dialog").addClass("modal-sm")
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }

        var idToDelete: number = 0;
        function deleteAttachment(id: number): void {
            idToDelete = id;
        }

        function confirmDelete(): void {
            $resource(util.getPrePath() + '/api/PolicyApi/DeleteAttachment').delete({ id: idToDelete },
                function (data) {
                    location.reload(true);
                },
                function () {
                    alert("Error!");
                });
        }

        function amountChanged(): void {
            var amount: number = 0;
            amount = parseFloat(newEndt.Mt.toString())
                + parseFloat(newEndt.Pt.toString())
                + parseFloat(newEndt.Dst.toString())
                + parseFloat(newEndt.Fst.toString())
                + parseFloat(newEndt.Vat.toString());
            if (newEndt.IsRet == true)
                amount -= parseFloat(newEndt.EndorsementAmount.toString());
            else
                amount += parseFloat(newEndt.EndorsementAmount.toString());
            $scope.newEndt.TotalEndorsementAmount = amount;
        }

        function getEditActionUrl(): string {
            return util.getPrePath() + "/Policy/EditEndorsement/" + newEndt.Id;
        }

        function edit(id: number): void {
            endtList.forEach(function (item: EndorsementModel) {
                if (item.Id == id) {
                    newEndt.Id = item.Id;
                    newEndt.IssueDate = item.IssueDate;
                    newEndt.EffectiveDate = item.EffectiveDate;
                    newEndt.EndorsementNumber = item.EndorsementNumber;
                    newEndt.IsRet = item.EndorsementAmount < 0;
                    newEndt.EndorsementAmount = Math.abs(item.EndorsementAmount);
                    newEndt.TotalEndorsementAmount = item.TotalEndorsementAmount;
                    newEndt.Remarks = item.Remarks;
                    newEndt.Mortgagee = item.Mortgagee;
                    newEndt.Mt = item.Mt;
                    newEndt.Pt = item.Pt;
                    newEndt.Dst = item.Dst;
                    newEndt.Fst = item.Fst;
                    newEndt.Vat = item.Vat;
                    newEndt.PolicyId = item.PolicyId;
                }
            });
        }

        init();

        $scope.ctrlInit = ctrlInit;
        $scope.newEndt = newEndt;
        $scope.emailRenewalNotice = emailRenewalNotice;
        $scope.generatePDF = generatePDF;
        $scope.cancelPolicy = cancelPolicy;
        $scope.getEditActionUrl = getEditActionUrl;
        $scope.deleteAttachment = deleteAttachment;
        $scope.confirmDelete = confirmDelete;
        //$scope.totalEndorsementAmount = 0;
        $scope.edit = edit;
        $scope.amountChanged = amountChanged;
    }
})();