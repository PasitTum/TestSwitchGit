//var app = angular.module("registerFinalModule", ["ui.bootstrap", "cspModule"]);
app.controller("errorRegisterController", ["$scope", "CspUtils", "LookupService", "$http", "$interval", function ($scope, $CspUtils, $LookupService, $http, $interval) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.CountDown = 10;
    var timer;
    $scope.init = function () {
        $scope.SetTimerBackHome();
    };

    $scope.SetTimerBackHome = function () {
        if (angular.isDefined(timer)) {
            $interval.cancel(timer);
            timer = undefined;
        }
        timer = $interval(function () {
            if ($scope.CountDown == 0) {
                window.location.replace(registerUrl);
            } else {
                $scope.CountDown = $scope.CountDown - 1;
            }
        }, 1000);
    }

    $scope.init();

}]);


