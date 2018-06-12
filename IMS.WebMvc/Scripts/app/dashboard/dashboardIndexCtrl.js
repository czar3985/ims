(function () {
    imsApp.controller("dashboardIndexCtrl", dashboardIndexCtrl);
    dashboardIndexCtrl.$inject = ["$scope", "$resource"];
    function dashboardIndexCtrl($scope, $resource) {
        $scope.showEntriesVal = "10";
        $scope.currTab = 0;
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var e = $.Event("input");
        function init() {
            $(function () {
                tabClick(0);
            });
        }
        function tabClick(idx) {
            $scope.currTab = util.getNumber(idx);
            var tabId;
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
        function emailRenewalNotice(id) {
            $resource(util.getPrePath() + '/api/DashboardApi').get({ id: id }, function () {
                toastr.success("Renewal notice successfully sent!");
            }, function () {
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
//# sourceMappingURL=dashboardIndexCtrl.js.map