(function () {
    imsApp.controller("claimsIndexCtrl", claimsIndexCtrl);
    claimsIndexCtrl.$inject = ["$scope", "$timeout"];
    function claimsIndexCtrl($scope, $timeout) {
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var e = $.Event("input");
        function init() {
            $(function () {
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
                $("#menu-claim").addClass('active');
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
        $scope.tabClick = tabClick;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.download = download;
    }
})();
//# sourceMappingURL=claimsIndexCtrl.js.map