(function () {
    imsApp.controller("soaIndexCtrl", soaIndexCtrl);
    soaIndexCtrl.$inject = ["$scope", "$timeout", "$resource"];
    function soaIndexCtrl($scope, $timeout, $resource) {
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var e = $.Event("input");
        var soaVm = {
            Id: 0,
            ClientId: "0"
        };
        var errorVar = {
            hasError: false,
            errorMessage: ""
        };
        var soas;
        var companyList = new Array();
        var selectedCompany;
        var returnUrl = util.getPrePath() + "/Soa/Index";
        function ctrlInit(_vm) {
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
        function showEntriesChanged() {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }
        function searchChanged() {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }
        function create() {
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
        function submitCreate(form) {
            if (form.$valid) {
                if (soaVm.InsuranceProviderId === "0" ||
                    soaVm.ClientId === "0") {
                    return;
                }
                $resource(util.getPrePath() + '/api/SoaApi').save(soaVm, function (data) {
                    location.replace(returnUrl);
                }, function (data) {
                    errorVar.hasError = true;
                    errorVar.errorMessage = data.data.errorMessage;
                });
            }
        }
        function submitUpdate(form) {
            if (form.$valid) {
                if (soaVm.InsuranceProviderId === "0" ||
                    soaVm.ClientId === "0") {
                    return;
                }
                var resource = $resource(util.getPrePath() + '/api/SoaApi', null, {
                    'patch': { method: 'PATCH' }
                });
                resource.patch(soaVm, function (data) {
                    location.replace(returnUrl);
                }, function (data) {
                    errorVar.hasError = true;
                    errorVar.errorMessage = data.data.errorMessage;
                });
            }
        }
        function getEditActionUrl() {
            return util.getPrePath() + "/Soa/Edit/" + soaVm.Id;
        }
        function edit(id) {
            soas.forEach(function (item) {
                if (item.Id == id) {
                    soaVm.Id = item.Id;
                    var dateObject = stringToUTCDate(item.IssueDate.toString());
                    ;
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
        function stringToUTCDate(dateStr) {
            var dateTime = dateStr.split("T");
            var dateSplit = dateTime[0].split("-");
            var timeSplit = dateTime[1].split(":");
            var utcDate = new Date(parseInt(dateSplit[0]), parseInt(dateSplit[1]) - 1, parseInt(dateSplit[2]), parseInt(timeSplit[0]), parseInt(timeSplit[1]));
            return utcDate;
        }
        function convertUTCDateToLocalDate(date) {
            var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);
            var offset = date.getTimezoneOffset() / 60;
            var hours = date.getHours();
            newDate.setHours(hours - offset);
            return newDate;
        }
        function clientChanged() {
            $resource(util.getPrePath() + '/api/CompanyApi/GetCompanyListPerClient').query({ id: $scope.soaVm.ClientId }, function (data) {
                companyList.splice(0, companyList.length);
                data.forEach(function (item) {
                    companyList.push(item);
                });
                if (companyList.length > 0) {
                    selectedCompany = companyList[0];
                    soaVm.InsuranceProviderId = selectedCompany.Id.toString();
                }
            }, function () {
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
//# sourceMappingURL=soaIndexCtrl.js.map