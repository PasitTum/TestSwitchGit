var app = angular.module("registerModule", ["ui.bootstrap", "cspModule"]);
//var dlgModule = angular.module("dlgModule", ["cspModule"]);
app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    //$scope.ok = function () {
    //    $scope.$close(true);
    //};
}]);

//var dlgStepModule = angular.module("dlgStepModule", ["cspModule"]);
app.controller("dialogStepController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    $scope.ok = function () {
        $scope.$close(true);
    };
}]);
app.controller("registerController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $uibModal, $CspUtils, $LookupService, $http, $filter) {
    $scope.prefixes = [];
    $scope.isLoading = true;
    $scope.model = {};


    $scope.openDialogStep = function () {
        var modalInstance = $uibModal.open({
            templateUrl: REGISTER_DIALOG_URL,
            controller: "dialogStepController",
            size: "xl"
        });

        modalInstance.result.then(function (result) {
            //$scope.model.SMSService = result;
        }, function () {
            //$scope.model.SMSService = false;
            window.location.replace(HOME_URL)
        });
    };

    $scope.openDialogStep();

    $scope.init = function () {
        $scope.getPrefixes();
    };

    $scope.getPrefixes = function () {
        $scope.isLoading = true;
        $LookupService.getLookupPrefixes().then(function (result) {
            $scope.prefixes = result.data;
        }, function (err) {
            console.log("Error on getLookupPrefixes", err);
            $scope.prefixes = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.setFocus = function (elementid) {
        $CspUtils.SetFocus(elementid);
    };

    $scope.openDialog = function () {
        var modalInstance = $uibModal.open({
            templateUrl: DIALOG_URL,
            controller: "dialogController"
        });

        modalInstance.result.then(function (result) {
            //$scope.model.SMSService = result;
        }, function () {
            //$scope.model.SMSService = false;
        });
    };

    $scope.init();

}]);


