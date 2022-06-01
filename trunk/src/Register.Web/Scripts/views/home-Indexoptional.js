var app = angular.module("newsModule", ["ui.bootstrap", "cspModule"]);
app.controller("newsController", ["$scope", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $CspUtils, $LookupService, $http, $filter) {
    $scope.news = [];
    $scope.isLoading = true;
    $scope.lstOptionals = [];
    $scope.lstImgofType = [{ "ExamType": "EEXAM", "Img": "AW_OCSC_EXAM_Banner2-02.jpg" }, { "ExamType": "PAPER", "Img": "AW_OCSC_EXAM_Banner2-01.jpg" }];
    $scope.WWW_ROOT = WWW_ROOT;

    $scope.init = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/News/SelectTypes";
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.news = result.data;
            }, function (err) {
                $scope.news = [];
            }).finally(function () { $scope.isLoading = false; });

        $scope.isLoading = true;
        var url = API_ROOT + "/Master/ExamTypeOptionals";
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.lstOptionals = result.data;
            }, function (err) {
                $scope.lstOptionals = [];
            }).finally(function () { $scope.isLoading = false; });
    };

    $scope.toRegister = function (testTypeID, examType, year) {
        if (examType == 'PAPER') {
            return PEPER_URL + "?testTypeID=" + testTypeID + "&examType=" + examType + "&yearno=" + year;
        } else {
            return EEXAM_URL + "?testTypeID=" + testTypeID + "&examType=" + examType+"&yearno="+year;
        }
    };

    $scope.getImgoftype = function (examType) {
        var rtn = $filter('filter')($scope.lstImgofType, { ExamType: examType });
        return rtn[0].Img;
    };

    $scope.init();
}]);
