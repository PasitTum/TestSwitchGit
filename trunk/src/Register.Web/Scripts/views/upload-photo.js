//var app = angular.module("uploadPictureModule", ["ui.bootstrap", "cspModule"]);
//Resolve "Required attribute Not working with File input"
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
//End Resolve
app.controller("uploadPhotoController", ["$scope", "CspUtils", "LookupService", "$http", "$interval", function ($scope, $CspUtils, $LookupService, $http, $interval) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.uploadStatus = {};
    $scope.isLoading = false;
    $scope.showUpload = false;
    $scope.showStatus = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.fileSizeValid = false;
    $scope.imageValid = false;
    $scope.hasFile = false;
    $scope.files = null;
    $scope.takePhoto = (btnCamera == "True" ? true : false);
    $scope.checked = false;
    $scope.detectTimer = parseInt(detectTimer) * 1000;
    $scope.takePhoto = (btnCamera == "True" ? true : false);
    var timer;

    $scope.init = function () {

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

        var url = TEST_TYPE_ROOT + "/Photo/Upload";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        $scope.uploadStatus = result.data.Data;
                        $scope.resetImage();

                        if (!$CspUtils.IsNullOrEmpty($scope.uploadStatus.ENROLL_NO)) {
                            if ($CspUtils.IsNullOrEmpty($scope.uploadStatus.UPLOAD_FLAG) || $scope.uploadStatus.UPLOAD_FLAG == "Y") {
                                $scope.showUpload = false;
                                $scope.showStatus = true;
                                $scope.isInfo = false;
                            } else {
                                $scope.showUpload = true;
                                $scope.showStatus = false;
                                $scope.isInfo = false;
                            }
                        }
                        else {
                            $scope.showUpload = false;
                            $scope.showStatus = false;
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

    $scope.resetImage = function () {
        angular.element(document.querySelector("#customFile")).val("");
        $scope.model.ImageBase64 = "";
        $scope.fileSizeValid = false;
        $scope.imageValid = false;
        $scope.hasFile = false;
        $scope.files = null;
        $scope.checked = false;
        var pre = $('.cropit-preview-image');
        pre.attr('src', '');
    }

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
            data.ProjectCode = "CDD";
            data.EnableAutoBrightness = true;
            data.TransactionID = $scope.model.CitizenID + "-" + Date.now();  // "TEST00001";
            data.SourceBase64 = imageData;
            $scope.imageValid = false;
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

    $scope.confirmUpload = function () {
        var errorList = [];
        $scope.checked = true;
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
        // มีบางอันไม่ผ่าน validation
        if (errorList.length > 0) {
            //var msg = errorList.join('\n');
            var msg = errorList.slice(0, 5).join('<br/>');
            alert(msg);
            $scope.checked = false;
            return;
        }

        if ($scope.waitingDetect == true) {

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
                    data.ProjectCode = "CDD";
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
                                    $('#detectModal').modal('hide');

                                    var data = {};
                                    data.TestTypeID = $scope.testTypeID;
                                    data.CitizenID = $scope.model.CitizenID;
                                    data.EnrollNo = $scope.uploadStatus.ENROLL_NO;
                                    data.ImageBase64 = $scope.model.ImageBase64;
                                    $scope.isLoading = true;
                                    var url = API_ROOT + "/Photo/InsertPhoto";
                                    $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
                                    var config = {
                                        headers: {
                                            'RequestVerificationToken': $scope.antiForgeryToken,
                                            'X-Requested-With': 'XMLHttpRequest'
                                        }
                                    };

                                    $http.post(url, data, config)
                                            .then(function (result) {
                                                if (result.data.Success) {
                                                    alert("อัปโหลดรูปถ่ายสำเร็จ");
                                                    $scope.isLoading = true;
                                                    var url = API_ROOT + "/Inquiry/UploadStatus?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
                                                    var config = {};
                                                    $http.get(url, config)
                                                        .then(function (subResult) {
                                                            $scope.uploadStatus = subResult.data;
                                                            if ($scope.uploadStatus) {
                                                                if ($CspUtils.IsNullOrEmpty($scope.uploadStatus.UPLOAD_FLAG) || $scope.uploadStatus.UPLOAD_FLAG == "N") {
                                                                    $scope.showUpload = true;
                                                                    $scope.showStatus = false;
                                                                    $scope.isInfo = false;
                                                                } else {
                                                                    $scope.showUpload = false;
                                                                    $scope.showStatus = true;
                                                                    $scope.isInfo = false;
                                                                }
                                                            }
                                                            else {
                                                                $scope.showUpload = false;
                                                                $scope.showStatus = false;
                                                                $scope.isInfo = true;
                                                            }
                                                        }
                                                        , function (err) {
                                                            $scope.uploadStatus = {};
                                                            console.log("Save Doc Error", err);
                                                            alert("เกิดข้อผิดพลาด");
                                                        }).finally(function () { $scope.isLoading = false; });

                                                }
                                                else {
                                                    console.log(result.data);
                                                    alert("อัปโหลดรูปถ่ายไม่สำเร็จ");
                                                }
                                            }
                                            , function (err) {
                                                console.log("InsertPhotos Error", err);
                                                alert("เกิดข้อผิดพลาด");
                                            }).finally(function () { $scope.isLoading = false; });
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
            var data = {};
            data.TestTypeID = $scope.testTypeID;
            data.CitizenID = $scope.model.CitizenID;
            data.EnrollNo = $scope.uploadStatus.ENROLL_NO;
            data.ImageBase64 = $scope.model.ImageBase64;
            $scope.isLoading = true;
            var url = API_ROOT + "/Photo/InsertPhoto";
            $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
            var config = {
                headers: {
                    'RequestVerificationToken': $scope.antiForgeryToken,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            };

            $http.post(url, data, config)
                    .then(function (result) {
                        if (result.data.Success) {
                            alert("อัปโหลดรูปถ่ายสำเร็จ");
                            $scope.isLoading = true;
                            var url = API_ROOT + "/Inquiry/UploadStatus?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
                            var config = {};
                            $http.get(url, config)
                                .then(function (subResult) {
                                    $scope.uploadStatus = subResult.data;
                                    if ($scope.uploadStatus) {
                                        if ($CspUtils.IsNullOrEmpty($scope.uploadStatus.UPLOAD_FLAG) || $scope.uploadStatus.UPLOAD_FLAG == "N") {
                                            $scope.showUpload = true;
                                            $scope.showStatus = false;
                                            $scope.isInfo = false;
                                        } else {
                                            $scope.showUpload = false;
                                            $scope.showStatus = true;
                                            $scope.isInfo = false;
                                        }
                                    }
                                    else {
                                        $scope.showUpload = false;
                                        $scope.showStatus = false;
                                        $scope.isInfo = true;
                                    }
                                }
                                , function (err) {
                                    $scope.uploadStatus = {};
                                    console.log("Save Doc Error", err);
                                    alert("เกิดข้อผิดพลาด");
                                }).finally(function () { $scope.isLoading = false; });

                        }
                        else {
                            console.log(result.data);
                            alert("อัปโหลดรูปถ่ายไม่สำเร็จ");
                        }
                    }
                    , function (err) {
                        console.log("InsertPhotos Error", err);
                        alert("เกิดข้อผิดพลาด");
                    }).finally(function () { $scope.isLoading = false; });
        }

    };

    //$scope.uploadPhoto = function () {
    //    var data = {};
    //    data.TestTypeID = $scope.testTypeID;
    //    data.CitizenID = $scope.model.CitizenID;
    //    data.EnrollNo = $scope.uploadStatus.ENROLL_NO;
    //    data.ImageBase64 = $scope.model.ImageBase64;
    //    $scope.isLoading = true;
    //    var url = API_ROOT + "/Photo/InsertPhoto";
    //    $scope.antiForgeryToken = document.querySelector("input[name='__RequestVerificationToken']").value;
    //    var config = {
    //        headers: {
    //            'RequestVerificationToken': $scope.antiForgeryToken,
    //            'X-Requested-With': 'XMLHttpRequest'
    //        }
    //    };

    //    $http.post(url, data, config)
    //            .then(function (result) {
    //                if (result.data.Success) {
    //                    alert("อัปโหลดรูปถ่ายสำเร็จ");
    //                    $scope.isLoading = true;
    //                    var url = API_ROOT + "/Inquiry/UploadStatus?testTypeID=" + $scope.testTypeID + "&citizenID=" + $scope.model.CitizenID;
    //                    var config = {};
    //                    $http.get(url, config)
    //                        .then(function (subResult) {
    //                            $scope.uploadStatus = subResult.data;
    //                            if ($scope.uploadStatus) {
    //                                if ($CspUtils.IsNullOrEmpty($scope.uploadStatus.UPLOAD_FLAG) || $scope.uploadStatus.UPLOAD_FLAG == "N") {
    //                                    $scope.showUpload = true;
    //                                    $scope.showStatus = false;
    //                                    $scope.isInfo = false;
    //                                } else {
    //                                    $scope.showUpload = false;
    //                                    $scope.showStatus = true;
    //                                    $scope.isInfo = false;
    //                                }
    //                            }
    //                            else {
    //                                $scope.showUpload = false;
    //                                $scope.showStatus = false;
    //                                $scope.isInfo = true;
    //                            }
    //                        }
    //                        , function (err) {
    //                            $scope.uploadStatus = {};
    //                            console.log("Save Doc Error", err);
    //                            alert("เกิดข้อผิดพลาด");
    //                        }).finally(function () { $scope.isLoading = false; });

    //                }
    //                else {
    //                    console.log(result.data);
    //                    alert("อัปโหลดรูปถ่ายไม่สำเร็จ");
    //                }
    //            }
    //            , function (err) {
    //                console.log("InsertPhotos Error", err);
    //                alert("เกิดข้อผิดพลาด");
    //            }).finally(function () { $scope.isLoading = false; });
    //};

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

    $scope.init();

}]);


