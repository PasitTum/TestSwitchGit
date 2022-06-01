//var dlgModule = angular.module("dlgModule", ["cspModule"]);
//var app = angular.module("registerModule", ["ui.bootstrap", "cspModule"]);
app.controller("dialogWarningController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    var params = $scope.$resolve.params;
    $scope.model = params;

    $scope.cancel = function () {
        $scope.$dismiss();
    };
    $scope.ok = function () {
        $scope.$close();
    };
}]);

app.controller("dialogDetailController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {

    $scope.cancel = function () {
        $scope.$dismiss();
    };

    $scope.ok = function () {
        $scope.$close();
    };

}]);

app.controller("dialogController", ["$scope", "CspUtils", "$http", "$filter", function ($scope, $CspUtils, $http, $filter) {
    $scope.cancel = function () {
        $scope.$dismiss();
    };
    $scope.ok = function () {
        $scope.$close();
    };
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
//app.run(function ($http) {
//    // Sends this header with any AJAX request
//    //$http.defaults.headers.common['Access-Control-Allow-Origin'] = 'http://192.168.72.243:5000';
//    $http.defaults.headers.common['Access-Control-Allow-Origin'] = 'http://10.10.20.252';
//});

app.controller("registerController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", "$filter", "$interval", function ($scope, $uibModal, $CspUtils, $LookupService, $http, $filter, $interval) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = EXAM_TYPE;
    $scope.yearNo = '';
    $scope.register = {};
    $scope.remarks = [];
    $scope.lstProvinces = [];
    $scope.prefixes = [];
    $scope.genders = [];
    $scope.religions = [];
    $scope.statuses = [];
    $scope.lstEducationals = [];
    $scope.lstClasses = [];
    $scope.lstClassGroups = [];
    $scope.lstTeachClasses = [];
    $scope.lstHighestEducationals = [{ "id": "1", "text": "ปริญญาตรี" }, { "id": "2", "text": "ปริญญาโท" }, { "id": "3", "text": "ปริญญาเอก" }];
    $scope.schGroups = [];
    $scope.lstSchool = [];
    $scope.schLocations = [];
    $scope.lstDegrees = [];
    $scope.lstMajors = [];
    $scope.lstOCSCEducationals = [];
    $scope.lstOccupation = [];
    $scope.lstDefHelps = [];
    $scope.lstAmphs = [];
    $scope.lstTmbls = [];
    $scope.lstTbbAmphs = [];
    $scope.lstTbbTmbls = [];
    $scope.lstStdYears = [];
    $scope.lstExamSites = [];
    $scope.modelOrg = angular.copy(oldData);
    $scope.model = angular.copy(oldData);
    //console.log($scope.model);
    $scope.model.TestTypeID = $scope.testTypeID;
    $scope.model.ExamType = $scope.examType;
    $scope.age = [];
    $scope.closeEnrollDate = '';
    $scope.lstDay = ["--", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"];
    $scope.lstMonth = ["--", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.lstYear = ["2548", "2547", "2546", "2545", "2544", "2543", "2542", "2541", "2540", "2539", "2538", "2537", "2536", "2535", "2534", "2533", "2532", "2531", "2530", "2529", "2528", "2527", "2526", "2525", "2524", "2523", "2522", "2521", "2520", "2519", "2518", "2517", "2516", "2515", "2514", "2513", "2512", "2511", "2510", "2509", "2508", "2507", "2506", "2505", "2504"];
    $scope.eduFlag = $scope.model.EduFlag == 'Y' ? true : false;
    $scope.tbbFlag = $scope.model.TbbFlag == 'Y' ? true : false;
    $scope.ocscFlag = $scope.model.OCSCFlag == 'Y' ? true : false;
    $scope.smsStatus = $scope.model.SMSStatus == 'Y' ? true : false;
    $scope.occFlag = false;
    $scope.isLoading = false;
    //$scope.isInitializing = false;

    $scope.ErrorMessImage = "";
    $scope.fileSizeValid = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;
    $scope.hasFile = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;

    $scope.imageValid = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;
    $scope.waitingDetect = false;
    $scope.detecting = false;
    $scope.cropitZoomDisable = false;

    $scope.files = null;
    $scope.checked = false;
    if ($scope.model.Mode == "Register") {
        $scope.detectTimer = parseInt(detectTimer) * 1000;
        $scope.takePhoto = (btnCamera == "True" ? true : false);
    }

    $scope.errorText = ""
    var timer;

    $scope.init = function () {
        if ($scope.model.Mode == "Confirm") {
            if (!$CspUtils.IsNullOrEmpty($scope.model.ErrorMessage)) {
                alert($scope.model.ErrorMessage);
            }
        }
        $scope.isLoading = true;
        var url = API_ROOT + "/Register/EnrollDate?testTypeID=" + $scope.testTypeID + "&scheduleTypeCode=" + "ENROLL_CLOSE_DATE";
        var config = {};
        $http.get(url, config)
            .then(function (result) {
                $scope.closeEnrollDate = result.data;
                //$scope.getYearList();
            }
            , function (err) {
                $scope.closeEnrollDate = '';
                console.log("Save Doc Error", err);
                alert("เกิดข้อผิดพลาด");
            }).finally(function () { $scope.isLoading = false; });

        if ($scope.model.Mode == "Register") {
            //$scope.openDialog();

            $scope.model.RegClassID = $CspUtils.NullToString($scope.model.RegClassID);
            $scope.modelOrg.RegClassID = $CspUtils.NullToString($scope.modelOrg.RegClassID);
            $scope.model.NationName = "ไทย";

            $scope.getProvinces();
            $scope.getPrefixes();
            $scope.getReligions();
            $scope.getStatuses();
            $scope.getTeachClass();
            //$scope.getClassGroups();
            $scope.classGroupChange();
            $scope.getOccupation();
            $scope.provinceChange();
            $scope.tbbprovinceChange();
            $scope.getExamSites();

            if (!$CspUtils.IsNullOrEmpty($scope.model.OccuupationID)) {
                if ($scope.model.OccuupationID == "5" || $scope.model.OccuupationID == "4") {
                    $scope.occFlag = true;
                }
            }

            //$scope.isInitializing = true;

            //อัปโหลดรูป

            $(function () {
                var $editor = $('.image-editor');
                $editor.cropit({
                    allowDragNDrop: false,
                    maxZoom: 0.8,
                    minZoom: 'fit',
                    smallImage: 'stretch',
                    previewSize: { width: 198, height: 264 },
                    onImageLoaded: function () {
                        $scope.SetTimerImage();
                    },
                    onZoomDisabled: function () {
                        $scope.cropitZoomDisable = true;
                    },
                    onZoomChange: function () {
                        $scope.SetTimerImage();
                    },
                    onOffsetChange: function () {
                        $scope.SetTimerImage();
                    }
                });

                $('#btnCustomFile').click(function () {
                    $('.cropit-image-input').click();
                });

                //$('.rotate-cw').click(function () {
                //    $editor.cropit('rotateCW');
                //    $scope.SetTimerImage();
                //});

                //$('.rotate-ccw').click(function () {
                //    $editor.cropit('rotateCCW');
                //    $scope.SetTimerImage();
                //});

                //$('.cropit-preview-image').mouseup(function () {
                //    $scope.SetTimerImage();
                //});

                //$('.cropit-image-zoom-input').mouseup(function () {
                //    $scope.SetTimerImage();
                //});

                var captureVideoButton = document.querySelector('.capture-button');
                var screenshotButton = document.querySelector('#screenshot-button');
                var img = document.querySelector('#screenshot img');
                var video = document.querySelector('#screenshot video');

                var canvas = document.createElement('canvas');

                captureVideoButton.onclick = function () {
                    const constraints = {
                        audio: false,
                        video: {
                            width: {
                                min: 1280,
                                ideal: 1920,
                                max: 2560,
                            },
                            height: {
                                min: 720,
                                ideal: 1080,
                                max: 1440
                            },
                        },
                    };
                    if (!navigator.mediaDevices) {
                        // console.log("ไม่พบ Web Camera ในเครื่องของท่าน / หรือท่านไม่ได้อนุญาตให้ใช้งาน Web Camera บน Web Site นี้");
                        $scope.errorText = "ไม่พบ Web Camera ในเครื่องของท่าน / หรือท่านไม่ได้อนุญาตให้ใช้งาน Web Camera บน Web Site นี้";
                        $scope.$apply();
                    } else {
                        navigator.mediaDevices
                            .getUserMedia(constraints)
                            .then(function (stream) {
                                let videoIn = document.querySelector('#screenshot video');
                                videoIn.autoplay = true;
                                videoIn.srcObject = stream;
                                videoIn.play();
                            })
                            .catch(function (err) {
                                // console.log("ขออภัยเครื่องของท่านไม่รองรับการใช้งาน Web Camera", err);
                                $scope.errorText = "ขออภัยเครื่องของท่านไม่รองรับการใช้งาน Web Camera " + err;
                                $scope.$apply();
                            });
                    }
                };

                screenshotButton.onclick = video.onclick = function () {
                    canvas.width = video.videoWidth;
                    canvas.height = video.videoHeight;
                    canvas.getContext('2d').drawImage(video, 0, 0);
                    var img = canvas.toDataURL('image/png');
                    img.src = img;
                    $editor.cropit('imageSrc', img);
                    $scope.hasFile = true;
                    $scope.fileSizeValid = true;
                    $('#cameraModal').modal('toggle');
                    $scope.reset();

                    // ถ่ายเสร็จต้องปิดกล้องด้วยนะจ๊ะ
                    let videoIn = document.querySelector('#screenshot video');
                    stream = videoIn.srcObject;
                    tracks = stream.getTracks();
                    // now close each track by having forEach loop
                    tracks.forEach(function (track) {
                        // stopping every track
                        track.stop();
                    });
                    // assign null to srcObject of video
                    videoIn.srcObject = null;
                };

                function handleSuccess(stream) {
                    screenshotButton.disabled = false;
                    video.srcObject = stream;
                };

                function handleError(stream) {
                };

                if (!$CspUtils.IsNullOrEmpty($scope.model.ImageBase64)) {
                    $editor.cropit('imageSrc', $scope.model.ImageBase64);
                }

            });
            $scope.checkDevice();

        }
        //end อัปโหลดรูป
    };

    $scope.getYearList = function () {
        $scope.lstYear = [];
        if (!$CspUtils.IsNullOrEmpty($scope.closeEnrollDate)) {
            var enrollDate = $scope.closeEnrollDate.split("/");
            var enrollYear = parseInt(enrollDate[2]);
            $scope.yearNo = enrollYear;
            for (var i = enrollYear - 18; i > enrollYear - 61; i--) {
                $scope.lstYear.push(i.toString());
            }
        }
    };

    $scope.submit = function (e) {
        e.preventDefault();
        $scope.checked = true;
        var errorList = [];
        //if ($CspUtils.IsNullOrEmpty($scope.model.CitizenProvinceID)) {
        //    errorList.push("ยังไม่ได้ระบุ จังหวัดที่ออกบัตรประชาชน");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.IssueDate)) {
        //    errorList.push("ยังไม่ได้ระบุ วันที่ออกบัตรประชาชน");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.ExpiredDate)) {
        //    errorList.push("ยังไม่ได้ระบุ วันที่บัตรหมดอายุ");
        //}
        //if (!$CspUtils.IsNullOrEmpty($scope.model.IssueDate) && !$CspUtils.IsNullOrEmpty($scope.model.ExpiredDate)) {
        //    var datearray = $scope.model.IssueDate.split("/");
        //    var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
        //    var datearray2 = $scope.model.ExpiredDate.split("/");
        //    var newdate2 = datearray2[1] + '/' + datearray2[0] + '/' + datearray2[2];
        //    var dateDiff = $CspUtils.DateDiff(newdate2, newdate);
        //    if (dateDiff[0] < 0 || dateDiff[1] < 0 || dateDiff[2] < 0 || (dateDiff[0] == 0 && dateDiff[1] == 0 && dateDiff[2] == 0)) {
        //        errorList.push("กรุณาระบุ วันที่ออกบัตรประชาชน และวันที่บัตรหมดอายุ<br/>ให้ถูกต้อง");
        //    }
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.PrefixID)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้า");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.PrefixName)) {
                $("input[type=hidden][id='prefixName']").val($scope.getNameFromID($scope.prefixes, $scope.model.PrefixID));
            }
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Gender)) {
            errorList.push("ยังไม่ได้ระบุ เพศ");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.GenderName)) {
                $("input[type=hidden][id='genderName']").val($scope.getNameFromID($scope.genders, $scope.model.Gender));
            }
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.NationName)) {
        //    errorList.push("ยังไม่ได้ระบุ สัญชาติ");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.ReligionID)) {
        //    errorList.push("ยังไม่ได้ระบุ ศาสนา");
        //}
        //if ($scope.model.ReligionID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.ReligionName)) {
        //    errorList.push("ยังไม่ได้ระบุ ศาสนาอื่นๆ");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.StatusID)) {
            errorList.push("ยังไม่ได้ระบุ สถานภาพ");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.StatusName)) {
                $("input[type=hidden][id='statusName']").val($scope.getNameFromID($scope.statuses, $scope.model.StatusID));
            }
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.ClassGroupID)) {
        //    errorList.push("ยังไม่ได้ระบุ กลุ่มวิชาที่สมัครสอบ");
        //}
        //else {
        //    if ($CspUtils.IsNullOrEmpty($scope.model.ClassGroupName)) {
        //        $("input[type=hidden][id='classGroupName']").val($scope.getNameFromID($scope.lstClassGroups, $scope.model.ClassGroupID));
        //    }
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.RegClassID) || $scope.model.RegClassID == "0") {
            errorList.push("ยังไม่ได้ระบุ ตำแหน่งที่สมัครสอบ");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.RegClassName)) {
                $("input[type=hidden][id='regClassName']").val($scope.getNameFromID($scope.lstClasses, $scope.model.RegClassID));
            }
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
            errorList.push("ยังไม่ได้ระบุ วุฒิที่ใช้ในการสมัคร");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.ClassLavelName)) {
                $("input[type=hidden][id='classLavelName']").val($scope.getNameFromID($scope.lstEducationals, $scope.model.ClassLavelID));
            }
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.DegreeID)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อหลักสูตร/ปริญญาบัตร");
        }

        if ($scope.model.DegreeID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.DegreeName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อหลักสูตร/ปริญญาบัตร อื่นๆ");
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.DegreeName)) {
        //    errorList.push("ยังไม่ได้ระบุ ชื่อหลักสูตร/ปริญญาบัตร");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.MajorID)) {
            errorList.push("ยังไม่ได้ระบุ สาขาวิชา/วิชาเอก");
        }
        if ($scope.model.MajorID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.MajorName)) {
            errorList.push("ยังไม่ได้ระบุ สาขาวิชา/วิชาเอก อื่นๆ");
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.MajorName)) {
        //    errorList.push("ยังไม่ได้ระบุ สาขาวิชา/วิชาเอก");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.SchoolName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อสถานศึกษาของวุฒิที่ใช้ในการสมัคร");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.FacultyName)) {
            errorList.push("ยังไม่ได้ระบุ คณะที่จบการศึกษา");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.SchoolFlag)) {
            errorList.push("ยังไม่ได้ระบุ ที่ตั้งสถานศึกษาของวุฒิที่ใช้ในการสมัคร");
        }
        if ($scope.model.SchoolFlag == "DOM" && $CspUtils.IsNullOrEmpty($scope.model.SchoolProvinceID)) {
            errorList.push("ยังไม่ได้ระบุ จังหวัดที่ตั้งสถานศึกษาของวุฒิที่ใช้ในการสมัคร");
        }
        if ($scope.model.SchoolFlag == "FOR" && $CspUtils.IsNullOrEmpty($scope.model.SchoolProvinceName)) {
            errorList.push("ยังไม่ได้ระบุ ประเทศที่ตั้งสถานศึกษาของวุฒิที่ใช้ในการสมัคร");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.GraduateDate)) {
            errorList.push("ยังไม่ได้ระบุ วัน/เดือน/ปี ที่สำเร็จการศึกษา");
        } else {
            var datearray = $scope.model.GraduateDate.split("/");
            var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
            var datearray2 = $scope.closeEnrollDate.split("/");
            var newdate2 = datearray2[1] + '/' + datearray2[0] + '/' + datearray2[2];
            var dateDiff = $CspUtils.DateDiff(newdate2, newdate);
            if (dateDiff[0] < 0 || dateDiff[1] < 0 || dateDiff[2] < 0) {
                errorList.push("วัน/เดือน/ปี ที่สำเร็จการศึกษา<br />ต้องภายในวันปิดรับสมัคร");
            }
        }
        // ก.พ.
        //if (!$scope.ocscFlag) {
        //    errorList.push("กรุณาระบุ ว่าเป็นผู้สอบผ่านการวัดความรู้ความสามารถทั่วไป (ภาค ก.) ของสำนักงาน ก.พ.");
        //}
        //if ($scope.ocscFlag && $CspUtils.IsNullOrEmpty($scope.model.OCSCLevelID)) {
        //    errorList.push("กรุณาระบุ ระดับวุฒิการศึกษา การวัดความรู้ความสามารถทั่วไป (ภาค ก.) ของสำนักงาน ก.พ.");
        //}
        //if ($scope.ocscFlag && $CspUtils.IsNullOrEmpty($scope.model.OCSCCertNo)) {
        //    errorList.push("กรุณาระบุ เลขที่หนังสือรับรองฯ การวัดความรู้ความสามารถทั่วไป (ภาค ก.) ของสำนักงาน ก.พ.");
        //}
        //if ($scope.ocscFlag && $CspUtils.IsNullOrEmpty($scope.model.OCSCExamDate)) {
        //    errorList.push("กรุณาระบุ วันที่สอบ การวัดความรู้ความสามารถทั่วไป (ภาค ก.) ของสำนักงาน ก.พ.");
        //}
        //if ($scope.ocscFlag && $CspUtils.IsNullOrEmpty($scope.model.OCSCPassDate)) {
        //    errorList.push("กรุณาระบุ วันประกาศขึ้นทะเบียนสอบ การวัดความรู้ความสามารถทั่วไป (ภาค ก.) ของสำนักงาน ก.พ.");
        //}
        // end ก.พ.
        //if ($CspUtils.IsNullOrEmpty($scope.model.GPA)) {
        //    errorList.push("ยังไม่ได้ระบุ เกรดเฉลี่ยสะสม");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.ClassHighestLavelID)) {
        //    errorList.push("ยังไม่ได้ระบุ วุฒิการศึกษาสูงสุด");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.DiplomaFlag)) {
        //    errorList.push("ยังไม่ได้ระบุ ข้อมูลคุณวุฒิระดับอนุปริญญา");
        //}
        //if ($scope.model.DiplomaFlag == 'Y' && $CspUtils.IsNullOrEmpty($scope.model.DiplomaMajor)) {
        //    errorList.push("ยังไม่ได้ระบุ สาขาวิชา/วิชาเอก คุณวุฒิระดับอนุปริญญา");
        //}
        //if ($scope.model.DiplomaFlag == 'Y' && $CspUtils.IsNullOrEmpty($scope.model.DiplomaAcademy)) {
        //    errorList.push("ยังไม่ได้ระบุ ชื่อสถานศึกษา คุณวุฒิระดับอนุปริญญา");
        //}

        //if (!$CspUtils.IsNullOrEmpty($scope.model.DiplomaTeachAcademy)) {

        //    if ($CspUtils.IsNullOrEmpty($scope.model.DiplomaTeachGraduateDate)) {
        //        errorList.push("ยังไม่ได้ระบุ วัน/เดือน/ปี<br />ที่สำเร็จการศึกษาระดับประกาศนียบัตรบัณฑิตวิชาชีพครู");
        //    } else {
        //        var datearray = $scope.model.DiplomaTeachGraduateDate.split("/");
        //        var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
        //        var datearray2 = $scope.closeEnrollDate.split("/");
        //        var newdate2 = datearray2[1] + '/' + datearray2[0] + '/' + datearray2[2];
        //        var dateDiff = $CspUtils.DateDiff(newdate2, newdate);
        //        if (dateDiff[0] < 0 || dateDiff[1] < 0 || dateDiff[2] < 0) {
        //            errorList.push("วัน/เดือน/ปี ที่สำเร็จการศึกษา<br />ระดับประกาศนียบัตรบัณฑิตวิชาชีพครู<br />ต้องภายในวันปิดรับสมัคร");
        //        }
        //    }
        //} else {
        //    if (!$CspUtils.IsNullOrEmpty($scope.model.DiplomaTeachGraduateDate)) {
        //        errorList.push("ยังไม่ได้ระบุ ชื่อสถานศึกษาที่สำเร็จการศึกษาระดับประกาศนียบัตรบัณฑิตวิชาชีพครู");
        //    }
        //}

        //if ($CspUtils.IsNullOrEmpty($scope.model.DocTypeID)) {
        //    errorList.push("ยังไม่ได้ระบุ สำเนาหลักฐานที่ใช้แสดงในการประกอบวิชาชีพครู");
        //} else {
        //    if ($scope.model.DocTypeID != '13' && $CspUtils.IsNullOrEmpty($scope.model.CertTeachNo)) {
        //        errorList.push("ยังไม่ได้ระบุ เลขที่เอกสารหลักฐานที่ใช้แสดงในการประกอบวิชาชีพครู");
        //    }
        //    if ($scope.model.DocTypeID != '13' && $CspUtils.IsNullOrEmpty($scope.model.CertTeachIssuedDate)) {
        //        errorList.push("ยังไม่ได้ระบุ วันที่ออกให้ เอกสารหลักฐานที่ใช้แสดงในการประกอบวิชาชีพครู");
        //    }
        //    if ($scope.model.DocTypeID != '13' && $CspUtils.IsNullOrEmpty($scope.model.CertTeachExpireDate)) {
        //        errorList.push("ยังไม่ได้ระบุ วันที่หมดอายุ เอกสารหลักฐานที่ใช้แสดงในการประกอบวิชาชีพครู");
        //    }
        //    if ($scope.model.DocTypeID != '13' && !$CspUtils.IsNullOrEmpty($scope.model.CertTeachExpireDate)) {
        //        var datearray = $scope.closeEnrollDate.split("/");
        //        var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
        //        var datearray2 = $scope.model.CertTeachExpireDate.split("/");
        //        var newdate2 = datearray2[1] + '/' + datearray2[0] + '/' + datearray2[2];
        //        var dateDiff = $CspUtils.DateDiff(newdate2, newdate);
        //        if (dateDiff[0] < 0 || dateDiff[1] < 0 || dateDiff[2] < 0) {
        //            errorList.push("วันที่หมดอายุ เอกสารหลักฐานที่ใช้แสดงในการประกอบวิชาชีพครู<br />ต้องไม่หมดอายุก่อนวันรับสมัครสอบวันสุดท้าย");
        //        }
        //    }
        //    if ($CspUtils.IsNullOrEmpty($scope.model.DocTypeName)) {
        //        $("input[type=hidden][id='docTypeName']").val($scope.getNameFromID($scope.lstCerts, $scope.model.DocTypeID));
        //    }

        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.ClassHighestLavelSchoolName)) {
        //    errorList.push("ยังไม่ได้ระบุ สถานศึกษาของวุฒิการศึกษาสูงสุด");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.DefectiveFlag)) {
        //    errorList.push("ยังไม่ได้ระบุ ข้อมูลความพิการ");
        //}
        //if ($scope.model.DefectiveFlag == 'Y' && $CspUtils.IsNullOrEmpty($scope.model.DefectiveHelpID)) {
        //    errorList.push("ยังไม่ได้ระบุ ความช่วยเหลือ");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.TeachClassLevelID)) {
        //    errorList.push("ยังไม่ได้ระบุ ระดับชั้นที่ประสงค์จะสอบสาธิตการปฏิบัติการสอน");
        //} else {
        //    if ($CspUtils.IsNullOrEmpty($scope.model.TeachClassLevelName)) {
        //        $("input[type=hidden][id='teachClassLevelName']").val($scope.getNameFromID($scope.lstTeachClasses, $scope.model.TeachClassLevelID));
        //    }
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.EduPrefixID)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้า ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($scope.model.EduPrefixID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.EduPrefixName)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้าอื่นๆ ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EduFirstName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อตัว ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EduLastName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อสกุล ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.OccuupationID)) {
            errorList.push("ยังไม่ได้ระบุ อาชีพปัจจุบัน");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.OccuupationName)) {
                $("input[type=hidden][id='occuupationName']").val($scope.getNameFromID($scope.lstOccupation, $scope.model.OccuupationID));
            }
        }
        //if ($scope.model.OccuupationID == '1' && $CspUtils.IsNullOrEmpty($scope.model.OccuupationName4Gov)) {
        //    errorList.push("ยังไม่ได้ระบุ ประเภทของข้าราชการ");
        //}
        if ($scope.model.OccuupationID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.OccuupationName)) {
            errorList.push("ยังไม่ได้ระบุ อาชีพอื่นๆ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.PositionName)) {
            errorList.push("ยังไม่ได้ระบุ ตำแหน่ง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.StationName)) {
            errorList.push("ยังไม่ได้ระบุ สถานที่ทำงาน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.DepartmentName)) {
            errorList.push("ยังไม่ได้ระบุ กอง/สำนัก/อื่น ๆ");
        }
        //if ($scope.model.OccuupationID != "5" && $scope.model.OccuupationID != "4") {
        //    if ($CspUtils.IsNullOrEmpty($scope.model.OfficeMobile)) {
        //        errorList.push("ยังไม่ได้ระบุ โทรศัพท์เคลื่อนที่ที่ทำงาน");
        //    }
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.OfficeTel)) {
        //    errorList.push("ยังไม่ได้ระบุ โทรศัพท์ที่ทำงาน");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.Salary)) {
            errorList.push("ยังไม่ได้ระบุ อัตราเงินเดือน");
        } else {
            if (parseFloat($scope.model.Salary) > 99999999.99) {
                errorList.push("กรุณาระบุอัตราเงินเดือน ไม่เกิน 99,999,999.99");
            }
        }


        if ($CspUtils.IsNullOrEmpty($scope.model.TbbAddrNo)) {
            errorList.push("ยังไม่ได้ระบุ บ้านเลขที่");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbVillage)) {
            errorList.push("ยังไม่ได้ระบุ ชั้น/อาคาร/หมู่บ้าน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbMoo)) {
            errorList.push("ยังไม่ได้ระบุ หมู่ที่");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbSoi)) {
            errorList.push("ยังไม่ได้ระบุ ตรอก/ซอย/แยก");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbRoad)) {
            errorList.push("ยังไม่ได้ระบุ ถนน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbProvID)) {
            errorList.push("ยังไม่ได้ระบุ จังหวัด");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbAmphID)) {
            errorList.push("ยังไม่ได้ระบุ เขต/อำเภอ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbTmblID)) {
            errorList.push("ยังไม่ได้ระบุ แขวง/ตำบล");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbZipcode)) {
            errorList.push("กรุณาระบุ รหัสไปรษณีย์ ให้ถูกต้อง");
        }


        //if ($CspUtils.IsNullOrEmpty($scope.model.TbbTel)) {
        //    errorList.push("ยังไม่ได้ระบุ โทรศัพท์");
        //}

        //if ($CspUtils.IsNullOrEmpty($scope.model.AddrNo)) {
        //    errorList.push("ยังไม่ได้ระบุ บ้านเลขที่");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Village)) {
        //    errorList.push("ยังไม่ได้ระบุ ชั้น/อาคาร/หมู่บ้าน");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Moo)) {
        //    errorList.push("ยังไม่ได้ระบุ หมู่ที่");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Soi)) {
        //    errorList.push("ยังไม่ได้ระบุ ตรอก/ซอย/แยก");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Road)) {
        //    errorList.push("ยังไม่ได้ระบุ ถนน");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.ProvID)) {
            errorList.push("ยังไม่ได้ระบุ จังหวัด");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.ProvName)) {
                $("input[type=hidden][id='provName']").val($scope.getNameFromID($scope.lstProvinces, $scope.model.ProvID));
            }
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.AmphID)) {
            errorList.push("ยังไม่ได้ระบุ เขต/อำเภอ");
        }
        else {
            if ($CspUtils.IsNullOrEmpty($scope.model.AmphName)) {
                $("input[type=hidden][id='amphName']").val($scope.getNameFromID($scope.lstAmphs, $scope.model.AmphID));
            }
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.TmblID)) {
        //    errorList.push("ยังไม่ได้ระบุ แขวง/ตำบล");
        //}
        //else {
        //    if ($CspUtils.IsNullOrEmpty($scope.model.TmblName)) {
        //        $("input[type=hidden][id='tmblName']").val($scope.getNameFromID($scope.lstTmbls, $scope.model.TmblID));
        //    }
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Zipcode)) {
        //    errorList.push("กรุณาระบุ รหัสไปรษณีย์ ให้ถูกต้อง");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.Tel)) {
        //    errorList.push("ยังไม่ได้ระบุ โทรศัพท์");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.Mobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Email)) {
            errorList.push("กรุณาระบุ อีเมล ให้ถูกต้อง");
        }

        //if ($CspUtils.IsNullOrEmpty($scope.model.ExamSiteID)) {
        //    errorList.push("กรุณาระบุ จังหวัดที่ประสงค์จะเข้าสอบ");
        //}
        //else {
        //    if ($CspUtils.IsNullOrEmpty($scope.model.ExamSiteName)) {
        //        $("input[type=hidden][id='ExamSiteName']").val($scope.getNameFromID($scope.lstExamSites, $scope.model.ExamSiteID));
        //    }
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.SpecialSkills)) {
        //    errorList.push("ยังไม่ได้ระบุ ความรู้ความสามารถพิเศษ");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyFirstName)) {
        //    errorList.push("ยังไม่ได้ระบุ ชื่อบุคคลที่ติดต่อได้");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyLastName)) {
        //    errorList.push("ยังไม่ได้ระบุ นามสกุลบุคคลที่ติดต่อได้");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyRelation)) {
        //    errorList.push("ยังไม่ได้ระบุ ความสัมพันธ์บุคคลที่ติดต่อได้");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyMoblie) && $CspUtils.IsNullOrEmpty($scope.model.EmergencyTel)) {
        //    errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ หรือ โทรศัพท์บ้าน/สถานที่ทำงาน ของบุคคลที่ติดต่อได้<br/>อย่างใดอย่างหนึ่งให้ถูกต้อง");
        //}
        //if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyMoblie)) {
        //    errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่บุคคลที่ติดต่อได้ ให้ถูกต้อง");
        //}

        if ($scope.hasFile == false) {
            errorList.push("กรุณาเลือกไฟล์รูปถ่าย");
        } else {
            if ($scope.fileSizeValid == false) {
                errorList.push("ขนาดไฟล์ไม่ถูกต้อง");
            } else {
                if ($scope.imageValid == false && $scope.waitingDetect == false) {
                    errorList.push("รูปถ่ายไม่ถูกต้อง");
                }
            }
        }
        if ($CspUtils.IsNullOrEmpty($scope.confirm) || $scope.confirm == false) {
            errorList.push("กรุณายืนยันการสมัคร");
        }
        if ($scope.smsStatus && $CspUtils.IsNullOrEmpty($scope.model.SMSMobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
        }
        if ($scope.smsStatus && $CspUtils.IsNullOrEmpty($scope.model.SMSEmail)) {
            errorList.push("กรุณาระบุ อีเมล ให้ถูกต้อง");
        }

        //$("input[type=hidden][id*='Name']").each(function () {
        //    $(this)[0].value = ''
        //});

        //prefixName
        //genderName
        //statusName
        //classGroupName
        //regClassName
        //classLavelName
        //docTypeName
        //teachClassLevelName
        //occuupationName
        //provName
        //amphName
        //tmblName
        //ExamSiteName


        // เช็ค require document upload
        //if ($scope.uploadChild.documentList && $scope.uploadChild.documentList.length > 0) {
        //    for (var j = 0; j < $scope.uploadChild.documentList.length; j++) {
        //        if ($scope.uploadChild.documentList[j].REQUIRE_FLAG == "Y" && $scope.uploadChild.documentList[j].UPLOAD_STATUS != "Y") {
        //            errorList.push("กรุณาอัปโหลดเอกสาร (" + $scope.uploadChild.documentList[j].DOC_TYPE_NAME + ")");
        //        }
        //    }
        //}

        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            var msg = errorList.slice(0, 5).join('<br/>');
            e.preventDefault();
            $scope.checked = false;
            var options = {
                onAfterClose: () => {
                    var invalidObject = angular.element(document.querySelector("input.ng-invalid,select.ng-invalid"))[0];
                    if (invalidObject) invalidObject.focus();
                }
            };
            alertWarning(msg, "", options);
            return;
        }

        if ($scope.eduFlag) {
            $scope.model.EduFlag = 'Y'
        } else {
            $scope.model.EduFlag = 'N'
        }

        if ($scope.tbbFlag) {
            $scope.model.TbbFlag = 'Y'
        } else {
            $scope.model.TbbFlag = 'N'
        }

        if ($scope.ocscFlag) {
            $scope.model.OCSCFlag = 'Y'
        } else {
            $scope.model.OCSCFlag = 'N'
        }

        if ($scope.smsStatus) {
            $scope.model.SMSStatus = 'Y'
        } else {
            $scope.model.SMSStatus = 'N'
        }
        if ($scope.waitingDetect == true) {
            e.preventDefault();

            $scope.waitingDetect = false;
            $scope.detecting = true;

            $("#detectModal").modal();

            var $editor = $('.image-editor');
            var imgSrc = $editor.cropit('imageSrc');
            var offset = $editor.cropit('offset');
            var zoom = $editor.cropit('zoom');
            var previewSize = $editor.cropit('previewSize');

            var img = new Image();
            img.src = imgSrc;

            // Draw image in original size on a canvas
            var originalCanvas = document.createElement('canvas');
            originalCanvas.width = previewSize.width / zoom;
            originalCanvas.height = previewSize.height / zoom;
            var ctx = originalCanvas.getContext('2d');
            ctx.fillStyle = "#ffffff";
            ctx.fillRect(0, 0, originalCanvas.width, originalCanvas.height);
            ctx.drawImage(img, offset.x / zoom, offset.y / zoom);

            // Use pica to resize image and paint on destination canvas
            var zoomedCanvas = document.createElement('canvas');
            zoomedCanvas.width = previewSize.width * 3;         // 594;
            zoomedCanvas.height = previewSize.height * 3;       // 774;

            var resizer_mode = {
                js: true,
                wasm: true,
                cib: true,
                ww: true
            };
            var opts = [];

            Object.keys(resizer_mode).forEach(function (k) {
                if (resizer_mode[k]) opts.push(k);
            });

            resizer = window.pica({ features: opts });

            resizer.resize(originalCanvas, zoomedCanvas, {
                unsharpAmount: 80,
                unsharpRadius: 0.6,
                unsharpThreshold: 2
            })
            .then(result => {
                // Resizing completed
                // Read resized image data
                var picaImageData = zoomedCanvas.toDataURL("image/jpeg");
                if (!$CspUtils.IsNullOrEmpty(picaImageData) && $scope.fileSizeValid == true) {
                    var data = {};
                    data.ProjectCode = "VEC";
                    data.EnableAutoBrightness = true;
                    data.TransactionID = $scope.model.CitizenID + "-" + Date.now();  // "TEST00001";
                    data.SourceBase64 = picaImageData;
                    $scope.imageValid = false;
                    var url = faceApiUrl;
                    $http.post(url, data)
                            .then(function (result) {
                                if (result.data.Success && result.data.IsFaceDetected && $CspUtils.IsNullOrEmpty(result.data.ErrorCode)) {
                                    $scope.imageValid = true;
                                    $scope.model.ImageBase64 = picaImageData;
                                    document.getElementById("register").submit();
                                }
                                else {
                                    $scope.imageValid = false;
                                    $scope.checked = false;
                                    $scope.model.ImageBase64 = "";
                                    if (result.data.ErrorCode == "UNKNOWN_ERROR" || result.data.ErrorCode == "IMAGE_CONVERTER") {
                                        $scope.ErrorMessImage = "รูปถ่ายไม่ถูกต้อง";

                                    } else {
                                        $scope.ErrorMessImage = result.data.ErrorMessage;
                                    }
                                }
                            }
                            , function (err) {
                                $scope.imageValid = false;
                                $scope.ErrorMessImage = "เกิดข้อผิดพลาดในการตรวจสอบรูปถ่าย";
                                $scope.model.ImageBase64 = "";
                                console.log("DetectFaces Error", err);
                                alert("เกิดข้อผิดพลาดในการตรวจสอบรูปถ่าย");
                            }).finally(function () { $scope.detecting = false; $scope.waitingDetect = false; });
                }
            });
        }
        else {

            document.getElementById("register").submit();
        }
    };

    $scope.submitConfirm = function () {

        var capcha = $("#CaptchaInputText").val();
        //console.log("capcha", capcha);
        var errorList = [];
        if ($CspUtils.IsNullOrEmpty(capcha) || capcha.length != 5) {
            errorList.push("กรุณากรอกตัวอักษรที่ท่านเห็น");
        }

        if (errorList.length > 0) {
            var msg = errorList.slice(0, 5).join('<br/>');
            // var msg = errorList[0];
            alert(msg);
            $scope.checked = false;
            return;
        }

        if (!$scope.checked) {
            $scope.openDialogWarning();
            return;
        }

        document.getElementById("confirmRegister").submit();
    };


    $scope.getProvinces = function () {
        $scope.isLoading = true;
        $scope.lstProvinces = [];
        $LookupService.getLookupProvince().then(function (result) {
            $scope.lstProvinces = result.data;
        }, function (err) {
            console.log("Error on getLookupProvince", err);
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.eduPrefixChange = function () {
        $scope.eduFlag = false;
        $scope.model.EduFlag = 'N'
    };

    $scope.provinceChange = function () {
        //console.log("ProvID", $scope.modelOrg.ProvID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.ProvID)) {
            $scope.model.ProvID = $scope.modelOrg.ProvID;
            $scope.model.ProvName = $scope.modelOrg.ProvName;
            $scope.modelOrg.ProvID = "";
            $scope.modelOrg.ProvName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.ProvID)) {
                $scope.model.ProvName = $scope.getNameFromID($scope.lstProvinces, $scope.model.ProvID);
            }
            else {
                $scope.model.ProvName = "";
            }
        }

        $scope.lstAmphs = [];
        $scope.model.AmphID = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.ProvID)) {
            $scope.SetLoading("lstAmphs", true);
            $LookupService.getLookupAmphur($scope.model.ProvID).then(function (result) {
                $scope.lstAmphs = result.data;
                $scope.amphurChange();
            }, function (err) {
                console.log("Error on getLookupAmphur", err);
            }).finally(function () { $scope.SetLoading("lstAmphs", false); });
        }
    };

    $scope.amphurChange = function () {
        //console.log("AmphID", $scope.modelOrg.AmphID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.AmphID)) {
            $scope.model.AmphID = $scope.modelOrg.AmphID;
            $scope.model.AmphName = $scope.modelOrg.AmphName;
            $scope.modelOrg.AmphID = "";
            $scope.modelOrg.AmphName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.AmphID)) {
                $scope.model.AmphName = $scope.getNameFromID($scope.lstAmphs, $scope.model.AmphID);
            }
            else {
                $scope.model.AmphName = "";
            }
        }

        $scope.lstTmbls = [];
        $scope.model.TmblID = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.AmphID)) {
            $scope.SetLoading("lstTmbls", true);
            $LookupService.getLookupTumbon($scope.model.AmphID).then(function (result) {
                $scope.lstTmbls = result.data;
                $scope.tmblChange();
            }, function (err) {
                console.log("Error on getLookupTumbon", err);
            }).finally(function () { $scope.SetLoading("lstTmbls", false); });
        }
    };

    $scope.tmblChange = function () {
        //console.log("TmblID", $scope.modelOrg.TmblID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.TmblID)) {
            $scope.model.TmblID = $scope.modelOrg.TmblID;
            $scope.model.TmblName = $scope.modelOrg.TmblName;
            $scope.modelOrg.TmblID = "";
            $scope.modelOrg.TmblName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.TmblID)) {
                $scope.model.TmblName = $scope.getNameFromID($scope.lstTmbls, $scope.model.TmblID);
            }
            else {
                $scope.model.TmblName = "";
            }
        }
    };

    $scope.tbbprovinceChange = function () {
        //console.log("TbbProvID", $scope.modelOrg.TbbProvID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.TbbProvID)) {
            $scope.model.TbbProvID = $scope.modelOrg.TbbProvID;
            $scope.model.TbbProvName = $scope.modelOrg.TbbProvName;
            $scope.modelOrg.TbbProvID = "";
            $scope.modelOrg.TbbProvName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.TbbProvID)) {
                $scope.model.TbbProvName = $scope.getNameFromID($scope.lstProvinces, $scope.model.TbbProvID);
            }
            else {
                $scope.model.TbbProvName = "";
            }
        }

        $scope.model.TbbAmphID = "";
        $scope.lstTbbAmphs = [];
        if (!$CspUtils.IsNullOrEmpty($scope.model.TbbProvID)) {
            $scope.SetLoading("lstTbbAmphs", true);
            $LookupService.getLookupAmphur($scope.model.TbbProvID).then(function (result) {
                $scope.lstTbbAmphs = result.data;
                $scope.tbbamphurChange();
            }, function (err) {
                console.log("Error on getLookupAmphur", err);
            }).finally(function () { $scope.SetLoading("lstTbbAmphs", false); });
        }
    };

    $scope.tbbamphurChange = function () {
        //console.log("TbbAmphID", $scope.modelOrg.TbbAmphID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.TbbAmphID)) {
            $scope.model.TbbAmphID = $scope.modelOrg.TbbAmphID;
            $scope.model.TbbAmphName = $scope.modelOrg.TbbAmphName;
            $scope.modelOrg.TbbAmphID = "";
            $scope.modelOrg.TbbAmphName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.TbbAmphID)) {
                $scope.model.TbbAmphName = $scope.getNameFromID($scope.lstTbbAmphs, $scope.model.TbbAmphID);
            }
            else {
                $scope.model.TbbAmphName = "";
            }
        }

        $scope.lstTbbTmbls = [];
        $scope.model.TbbTmblID = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.TbbAmphID)) {
            $scope.SetLoading("lstTbbTmbls", true);
            $LookupService.getLookupTumbon($scope.model.TbbAmphID).then(function (result) {
                $scope.lstTbbTmbls = result.data;
                $scope.tbbTmblChange();
            }, function (err) {
                console.log("Error on getLookupTumbon", err);
            }).finally(function () { $scope.SetLoading("lstTbbTmbls", false); });
        }
    };

    $scope.tbbTmblChange = function () {
        //console.log("TbbTmblID", $scope.modelOrg.TbbTmblID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.TbbTmblID)) {
            $scope.model.TbbTmblID = $scope.modelOrg.TbbTmblID;
            $scope.model.TbbTmblName = $scope.modelOrg.TbbTmblName;
            $scope.modelOrg.TbbTmblID = "";
            $scope.modelOrg.TbbTmblName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.TbbTmblID)) {
                $scope.model.TbbTmblName = $scope.getNameFromID($scope.lstTbbTmbls, $scope.model.TbbTmblID);
            }
            else {
                $scope.model.TbbTmblName = "";
            }
        }
    };

    $scope.getExamSites = function () {
        $scope.isLoading = true;
        $scope.lstExamSites = [];
        $LookupService.getLookupExamSites($scope.testTypeID).then(function (result) {
            $scope.lstExamSites = result.data;
            $scope.getGenders();
        }, function (err) {
            console.log("Error on getLookupPrefixes", err);
            $scope.lstExamSites = [];
        }).finally(function () { $scope.isLoading = true; });
    };
    $scope.getPrefixes = function () {
        $scope.isLoading = true;
        $scope.prefixes = [];
        $LookupService.getLookupPrefixes().then(function (result) {
            $scope.prefixes = result.data;
            $scope.getGenders();
        }, function (err) {
            console.log("Error on getLookupPrefixes", err);
            $scope.prefixes = [];
        }).finally(function () { $scope.isLoading = true; });
    };

    $scope.getGenders = function () {
        $scope.isLoading = true;
        $scope.genders = [];
        $LookupService.getLookupGender().then(function (result) {
            $scope.genders = result.data;
            if (!$CspUtils.IsNullOrEmpty($scope.model.PrefixID)) {
                if ($scope.model.PrefixID != '999999')
                    $scope.model.Gender = $scope.prefixes.find(x => x.id === $scope.model.PrefixID).gender;
            }
        }, function (err) {
            console.log("Error on getLookupGender", err);
            $scope.genders = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.genderChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.PrefixID)) {
            $scope.model.Gender = $scope.prefixes.find(x => x.id === $scope.model.PrefixID).gender;
        }
    };

    $scope.getReligions = function () {
        $scope.isLoading = true;
        $scope.religions = [];
        $LookupService.getLookupReligions().then(function (result) {
            $scope.religions = result.data;
        }, function (err) {
            console.log("Error on getLookupReligions", err);
            $scope.religions = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getStatuses = function () {
        $scope.isLoading = true;
        $scope.statuses = [];
        $LookupService.getLookupStatuses().then(function (result) {
            $scope.statuses = result.data;
        }, function (err) {
            console.log("Error on getLookupStatuses", err);
            $scope.statuses = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getTeachClass = function () {
        $scope.isLoading = true;
        $scope.lstTeachClasses = [];
        $LookupService.getLookupTeachClasses($scope.testTypeID).then(function (result) {
            $scope.lstTeachClasses = result.data;
        }, function (err) {
            console.log("Error on getLookupTeachClasses", err);
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getClassGroups = function () {
        $scope.isLoading = true;
        $scope.lstClassGroups = [];
        $LookupService.getLookupClassGroups($scope.testTypeID).then(function (result) {
            $scope.lstClassGroups = result.data;
            if ($scope.lstClassGroups.length == 1) {
                $scope.model.RegClassID = $scope.lstClassGroups[0].id;
            }
            $scope.classGroupChange();
        }, function (err) {
            console.log("Error on getLookupClassGroups", err);
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.classGroupChange = function () {
        //if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.ClassGroupID)) {
        //    $scope.model.ClassGroupID = $scope.modelOrg.ClassGroupID;
        //    $scope.model.ClassGroupName = $scope.modelOrg.ClassGroupName;
        //    $scope.modelOrg.ClassGroupID = "";
        //    $scope.modelOrg.ClassGroupName = "";
        //    //$scope.reloadDocumentList();
        //}
        //else {
        //    if (!$CspUtils.IsNullOrEmpty($scope.model.ClassGroupID)) {
        //        $scope.model.ClassGroupName = $scope.getNameFromID($scope.lstClassGroups, $scope.model.ClassGroupID);
        //        //$scope.reloadDocumentList();
        //    }
        //    else {
        //        $scope.model.ClassGroupName = "";
        //    }
        //}

        $scope.lstClasses = [];
        $scope.model.RegClassID = "0";
        $scope.model.RegClassName = "";
        $scope.lstCerts = [];
        $scope.model.DocTypeID = "";
        $scope.model.DocTypeName = "";
        //if (!$CspUtils.IsNullOrEmpty($scope.model.ClassGroupID)) {
        $scope.SetLoading("lstClasses", true);
        $LookupService.getLookupClasses($scope.testTypeID, -1).then(function (result) {
            $scope.lstClasses = result.data;
            //console.log(result.data);
            if ($scope.lstClasses.length == 1) {
                $scope.model.RegClassID = $scope.lstClasses[0].id;
            }
            $scope.regClassChange();
        }, function (err) {
            console.log("Error on getLookupClasses", err);
        }).finally(function () { $scope.SetLoading("lstClasses", false); });

        //$scope.SetLoading("lstCerts", true);
        //$LookupService.getLookupCerts($scope.testTypeID, $scope.model.ClassGroupID).then(function (result) {
        //    $scope.lstCerts = result.data;
        //    $scope.certChange();
        //}, function (err) {
        //    console.log("Error on getLookupCerts", err);
        //}).finally(function () { $scope.SetLoading("lstCerts", false); });
        //}
    };

    $scope.certChange = function () {
        //console.log("DocTypeID", $scope.modelOrg.DocTypeID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.DocTypeID)) {
            $scope.model.DocTypeID = $scope.modelOrg.DocTypeID;
            $scope.model.DocTypeName = $scope.modelOrg.DocTypeName;
            $scope.modelOrg.DocTypeID = "";
            $scope.modelOrg.DocTypeName = "";
            //$scope.reloadDocumentList();
        }
        else {
            console.log('$scope.model.DocTypeID', $scope.model.DocTypeID);
            if (!$CspUtils.IsNullOrEmpty($scope.model.DocTypeID)) {
                $scope.model.DocTypeName = $scope.getNameFromID($scope.lstCerts, $scope.model.DocTypeID);
                if ($scope.model.DocTypeID == '13') {
                    $scope.model.CertTeachNo = '';
                    $scope.model.CertTeachIssuedDate = '';
                    $scope.model.CertTeachExpireDate = '';
                }
                $scope.reloadDocumentList();
            } else {
                $scope.model.DocTypeName = "";
            }
        }
    };

    $scope.regClassChange = function () {
        //console.log("RegClassID", $scope.modelOrg.RegClassID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.RegClassID) && $scope.modelOrg.RegClassID != "0") {
            $scope.model.RegClassID = $scope.modelOrg.RegClassID;
            $scope.model.RegClassName = $scope.modelOrg.RegClassName;
            $scope.modelOrg.RegClassID = "0";
            $scope.modelOrg.RegClassName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.RegClassID) && $scope.model.RegClassID != "0") {
                $scope.model.RegClassName = $scope.getNameFromID($scope.lstClasses, $scope.model.RegClassID);
                $scope.model.ClassGroupName = $scope.lstClasses.find(x => x.id === $scope.model.RegClassID).sbjgroup;
                //Get เอกสารที่ต้องอัปโหลด
                //$scope.reloadDocumentList();
            }
            else {
                $scope.model.RegClassName = "";
            }
        }


        $scope.lstEducationals = [];
        $scope.model.ClassLavelID = "";
        $scope.model.ClassLavelName = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.RegClassID) && $scope.model.RegClassID != "0") {
            $scope.SetLoading("lstEducationals", true);
            $LookupService.getLookupEducationals($scope.model.RegClassID).then(function (result) {
                $scope.lstEducationals = result.data;
                //console.log($scope.lstEducationals);
                if ($scope.lstEducationals.length == 1) {
                    $scope.model.ClassLavelID = $scope.lstEducationals[0].id;
                }
                $scope.educationChange();
            }, function (err) {
                console.log("Error on getLookupEducationals", err);
            }).finally(function () { $scope.SetLoading("lstEducationals", false); });
        }
    };

    $scope.educationChange = function () {
        //console.log("ClassLavelID", $scope.modelOrg.ClassLavelID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.ClassLavelID)) {
            $scope.model.ClassLavelID = $scope.modelOrg.ClassLavelID;
            $scope.model.ClassLavelName = $scope.modelOrg.ClassLavelName;
            $scope.modelOrg.ClassLavelID = "";
            $scope.modelOrg.ClassLavelName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
                $scope.model.ClassLavelName = $scope.getNameFromID($scope.lstEducationals, $scope.model.ClassLavelID);
            } else {
                $scope.model.ClassLavelName = "";
            }
        }

        $scope.lstDegrees = [];
        $scope.model.DegreeID = "";
        $scope.model.DegreeName = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
            $scope.SetLoading("lstDegrees", true);
            $LookupService.getLookupDegrees($scope.model.RegClassID, $scope.model.ClassLavelID).then(function (result) {
                $scope.lstDegrees = result.data;
                if ($scope.lstDegrees.length == 1) {
                    $scope.model.DegreeID = $scope.lstDegrees[0].id;
                }
                $scope.degreeChange();
            }, function (err) {
                console.log("Error on getLookupDegrees", err);
            }).finally(function () { $scope.SetLoading("lstDegrees", false); });
        }

        $scope.model.OCSCLevelID = "";
        $scope.model.OCSCLevelName = "";
        $scope.lstOCSCEducationals = [];
        if (!$CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
            $scope.SetLoading("lstOCSCEducationals", true);
            $LookupService.getLookupOCSCEducationals($scope.testTypeID, $scope.model.ClassLavelID).then(function (result) {
                $scope.lstOCSCEducationals = result.data;
                if ($scope.lstOCSCEducationals.length == 1) {
                    $scope.model.OCSCLevelID = $scope.lstOCSCEducationals[0].id;
                }
                $scope.ocscLevelChange();
            }, function (err) {
                console.log("Error on getLookupOCSCEducationals", err);
            }).finally(function () { $scope.SetLoading("lstOCSCEducationals", false); });
        }
    };

    $scope.degreeChange = function () {
        //console.log("DegreeID", $scope.modelOrg.DegreeID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.DegreeID)) {
            $scope.model.DegreeID = $scope.modelOrg.DegreeID;
            $scope.model.DegreeName = $scope.modelOrg.DegreeName;
            $scope.modelOrg.DegreeID = "";
            $scope.modelOrg.DegreeName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.DegreeID) && $scope.model.DegreeID != "999999") {
                $scope.model.DegreeName = $scope.getNameFromID($scope.lstDegrees, $scope.model.DegreeID);
            } else {
                $scope.model.DegreeName = "";
            }
        }

        $scope.model.MajorID = "";
        $scope.model.MajorName = "";
        $scope.lstMajors = [];
        if (!$CspUtils.IsNullOrEmpty($scope.model.DegreeID)) {
            $scope.SetLoading("lstMajors", true);
            $LookupService.getLookupMajors($scope.model.RegClassID, $scope.model.ClassLavelID, $scope.model.DegreeID).then(function (result) {
                $scope.lstMajors = result.data;
                if ($scope.lstMajors.length == 1) {
                    $scope.model.MajorID = $scope.lstMajors[0].id;
                }
                $scope.majorChange();
            }, function (err) {
                console.log("Error on getLookupMajors", err);
            }).finally(function () { $scope.SetLoading("lstMajors", false); });
        }
    };

    $scope.majorChange = function () {
        //console.log("MajorID", $scope.modelOrg.MajorID);
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.MajorID)) {
            $scope.model.MajorID = $scope.modelOrg.MajorID;
            $scope.model.MajorName = $scope.modelOrg.MajorName;
            $scope.modelOrg.MajorID = "";
            $scope.modelOrg.MajorName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.MajorID) && $scope.model.MajorID != "999999") {
                $scope.model.MajorName = $scope.getNameFromID($scope.lstMajors, $scope.model.MajorID);
            } else {
                $scope.model.MajorName = "";
            }
        }
    };

    $scope.ocscLevelChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.OCSCLevelID)) {
            $scope.model.OCSCLevelID = $scope.modelOrg.OCSCLevelID;
            $scope.model.OCSCLevelName = $scope.modelOrg.OCSCLevelName;
            $scope.modelOrg.OCSCLevelID = "";
            $scope.modelOrg.OCSCLevelName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.OCSCLevelID)) {
                $scope.model.OCSCLevelName = $scope.getNameFromID($scope.lstOCSCEducationals, $scope.model.OCSCLevelID);
            } else {
                $scope.model.OCSCLevelName = "";
            }
        }
    };

    $scope.highestEducationChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.ClassHighestLavelID)) {
            $scope.model.ClassHighestLavelID = $scope.modelOrg.ClassHighestLavelID;
            $scope.model.ClassHighestLavelName = $scope.modelOrg.ClassHighestLavelName;
            $scope.modelOrg.ClassHighestLavelID = "";
            $scope.modelOrg.ClassHighestLavelName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.ClassHighestLavelID)) {
                $scope.model.ClassHighestLavelName = $scope.getNameFromID($scope.lstHighestEducationals, $scope.model.ClassHighestLavelID);
            } else {
                $scope.model.ClassHighestLavelName = "";
            }
        }
    };

    $scope.getLookupHighestEducationals = function () {
        $scope.lstHighestEducationals = [];
        if (!$CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
            $scope.SetLoading("lstHighestEducationals", true);
            $LookupService.getLookupHighestEducationals($scope.model.ClassLavelID).then(function (result) {
                $scope.lstHighestEducationals = result.data;
            }, function (err) {
                console.log("Error on getLookupHighestEducationals", err);
                $scope.lstHighestEducationals = [];
            }).finally(function () { $scope.SetLoading("lstHighestEducationals", false); });
        } else {
            $scope.lstHighestEducationals = [];
        }
    };

    $scope.diplomaFlagChange = function () {
        if ($scope.model.DiplomaFlag == 'N') {
            $scope.model.DiplomaMajor = '';
            $scope.model.DiplomaAcademy = '';
        }
    };


    $scope.teachClassChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.TeachClassLevelID)) {
            $scope.model.TeachClassLevelName = $scope.getNameFromID($scope.lstTeachClasses, $scope.model.TeachClassLevelID);
        } else {
            $scope.model.TeachClassLevelName = "";
        }
    };

    $scope.getOccupation = function () {
        $scope.isLoading = true;
        $scope.lstOccupation = [];
        $LookupService.getLookupOccupation().then(function (result) {
            $scope.lstOccupation = result.data;
        }, function (err) {
            console.log("Error on getLookupOccupation", err);
            $scope.lstOccupation = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.carreerChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.OccuupationID)) {
            if ($scope.model.OccuupationID == "5" || $scope.model.OccuupationID == "4") {
                $scope.occFlag = true;
                $scope.model.PositionName = "-";
                $scope.model.StationName = "-";
                $scope.model.DepartmentName = "-";
                //$scope.model.OfficeMobile = "";
                //$scope.model.OfficeTel = "-";
                //$scope.model.OfficeTelEx = "";
                $scope.model.Salary = "0.00";
            } else {
                $scope.occFlag = false;
                $scope.model.PositionName = "";
                $scope.model.StationName = "";
                $scope.model.DepartmentName = "";
                //$scope.model.OfficeMobile = "";
                //$scope.model.OfficeTel = "";
                //$scope.model.OfficeTelEx = "";
                $scope.model.Salary = "";
            }

            if ($scope.model.OccuupationID == "999999") {
                $scope.model.OccuupationName = '';
            }

            if ($scope.model.OccuupationID != "1") {
                $scope.model.OccuupationName4Gov = '';
            }

        }
    };

    $scope.getNameFromID = function (lst, id) {
        if (lst.length > 0 && !$CspUtils.IsNullOrEmpty(id)) {
            var name = lst.find(x => x.id === id);
            if (!$CspUtils.IsNullOrEmpty(name)) {
                return name.text;
            }
        }
        return "";
    };

    $scope.eduFlagChange = function () {
        if ($scope.eduFlag) {
            $scope.model.EduPrefixID = $scope.model.PrefixID;
            $scope.model.EduPrefixName = $scope.model.PrefixName;
            $scope.model.EduFirstName = $scope.model.FirstName;
            $scope.model.EduLastName = $scope.model.LastName;
            $scope.model.EduFlag = 'Y'
        }
        else {
            $scope.model.EduFlag = 'N'
        }
    };

    $scope.smsStatusChange = function () {
        if ($scope.smsStatus) {
            $scope.model.SMSStatus = 'Y'
        } else {
            $scope.model.SMSStatus = 'N'
        }
    };

    $scope.ocscFlagChange = function () {
        if ($scope.ocscFlag) {
            $scope.model.OCSCFlag = 'Y'
        } else {
            $scope.model.OCSCFlag = 'N'
        }
    };

    $scope.tbbFlagChange = function () {
        if (tbbFlag) {
            $scope.model.AddrNo = $scope.model.TbbAddrNo;
            $scope.model.Village = $scope.model.TbbVillage;
            $scope.model.Moo = $scope.model.TbbMoo;
            $scope.model.Soi = $scope.model.TbbSoi;
            $scope.model.Road = $scope.model.TbbRoad;
            $scope.model.ProvID = $scope.model.TbbProvID;
            $scope.provinceChange();
            $scope.model.AmphID = $scope.model.TbbAmphID;
            $scope.amphurChange();
            $scope.model.TmblID = $scope.model.TbbTmblID;
            $scope.model.Zipcode = $scope.model.TbbZipcode;
            $scope.model.Tel = $scope.model.TbbTel;
            $scope.model.TelEx = $scope.model.TbbTelEx;
        }
    };

    $scope.examSiteChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.modelOrg.ExamSiteID)) {
            $scope.model.ExamSiteID = $scope.modelOrg.ExamSiteID;
            $scope.model.ExamSiteName = $scope.modelOrg.ExamSiteName;
            $scope.modelOrg.ExamSiteID = "";
            $scope.modelOrg.ExamSiteName = "";
        }
        else {
            if (!$CspUtils.IsNullOrEmpty($scope.model.ExamSiteID)) {
                $scope.model.ExamSiteName = $scope.getNameFromID($scope.lstExamSites, $scope.model.ExamSiteID);
            } else {
                $scope.model.ExamSiteName = "";
            }
        }
    };

    $scope.setFocus = function (elementid) {
        $CspUtils.SetFocus(elementid);
    };

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

    $scope.SetTimerImage = function () {
        $scope.imageValid = false;
        $scope.waitingDetect = true;
        $scope.detecting = false;
        $scope.$apply();
        if (angular.isDefined(timer)) {
            $interval.cancel(timer);
            timer = undefined;
        }
        timer = $interval(function () {
            if ($scope.waitingDetect) {
                $scope.ExportDetect();
            }
        }, $scope.detectTimer);
    };

    $scope.ExportDetect = function () {
        // Get cropping information
        var $editor = $('.image-editor');
        var imgSrc = $editor.cropit('imageSrc');
        var offset = $editor.cropit('offset');
        var zoom = $editor.cropit('zoom');
        var previewSize = $editor.cropit('previewSize');

        var img = new Image();
        img.src = imgSrc;

        // Draw image in original size on a canvas
        var originalCanvas = document.createElement('canvas');
        originalCanvas.width = previewSize.width / zoom;
        originalCanvas.height = previewSize.height / zoom;
        var ctx = originalCanvas.getContext('2d');
        ctx.fillStyle = "#ffffff";
        ctx.fillRect(0, 0, originalCanvas.width, originalCanvas.height);
        ctx.drawImage(img, offset.x / zoom, offset.y / zoom);

        // Use pica to resize image and paint on destination canvas
        var zoomedCanvas = document.createElement('canvas');
        zoomedCanvas.width = previewSize.width * 3;  //594;
        zoomedCanvas.height = previewSize.height * 3;  //774;

        var resizer_mode = {
            js: true,
            wasm: true,
            cib: true,
            ww: true
        };
        var opts = [];

        Object.keys(resizer_mode).forEach(function (k) {
            if (resizer_mode[k]) opts.push(k);
        });

        resizer = window.pica({ features: opts });

        resizer.resize(originalCanvas, zoomedCanvas, {
            unsharpAmount: 80,
            unsharpRadius: 0.6,
            unsharpThreshold: 2
        })
        .then(result => {
            // Resizing completed
            // Read resized image data
            var picaImageData = zoomedCanvas.toDataURL("image/jpeg");
            //console.log("picaImageData", picaImageData);
            $scope.DetectFaces(picaImageData);
        });
    };

    $scope.DetectFaces = function (imageData) {
        $scope.waitingDetect = false;
        $scope.detecting = true;
        //console.log("image",imageData);
        if (!$CspUtils.IsNullOrEmpty(imageData) && $scope.fileSizeValid == true) {
            var data = {};
            data.ProjectCode = "VEC";
            data.EnableAutoBrightness = true;
            data.TransactionID = $scope.model.CitizenID + "-" + Date.now();  // "TEST00001";
            data.SourceBase64 = imageData;
            $scope.imageValid = false;
            //console.log("data ", data);
            var url = faceApiUrl;
            $http.post(url, data)
                    .then(function (result) {
                        //console.log("result", result);
                        if (result.data.Success && result.data.IsFaceDetected && $CspUtils.IsNullOrEmpty(result.data.ErrorCode)) {
                            $scope.imageValid = true;
                            $scope.model.ImageBase64 = imageData;
                        }
                        else {
                            $scope.imageValid = false;
                            $scope.model.ImageBase64 = "";
                            if (result.data.ErrorCode == "UNKNOWN_ERROR" || result.data.ErrorCode == "IMAGE_CONVERTER") {
                                $scope.ErrorMessImage = "รูปถ่ายไม่ถูกต้อง";
                            } else {
                                $scope.ErrorMessImage = result.data.ErrorMessage;
                            }
                        }
                    }
                    , function (err) {
                        $scope.imageValid = false;
                        $scope.ErrorMessImage = "เกิดข้อผิดพลาดในการตรวจสอบรูปถ่าย";
                        $scope.model.ImageBase64 = "";
                        console.log("DetectFaces Error", err);
                        alert("เกิดข้อผิดพลาดในการตรวจสอบรูปถ่าย");
                    }).finally(function () { $scope.detecting = false; $scope.waitingDetect = false; });
        }
    };

    $scope.contentChanged = function (file) {
        $scope.hasFile = true;
        var myFileSelected = file.files[0];
        $scope.files = myFileSelected;

        var errorList = [];
        if ($scope.files.type != 'image/jpeg' && $scope.files.type != 'image/png') {
            errorList.push("ประเภทไฟล์ต้องเป็น .jpg หรือ .png เท่านั้น");
        }
        //if ($scope.files.size > 100000 || $scope.files.size < 36000) {
        if ($scope.files.size < 36000) {
            errorList.push("ขนาดไฟล์ต้องไม่ต่ำกว่า 40KB");
        }
        if (errorList.length > 0) {
            var msg = errorList.join('\n');
            //var msg = errorList[0];
            $scope.fileSizeValid = false;
            $scope.reset();
            $scope.hasFile = false;
            alertWarning(msg);
        } else {
            $scope.fileSizeValid = true;
        }

        var $preview = document.querySelector(".cropit-preview-image-container");
        $preview.classList.add("choosed");
    };

    $scope.reset = function () {
        angular.element(document.querySelector("#customFile")).val("");
        $scope.files = null;
    };

    $scope.checkDevice = function () {
        //console.info(navigator.userAgent);
        if (btnCamera == "True") {
            if (/android/i.test(navigator.userAgent.toLowerCase())) {
                $scope.takePhoto = false;
            }
            else if (/ipad|iphone|ipod/i.test(navigator.userAgent.toLowerCase())) {
                $scope.takePhoto = false;
            }
            else {
                $scope.takePhoto = true;
            }
        } else {
            $scope.takePhoto = false;
        }
    };

    $scope.openDialogWarning = function () {
        //console.log($scope.model.ClassLavelID);
        var modalInstance = $uibModal.open({
            templateUrl: "_ConfirmRegisterWarning.html",
            controller: "dialogWarningController",
            size: "xl",
            backdrop: 'static',
            resolve: {
                params: function () {
                    return {
                        FullName: $scope.model.PrefixName + $scope.model.FirstName + " " + $scope.model.LastName,
                        ClassLevel: $scope.model.ClassLavelName,
                        CenterExamName: $scope.model.CenterExamName,
                        RegClassName: $scope.model.RegClassName,
                        ClassGroupName: $scope.model.ClassGroupName
                    };
                }
            }
        });

        modalInstance.result.then(function (result) {
            $scope.checked = true;
            $scope.submitConfirm();
        }, function () {
            $scope.checked = false;
        });
    };

    $scope.openDialog = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "_CheckCitizenWarning.html",
            controller: "dialogController",
            size: "lg",
            backdrop: 'static'
        });

        modalInstance.result.then(function (result) {
            //true
        }, function () {
            //false
        });
    };

    //$scope.openDialog = function () {
    //    var modalInstance = $uibModal.open({
    //        templateUrl: DIALOG_URL,
    //        controller: "dialogController",
    //        backdrop: 'static',
    //        size: "xl"
    //    });

    //    modalInstance.result.then(function (result) {
    //        $scope.smsStatus = result.smsStatus;
    //        $scope.model.ConsentDate = result.consentDate;
    //    }, function () {
    //        $scope.smsStatus = false;
    //        $scope.model.ConsentDate = '';
    //    });
    //};


    $scope.openDialogDetail = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "_SMSDialogModalDetail.html",
            controller: "dialogDetailController",
            backdrop: 'static',
            size: "lg"
        });

        modalInstance.result.then(function (result) {
            //
        }, function () {
            //
        });
    };

    $scope.reloadDocumentList = function () {
        $scope.$broadcast('reload-documentlist', {
            "testingClassID": $scope.model.ClassGroupID,
            "RegClassID": $scope.model.RegClassID,
            "otherKey": $scope.model.DocTypeID,
            "mode": "Register"
        });       // broadcast เพื่อให้อัปโหลดเอกสารมัน refresh ตัวเอง
    }

    $scope.uploadChild = {};


    $scope.init();

}]);


