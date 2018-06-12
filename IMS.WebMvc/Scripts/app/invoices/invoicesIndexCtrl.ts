module invoicesIndexCtrl {
    export interface IScope {
        currTab: number;
        tabClick(id: string): void;
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;
        download(): void;

        approve(): void;
        reject(): void;
        actionUrl(): string;
        isApprove: boolean;

        emailApprovedInvoice(id: number): void;
        changeState(val: number, id: number): void;
        invoiceVm: InvoiceItemViewModel;
    }
}

(function () {
    imsApp.controller("invoicesIndexCtrl", invoicesIndexCtrl);

    invoicesIndexCtrl.$inject = ["$scope", "$timeout", "$resource"];

    function invoicesIndexCtrl(
        $scope: invoicesIndexCtrl.IScope,
        $timeout: ng.ITimeoutService,
        $resource: any
    ) {
        $scope.showEntriesVal = "10";
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var $dtDownloadBtn: JQuery;
        var $form: JQuery;
        var e = $.Event("input");
        var invoiceVm: InvoiceItemViewModel = {};

        function init() {
            $(function () {
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
                $form = $(".tab-content form").first();
                $("#menu-invoice").addClass('active');
            });
        }

        function tabClick(id: string): void {
            var tabId: string = "#" + id;

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

        function showEntriesChanged(): void {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }

        function searchChanged(): void {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }

        function download() {
            $dtDownloadBtn.click();
        }

        function submit() {
            $form.submit();
        }

        function approve(): void {
            bootbox.confirm("Are you sure you want to appove the selected items?", function (result: boolean) {
                if (result) {
                    $("input.is-approve").val("true");
                    submit();
                }
            }).find("div.modal-dialog").addClass("modal-sm");
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }

        function reject(): void {

            bootbox.confirm("Are you sure you want to reject the selected items?", function (result: boolean) {
                if (result) {
                    $("input.is-approve").val("false");
                    submit();
                }
            }).find("div.modal-dialog").addClass("modal-sm");
            $("div.modal-footer button.btn-primary").html("Yes");
            $("div.modal-footer button.btn-default").html("No");
        }

        function changeState(val: number, id: number): void {
            invoiceVm.InvoiceAction = val;
            invoiceVm.Id = id;
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

        function emailApprovedInvoice(id: number): void {
            $resource(util.getPrePath() + '/api/InvoiceApi').get({ id: id },
                function () {
                    changeState(3, id);
                    toastr.success("Invoice successfully sent!");
                },
                function () {
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