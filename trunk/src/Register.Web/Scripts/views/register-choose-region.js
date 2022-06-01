app.controller("ChooseRegionControler", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examCenters = [];
    $scope.isLoading = {};

    $scope.init = function () {
        $scope.loadRegion();
    };

    $scope.loadRegion = function () {
        $scope.isLoading.examCenters = true;
        var url = API_ROOT + "/Master/TestTypeID/"+ $scope.testTypeID + "/ExamCenters" ;
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.examCenters = result.data;
            }, function (err) {
                $scope.examCenters = [];
            }).finally(function () { $scope.isLoading.examCenters = false; });
    };

    $scope.init();
}]);