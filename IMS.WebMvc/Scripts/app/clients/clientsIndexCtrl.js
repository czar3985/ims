(function () {
    imsApp.controller("clientsIndexCtrl", clientsIndexCtrl);
    clientsIndexCtrl.$inject = ["$scope", "$timeout"];
    function clientsIndexCtrl($scope, $timeout) {
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var $dtDownloadBtn;
        var e = $.Event("input");
        var editItem = {
            ReturnUrl: util.getPrePath() + "/Client/Index"
        };
        var clients;
        function init() {
            $(function () {
                $("#menu-client").addClass('active');
                $dtSelectInput = $("#clients-list-table select");
                $dtSearchText = $("#clients-list-table input[type=search]");
                $dtDownloadBtn = $("#clients-list-table .buttons-excel");
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
        function download() {
            $dtDownloadBtn.click();
        }
        function ctrlInit(_clientVM) {
            clients = _clientVM.ClientsList;
        }
        function getEditActionUrl() {
            return util.getPrePath() + "/Client/Edit/" + editItem.Id;
        }
        function edit(id) {
            clients.forEach(function (item) {
                if (item.Id == id) {
                    editItem.Id = item.Id;
                    editItem.AccountNumber = item.AccountNumber;
                    editItem.IsOrganization = item.IsOrganization;
                    editItem.LastName = item.LastName;
                    editItem.FirstName = item.FirstName;
                    editItem.OrganizationName = item.OrganizationName;
                    editItem.Address = item.Address;
                    editItem.ContactNumber = item.ContactNumber;
                    editItem.Email = item.Email;
                    editItem.Remarks = item.Remarks;
                }
            });
        }
        init();
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.editItem = editItem;
        $scope.edit = edit;
        $scope.download = download;
        $scope.ctrlInit = ctrlInit;
        $scope.getEditActionUrl = getEditActionUrl;
    }
})();
//# sourceMappingURL=clientsIndexCtrl.js.map