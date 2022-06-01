//var app = angular.module("registerModule", ["ui.bootstrap", "cspModule"]);
//var dlgModule = angular.module("dlgModule", ["cspModule"]);
app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    //$scope.ok = function () {
    //    $scope.$close(true);
    //};
}]);

app.controller("dialogStepController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    var params = $scope.$resolve.params;
    $scope.testTypeID = params.testTypeID;
    $scope.lstRegConfigs = [];
    $scope.isLoading = true;

    $scope.init = function () {
        $scope.isLoading = true;
        var url = API_ROOT + "/Master/RegConfigs/TestTypeID/" + $scope.testTypeID;
        $http.get(url, { cache: false }).then(
            function (result) {
                $scope.lstRegConfigs = result.data;
            }, function (err) {
                $scope.lstRegConfigs = [];
            }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getRegConfig = function (configKey) {
        if ($scope.lstRegConfigs.length > 0) {
            var rtn = $filter('filter')($scope.lstRegConfigs, {
                CONFIG_KEY: configKey
            }, true);
            return rtn[0].CONFIG_DESC;
        }
    };

    $scope.cancel = function () {
        $scope.$dismiss();
    };
    $scope.ok = function () {
        $scope.$close(true);
    };

    $scope.init();
}]);

app.controller("registerController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $uibModal, $CspUtils, $LookupService, $http, $filter) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = EXAM_TYPE;
    //$scope.year = YEAR_NO;
    $scope.closeEnrollDate = '';
    $scope.prefixes = [];
    $scope.age = [];
    $scope.showAges = false;
    $scope.isLoading = false;
    $scope.checked = false;
    $scope.model = oldData;
    $scope.model.TestTypeID = $scope.testTypeID;
    $scope.model.ExamType = $scope.examType;
    $scope.model.BirthDateDay = $CspUtils.IsNullOrEmpty($scope.model.BirthDateDay) ? "--" : $scope.model.BirthDateDay;
    $scope.model.BirthDateMonth = $CspUtils.IsNullOrEmpty($scope.model.BirthDateMonth) ? "--" : $scope.model.BirthDateMonth;
    $scope.model.BirthDateYear = $CspUtils.IsNullOrEmpty($scope.model.BirthDateYear) ? "0000" : $scope.model.BirthDateYear;
    $scope.model.BirthDate = "";
    $scope.model.BirthDateChar = "";
    $scope.lstDay = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"];
    $scope.lstMonth = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.lstYear = ["2548", "2547", "2546", "2545", "2544", "2543", "2542", "2541", "2540", "2539", "2538", "2537", "2536", "2535", "2534", "2533", "2532", "2531", "2530", "2529", "2528", "2527", "2526", "2525", "2524", "2523", "2522", "2521", "2520", "2519", "2518", "2517", "2516", "2515", "2514", "2513", "2512", "2511", "2510", "2509", "2508", "2507", "2506", "2505", "2504"];
    $scope.init = function () {

        $scope.openDialogStep();
        $scope.getPrefixes();
        $scope.isLoading = true;
        var url = API_ROOT + "/Register/EnrollDate?testTypeID=" + $scope.testTypeID + "&scheduleTypeCode=" + "ENROLL_CLOSE_DATE";
        var config = {};
        $http.get(url, config)
            .then(function (result) {
                $scope.closeEnrollDate = result.data;
                //console.log($scope.closeEnrollDate);
                //$scope.getYearList();
                $scope.validateAge();
            }
            , function (err) {
                $scope.closeEnrollDate = '';
                console.log("Save Doc Error", err);
                alert("เกิดข้อผิดพลาด");
            }).finally(function () { $scope.isLoading = false; });
        if (!$CspUtils.IsNullOrEmpty($scope.model.ErrorMessage)) {
            alert($scope.model.ErrorMessage);
        }

        //$("#laserCode").change(function () {
        //    var laserCode = $("#laserCode").val();
        //    if (laserCode.length > 2 && (laserCode[2] == "O" || laserCode[2] == "o")) {
        //        laserCode = laserCode.substring(0, 2) + "0" + laserCode.substring(3);
        //        $("#laserCode").val(laserCode);
        //    }
        //});

    };

    function checkID(id) {
        var result = false;
        if (id) {
            if (id.length != 13) result = false;
            var sum = 0;
            for (i = 0; i < 12; i++) {
                sum += parseFloat(id.charAt(i)) * (13 - i);
            }
            if ((11 - sum % 11) % 10 != parseFloat(id.charAt(12))) {
                result = false;
            } else {
                result = true;
            }
        }
        return result;
    }

    $scope.validateAge = function () {
        if ($scope.model.BirthDateYear != "0000") {
            $scope.model.BirthDate = ($scope.model.BirthDateDay == "--" ? "01" : $scope.model.BirthDateDay) + "/" + ($scope.model.BirthDateMonth == "--" ? "01" : $scope.model.BirthDateMonth) + "/" + $scope.model.BirthDateYear;
            $scope.model.BirthDateChar = $scope.model.BirthDateYear + ($scope.model.BirthDateMonth == "--" ? "00" : $scope.model.BirthDateMonth) + ($scope.model.BirthDateDay == "--" ? "00" : $scope.model.BirthDateDay);
        }
        if (!$CspUtils.IsNullOrEmpty($scope.model.BirthDate)) {
            var datearray = $scope.closeEnrollDate.split("/");
            var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
            var datearray2 = $scope.model.BirthDate.split("/");
            var newdate2 = datearray2[1] + '/' + datearray2[0] + '/' + datearray2[2];
            $scope.age = $CspUtils.DateDiff(newdate, newdate2);
            $scope.model.AgeYear = $scope.age[0];
            $scope.model.AgeMonth = $scope.age[1];
            $scope.model.AgeDay = $scope.age[2];
            $scope.showAges = true;
        }
    };

    $scope.getYearList = function () {
        $scope.lstYear = [];
        if (!$CspUtils.IsNullOrEmpty($scope.closeEnrollDate)) {
            var enrollDate = $scope.closeEnrollDate.split("/");
            var enrollYear = parseInt(enrollDate[2]);
            for (var i = enrollYear - 18; i > enrollYear - 61; i--) {
                $scope.lstYear.push(i.toString());
            }
        }
    };

    $scope.validateCitizen = function (e) {
        $scope.checked = true;
        var errorList = [];
        if ($CspUtils.IsNullOrEmpty($scope.model.CitizenID)) {
            errorList.push("กรุณาระบุ เลขบัตรประชาชน ให้ถูกต้อง");
        }
        if (!checkID($scope.model.CitizenID)) {
            errorList.push("กรุณาระบุ เลขบัตรประชาชน ให้ถูกต้อง");
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.LaserCode)) {
        //    errorList.push("กรุณาระบุ เลขหลังบัตรประจำตัวประชาชน ให้ถูกต้อง");
        //} else {
        //    $scope.model.LaserCode[2] = $scope.model.LaserCode[2] == 0 ? "O" : $scope.model.LaserCode[2];
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.PrefixID)) {
        //    errorList.push("ยังไม่ได้ระบุ คำนำหน้า");
        //}
        if ($scope.model.PrefixID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.PrefixName)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้าอื่นๆ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.FirstName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อตัว");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.LastName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อสกุล");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.BirthDate)) {
            errorList.push("ยังไม่ได้ระบุ วัน/เดือน/ปี เกิด");
        } else {
            if ($scope.age[0] < 18) {
                errorList.push("อายุต้องไม่ต่ำกว่า 18 ปี");
            }
        }

        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            //var msg = errorList.join('\n');
            var msg = errorList[0];
            alert(msg);
            e.preventDefault();
            $scope.checked = false;
            return;
        }

        //if (!$scope.checked) {
        //    e.preventDefault();
        //    $scope.openDialogWarning();
        //    return;
        //}
        document.getElementById("checkCitizenID").submit();
    };

    $scope.getPrefixes = function () {
        $scope.isLoading = true;
        $LookupService.getLookupPrefixes().then(function (result) {
            $scope.prefixes = result.data;
        }, function (err) {
            console.log("Error on getLookupPrefixes", err);
            $scope.prefixes = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.setFocus = function (elementid) {
        $CspUtils.SetFocus(elementid);
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

    $scope.openDialogStep = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "_RegisterStepDialogModal.html",
            controller: "dialogStepController",
            size: "xl",
            backdrop: 'static',
            resolve: {
                params: function () {
                    return {
                        testTypeID: $scope.testTypeID
                    };
                }
            }
        });

        modalInstance.result.then(function (result) {
            //result;
        }, function () {
            //false;
            window.location.replace(HOME_URL)
        });
    };

    $scope.init();

}]);


