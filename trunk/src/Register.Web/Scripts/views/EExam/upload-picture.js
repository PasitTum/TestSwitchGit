var app = angular.module("uploadPictureModule", ["ui.bootstrap", "cspModule"]);
//Resolve "Required attribute Not working with File input"
app.directive('validFile', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ngModel) {
            //change event is fired when file is selected
            el.bind('change', function () {
                scope.$apply(function () {
                    ngModel.$setViewValue(el.val());
                    ngModel.$render();
                });
            });
        }
    }
});
//End Resolve
app.controller("uploadPictureController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.isLoading = true;
    $scope.showUpload = false;
    $scope.showSuccess = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
    };

    $scope.submit = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            if ($scope.model.CitizenID == "1111111111111") {
                $scope.showUpload = false;
                $scope.showSuccess = true;
                $scope.isInfo = false;
            }
            else {
                $scope.showUpload = true;
                $scope.showSuccess = false;
                $scope.isInfo = false;
            }
        } else {
            $scope.showUpload = false;
            $scope.showSuccess = false;
            $scope.isInfo = true;
        }
    };

    $scope.contentChanged = function (file) {
        var myFileSelected = file.files[0];
        $scope.files = myFileSelected;
        console.log("file", $scope.files.size);
        $scope.reset();
        //$scope.uploadFile();
    };

    $scope.init();

    $scope.reset = function () {
        angular.element(document.querySelector("#customFile")).val("");
    };

}]);


