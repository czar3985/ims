module defaultRebatesIndexCtrl {
    export interface IScope {
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;
    }
} 

(function () {
    imsApp.controller("defaultRebatesIndexCtrl", defaultRebatesIndexCtrl);

    defaultRebatesIndexCtrl.$inject = ["$scope"];

    function defaultRebatesIndexCtrl(
        $scope: defaultRebatesIndexCtrl.IScope
    ) {
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var e = $.Event("input");
        

        function init() {
            $(function () {
                $("#menu-default-rebate").addClass('active');
                $dtSelectInput = $("#default-rebates-list-table select");
                $dtSearchText = $("#default-rebates-list-table input[type=search]");
            });
        }

        function showEntriesChanged(): void {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }

        function searchChanged(): void {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }

        init();

        $scope.showEntriesVal = "10";
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
    }

})();