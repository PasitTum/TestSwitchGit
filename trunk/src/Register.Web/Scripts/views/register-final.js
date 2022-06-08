//var app = angular.module("registerFinalModule", ["ui.bootstrap", "cspModule"]);
app.controller("registerFinalController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = EXAM_TYPE;
    //$scope.year = YEAR_NO;
    $scope.citizenID = citizenID;
    $scope.qrcodeInfo = {};
    $scope.isLoading = false;

    $scope.init = function () {
        sessionStorage.clear()
        $scope.isLoading = true;
        var url = API_ROOT + "/Register/QrcodeInfo?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.citizenID;
        var config = {};
        $http.get(url, config)
            .then(function (result) {
                $scope.qrcodeInfo = result.data;
            }
            , function (err) {
                $scope.qrcodeInfo = {};
                console.log("Save Doc Error", err);
                alert("เกิดข้อผิดพลาด");
            }).finally(function () { $scope.isLoading = false; });
    };

    $scope.init();

}]);


