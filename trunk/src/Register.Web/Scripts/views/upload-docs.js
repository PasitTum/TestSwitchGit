app.controller("uploadDocsController", ["$scope", "CspUtils", "LookupService", "$http", "$filter",
function ($scope, $CspUtils, $LookupService, $http, $filter) {
    $scope.API_ROOT = API_ROOT;
    $scope.TEST_TYPE_ROOT = TEST_TYPE_ROOT;
    $scope.testTypeID = TEST_TYPE_ID;
    $scope.examType = EXAM_TYPE;
    $scope.isLoading = {};
    $scope.testingClassID = $CspUtils.IsNullOrEmpty(oldData.ClassGroupID) ? '0' : angular.copy(oldData.ClassGroupID); 
    $scope.regClassID = $CspUtils.IsNullOrEmpty(oldData.RegClassID) ? '0' : angular.copy(oldData.RegClassID);
    $scope.otherKey = $CspUtils.IsNullOrEmpty(oldData.DocTypeID) ? '' : angular.copy(oldData.DocTypeID);
    $scope.citizenID = "";
    $scope.mode = "";
    $scope.modelInput =  {};
    $scope.documentListOrg = $CspUtils.IsNullOrEmpty(oldData.DOCS) ? [] : angular.copy(oldData.DOCS);
    $scope.documentList = [];

    // Set Child to parent
    var parentScope = $scope.$parent;
    parentScope.uploadChild = $scope;

    $scope.init = function () {
        $scope.loadDocumentList();
    };

    $scope.loadDocumentList = function () {
        var url = "";
        var config = { cache: false };
        $scope.isLoading.documentList = true;
        $scope.documentList = [];

        if ($scope.mode == "Upload") {
            var citizenID = $scope.citizenID;
            url = API_ROOT + "/ReUploadDocs/DocsByCitizen/";

            $http.post(url, $scope.modelInput, config)
                .then(function (result) {
                    $scope.documentList = result.data;
                    if ($scope.documentListOrg.length > 0 && $scope.documentList.length > 0) {
                        angular.forEach($scope.documentList, function (item, index) {
                            var itemOrg = $filter('filter')($scope.documentListOrg, {
                                DOC_TYPE_ID: item.DOC_TYPE_ID
                            }, true);
                            if (itemOrg.length > 0 && !$CspUtils.IsNullOrEmpty(itemOrg[0].DOC_GUID)) {
                                item.UPLOAD_STATUS = "Y";
                                item.TEST_TYPE_ID = itemOrg[0].TEST_TYPE_ID;
                                item.FILE_NAME = itemOrg[0].FILE_NAME;
                                item.FILE_TYPE = itemOrg[0].FILE_TYPE;
                                item.DOC_GUID = itemOrg[0].DOC_GUID;
                                item.TEST_TYPE_ID = itemOrg[0].TEST_TYPE_ID;
                            }
                        });
                        $scope.documentListOrg = [];
                    }
                }
                , function (err) {
                    console.log("Save Doc Error", err);
                    alert("เกิดข้อผิดพลาด ไม่สามารถอ่านข้อมูลรายการเอกสารที่ต้องอัปโหลดได้");
                }).finally(function () { $scope.isLoading.documentList = false; });
        } else {
            var testingClassID = $scope.testingClassID;   // เปลี่ยนไปตามที่จะนำไปใช้
            var regClassID = $scope.regClassID;
            var otherKey = $scope.otherKey;
            //url = API_ROOT + "/Register/DocumentToUploads?testTypeID=" + $scope.testTypeID + "&testingClassID=" + testingClassID + "&otherKey=" + otherKey;

            url = API_ROOT + "/Register/DocumentToUploads?testTypeID=" + $scope.testTypeID + "&testingClassID=" + regClassID + "&otherKey=" + otherKey;
            $http.get(url, config)
                    .then(function (result) {
                        $scope.documentList = result.data;
                        if ($scope.documentListOrg.length > 0 && $scope.documentList.length > 0) {
                            angular.forEach($scope.documentList, function (item, index) {
                                var itemOrg = $filter('filter')($scope.documentListOrg, {
                                    DOC_TYPE_ID: item.DOC_TYPE_ID
                                }, true);
                                if (itemOrg.length > 0 && !$CspUtils.IsNullOrEmpty(itemOrg[0].DOC_GUID)) {
                                    item.UPLOAD_STATUS = "Y";
                                    item.TEST_TYPE_ID = itemOrg[0].TEST_TYPE_ID;
                                    item.FILE_NAME = itemOrg[0].FILE_NAME;
                                    item.FILE_TYPE = itemOrg[0].FILE_TYPE;
                                    item.DOC_GUID = itemOrg[0].DOC_GUID;
                                    item.TEST_TYPE_ID = itemOrg[0].TEST_TYPE_ID;
                                }
                            });
                            $scope.documentListOrg = [];
                        }
                    }
                    , function (err) {
                        console.log("Save Doc Error", err);
                        alert("เกิดข้อผิดพลาด ไม่สามารถอ่านข้อมูลรายการเอกสารที่ต้องอัปโหลดได้");
                    }).finally(function () { $scope.isLoading.documentList = false; });
        }
    }

    $scope.$on('reload-documentlist', function (event, args) {
        if (args) {
            if (args.mode == 'Upload') {
                $scope.citizenID = args.citizenID;
                $scope.mode = args.mode;
                $scope.modelInput = args.model;
            } else {
                $scope.testingClassID = args.testingClassID;
                $scope.regClassID = $CspUtils.IsNullOrEmpty(args.RegClassID) ? '0' : args.RegClassID;
                $scope.otherKey = args.otherKey;
                $scope.mode = args.mode;
            }
        }
        $scope.loadDocumentList();
    });

    $scope.browseFile = function (idx) {
        var fi = document.querySelector("#fileUpload" + idx);
        if (fi) {
            fi.value = "";
            fi.click();
        }
    }

    $scope.fileChanged = function ($event, files) {
        var element = angular.element($event.currentTarget);
        var docTypeID = element.data("doctypeid");
        var index = element.data("index");
        var progressBar = angular.element(document.querySelector("#progressBar" + index));
        var model = $scope.documentList[index];
        var maxSize = toByte(model.MAX_LIMITED_SIZE, model.UNIT_MAX_SIZE);
        // validate file type        
        var fileName = element.val();
        var allowedExtensions = model.FILE_TYPE_ACCEPT.toLowerCase();
        var ext = fileName.split('.').pop().toLowerCase();
        if (parseInt(allowedExtensions.indexOf(ext)) < 0) {
            alertWarning("อับโหลดได้เฉพาะไฟล์นามสกุล " + model.FILE_TYPE_ACCEPT + " เท่านั้น");
            element.val("");
            return false;
        }
        //else {
        //    //Image preview
        //    if (fileInput.files && fileInput.files[0]) {
        //        var reader = new FileReader();
        //        reader.onload = function (e) {
        //            document.getElementById('imagePreview').innerHTML = '<img src="' + e.target.result + '"/>';
        //        };
        //        reader.readAsDataURL(fileInput.files[0]);
        //    }
        //}

        // validate file size
        if (files[0].size > maxSize) {
            alertWarning("กรุณาอัปโหลดไฟล์ขนาดไม่เกิน " + model.MAX_LIMITED_SIZE + " " + model.UNIT_MAX_SIZE);
            element.val("");
            return false;
        }

        // เตรียม data
        var data = new FormData();
        data.append("TesttypeID", $scope.testTypeID);
        data.append("DocTypeID", docTypeID);
        data.append('file', files[0]);

        // reset ค่า
        $scope.documentList[index].UPLOAD_STATUS = "W";
        var progress = 0;
        progressBar.css('width', progress + '%').attr('aria-valuenow', progress).html(progress + '%');

        // TODO : ต้องเช็ค json ที่ return มาว่าสำเร็จหรือไม่
        var uploadUrl = WWW_ROOT + "/" + $scope.testTypeID + "/Upload/UploadDocs";

        $http.post(uploadUrl, data, {
            cache: false,
            contentType: false,
            processData: false,
            headers: { "Content-Type": undefined },
            eventHandlers: {
                readystatechange: function (event) {
                    //if (event.currentTarget.readyState === 4) {
                    //    console.log("readyState=4: Server has finished extra work!");
                    //}
                }
            },
            uploadEventHandlers: {
                progress: function (e) {
                    if (e.lengthComputable) {
                        progress = Math.round(e.loaded * 100 / e.total);
                        progressBar.css('width', progress + '%').attr('aria-valuenow', progress).html(progress + '%');
                        if (e.loaded == e.total) {
                            // console.log("File upload finished!");
                            // console.log("Server will perform extra work now...");
                        }
                    }
                }
            }
        }).then(function (result) {
            if (result.data) {
                var model = $scope.documentList[index];
                model.UPLOAD_STATUS = result.data.UPLOAD_STATUS;
                model.TEST_TYPE_ID = result.data.TEST_TYPE_ID;
                model.FILE_NAME = result.data.FILE_NAME;
                model.FILE_TYPE = result.data.FILE_TYPE;
                model.DOC_GUID = result.data.DOC_GUID;
            }
            if ($scope.documentList[index].UPLOAD_STATUS != "Y") {
                progress = 0;
                progressBar.css('width', progress + '%').attr('aria-valuenow', progress).html(progress + '%');
            }
        }, function (err) {
            console.log("เกิดข้อผิดพลาด ระหว่างอัปโหลดเอกสาร", err);
            alert("เกิดข้อผิดพลาด ระหว่างอัปโหลดเอกสาร");
        }).finally(function () { });
    }

    $scope.cancelUpload = function (index) {
        var progressBar = angular.element(document.querySelector("#progressBar" + index));
        var fileUpload = angular.element(document.querySelector("#fileUpload" + index));
        fileUpload.val("");
        var progress = 0;
        progressBar.css('width', progress + '%').attr('aria-valuenow', progress).html(progress + '%');
        var model = $scope.documentList[index];
        model.UPLOAD_STATUS = "";
        model.FILE_NAME = "";
        model.FILE_TYPE = "";
        model.DOC_GUID = "";
    };

    $scope.init();
}]);