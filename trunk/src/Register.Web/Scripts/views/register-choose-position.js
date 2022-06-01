app.controller("ChoosePositionControler", ["$scope", "CspUtils", "LookupService", "$http"
, function ($scope, $CspUtils, $LookupService, $http) {

    $scope.testTypeID = TEST_TYPE_ID;
    $scope.centerID = CENTER_ID;
    $scope.centerName = CENTER_NAME;
    $scope.TestingClasses = [];
    $scope.isLoading = {};
    $scope.classGroups = [];
    $scope.testingInfo = {};

    $scope.init = function () {
        $scope.loadTestingClasses();
    };

    $scope.loadTestingClasses = function () {
        $scope.isLoading.TestingClasses = true;
        $scope.classGroups = [];
        $scope.testingClasses = [];
        $scope.testingInfo = {};

        var url = API_ROOT + "/Master/TestTypeID/" + $scope.testTypeID
                + "/ExamCenters/" + $scope.centerID;
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.testingInfo = result.data;
            }, function (err) {
                $scope.testingInfo = {};
            }).finally(function () { $scope.isLoading.testingClasses = false; });

        url = API_ROOT + "/Master/TestTypeID/" + $scope.testTypeID
                + "/ExamCenters/" + $scope.centerID + '/TestingClasses';
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.testingClasses = result.data;
                $scope.classGroups = $CspUtils.CreateUniqueArray($scope.testingClasses, "CLASS_GROUP_ID", "CLASS_GROUP_NAME_TH", "CLASS_LEVEL_NAME_TH");
            }, function (err) {
                $scope.testingClasses = [];
            }).finally(function () { $scope.isLoading.testingClasses = false; });

    };

    $scope.init();
}]);