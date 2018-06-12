module reportsShowReportCtrl {
    export interface IScope {
        tabClick(tabId: string): void;
        download(tabId: string): void;
    }
}

interface JQuery {
    battatech_excelexport: any;
}

(function () {
    imsApp.controller("reportsShowReportCtrl", reportsShowReportCtrl);

    reportsShowReportCtrl.$inject = ["$scope"];

    function reportsShowReportCtrl(
        $scope: reportsShowReportCtrl.IScope
    ) {
        var tableId: string = "";
        var contId: string = "";

        function init() {
            $(function () {
                $("#menu-report").addClass('active');
            });
        }

        function tabClick(tabId: string): void {
            tableId = "#" + tabId;
            contId = tabId;
        }

        function download(tabId: string) {
            if (tableId == "") {
                tableId = "#" + tabId;
                contId = tabId;
            }
            
            $(tableId).battatech_excelexport({
                containerid: contId
            });
        }

        init();

        $scope.tabClick = tabClick;
        $scope.download = download;
    }

})();