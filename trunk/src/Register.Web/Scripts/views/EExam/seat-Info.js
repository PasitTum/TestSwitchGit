var app = angular.module("seatInfoModule", ["ui.bootstrap", "cspModule"]);
app.controller("seatInfoController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.isLoading = true;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
    };

    $scope.submit = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            $scope.show = true;
            $scope.isInfo = false;
        } else {
            $scope.show = false;
            $scope.isInfo = true;
        }
    };

    $scope.init();

}]);


