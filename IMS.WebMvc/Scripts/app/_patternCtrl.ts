module patternCtrl {
    export interface IScope {
    }
} 

(function () {
    imsApp.controller("patternCtrl", patternCtrl);

    patternCtrl.$inject = ["$scope"];

    function patternCtrl(
        $scope: patternCtrl.IScope
    ) {
        function init() {
        }

        init();
    }

})();