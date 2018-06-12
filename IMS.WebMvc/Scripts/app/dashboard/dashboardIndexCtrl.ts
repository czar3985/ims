module dashboardIndexCtrl {
    export interface IScope {
        currTab: number;
        tabClick(idx: number): void;
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;
        download(): void;

        emailRenewalNotice(id: number): void;
    }
}

(function () {
    imsApp.controller("dashboardIndexCtrl", dashboardIndexCtrl);

    dashboardIndexCtrl.$inject = ["$scope", "$resource"];

    function dashboardIndexCtrl(
        $scope: dashboardIndexCtrl.IScope,
        $resource: any
    ) {
        $scope.showEntriesVal = "10";
        $scope.currTab = 0;
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var $dtDownloadBtn: JQuery;
        var e = $.Event("input");

        function init() {
            $(function () {
                tabClick(0);
            });
        }

        function tabClick(idx: number): void {
            $scope.currTab = util.getNumber(idx);
            var tabId: string;
            switch ($scope.currTab) {
                case 0:
                    tabId = "#expiring-policies";
                    break;
                case 1:
                    tabId = "#outstanding-invoices";
                    break;
                case 2:
                    tabId = "#soa";
                    break;
            }

            if ($dtSearchText !== undefined) {
                $scope.searchText = "";
                $dtSearchText.val($scope.searchText);
                $dtSearchText.trigger(e);
            }
            $dtSearchText = $(tabId + " input[type=search]");
            $dtSelectInput = $(tabId + " select");
            $dtDownloadBtn = $(tabId + " .buttons-excel");
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

        function emailRenewalNotice(id: number): void {
            $resource(util.getPrePath() + '/api/DashboardApi').get({ id: id },
                function () {
                    toastr.success("Renewal notice successfully sent!");
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

        $scope.emailRenewalNotice = emailRenewalNotice;
    }

})();