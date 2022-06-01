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
app.run(function ($http) {
    // Sends this header with any AJAX request
    //$http.defaults.headers.common['Access-Control-Allow-Origin'] = 'http://192.168.72.243:5000';
    $http.defaults.headers.common['Access-Control-Allow-Origin'] = 'http://10.10.20.252';
});

app.controller("registerController", ["$scope", "$uibModal", "CspUtils", "LookupService", "$http", "$filter", function ($scope, $uibModal, $CspUtils, $LookupService, $http, $filter) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = EXAM_TYPE;
    $scope.yearNo = '';
    $scope.register = {};
    $scope.remarks = [];
    //$scope.examCenters = [];
    $scope.lstProvinces = [];
    $scope.prefixes = [];
    $scope.genders = [];
    $scope.religions = [];
    $scope.statuses = [];
    $scope.lstEducationals = [];
    $scope.schGroups = [];
    $scope.lstSchool = [];
    $scope.schLocations = [];
    $scope.lstDegrees = [];
    $scope.lstOccupation = [];
    $scope.lstDefHelps = [];
    $scope.lstAmphs = [];
    $scope.lstTmbls = [];
    $scope.lstTbbAmphs = [];
    $scope.lstTbbTmbls = [];
    $scope.lstStdYears = [];
    $scope.model = oldData;
    $scope.model.TestTypeID = $scope.testTypeID;
    $scope.model.ExamType = $scope.examType;
    $scope.age = [];
    $scope.closeEnrollDate = '';
    $scope.lstDay = ["--", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"];
    $scope.lstMonth = ["--", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.lstYear = [];
    //$scope.smsStatus = $scope.model.SMSStatus == 'Y' ? true : false;
    $scope.eduFlag = $scope.model.EduFlag == 'Y' ? true : false;
    $scope.tbbFlag = $scope.model.TbbFlag == 'Y' ? true : false;
    $scope.occFlag = false;
    $scope.classID = $scope.model.RegClassID;
    $scope.isLoading = false;
    $scope.isLoadingImage = false;
    $scope.fileSizeValid = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;
    $scope.imageValid = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;
    $scope.hasFile = !$CspUtils.IsNullOrEmpty($scope.model.ImageBase64) ? true : false;
    $scope.files = null;
    $scope.checked = false;
    if ($scope.model.Mode == "Register") {
        $scope.takePhoto = (btnCamera == "True" ? true : false);
    }

    $scope.init = function () {
        //$scope.getExamCenter();
        if ($scope.model.Mode == "Register") {
            $scope.openDialog();
        }
        $scope.getProvinces();
        $scope.getPrefixes();
        $scope.getReligions();
        $scope.getStatuses();
        // $scope.getRemark();
        $scope.getEducationals();
        $scope.getOccupation();
        $scope.getEduYear();
        $scope.getDefectiveHelp();
        $scope.provinceChange();
        $scope.amphurChange();
        $scope.tbbprovinceChange();
        $scope.tbbamphurChange();
        $scope.carreerChange();
        $scope.isLoading = true;
        var url = API_ROOT + "/Register/EnrollDate?testTypeID=" + $scope.testTypeID + "&scheduleTypeCode=" + "ENROLL_CLOSE_DATE";
        var config = {};
        $http.get(url, config)
            .then(function (result) {
                $scope.closeEnrollDate = result.data;
                $scope.getYearList();

            }
            , function (err) {
                $scope.closeEnrollDate = '';
                console.log("Save Doc Error", err);
                alert("เกิดข้อผิดพลาด");
            }).finally(function () { $scope.isLoading = false; });

        //อัปโหลดรูป
        if ($scope.model.Mode == "Register") {

            $(function () {
                $('.image-editor').cropit({
                    allowDragNDrop: false,
                    // maxZoom: 0.3,
                    maxZoom: 1,
                    minZoom: 'fit',
                    smallImage: 'stretch',
                    previewSize: { width: 100, height: 200 },
                    onImageLoaded: function () {
                        $scope.DetectFaces();
                    }
                });

                $('#btnCustomFile').click(function () {
                    $('.cropit-image-input').click();
                });

                $('.rotate-cw').click(function () {
                    $('.image-editor').cropit('rotateCW');
                    $scope.DetectFaces();
                });

                $('.rotate-ccw').click(function () {
                    $('.image-editor').cropit('rotateCCW');
                    $scope.DetectFaces();
                });

                $('.cropit-preview-image').mouseup(function () {
                    $scope.DetectFaces();
                });

                $('.cropit-image-zoom-input').mouseup(function () {
                    $scope.DetectFaces();
                });

                var captureVideoButton = document.querySelector('.capture-button');
                var screenshotButton = document.querySelector('#screenshot-button');
                var img = document.querySelector('#screenshot img');
                var video = document.querySelector('#screenshot video');

                var canvas = document.createElement('canvas');

                captureVideoButton.onclick = function () {
                    navigator.mediaDevices.enumerateDevices().
                      then(function (devices) {
                          devices.forEach(function (device) {
                              if (device.kind == "videoinput") {
                                  $scope.videoDevices = device;
                              }
                          });
                          const constraints = {
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
                                  deviceId: {
                                      exact: $scope.videoDevices.deviceId
                                  }
                              }
                          };
                          navigator.mediaDevices.getUserMedia(constraints).
                            then(handleSuccess).catch(handleError);
                      });
                };

                screenshotButton.onclick = video.onclick = function () {
                    canvas.width = video.videoWidth;
                    canvas.height = video.videoHeight;
                    canvas.getContext('2d').drawImage(video, 0, 0);
                    var img = canvas.toDataURL('image/png');
                    img.src = img;
                    $('.image-editor').cropit('imageSrc', img);
                    $scope.hasFile = true;
                    $scope.fileSizeValid = true;
                    $('#cameraModal').modal('toggle');
                    $scope.reset();
                };

                function handleSuccess(stream) {
                    screenshotButton.disabled = false;
                    video.srcObject = stream;
                };

                function handleError(stream) {
                };

                if (!$CspUtils.IsNullOrEmpty($scope.model.ImageBase64)) {
                    $('.image-editor').cropit('imageSrc', $scope.model.ImageBase64);
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
        var errorList = [];
        //if ($CspUtils.IsNullOrEmpty($scope.model.CenterExamID)) {
        //    errorList.push("ยังไม่ได้ระบุ ศูนย์สอบ");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.CitizenProvinceID)) {
            errorList.push("ยังไม่ได้ระบุ จังหวัดที่ออกบัตรประชาชน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.IssueDate)) {
            errorList.push("ยังไม่ได้ระบุ วันที่ออกบัตรประชาชน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ExpiredDate)) {
            errorList.push("ยังไม่ได้ระบุ วันที่บัตรหมดอายุ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Gender)) {
            errorList.push("ยังไม่ได้ระบุ เพศ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.NationName)) {
            errorList.push("ยังไม่ได้ระบุ สัญชาติ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.RaceName)) {
            errorList.push("ยังไม่ได้ระบุ เชื้อชาติ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ReligionID)) {
            errorList.push("ยังไม่ได้ระบุ ศาสนา");
        }
        if ($scope.model.ReligionID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.ReligionName)) {
            errorList.push("ยังไม่ได้ระบุ ศาสนาอื่นๆ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.StatusID)) {
            errorList.push("ยังไม่ได้ระบุ สถานภาพ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EduPrefixID)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้า ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($scope.model.EduPrefixID == '999999' && $CspUtils.IsNullOrEmpty($scope.model.EduPrefixName)) {
            errorList.push("ยังไม่ได้ระบุ คำนำหน้าอื่นๆ ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EduFirstName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อ ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EduLastName)) {
            errorList.push("ยังไม่ได้ระบุ นามสกุล ในคุณวุฒิการศึกษาที่ใช้สมัครสอบ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ClassLavelID)) {
            errorList.push("ยังไม่ได้ระบุ วุฒิที่ใช้ในการสมัคร");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.DegreeName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อปริญญา");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.SchoolName)) {
            errorList.push("ยังไม่ได้ระบุ สถานศึกษาของวุฒิที่ใช้ในการสมัคร");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.MajorName)) {
            errorList.push("ยังไม่ได้ระบุ สาขาวิชา/วิชาเอก");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.GraduateDate)) {
            errorList.push("ยังไม่ได้ระบุ วัน/เดือน/ปี ที่จบการศึกษา");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.GPA)) {
            errorList.push("ยังไม่ได้ระบุ เกรดเฉลี่ยสะสม");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ClassHighestLavelID)) {
            errorList.push("ยังไม่ได้ระบุ วุฒิการศึกษาสูงสุด");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ClassHighestLavelSchoolName)) {
            errorList.push("ยังไม่ได้ระบุ สถานศึกษาของวุฒิการศึกษาสูงสุด");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.DefectiveFlag)) {
            errorList.push("ยังไม่ได้ระบุ ข้อมูลความพิการ");
        }
        if ($scope.model.DefectiveFlag == 'Y' && $CspUtils.IsNullOrEmpty($scope.model.DefectiveHelpID)) {
            errorList.push("ยังไม่ได้ระบุ ความช่วยเหลือ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.OccuupationID)) {
            errorList.push("ยังไม่ได้ระบุ อาชีพปัจจุบัน");
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
        if ($scope.model.OccuupationID != "5" && $scope.model.OccuupationID != "4") {
            if ($CspUtils.IsNullOrEmpty($scope.model.OfficeMobile)) {
                errorList.push("ยังไม่ได้ระบุ โทรศัพท์เคลื่อนที่ที่ทำงาน");
            }
        }
        //if ($CspUtils.IsNullOrEmpty($scope.model.OfficeTel)) {
        //    errorList.push("ยังไม่ได้ระบุ โทรศัพท์ที่ทำงาน");
        //}
        if ($CspUtils.IsNullOrEmpty($scope.model.Salary)) {
            errorList.push("ยังไม่ได้ระบุ อัตราเงินเดือน");
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
        if ($CspUtils.IsNullOrEmpty($scope.model.TbbTel)) {
            errorList.push("ยังไม่ได้ระบุ โทรศัพท์");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.AddrNo)) {
            errorList.push("ยังไม่ได้ระบุ บ้านเลขที่");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Village)) {
            errorList.push("ยังไม่ได้ระบุ ชั้น/อาคาร/หมู่บ้าน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Moo)) {
            errorList.push("ยังไม่ได้ระบุ หมู่ที่");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Soi)) {
            errorList.push("ยังไม่ได้ระบุ ตรอก/ซอย/แยก");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Road)) {
            errorList.push("ยังไม่ได้ระบุ ถนน");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.ProvID)) {
            errorList.push("ยังไม่ได้ระบุ จังหวัด");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.AmphID)) {
            errorList.push("ยังไม่ได้ระบุ เขต/อำเภอ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.TmblID)) {
            errorList.push("ยังไม่ได้ระบุ แขวง/ตำบล");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Zipcode)) {
            errorList.push("กรุณาระบุ รหัสไปรษณีย์ ให้ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Tel)) {
            errorList.push("ยังไม่ได้ระบุ โทรศัพท์");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Mobile)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.Email)) {
            errorList.push("กรุณาระบุ อีเมล ให้ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.SpecialSkills)) {
            errorList.push("ยังไม่ได้ระบุ ความรู้ความสามารถพิเศษ");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyFirstName)) {
            errorList.push("ยังไม่ได้ระบุ ชื่อบุคคลที่ติดต่อได้");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyLastName)) {
            errorList.push("ยังไม่ได้ระบุ นามสกุลบุคคลที่ติดต่อได้");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyRelation)) {
            errorList.push("ยังไม่ได้ระบุ ความสัมพันธ์บุคคลที่ติดต่อได้");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyTel)) {
            errorList.push("ยังไม่ได้ระบุ โทรศัพท์บ้าน/สถานที่ทำงาน บุคคลที่ติดต่อได้");
        }
        if ($CspUtils.IsNullOrEmpty($scope.model.EmergencyMoblie)) {
            errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่บุคคลที่ติดต่อได้ ให้ถูกต้อง");
        }
        if ($scope.hasFile == false) {
            errorList.push("กรุณาเลือกไฟล์");
        }
        if ($scope.fileSizeValid == false) {
            errorList.push("ขนาดไฟล์ไม่ถูกต้อง");
        }
        if ($scope.imageValid == false) {
            errorList.push("รูปถ่ายไม่ถูกต้อง");
        }
        if ($CspUtils.IsNullOrEmpty($scope.confirm) || $scope.confirm == false) {
            errorList.push("กรุณายืนยันการสมัคร");
        }
        //if ($scope.smsStatus == true && $CspUtils.IsNullOrEmpty($scope.model.SMSMobile)) {
        //    errorList.push("กรุณาระบุ หมายเลขโทรศัพท์เคลื่อนที่ ให้ถูกต้อง");
        //}
        //if ($scope.smsStatus == true && $CspUtils.IsNullOrEmpty($scope.model.SMSEmail)) {
        //    errorList.push("กรุณาระบุ อีเมล ให้ถูกต้อง");
        //}

        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            //var msg = errorList.join('\n');
            var msg = errorList[0];
            alert(msg);
            e.preventDefault();
            return;
        }

        //if ($scope.smsStatus) {
        //    $scope.model.SMSStatus = 'Y'
        //} else {
        //    $scope.model.SMSStatus = 'N'
        //}

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

        if ($scope.imageValid == false) {
            // Call FaceAPI
            // CallBack => { document.getElementById("register").submit(); }
        } else {
            document.getElementById("register").submit();
        }

        
    };

    $scope.submitConfirm = function (e) {

        if (!$scope.checked) {
            e.preventDefault();
            $scope.openDialogWarning();
            return;
        }

        document.getElementById("confirmRegister").submit();
    };

    //$scope.getRemark = function () {
    //    $scope.isLoading = true;
    //    var url = API_ROOT + "/Master/Remarks";
    //    $http.get(url, { cache: false }).then(
    //        function (result) {
    //            $scope.remarks = result.data;
    //        }, function (err) {
    //            $scope.remarks = [];
    //        }).finally(function () { $scope.isLoading = false; });
    //};

    //$scope.getExamCenter = function () {
    //    $scope.isLoading = true;
    //    $LookupService.getLookupExamCenters($scope.testTypeID).then(function (result) {
    //        $scope.examCenters = result.data;
    //    }, function (err) {
    //        console.log("Error on getLookupExamCenters", err);
    //        $scope.examCenters = [];
    //    }).finally(function () { $scope.isLoading = false; });
    //};


    $scope.getProvinces = function () {
        $scope.isLoading = true;
        $LookupService.getLookupProvince().then(function (result) {
            $scope.lstProvinces = result.data;
        }, function (err) {
            console.log("Error on getLookupProvince", err);
            $scope.lstProvinces = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.provinceChange = function () {
        //$scope.model.AmphID = "";
        $scope.amphurChange();
        if (!$CspUtils.IsNullOrEmpty($scope.model.ProvID)) {
            $scope.SetLoading("lstAmphs", true);
            $LookupService.getLookupAmphur($scope.model.ProvID).then(function (result) {
                $scope.lstAmphs = result.data;
            }, function (err) {
                console.log("Error on getLookupAmphur", err);
                $scope.lstAmphs = [];
            }).finally(function () { $scope.SetLoading("lstAmphs", false); });
        } else {
            $scope.lstAmphs = [];
        }
    };

    $scope.amphurChange = function () {
        //$scope.model.TmblID = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.AmphID)) {
            $scope.SetLoading("lstTmbls", true);
            $LookupService.getLookupTumbon($scope.model.AmphID).then(function (result) {
                $scope.lstTmbls = result.data;
            }, function (err) {
                console.log("Error on getLookupTumbon", err);
                $scope.lstTmbls = [];
            }).finally(function () { $scope.SetLoading("lstTmbls", false); });
        } else {
            $scope.lstTmbls = [];
        }
    };


    $scope.tbbprovinceChange = function () {
        //$scope.model.AmphID = "";
        $scope.tbbamphurChange();
        if (!$CspUtils.IsNullOrEmpty($scope.model.TbbProvID)) {
            $scope.SetLoading("lstTbbAmphs", true);
            $LookupService.getLookupAmphur($scope.model.TbbProvID).then(function (result) {
                $scope.lstTbbAmphs = result.data;
            }, function (err) {
                console.log("Error on getLookupAmphur", err);
                $scope.lstTbbAmphs = [];
            }).finally(function () { $scope.SetLoading("lstTbbAmphs", false); });
        } else {
            $scope.lstAmphs = [];
        }
    };

    $scope.tbbamphurChange = function () {
        //$scope.model.TmblID = "";
        if (!$CspUtils.IsNullOrEmpty($scope.model.TbbAmphID)) {
            $scope.SetLoading("lstTbbTmbls", true);
            $LookupService.getLookupTumbon($scope.model.TbbAmphID).then(function (result) {
                $scope.lstTbbTmbls = result.data;
            }, function (err) {
                console.log("Error on getLookupTumbon", err);
                $scope.lstTbbTmbls = [];
            }).finally(function () { $scope.SetLoading("lstTbbTmbls", false); });
        } else {
            $scope.lstTmbls = [];
        }
    };

    $scope.getPrefixes = function () {
        $scope.isLoading = true;
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
        $LookupService.getLookupGender().then(function (result) {
            $scope.genders = result.data;
            $scope.model.Gender = $scope.prefixes.find(x => x.id === $scope.model.PrefixID).gender;
        }, function (err) {
            console.log("Error on getLookupGender", err);
            $scope.genders = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getReligions = function () {
        $scope.isLoading = true;
        $LookupService.getLookupReligions().then(function (result) {
            $scope.religions = result.data;
        }, function (err) {
            console.log("Error on getLookupReligions", err);
            $scope.religions = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getStatuses = function () {
        $scope.isLoading = true;
        $LookupService.getLookupStatuses().then(function (result) {
            $scope.statuses = result.data;
        }, function (err) {
            console.log("Error on getLookupStatuses", err);
            $scope.statuses = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getEducationals = function () {
        $scope.isLoading = true;
        $LookupService.getLookupEducationals($scope.classID).then(function (result) {
            $scope.lstEducationals = result.data;
        }, function (err) {
            console.log("Error on getLookupEducationals", err);
            $scope.lstEducationals = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getOccupation = function () {
        $scope.isLoading = true;
        $LookupService.getLookupOccupation().then(function (result) {
            $scope.lstOccupation = result.data;
        }, function (err) {
            console.log("Error on getLookupOccupation", err);
            $scope.lstOccupation = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getEduYear = function () {
        $scope.isLoading = true;
        $LookupService.getLookupEduYears().then(function (result) {
            $scope.lstStdYears = result.data;
        }, function (err) {
            console.log("Error on getLookupEduYears", err);
            $scope.lstStdYears = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.getDefectiveHelp = function () {
        $scope.isLoading = true;
        $LookupService.getLookupDefectiveHelp().then(function (result) {
            $scope.lstDefHelps = result.data;
        }, function (err) {
            console.log("Error on getLookupDefectiveHelp", err);
            $scope.lstDefHelps = [];
        }).finally(function () { $scope.isLoading = false; });
    };

    $scope.carreerChange = function () {
        if (!$CspUtils.IsNullOrEmpty($scope.model.OccuupationID)) {
            if ($scope.model.OccuupationID == "5" || $scope.model.OccuupationID == "4") {
                $scope.occFlag = true;
                $scope.model.PositionName = "-";
                $scope.model.StationName = "-";
                $scope.model.DepartmentName = "-";
                $scope.model.OfficeMobile = "";
                $scope.model.OfficeTel = "";
                $scope.model.OfficeTelEx = "";
                $scope.model.Salary = "0.00";
            } else {
                $scope.occFlag = false;
                $scope.model.PositionName = "";
                $scope.model.StationName = "";
                $scope.model.DepartmentName = "";
                $scope.model.OfficeMobile = "";
                $scope.model.OfficeTel = "";
                $scope.model.OfficeTelEx = "";
                $scope.model.Salary = "";
            }
        }
    };

    $scope.getNameFromID = function (lst, id) {
        if (lst.length > 0 && !$CspUtils.IsNullOrEmpty(id)) {
            var name = $filter('filter')(lst, {
                id: id
            });
            return name[0].text;
        }
    };

    $scope.eduFlagChange = function () {
        if (eduFlag) {
            $scope.model.EduPrefixID = $scope.model.PrefixID;
            $scope.model.EduPrefixName = $scope.model.PrefixName;
            $scope.model.EduFirstName = $scope.model.FirstName;
            $scope.model.EduLastName = $scope.model.LastName;
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

    $scope.setFocus = function (elementid) {
        $CspUtils.SetFocus(elementid);
    };

    //$scope.smsServiceChange = function () {
    //    if ($scope.smsStatus) {
    //        $scope.openDialog();
    //    } else {
    //        $scope.model.ConsentDate = '';
    //    }
    //};

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

    $scope.DetectFaces = function () {
        var imageData = $('.image-editor').cropit('export', {
            type: 'image/jpeg',
            quality: 1,
            originalSize: false
        });

        // TODO : ลองเปลี่ยนวิธี ExportFile ไป Save
        // TODO : ถ้ากำลังตรวจอยู่ ยังไม่ต้องส่งไปตรวจ  
        // TODO : ระหว่างตรวจอยู่  น่าจะต้อง block ปุ่มบาง action เช่นหมุนรูป ขยายรูป  ขยับรูป

        //console.log("image",imageData);
        if (!$CspUtils.IsNullOrEmpty(imageData) && $scope.fileSizeValid == true) {
            var data = {};
            data.ProjectCode = "OCS";
            data.EnableAutoBrightness = true;
            data.TransactionID = "TEST00001";
            data.SourceBase64 = imageData;
            $scope.isLoadingImage = true;
            $scope.imageValid = false;
            var url = faceApiUrl;
            $http.post(url, data)
                    .then(function (result) {
                        //console.log("result", result);
                        if (result.data.Success) {
                            $scope.imageValid = true;
                            $scope.model.ImageBase64 = imageData;
                        }
                        else {
                            $scope.imageValid = false;
                            $scope.model.ImageBase64 = "";
                        }
                    }
                    , function (err) {
                        console.log("DetectFaces Error", err);
                        alert("เกิดข้อผิดพลาด");
                    }).finally(function () { $scope.isLoadingImage = false; });
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
        if ($scope.files.size < 3600) {
            errorList.push("ขนาดไฟล์ต้องไม่ต่ำกว่า 40 kb");
        }
        if (errorList.length > 0) {
            var msg = errorList.join('\n');
            //var msg = errorList[0];
            $scope.fileSizeValid = false;
            $scope.reset();
            $scope.hasFile = false;
            alert(msg);
        } else {
            $scope.fileSizeValid = true;
        }
    };

    $scope.reset = function () {
        angular.element(document.querySelector("#customFile")).val("");
        $scope.files = null;
    };

    $scope.checkDevice = function () {
        //console.info(navigator.userAgent);
        if (btnCamera == "True") {
            if (/android/i.test(navigator.userAgent.toLowerCase())) {
                $scope.takePhoto = true;
            }
            else if (/ipad|iphone|ipod/i.test(navigator.userAgent.toLowerCase())) {
                $scope.takePhoto = true;
            }
            else {
                $scope.takePhoto = true;
            }
        } else {
            $scope.takePhoto = false;
        }
    };

    $scope.openDialogWarning = function () {
        console.log($scope.model.ClassLavelID);
        var modalInstance = $uibModal.open({
            templateUrl: "_ConfirmRegisterWarning.html",
            controller: "dialogWarningController",
            size: "xl",
            backdrop: 'static',
            resolve: {
                params: function () {
                    return {
                        FullName: $scope.getNameFromID($scope.prefixes, $scope.model.PrefixID) + $scope.model.FirstName + " " + $scope.model.LastName,
                        ClassLevel: $scope.getNameFromID($scope.lstEducationals, $scope.model.ClassLavelID),
                        CenterExamName: $scope.model.CenterExamName,
                        RegClassName: $scope.model.RegClassName
                    };
                }
            }
        });

        modalInstance.result.then(function (result) {
            $scope.checked = true;
            document.getElementById("confirmRegister").submit();
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


    //$scope.openDialogDetail = function () {
    //    var modalInstance = $uibModal.open({
    //        templateUrl: "_SMSDialogModalDetail.html",
    //        controller: "dialogDetailController",
    //        backdrop: 'static',
    //        size: "xl"
    //    });

    //    modalInstance.result.then(function (result) {
    //        //
    //    }, function () {
    //        //
    //    });
    //};

    $scope.init();

}]);


