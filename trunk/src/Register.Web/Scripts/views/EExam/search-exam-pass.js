var app = angular.module("searchExamPassModule", ["ui.bootstrap", "cspModule"]);
app.controller("searchExamPassController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.isLoading = true;
    $scope.showPass = false;
    $scope.showFail = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
    };

    $scope.submit = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            if ($scope.model.CitizenID == "1111111111111") {
                $scope.showPass = false;
                $scope.showFail = true;
                $scope.isInfo = false;
            }
            else {
                $scope.showPass = true;
                $scope.showFail = false;
                $scope.isInfo = false;
            }
        } else {
            $scope.showPass = false;
            $scope.showFail = false;
            $scope.isInfo = true;
        }
    };

    $scope.init();

}]);


