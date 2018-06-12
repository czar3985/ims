(function () {
    imsApp.controller("invoicesIndexCtrl", invoicesIndexCtrl);
    invoicesIndexCtrl.$inject = ["$scope", "$timeout", "$resource"];
    function invoicesIndexCtrl($scope, $timeout, $resource) {
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var $form;
        var e = $.Event("input");
        var invoiceVm = {};
        function init() {
            $(function () {
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
                $form = $(".tab-content form").first();
                $("#menu-invoice").addClass('active');
            });
        }
        function tabClick(id) {
            var tabId = "#" + id;
            if ($dtSearchText !== undefined) {
                $scope.searchText = "";
                $dtSearchText.val($scope.searchText);
                $dtSearchText.trigger(e);
            }
            $dtSearchText = $(tabId + " input[type=search]");
            $dtSelectInput = $(tabId + " select");
            $dtDownloadBtn = $(tabId + " .buttons-excel");
            $form = $("#form_" + id);
            $dtSelectInput.val($scope.showEntriesVal);
        }
        function showEntriesChanged() {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }
        function searchChanged() {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }
        function download() {
            $dtDownloadBtn.click();
        }
        function submit() {
            $form.submit();
        }
        function approve() {
            bootbox.confirm("Are you sure you want to appove the selected items?", function (result) {
                if (result) {
                    $("input.is-approve").val("true");
                    submit();
                }
            }).find("div.modal-dialog").addClass("modal-sm");
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }
        function reject() {
            bootbox.confirm("Are you sure you want to reject the selected items?", function (result) {
                if (result) {
                    $("input.is-approve").val("false");
                    submit();
                }
            }).find("div.modal-dialog").addClass("modal-sm");
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }
        function changeState(val, id) {
            invoiceVm.InvoiceAction = val;
            invoiceVm.Id = id;
            $resource(util.getPrePath() + '/api/InvoiceApi/ChangeState', null, {
                'patch': { method: 'PATCH' }
            }).patch(invoiceVm, function () {
                location.replace(util.getPrePath() + "/Invoice/Index");
            }, function () {
                alert("Error!");
            });
        }
        function emailApprovedInvoice(id) {
            $resource(util.getPrePath() + '/api/InvoiceApi').get({ id: id }, function () {
                changeState(3, id);
                toastr.success("Invoice successfully sent!");
            }, function () {
                toastr.error("Something went wrong with renewal notice send!");
            });
        }
        init();
        $scope.tabClick = tabClick;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.download = download;
        $scope.approve = approve;
        $scope.reject = reject;
        $scope.emailApprovedInvoice = emailApprovedInvoice;
        $scope.changeState = changeState;
    }
})();
//# sourceMappingURL=invoicesIndexCtrl.js.map