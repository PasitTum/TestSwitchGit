var app = angular.module("homeModule", ["ui.bootstrap", "cspModule"]);
app.controller("homeController", ["$scope", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $CspUtils, $LookupService, $http, $filter) {
    $scope.lstTypeHome = [];
    $scope.isLoading = true;
    $scope.lstImgofType = [{ "TestTypeID": 1, "Img": "AW_OCSC_EXAM_Banner1-01.jpg" }, { "TestTypeID": 4, "Img": "AW_OCSC_EXAM_Banner1-04.jpg" }, { "TestTypeID": 11, "Img": "AW_OCSC_EXAM_Banner1-05.jpg" }, { "TestTypeID": 12, "Img": "AW_OCSC_EXAM_Banner1-06.jpg" }];

    $scope.init = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/Master/ExamTypeHomes";
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.lstTypeHome = result.data;
            }, function (err) {
                $scope.lstTypeHome = [];
            }).finally(function () { $scope.isLoading = false; });

    };

    $scope.getImgoftype = function (testTypeID) {
        var rtn = $filter('filter')($scope.lstImgofType, { TestTypeID: testTypeID });
        return rtn[0].Img;
    };

    $scope.toOptional = function () {

    };

    $scope.init();
}]);
