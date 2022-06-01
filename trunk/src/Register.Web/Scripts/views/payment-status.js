//var app = angular.module("searchPayinModule", ["ui.bootstrap", "cspModule"]);
app.controller("searchPayinController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";//EXAM_TYPE;
    //$scope.year = YEAR_NO;
    $scope.paymentStatus = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.init = function () {
    };

    $scope.submit = function () {
        $scope.show = false;
        $scope.isInfo = false;

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

        var url = TEST_TYPE_ROOT + $scope.examType + "/Inquiry/PaymentStatus";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.paymentStatus = result.data.Data;
                        if ($scope.paymentStatus) {
                            $scope.show = true;
                            $scope.isInfo = false;
                            if ($scope.paymentStatus.PAYIN_BUTTON_FLAG == 'Y') {
                                $scope.getQrCode();
                            }
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

    $scope.getQrCode = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/Register/QrcodeInfo?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
        var config = {};
        $http.get(url, config)
            .then(function (Result) {
                $scope.qrcodeInfo = Result.data;
            }
            , function (err) {
                $scope.qrcodeInfo = {};
                console.log("Save Doc Error", err);
                alert("เกิดข้อผิดพลาด");
            }).finally(function () { $scope.isLoading = false; });
    };

    $scope.init();

}]);


