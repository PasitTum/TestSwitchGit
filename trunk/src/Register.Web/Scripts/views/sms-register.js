//var app = angular.module("smsRegisterModule", ["ui.bootstrap", "cspModule"]);
app.controller("smsRegisterController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", function ($scope, $uibModal, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.smsStatus = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.isRegisted = true;
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

        var url = TEST_TYPE_ROOT + $scope.examType + "/SMS/Register";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.smsStatus = result.data.Data;
                        if ($scope.smsStatus) {
                            if ($scope.smsStatus.SMS_STATUS == 'Y') {
                                $scope.isRegisted = true;
                            } else {
                                $scope.isRegisted = false;
                            }
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

    $scope.smsRegis = function () {
        var errorList = [];
        if ($CspUtils.IsNullOrEmpty($scope.model.SMSMobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
        }
        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            //var msg = errorList.join('\n');
            var msg = errorList[0];
            alert(msg);
            return;
        }

        cspConfirm('ยืนยันสมัครรับข่าวสารทาง SMS')
            .then((result) => {
                if (result.value) {
                    var modelData = {
                        "TestTypeID": $scope.testTypeID,
                        "FName": $scope.smsStatus.FNAME,
                        "LName": $scope.smsStatus.LNAME,
                        "CitizenID": $scope.model.CitizenID,
                        "PhoneNo": $scope.model.SMSMobile
                    }
                    //$scope.openDialog();
                    $scope.isLoading = true;
                    var url = API_ROOT + "/SMS/SMSRegister";
                    $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
                    var config = {
                        headers: {
                            'RequestVerificationToken': $scope.antiForgeryToken,
                            'X-Requested-With': 'XMLHttpRequest'
                        }
                    };
                    //console.log(modelData);
                    $http.post(url, modelData, config)
                        .then(function (result) {
                            //console.log(result.data);
                            if (result.data.Success) {

                                $scope.isRegisted = true;
                                url = API_ROOT + "/SMS/SMSStatus?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
                                var config = {};
                                $http.get(url, config)
                                    .then(function (subResult) {
                                        $scope.smsStatus = subResult.data;
                                        if ($scope.smsStatus) {
                                            if ($scope.smsStatus.SMS_STATUS == 'Y') {
                                                $scope.isRegisted = true;
                                            } else {
                                                $scope.isRegisted = false;
                                            }
                                            $scope.show = true;
                                            $scope.isInfo = false;
                                        }
                                        else {
                                            $scope.show = false;
                                            $scope.isInfo = true;
                                        }
                                    }
                                    , function (err) {
                                        $scope.smsStatus = {};
                                        console.log("Save Doc Error", err);
                                        alert("เกิดข้อผิดพลาด");
                                    }).finally(function () { $scope.isLoading = false; });

                            } else {
                                $scope.isRegisted = false;
                                alert(result.data.ErrorMessage);
                            }
                        }
                        , function (err) {
                            console.log("Save Doc Error", err);
                            alert("เกิดข้อผิดพลาด");
                            $scope.$dismiss();
                        }).finally(function () { $scope.isLoading = false; });
                }
            });
    };

    //$scope.openDialog = function () {
    //    var modalInstance = $uibModal.open({
    //        templateUrl: "_SMSDialogModal.html",
    //        controller: "dialogController",
    //        size: "xl",
    //        resolve: {
    //            params: function () {
    //                return {
    //                    model: $scope.model
    //                };
    //            }
    //        }
    //    });

    //    modalInstance.result.then(function (result) {
    //        if (result.Success) {
    //            $scope.isRegisted = true;
    //            var url = API_ROOT + "/SMS/SMSStatus?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
    //            var config = {};
    //            $http.get(url, config)
    //                .then(function (subResult) {
    //                    $scope.smsStatus = subResult.data;
    //                    if ($scope.smsStatus) {
    //                        if ($scope.smsStatus.SMS_STATUS == 'Y') {
    //                            $scope.isRegisted = true;
    //                        } else {
    //                            $scope.isRegisted = false;
    //                        }
    //                        $scope.show = true;
    //                        $scope.isInfo = false;
    //                    }
    //                    else {
    //                        $scope.show = false;
    //                        $scope.isInfo = true;
    //                    }
    //                }
    //                , function (err) {
    //                    $scope.smsStatus = {};
    //                    console.log("Save Doc Error", err);
    //                    alert("เกิดข้อผิดพลาด");
    //                }).finally(function () { $scope.isLoading = false; });
    //        } else {
    //            $scope.isRegisted = false;
    //            alert(result.ErrorMessage);
    //        }
    //    }, function () {
    //        //
    //    });
    //};

    $scope.init();

}]);

//app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
//    var params = $scope.$resolve.params;
//    $scope.model = params.model;
//    $scope.model.ConsentDate = '';
//    $scope.isLoading = false;

//    $scope.cancel = function () {
//        $scope.$dismiss();
//    };

//    $scope.ok = function () {
//        $scope.isLoading = true;
//        var url = API_ROOT + "/Register/DateTime";
//        var config = {};
//        $http.get(url, config)
//            .then(function (result) {
//                $scope.model.consentDate = result.data;
//                $scope.isLoading = true;
//                var url = API_ROOT + "/SMS/SMSRegister";
//                $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
//                var config = {
//                    headers: {
//                        'RequestVerificationToken': $scope.antiForgeryToken,
//                        'X-Requested-With': 'XMLHttpRequest'
//                    }
//                };

//                $http.post(url, $scope.model, config)
//                    .then(function (result) {
//                        if (result.data.Success) {
//                            $scope.$close(result.data);
//                        } else {
//                            $scope.$close(result.data);
//                        }
//                    }
//                    , function (err) {
//                        console.log("Save Doc Error", err);
//                        alert("เกิดข้อผิดพลาด");
//                        $scope.$dismiss();
//                    }).finally(function () { $scope.isLoading = false; });
//            }
//            , function (err) {
//                console.log("Save Doc Error", err);
//                alert("เกิดข้อผิดพลาด");
//                $scope.$dismiss();
//            }).finally(function () { $scope.isLoading = false; });
//    };
//}]);


