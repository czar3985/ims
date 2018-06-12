module soaIndexCtrl {
    export interface IScope {
        showEntriesVal: string;
        showEntriesChanged(): void;
        searchText: string;
        searchChanged(): void;

        ctrlInit(soaVm: SoaIndexModel): void;

        getEditActionUrl(): string;
        soaVm: SoaModel;
        create(): void;
        submitCreate(form: any): void;
        submitUpdate(form: any): void;
        edit(id: number): void;
        clientChanged(): void;
        companyList: InsuranceProvider[];
        errorVar: any;
    }
}

(function () {
    imsApp.controller("soaIndexCtrl", soaIndexCtrl);

    soaIndexCtrl.$inject = ["$scope", "$timeout", "$resource"];

    function soaIndexCtrl(
        $scope: soaIndexCtrl.IScope,
        $timeout: ng.ITimeoutService,
        $resource: any
    ) {
        $scope.showEntriesVal = "10";
        var $dtSearchText: JQuery;
        var $dtSelectInput: JQuery;
        var e = $.Event("input");
        var soaVm: SoaModel = {
            Id: 0,
            ClientId: "0"
        };
        var errorVar: any = {
            hasError: false,
            errorMessage: ""
        }
        var soas: SoaModel[];
        var companyList: InsuranceProvider[] = new Array<InsuranceProvider>();
        var selectedCompany: InsuranceProvider;
        var returnUrl: string = util.getPrePath() + "/Soa/Index";

        function ctrlInit(_vm: SoaIndexModel) {
            soas = _vm.SoaList;
            $scope.soaVm = soaVm = _vm;
        }

        function init() {
            $(function () {
                $("#menu-soa").addClass('active');
                $dtSelectInput = $("#soa-list-table select");
                $dtSearchText = $("#soa-list-table input[type=search]");
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

        function create(): void {
            // initialize Issue Date (today) and Due Date (after 30 days)
            //var issueDate = new Date;
            //soaVm.IssueDate = issueDate;
            //$('#soa-add-issue-date').datepicker('setDate', issueDate);

            soaVm.IssueDate = new Date;
            $('#soa-add-issue-date').datepicker('setDate', soaVm.IssueDate);

            var dueDate = new Date;
            dueDate.setDate(dueDate.getDate() + 30);
            soaVm.ClientId = "0";
            soaVm.InsuranceProviderId = "0";
            soaVm.DueDate = dueDate;
            $('#soa-add-due-date').datepicker('setDate', dueDate);
            errorVar.hasError = false;
        }

        function submitCreate(form:any): void {
            if (form.$valid) {
                if (soaVm.InsuranceProviderId === "0" ||
                    soaVm.ClientId === "0") {
                    return;
                }
                $resource(util.getPrePath() + '/api/SoaApi').save(soaVm,
                    function (data: any) {
                        location.replace(returnUrl);
                    },
                    function (data: any) {
                        errorVar.hasError = true;
                        errorVar.errorMessage = data.data.errorMessage;
                    });
            }
        }       

        function submitUpdate(form: any): void {
            if (form.$valid) {
                if (soaVm.InsuranceProviderId === "0" ||
                    soaVm.ClientId === "0") {
                    return;
                }

                var resource = $resource(util.getPrePath() + '/api/SoaApi', null, {
                    'patch': { method: 'PATCH'} 
                });

               resource.patch(soaVm,
                    function (data: any) {
                        location.replace(returnUrl);
                    },
                    function (data: any) {
                        errorVar.hasError = true;
                        errorVar.errorMessage = data.data.errorMessage;
                    });
            }
        }

        function getEditActionUrl(): string {
            return util.getPrePath() + "/Soa/Edit/" + soaVm.Id;
        }

        function edit(id: number): void {   
            soas.forEach(function (item: SoaModel) {
                if (item.Id == id) {
                    soaVm.Id = item.Id;

                    var dateObject = stringToUTCDate(item.IssueDate.toString());;
                    soaVm.IssueDate = dateObject;
                    $('#soa-issue-date').datepicker('setDate', dateObject);

                    soaVm.DueDate = item.DueDate;
                    var dateObject2 = stringToUTCDate(soaVm.DueDate.toString());
                    $('#soa-due-date').datepicker('setDate', dateObject2);

                    soaVm.Remarks = item.Remarks;
                    soaVm.ClientId = item.ClientId.toString();
                    soaVm.StatusId = item.StatusId.toString();
                    soaVm.InsuranceProviderId = item.InsuranceProviderId.toString();
                    soaVm.TotalAmountDue = item.TotalAmountDue;
                }
            });
            clientChanged();
        }

        function stringToUTCDate(dateStr: string) {
            var dateTime: string[] = dateStr.split("T");
            var dateSplit: string[] = dateTime[0].split("-");
            var timeSplit: string[] = dateTime[1].split(":");

            var utcDate: Date = new Date(parseInt(dateSplit[0]),
                parseInt(dateSplit[1])-1,
                parseInt(dateSplit[2]),
                parseInt(timeSplit[0]),
                parseInt(timeSplit[1])
            );

            return utcDate;
        }

        function convertUTCDateToLocalDate(date) {
            var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

            var offset = date.getTimezoneOffset() / 60;
            var hours = date.getHours();

            newDate.setHours(hours - offset);

            return newDate;
        }

        function clientChanged(): void {
            $resource(util.getPrePath() + '/api/CompanyApi/GetCompanyListPerClient').query({ id: $scope.soaVm.ClientId },
                function (data: InsuranceProvider[]) {
                    companyList.splice(0, companyList.length);
                    data.forEach(function (item: InsuranceProvider) {
                        companyList.push(item);
                    });

                    if (companyList.length > 0) {
                        selectedCompany = companyList[0];
                        soaVm.InsuranceProviderId = selectedCompany.Id.toString();
                    }
                },
                function () {
                    alert("Error!");
                });
        }

        init();

        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.ctrlInit = ctrlInit;
        $scope.soaVm = soaVm;
        $scope.create = create;
        $scope.submitCreate = submitCreate;
        $scope.submitUpdate = submitUpdate;
        $scope.edit = edit;
        $scope.getEditActionUrl = getEditActionUrl;
        $scope.clientChanged = clientChanged;
        $scope.companyList = companyList;
        $scope.errorVar = errorVar;
    }
})();