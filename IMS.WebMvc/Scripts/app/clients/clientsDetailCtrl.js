(function () {
    imsApp.controller("clientsDetailCtrl", clientsDetailCtrl);
    clientsDetailCtrl.$inject = ["$scope", "$timeout"];
    function clientsDetailCtrl($scope, $timeout) {
        var editItem = {
            ReturnUrl: util.getPrePath() + "/Client/Detail"
        };
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var e = $.Event("input");
        function init() {
            $(function () {
                $("#menu-client").addClass('active');
                //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                //    $timeout(function () {
                //        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                //    }, 200);
                //});
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
            });
        }
        function ctrlInit(_clientListVm) {
            editItem = _clientListVm;
            editItem.ReturnUrl = "Client/Detail/" + editItem.Id;
        }
        function getEditActionUrl() {
            return util.getPrePath() + "/Client/Edit/" + editItem.Id;
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
        init();
        $scope.editItem = editItem;
        $scope.ctrlInit = ctrlInit;
        $scope.getEditActionUrl = getEditActionUrl;
        $scope.showEntriesVal = "10";
        $scope.tabClick = tabClick;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.download = download;
    }
})();
//# sourceMappingURL=clientsDetailCtrl.js.map