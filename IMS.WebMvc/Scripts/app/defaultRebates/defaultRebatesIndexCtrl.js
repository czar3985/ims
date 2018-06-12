(function () {
    imsApp.controller("defaultRebatesIndexCtrl", defaultRebatesIndexCtrl);
    defaultRebatesIndexCtrl.$inject = ["$scope"];
    function defaultRebatesIndexCtrl($scope) {
        var $dtSearchText;
        var $dtSelectInput;
        var e = $.Event("input");
        function init() {
            $(function () {
                $("#menu-default-rebate").addClass('active');
                $dtSelectInput = $("#default-rebates-list-table select");
                $dtSearchText = $("#default-rebates-list-table input[type=search]");
            });
        }
        function showEntriesChanged() {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }
        function searchChanged() {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }
        init();
        $scope.showEntriesVal = "10";
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
    }
})();
//# sourceMappingURL=defaultRebatesIndexCtrl.js.map