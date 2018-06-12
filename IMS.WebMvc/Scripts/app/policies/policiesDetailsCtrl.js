(function () {
    imsApp.controller("policiesDetailsCtrl", policiesDetailsCtrl);
    policiesDetailsCtrl.$inject = ["$scope", "$resource"];
    function policiesDetailsCtrl($scope, $resource) {
        var newEndt = {};
        var endtList;
        function init() {
        }
        function ctrlInit(_pvm) {
            $scope.newEndt = newEndt = _pvm.Endt;
            endtList = _pvm.Endorsements;
        }
        function emailRenewalNotice(id) {
            $resource(util.getPrePath() + '/api/DashboardApi').get({ id: id }, function () {
                toastr.success("Renewal notice successfully sent!");
            }, function () {
                toastr.error("Something went wrong with renewal notice send!");
            });
        }
        function generatePDF() {
            $(".modal").modal('hide');
        }
        function cancelPolicy() {
            bootbox.confirm("Are you sure you want to cancel the policy?", function (result) {
                if (result)
                    $("#cancel-policy").submit();
            }).find("div.modal-dialog").addClass("modal-sm");
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }
        var idToDelete = 0;
        function deleteAttachment(id) {
            idToDelete = id;
        }
        function confirmDelete() {
            $resource(util.getPrePath() + '/api/PolicyApi/DeleteAttachment').delete({ id: idToDelete }, function (data) {
                location.reload(true);
            }, function () {
                alert("Error!");
            });
        }
        function amountChanged() {
            var amount = 0;
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
        function getEditActionUrl() {
            return util.getPrePath() + "/Policy/EditEndorsement/" + newEndt.Id;
        }
        function edit(id) {
            endtList.forEach(function (item) {
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
//# sourceMappingURL=policiesDetailsCtrl.js.map