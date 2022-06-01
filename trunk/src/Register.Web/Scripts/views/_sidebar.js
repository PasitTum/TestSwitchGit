var app = angular.module("registerApp", ["ui.bootstrap", "cspModule", "ngSanitize"]);
app.controller("sideBarController", ["$scope", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $CspUtils, $LookupService, $http, $filter) {
    $scope.isLoading = true;
    $scope.lstSchedules = [];
    $scope.testtypeID = TEST_TYPE_ID;

    $scope.init = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/Master/TestTypeID/" + $scope.testtypeID + "/Schedules";
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.lstSchedules = result.data;
            }, function (err) {
                $scope.lstSchedules = [];
            }).finally(function () { $scope.isLoading = false; });
    };

    $scope.IsScheduleOpen = function (scheduleTypeCode) {
        if ($scope.lstSchedules.length > 0) {
            var rtn = $scope.lstSchedules.find(x => x.SCHEDULE_TYPE_CODE === scheduleTypeCode);
            if (rtn) {
                return (rtn.IS_USED == "Y");
            } else {
                return false;
            }
        }
    };

    $scope.init();
}]);