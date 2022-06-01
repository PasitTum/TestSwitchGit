//var app = angular.module("searchEnrollModule", ["ui.bootstrap", "cspModule"]);
app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    //$scope.ok = function () {
    //    $scope.$close(true);
    //};
}]);

app.controller("printApplicationController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", function ($scope, $uibModal, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.enrollInfo = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
        //$("#laserCode").change(function () {
        //    var laserCode = $("#laserCode").val();
        //    if (laserCode.length > 2 && (laserCode[2] == "O" || laserCode[2] == "o")) {
        //        laserCode = laserCode.substring(0, 2) + "0" + laserCode.substring(3);
        //        $("#laserCode").val(laserCode);
        //    }
        //});
    };

    $scope.submit = function () {
        var errorList = [];
        if ($CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            errorList.push("กรุณาระบุ เลขบัตรประชาชน ให้ถูกต้อง");
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.Mobile)) {
        //    errorList.push("กรุณาระบุ เบอร์โทรศัพท์มือถือ ให้ถูกต้อง");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.LaserCode)) {
        //    errorList.push("กรุณาระบุ เลขหลังบัตรประชาชน ให้ถูกต้อง");
        //}
        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            //var msg = errorList.join('\n');
            var msg = errorList[0];
            alert(msg);
            return;
        }

        $scope.model.CaptchaInputText = $("#CaptchaInputText").val();
        $scope.model.CaptchaDeText = $("#CaptchaDeText").val();
        $scope.model.TestTypeID = $scope.testTypeID;
        $scope.isLoading = true;
        $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
        var config = {
            headers: {
                'RequestVerificationToken': $scope.antiForgeryToken,
                'X-Requested-With': 'XMLHttpRequest'
            }
        };

        var url = TEST_TYPE_ROOT + $scope.examType + "/Inquiry/ExamApplication";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.enrollInfo = result.data.Data;
                        if ($scope.enrollInfo) {
                            $scope.show = true;
                            $scope.isInfo = false;
                        }
                        else {
                            $scope.show = false;
                            $scope.isInfo = true;
                        }

                        $('#CaptchaImage').attr('src', result.data.Captcha.CaptchaImage);
                        $('#CaptchaDeText').attr('value', result.data.Captcha.CaptchaDeText);
                        $('#CaptchaInputText').val('');
                    }
                    else {
                        alert(result.data.rtn.ErrorMessage);
                        $('#CaptchaImage').attr('src', result.data.Captcha.CaptchaImage);
                        $('#CaptchaDeText').attr('value', result.data.Captcha.CaptchaDeText);
                        $('#CaptchaInputText').val('');
                        return;
                    }
                }
                , function (err) {
                    console.log("Inquiry Error", err);
                    alert("เกิดข้อผิดพลาด");
                }).finally(function () { $scope.isLoading = false; });

    };

    $scope.openDialog = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "_CitizenDialogModal.html",
            controller: "dialogController",
            size: "lg",
            backdrop: 'static'
        });

        modalInstance.result.then(function (result) {
            //result;
        }, function () {
            //false;
        });
    };

    $scope.init();

}]);


