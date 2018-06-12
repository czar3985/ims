module clientsIndexCtrl {
    export interface IScope {
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;

        editItem: ClientsListModel;
        edit(id: number): void;

        download(): void;
        ctrlInit(clientVM: ClientViewModel): void;
        getEditActionUrl(): string;
    }
}

(function () {
    imsApp.controller("clientsIndexCtrl", clientsIndexCtrl);

    clientsIndexCtrl.$inject = ["$scope", "$timeout"];

    function clientsIndexCtrl(
        $scope: clientsIndexCtrl.IScope,
        $timeout: ng.ITimeoutService
    ) {
        $scope.showEntriesVal = "10";
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var $dtDownloadBtn: JQuery;
        var e = $.Event("input");
        var editItem: ClientsListModel = {
            ReturnUrl: util.getPrePath() + "/Client/Index"
        };
        var clients: ClientsListModel[];

        function init() {
            $(function () {
                $("#menu-client").addClass('active');
                $dtSelectInput = $("#clients-list-table select");
                $dtSearchText = $("#clients-list-table input[type=search]");
                $dtDownloadBtn = $("#clients-list-table .buttons-excel");
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

        function download() {
            $dtDownloadBtn.click();
        }

        function ctrlInit(_clientVM: ClientViewModel) {
            clients = _clientVM.ClientsList;
        }

        function getEditActionUrl(): string {
            return util.getPrePath() +  "/Client/Edit/" + editItem.Id;
        }

        function edit(id: number): void {
            clients.forEach(function (item: ClientsListModel) {
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
                    //editItem.Rebate = item.Rebate;
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