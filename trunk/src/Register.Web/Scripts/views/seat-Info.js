//var app = angular.module("seatInfoModule", ["ui.bootstrap", "cspModule"]);
app.controller("seatInfoController", ["$scope", "CspUtils", "LookupService", "$http", function ($scope, $CspUtils, $LookupService, $http) {
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = "";
    //$scope.year = YEAR_NO;
    $scope.examSiteInfo = {};
    $scope.isLoading = false;
    $scope.show = false;
    $scope.isInfo = false;
    $scope.model = {};
    $scope.latitude = "";
    $scope.longtitude = "";
    $scope.hasLocation = false;

    $scope.init = function () {
        $(function () {
            $(window).on('resize', function () {
                if ($(window).width() > 768) {
                    if (!$CspUtils.IsNullOrEmpty($scope.latitude) && !$CspUtils.IsNullOrEmpty($scope.longtitude)) {
                        $scope.displayMapAt($scope.latitude, $scope.longtitude, 15);
                    }
                    else {
                        $("#map").empty();
                    }
                } else {
                    $("#map").empty();
                }
            });

            if ($(window).width() > 768) {
                if (!$CspUtils.IsNullOrEmpty($scope.latitude) && !$CspUtils.IsNullOrEmpty($scope.longtitude)) {
                    $scope.displayMapAt($scope.latitude, $scope.longtitude, 15);
                } else {
                    $("#map").empty();
                }
            }
        });
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

        var url = TEST_TYPE_ROOT + $scope.examType + "/Inquiry/SeatInfo";
        $http.post(url, $scope.model, config)
                .then(function (result) {
                    if (result.data.rtn.Success) {
                        //console.log(result.data);
                        $scope.examSiteInfo = result.data.Data;
                        if ($scope.examSiteInfo) {
                            $scope.show = true;
                            $scope.isInfo = false;
                            $scope.latitude = $scope.examSiteInfo.LATITUDE;
                            $scope.longtitude = $scope.examSiteInfo.LONGTITUDE;
                            if (!$CspUtils.IsNullOrEmpty($scope.examSiteInfo.EXAM_LOCATION)) {
                                $scope.hasLocation = true;
                            } else {
                                $scope.hasLocation = false;
                            }
                            if (!$CspUtils.IsNullOrEmpty($scope.latitude) && !$CspUtils.IsNullOrEmpty($scope.longtitude)) {
                                $scope.displayMapAt($scope.latitude, $scope.longtitude, 15);
                                $scope.OpenMap($scope.latitude, $scope.longtitude);
                            } else {
                                $("#map").empty();
                            }
                        }
                        else {
                            $scope.show = false;
                            $scope.isInfo = true;
                            $scope.latitude = "";
                            $scope.longtitude = "";
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

    $scope.OpenMap = function (latitude, longtitude) {
        var latLon = latitude + ',' + longtitude;
        //console.info(navigator.userAgent);
        if (/android/i.test(navigator.userAgent.toLowerCase())) {
            //window.open('http://maps.google.com/maps?daddr=' + latLon, "_system");
            $("#btnOpenMap").attr("href", 'http://maps.google.com/maps?daddr=' + latLon);
        }
        else if (/ipad|iphone|ipod/i.test(navigator.userAgent.toLowerCase())) {
            url = 'http://maps.google.com/maps?daddr=' + latLon;
            //window.open(url, '_blank', 'location=yes');
            $("#btnOpenMap").attr("href", url);
        }
        else {
            var url = 'http://www.google.com/maps/place/' + latLon;
            //window.open(url, '_blank', 'location=yes, width=500, height=500');
            $("#btnOpenMap").attr("href", url);
        }
    };

    $scope.displayMapAt = function (lat, lon, zoom) {
        $("#map")
                .html(
                        "<iframe id=\"map_frame\" "
                                + "width=\"50%\" height=\"350px\" frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" "
                                + "src=\"https://www.google.com/maps/embed/v1/place?key=AIzaSyC083On9_2wiF9ux976gvtVFkJ4qH6fG5w&q="
                                + lat + "," + lon
                                + "&zoom="
                                + zoom + "\"" + "></iframe>");
    };

    $scope.init();

}]);


