//var app = angular.module("searchExamPassModule", ["ui.bootstrap", "cspModule"]);
app.controller("searchExamPassController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.examPassInfo = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
    };

    $scope.submit = function () {
        var errorList = [];
        if ($CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            errorList.push("กรุณาระบุ เลขบัตรประชาชน ให้ถูกต้อง");
        }
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

        var url = TEST_TYPE_ROOT + $scope.examType + "/Announce/PassScore";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.examPassInfo = result.data.Data;
                        if ($scope.examPassInfo) {
                            if ($scope.examPassInfo.EXAM_RESULT == 'ผ่าน') {
                                $scope.showPass = true;
                                $scope.showFail = false;
                                $scope.isInfo = false;
                            }
                            else {
                                $scope.showPass = false;
                                $scope.showFail = true;
                                $scope.isInfo = false;
                            }
                        }
                        else {
                            $scope.showPass = false;
                            $scope.showFail = false;
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

    $scope.init();

}]);


