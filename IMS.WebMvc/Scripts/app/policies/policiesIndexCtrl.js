(function () {
    imsApp.controller("policiesIndexCtrl", policiesIndexCtrl);
    policiesIndexCtrl.$inject = ["$scope", "$timeout"];
    function policiesIndexCtrl($scope, $timeout) {
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var e = $.Event("input");
        function init() {
            $(function () {
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
                $("#menu-policy").addClass('active');
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
        $scope.showEntriesVal = "10";
        $scope.tabClick = tabClick;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.download = download;
    }
})();
//# sourceMappingURL=policiesIndexCtrl.js.map