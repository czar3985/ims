(function () {
    imsApp.controller("formsIndexCtrl", formsIndexCtrl);
    formsIndexCtrl.$inject = ["$scope", "$timeout", "$resource"];
    function formsIndexCtrl($scope, $timeout, $resource) {
        $scope.showEntriesVal = "10";
        var $dtSearchText;
        var $dtSelectInput;
        var e = $.Event("input");
        var formEmail = {};
        var formVm;
        var forms;
        var clients;
        function ctrlInit(_formVm) {
            $scope.formVm = formVm = _formVm;
            forms = formVm.FormsList;
            clients = formVm.Clients;
            formEmail.ClientId = clients[0].Id.toString();
        }
        function init() {
            $(function () {
                $("#menu-form").addClass('active');
                $dtSelectInput = $("#forms-list-table select");
                $dtSearchText = $("#forms-list-table input[type=search]");
            });
            resetFormEmail();
        }
        function showEntriesChanged() {
            $dtSelectInput.val($scope.showEntriesVal);
            $dtSelectInput.change();
        }
        function searchChanged() {
            $dtSearchText.val($scope.searchText);
            $dtSearchText.trigger(e);
        }
        var idToDelete = 0;
        function deleteForm(id) {
            idToDelete = id;
        }
        function confirmDelete() {
            $resource(util.getPrePath() + '/api/FormApi').delete({ id: idToDelete }, function (data) {
                location.reload(true);
            }, function () {
                alert("Error!");
            });
        }
        function resetFormEmail() {
            formEmail.FormId = 0;
            formEmail.ClientId = 0;
            formEmail.Email = "";
            formEmail.OtherEmail = "";
            formEmail.Subject = "";
            formEmail.Body = "Requested File. This is a system generated email. Please do not reply to this email.";
        }
        var sendModal = {
            sendWait: false
        };
        function emailForm(id) {
            forms.forEach(function (item) {
                if (item.Id === id) {
                    formEmail.FileName = item.FileName;
                    formEmail.FormId = item.Id;
                    formEmail.FileUrl = item.FileUrl;
                    formEmail.Subject = item.CompanyName + " " + item.DocumentTypeName;
                }
            });
        }
        function confirmEmail() {
            sendModal.sendWait = true;
            $resource(util.getPrePath() + '/api/FormApi').save(formEmail, function (data) {
                resetFormEmail();
                location.reload(true);
                sendModal.sendWait = false;
            }, function () {
                alert("Error!");
                sendModal.sendWait = false;
            });
        }
        init();
        $scope.ctrlInit = ctrlInit;
        $scope.searchChanged = searchChanged;
        $scope.showEntriesChanged = showEntriesChanged;
        $scope.deleteForm = deleteForm;
        $scope.confirmDelete = confirmDelete;
        $scope.sendModal = sendModal;
        $scope.formEmail = formEmail;
        $scope.emailForm = emailForm;
        $scope.confirmEmail = confirmEmail;
    }
})();
//# sourceMappingURL=formsIndexCtrl.js.map