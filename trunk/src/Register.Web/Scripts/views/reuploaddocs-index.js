//var app = angular.module("searchPayinModule", ["ui.bootstrap", "cspModule"]);
app.controller("newDocsUploadController", ["$scope", "CspUtils", "LookupService", "$http", '$location', '$anchorScroll', function ($scope, $CspUtils, $LookupService, $http, $location, $anchorScroll) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";//EXAM_TYPE;
    //$scope.year = YEAR_NO;
    $scope.lstClasses = [];
    $scope.lstClassGroups = [];
    $scope.newDocsUpload = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.lstDay = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"];
    $scope.lstMonth = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.lstYear = ["2548", "2547", "2546", "2545", "2544", "2543", "2542", "2541", "2540", "2539", "2538", "2537", "2536", "2535", "2534", "2533", "2532", "2531", "2530", "2529", "2528", "2527", "2526", "2525", "2524", "2523", "2522", "2521", "2520", "2519", "2518", "2517", "2516", "2515", "2514", "2513", "2512", "2511", "2510", "2509", "2508", "2507", "2506", "2505", "2504"];
    $scope.model.BirthDateDay = "--";
    $scope.model.BirthDateMonth = "--";
    $scope.model.BirthDateYear = "0000";
    $scope.model.BirthDateChar = "";

    $scope.init = function () {
        //$scope.getClassGroups();
        $scope.classGroupChange();
    };

    $scope.validateAge = function () {
        if ($scope.model.BirthDateYear != "0000") {
            $scope.model.BirthDateChar = $scope.model.BirthDateYear + ($scope.model.BirthDateMonth == "--" ? "00" : $scope.model.BirthDateMonth) + ($scope.model.BirthDateDay == "--" ? "00" : $scope.model.BirthDateDay);
        }
    };

    //$scope.getClassGroups = function () {
    //    $scope.isLoading = true;
    //    $scope.lstClassGroups = [];
    //    $LookupService.getLookupClassGroups($scope.testTypeID).then(function (result) {
    //        $scope.lstClassGroups = result.data;
    //        if ($scope.lstClassGroups.length == 1) {
    //            $scope.model.TestingClassID = $scope.lstClassGroups[0].id;
    //        }
    //        $scope.classGroupChange();
    //    }, function (err) {
    //        console.log("Error on getLookupClassGroups", err);
    //    }).finally(function () { $scope.isLoading = false; });
    //};

    $scope.classGroupChange = function () {
        $scope.lstClasses = [];
        $scope.model.TestingClassID = "";
        $scope.model.RegClassName = "";
        //if (!$CspUtils.IsNullOrEmpty($scope.model.ClassGroupID)) {

        $scope.SetLoading("lstClasses", true);
        $LookupService.getLookupClasses($scope.testTypeID, -1).then(function (result) {
            $scope.lstClasses = result.data;
            if ($scope.lstClasses.length == 1) {
                $scope.model.TestingClassID = $scope.lstClasses[0].id;
            }

        }, function (err) {
            console.log("Error on getLookupClasses", err);
        }).finally(function () { $scope.SetLoading("lstClasses", false); });
        //}
    };

    $scope.submit = function () {
        $scope.show = false;
        $scope.isInfo = false;

        var errorList = [];
        if ($CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            errorList.push("กรุณาระบุ เลขบัตรประชาชน ให้ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.BirthDateChar)) {
            errorList.push("ยังไม่ได้ระบุ วัน/เดือน/ปี เกิด");
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.ClassGroupID)) {
        //    errorList.push("ยังไม่ได้ระบุ กลุ่มวิชาที่สมัครสอบ");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.TestingClassID)) {
            errorList.push("ยังไม่ได้ระบุ ตำแหน่งที่สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Mobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
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
            cache: false,
            headers: {
                'RequestVerificationToken': $scope.antiForgeryToken,
                'X-Requested-With': 'XMLHttpRequest'
            }
        };

        var url = TEST_TYPE_ROOT + $scope.examType + "/ReUploadDocs/Index";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.newDocsUpload = result.data.Data;
                        //console.log($scope.newDocsUpload);
                        if ($scope.newDocsUpload) {
                            $scope.show = true;
                            $scope.isInfo = false;
                            $scope.reloadDocumentList();
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
                        alert(result.data.rtn.ErrorMessage).then(() => {
                            $('#CaptchaInputText').focus();
                        });;
                        $('#CaptchaImage').attr('src', result.data.Captcha.CaptchaImage);
                        $('#CaptchaDeText').attr('value', result.data.Captcha.CaptchaDeText);
                        $('#CaptchaInputText').val('');
                        return;
                    }
                }
                , function (err) {
                    alert("เกิดข้อผิดพลาด" + err.ErrorMessage).then(() => {
                        $('#CaptchaInputText').focus();
                    });;

                }).finally(function () { $scope.isLoading = false; });
    };

    $scope.submitUpload = function (e) {
        e.preventDefault();
        $scope.checked = true;
        var errorList = [];
        var docNotUploadList = [];

        if ($scope.uploadChild.documentList && $scope.uploadChild.documentList.length > 0) {
            for (var j = 0; j < $scope.uploadChild.documentList.length; j++) {
                if ($scope.uploadChild.documentList[j].REQUIRE_FLAG == "Y"
                    && $scope.uploadChild.documentList[j].UPLOAD_STATUS != "Y") {
                    errorList.push("<li>" + $scope.uploadChild.documentList[j].DOC_TYPE_NAME + "</li>");
                }
                if (!$CspUtils.IsNullOrEmpty($scope.uploadChild.documentList[j].DOC_GUID_ORG)
                    && $scope.uploadChild.documentList[j].UPLOAD_STATUS != "Y") {
                    docNotUploadList.push("<li>" + $scope.uploadChild.documentList[j].DOC_TYPE_NAME + "</li>");
                }
            }
        }

        if (errorList.length > 0) {
            var msg = errorList.slice(0, 10).join('');
            e.preventDefault();
            $scope.checked = false;
            var options = {
                onAfterClose: () => {
                    var invalidObject = angular.element(document.querySelector("input.ng-invalid,select.ng-invalid"))[0];
                    if (invalidObject) invalidObject.focus();
                }
            };
            msg = "<b>ท่านยังอัปโหลดเอกสารไม่ครบ<br/>กรุณาอัปโหลดเอกสารต่อไปนี้</b>"
                    + "<div class='text-left'><ul style='list-style-position:outside;'>" + msg + "</ul></div>";
            alertWarning(msg, "", options);
            return;
        }

        if (docNotUploadList.length > 0) {
            var msg = docNotUploadList.slice(0, 10).join('');
            msg = "<div class='text-left'><ul style='list-style-position:outside;'>" + msg + "</ul></div>"
                    + "<b>หากท่านไม่ต้องการอัปโหลดเอกสารต่อไปนี้<br/>กรุณากดปุ่มยืนยัน<br/>หรือ กดปุ่มไม่ยืนยัน เพื่อแก้ไขข้อมูลให้ถูกต้อง</b>";

            cspConfirm(msg, 'ในครั้งนี้ท่านไม่ต้องการอัปโหลด<br/>เอกสารต่อไปนี้ ใช่หรือไม่ ?', 'ยืนยัน', 'ไม่ยืนยัน')
                .then((result) => {
                    if (result.isConfirmed) {
                        $scope.confirmUpload();
                    } else {
                        $scope.checked = false;
                        $scope.$apply();
                    }
                });
        } else {
            $scope.confirmUpload();
        }
    };

    $scope.confirmUpload = function (e) {
        cspConfirm('โปรดตรวจสอบเอกสารในแต่ละหัวข้อให้ถูกต้อง<br />ก่อนกดยืนยันอัปโหลดเอกสาร', 'ยืนยันอัปโหลดเอกสาร', 'ยืนยันอัปโหลดเอกสาร', 'แก้ไข')
            .then((result) => {
                if (result.isConfirmed) {
                    $scope.isLoading = true;
                    var url = TEST_TYPE_ROOT + $scope.examType + "/ReUploadDocs/ConfirmUpload";
                    $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
                    var config = {
                        headers: {
                            'RequestVerificationToken': $scope.antiForgeryToken,
                            'X-Requested-With': 'XMLHttpRequest'
                        }
                    };
                    $scope.model.EnrollNo = $scope.newDocsUpload.ENROLL_NO;
                    $scope.model.DOCS = $scope.uploadChild.documentList;
                    $http.post(url, $scope.model, config)
                            .then(function (result) {
                                if (result.data.rtn.Success) {
                                    $scope.newDocsUpload = result.data.Data;
                                    //console.log($scope.newDocsUpload);
                                    if ($scope.newDocsUpload) {
                                        $scope.show = true;
                                        $scope.isInfo = false;
                                        alertSuccess("บันทึกข้อมูลสำเร็จ").then(() => {
                                            $scope.reloadDocumentList();
                                        });;
                                    }
                                    else {
                                        $scope.show = false;
                                        $scope.isInfo = true;
                                    }
                                } else {
                                    //console.log("Insert Error", result.data.rtn.ErrorMessage);
                                    alertError("บันทึกข้อมูลไม่สำเร็จ<br/>" + result.data.rtn.ErrorMessage);
                                }
                            }
                            , function (err) {
                                alertError("เกิดข้อผิดพลาด");
                            }).finally(function () { $scope.isLoading = false; $scope.checked = false; });
                } else {
                    $scope.checked = false;
                    $scope.$apply();
                }
            });
    }

    $scope.reloadDocumentList = function () {
        $scope.$broadcast('reload-documentlist', {
            "citizenID": $scope.model.CitizenID,
            "mode": "Upload",
            "model": $scope.model
        });       // broadcast เพื่อให้อัปโหลดเอกสารมัน refresh ตัวเอง

        //var element = $("#boxResult");
        //angular.element(element).animate({ scrollTop: element.offset().top }, "slow")

        $location.hash('boxResult');
        $anchorScroll();
    };

    $scope.showNewLine = function (text) {
        if (text) {
            return text.replace(/(\\r)?\\n/g, '<br />')
        }
        return text;
    }

    $scope.getLoading = function (key) {
        key = key.toLowerCase();
        if (($scope.loadingx) && ($scope.loadingx[key])) {
            return $scope.loadingx[key];
        }
        return false;
    };

    $scope.SetLoading = function (key, value) {
        key = key.toLowerCase();
        if (!($scope.loadingx)) {
            $scope.loadingx = [];
        }
        $scope.loadingx[key] = value;
    };

    $scope.init();

}]);

app.filter("unique", function () {
    return function (collection, keyname) {
        var output = [],
          keys = [];
        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };
});
