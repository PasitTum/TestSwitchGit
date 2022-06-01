//var app = angular.module("searchExamScoreModule", ["ui.bootstrap", "cspModule"]);
app.controller("searchExamScoreController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.scorInfo = {};
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
        if ($CspUtils.IsNullOrEmpty($scope.model.Mobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์มือถือ ให้ถูกต้อง");
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

        var url = TEST_TYPE_ROOT + $scope.examType + "/Announce/Score";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.scorInfo = result.data.Data;
                        //console.log($scope.scorInfo);
                        if ($scope.scorInfo) {
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

    $scope.init();

}]);


