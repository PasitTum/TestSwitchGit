//var dlgModule = angular.module("dlgModule", ["cspModule"]);
var app = angular.module("registerModule", ["ui.bootstrap", "cspModule"]);
app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    $scope.ok = function () {
        $scope.$close(true);
    };
}]);
app.controller("registerController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $uibModal, $CspUtils, $LookupService, $http, $filter) {
    $scope.testTypeID = 2;
    $scope.register = {};
    $scope.remarks = [];
    $scope.examCenters = [];
    $scope.lstProvinces = [];
    $scope.prefixes = [];
    $scope.genders = [];
    $scope.religions = [];
    $scope.statuses = [];
    $scope.lstEducationals = [];
    $scope.lstDegrees = [];
    $scope.lstSpcs = [{ "id": "01", "text": "ตาบอด" }];
    $scope.lstOccupation = [];
    $scope.lstAmphs = [];
    $scope.lstTmbls = [];
    $scope.showConfirm = false;
    $scope.showRegister = true;
    $scope.isLoading = true;

    $scope.init = function () {
        $scope.getExamCenter();
        $scope.getProvinces();
        $scope.getPrefixes();
        $scope.getGenders();
        $scope.getReligions();
        $scope.getStatuses();
        $scope.getRemark();
        $scope.getEducationals();
        $scope.getOccupation();
    };

    $scope.submit = function () {
        $scope.showConfirm = true;
        $scope.showRegister = false;
    };

    $scope.edit = function () {
        $scope.showConfirm = false;
        $scope.showRegister = true;
    };

    $scope.confirmRegister = function () {
        var url = SAVE_URL;
        var config = {};

        $http.post(url, $scope.register, config)
            .then(function (result) {

            }
            , function (err) {
                console.log("Save Doc Error", err);
                alert(result.data.ErrorMessage);
            });
    };


    $scope.getRemark = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/Master/GetRemark";
        $http.get(url, { cache: false }).then(
            function (result) {
                //console.log(result.data);
                $scope.remarks = result.data;
                $scope.isLoading = false;
            }, function (err) {
                $scope.remarks = [];
                $scope.isLoading = false;
            });
    };

    $scope.getExamCenter = function () {
        $scope.isLoading = true;
        $LookupService.getLookupExamCenters($scope.testTypeID).then(function (result) {
            $scope.examCenters = result.data;
        }, function (err) {
            console.log("Error on getLookupExamCenters", err);
            $scope.examCenters = [];
        }).finally(function () { $scope.loading = false; });
    };


    $scope.getProvinces = function () {
        $scope.isLoading = true;
        $LookupService.getLookupProvince().then(function (result) {
            $scope.lstProvinces = result.data;
        }, function (err) {
            console.log("Error on getLookupProvince", err);
            $scope.lstProvinces = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.provinceChange = function () {
        $scope.model.Amph = "";
        $scope.amphurChange();
        if (!$CspUtils.IsNullOrEmpty($scope.model.Prov)) {
            $scope.isLoading = true;
            $LookupService.getLookupAmphur($scope.model.Prov).then(function (result) {
                $scope.lstAmphs = result.data;
            }, function (err) {
                console.log("Error on getLookupAmphur", err);
                $scope.lstAmphs = [];
            }).finally(function () { $scope.loading = false; });
        } else {
            $scope.lstAmphs = [];
        }
    };

    $scope.amphurChange = function () {
        $scope.model.Tmbl = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.Amph)) {
            $scope.isLoading = true;
            $LookupService.getLookupTumbon($scope.model.Amph).then(function (result) {
                $scope.lstTmbls = result.data;
            }, function (err) {
                console.log("Error on getLookupTumbon", err);
                $scope.lstTmbls = [];
            }).finally(function () { $scope.loading = false; });
        } else {
            $scope.lstTmbls = [];
        }
    };

    $scope.getPrefixes = function () {
        $scope.isLoading = true;
        $LookupService.getLookupPrefixes().then(function (result) {
            $scope.prefixes = result.data;
        }, function (err) {
            console.log("Error on getLookupPrefixes", err);
            $scope.prefixes = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.getGenders = function () {
        $scope.isLoading = true;
        $LookupService.getLookupGender().then(function (result) {
            $scope.genders = result.data;
            console.log($scope.genders);
        }, function (err) {
            console.log("Error on getLookupGender", err);
            $scope.genders = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.getReligions = function () {
        $scope.isLoading = true;
        $LookupService.getLookupReligions().then(function (result) {
            $scope.religions = result.data;
        }, function (err) {
            console.log("Error on getLookupReligions", err);
            $scope.religions = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.getStatuses = function () {
        $scope.isLoading = true;
        $LookupService.getLookupStatuses().then(function (result) {
            $scope.statuses = result.data;
        }, function (err) {
            console.log("Error on getLookupStatuses", err);
            $scope.statuses = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.getEducationals = function () {
        $scope.isLoading = true;
        $LookupService.getLookupEducationals($scope.testTypeID).then(function (result) {
            $scope.lstEducationals = result.data;
        }, function (err) {
            console.log("Error on getLookupEducationals", err);
            $scope.lstEducationals = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.educationalChange = function () {
        $scope.getDegrees();
    };

    $scope.getDegrees = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.EducationalID)) {
            $scope.isLoading = true;
            $LookupService.getLookupDegrees($scope.model.EducationalID).then(function (result) {
                $scope.lstDegrees = result.data;
            }, function (err) {
                console.log("Error on getLookupDegrees", err);
                $scope.lstDegrees = [];
            }).finally(function () { $scope.loading = false; });
        } else {
            $scope.lstDegrees = [];
        }
    };

    $scope.getOccupation = function () {
        $scope.isLoading = true;
        $LookupService.getLookupOccupation().then(function (result) {
            $scope.lstOccupation = result.data;
        }, function (err) {
            console.log("Error on getLookupOccupation", err);
            $scope.lstOccupation = [];
        }).finally(function () { $scope.loading = false; });
    };

    $scope.getNameFromID = function (lst, id) {
        if (lst.length > 0 && !$CspUtils.IsNullOrEmpty(id)) {
            var name = $filter('filter')(lst, {
                id: id
            });
            return name[0].text;
        }
    };

    $scope.setFocus = function (elementid) {
        $CspUtils.SetFocus(elementid);
    };

    $scope.smsServiceChange = function () {
        if ($scope.model.SMSService) {
            $scope.openDialog();
        }
    };

    $scope.openDialog = function () {
        var modalInstance = $uibModal.open({
            templateUrl: DIALOG_URL,
            controller: "dialogController",
            size: "xl"
        });

        modalInstance.result.then(function (result) {
            $scope.model.SMSService = result;
        }, function () {
            $scope.model.SMSService = false;
        });
    };

    $scope.init();

}]);


