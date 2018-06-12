(function () {
    imsApp.controller("reportsShowReportCtrl", reportsShowReportCtrl);
    reportsShowReportCtrl.$inject = ["$scope"];
    function reportsShowReportCtrl($scope) {
        var tableId = "";
        var contId = "";
        function init() {
            $(function () {
                $("#menu-report").addClass('active');
            });
        }
        function tabClick(tabId) {
            tableId = "#" + tabId;
            contId = tabId;
        }
        function download(tabId) {
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
//# sourceMappingURL=reportsShowReportCtrl.js.map