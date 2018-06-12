module claimsIndexCtrl {
    export interface IScope {
        currTab: number;
        tabClick(id: string): void;
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;
        download(): void;
    }
}

(function () {
    imsApp.controller("claimsIndexCtrl", claimsIndexCtrl);

    claimsIndexCtrl.$inject = ["$scope", "$timeout"];

    function claimsIndexCtrl(
        $scope: claimsIndexCtrl.IScope,
        $timeout: ng.ITimeoutService
    ) {
        $scope.showEntriesVal = "10";
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var $dtDownloadBtn: JQuery;
        var e = $.Event("input");

        function init() {
            $(function () {
                $dtSearchText = $(".tab-content input[type=search]").first();
                $dtSelectInput = $(".tab-content select").first();
                $dtDownloadBtn = $(".tab-content  .buttons-excel").first();
                $("#menu-claim").addClass('active');
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

        init();

        $scope.tabClick = tabClick;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;

        $scope.download = download;
    }

})();