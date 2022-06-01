//var app = angular.module("newsAnnouncementModule", ["ui.bootstrap", "cspModule"]);
app.controller("registerIndexController", ["$scope", "CspUtils", "LookupService", "$http", "$sce", "$compile", function ($scope, $CspUtils, $LookupService, $http, $sce, $compile) {
    $scope.DOC_ROOT = DOC_ROOT;
    $scope.TEST_TYPE_ROOT = TEST_TYPE_ROOT;
    $scope.isLoading = {};
    $scope.new_flag = {};

    $scope.init = function () {

    };

    $scope.resolveNewUrl = function () {
        return TEST_TYPE_ROOT + "/Home/ReadNews";
    }

    $scope.setNewFlag = function (variable, value) {
        $scope.new_flag[variable] = value;
    }

    $scope.init();   
}]);

$('.carousel').carousel();