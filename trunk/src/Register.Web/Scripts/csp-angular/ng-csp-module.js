var cspModule = angular.module("cspModule", []);
cspModule.factory('CspUtils', function () {
    //linkTo: function (link) {
    //    window.open(link, '_system');
    //};
    return {
        SetFocus: function (elementid) {
            // console.log("cspService.setFocus");
            document.getElementById(elementid).focus();
        },
        IsNullOrEmpty: function (value) {
            return (angular.isUndefined(value) || value === null || value === "");
        },
        NullToString: function (value) {
            return (angular.isUndefined(value) || value === null || value === "") ? "" : value.toString();
        },
        CreateUniqueArray: function (collection, keyname, valuename, value2, value3) {
            // we define our output and keys array;
            var output = [], keys = [];

            // we utilize angular's foreach function
            // this takes in our original collection and an iterator function
            angular.forEach(collection, function (item) {
                // we check to see whether our object exists
                var key = item[keyname];
                // if it's not already part of our keys array
                if (keys.indexOf(key) === -1) {
                    // add it to our keys array
                    keys.push(key);
                    // push this item to our final output array
                    //output.push(item);
                    var x = {};
                    x[keyname] = item[keyname];
                    x[valuename] = item[valuename];
                    if (value2) {
                        x[value2] = item[value2];
                    }
                    if (value3) {
                        x[value3] = item[value3];
                    }
                    output.push(x);
                }
            });
            // return our array which should be devoid of
            // any duplicates
            return output;
        },
        GetSelectedText: function (dropDownID) {
            dropDownID = (dropDownID.startsWith("#") ? "" : "#") + dropDownID;
            var sel = document.querySelector(dropDownID);
            console.log(dropDownID, sel);
            if (sel) {
                return sel.options[sel.selectedIndex].text;
            } else {
                return "";
            }

        },
        DaysInMonth: function (Y, M) {
            with (new Date(Y, M, 1, 12)) {
                setDate(0);
                return getDate();
            }
        },
        DateDiff: function (date1str, date2str) {
            // รับวันที่เป็น string รูปแบบ MM/dd/yyyy
            var date1 = new Date(date1str);
            var date2 = new Date(date2str);
            var y1 = date1.getFullYear(), m1 = date1.getMonth() + 1, d1 = date1.getDate(),
                y2 = date2.getFullYear(), m2 = date2.getMonth() + 1, d2 = date2.getDate();
            if (d1 < d2) {
                m1--;
                d1 += this.DaysInMonth(y2, m2);
            }
            if (m1 < m2) {
                y1--;
                m1 += 12;
            }
            return [y1 - y2, m1 - m2, d1 - d2];
        },
        encodeUrl: function (input) {
            if (input) {
                return window.encodeURIComponent(input);
            }
            return "";
        },
    };
});

// CALL THE "datepicker()" METHOD USING THE "element" OBJECT.
cspModule.directive("ngCspDatepicker", function () {
    function link(scope, element, attrs) {
        $(element).datepicker({
            language: 'th-th',
            format: 'dd/mm/yyyy',
            autoclose: true,
        });
        $(element).datepicker().on('changeDate', function (e) {
            angular.element(element).triggerHandler('input');
        });
    }
    return {
        require: 'ngModel',
        link: link
    };
});

cspModule.directive("ngCspTimepicker", function () {
    function link(scope, element, attrs) {
        $(element).datetimepicker({
            format: 'HH:mm:ss',
            keepOpen: true,
            debug: false
        });
        $(element).datetimepicker().on('dp.change', function (e) {
            angular.element(element).triggerHandler('input');
        });
    }
    return {
        require: 'ngModel',
        link: link
    };
});
// Add a custom HTTP Header to all requests
// $http.defaults.headers.common['My-Custom-Header'] = 'someValue';

// Auto-logout if any unauthorised web api request is made
cspModule.config(['$provide', '$httpProvider', function ($provide, $httpProvider) {
    $provide.factory('unauthorisedInterceptor', ['$q', function ($q) {
        return {
            'responseError': function (rejection) {
                if (rejection.status === 401) {
                    window.location.href = '/#/login';
                }

                return $q.reject(rejection);
            }
        };
    }]);
    $httpProvider.interceptors.push('unauthorisedInterceptor');
}])

// refresh  template: '<div class="refresher" ng-if="ngRefreshingWhen"><i class="fas fa-spin fa-sync-alt"></i></div>',
cspModule.directive('ngLoading', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngLoading: '='
        },
        template: '<div class="loader" ng-if="ngLoading"><div class="dot-typing"></div></div>',
        link: function (scope, element, attrs) {
            // add has-refresher class
            element.parent().addClass('has-loader');
        }
    };
});

cspModule.filter('escape', function () {
    return function (input) {
        if (input) {
            return window.encodeURIComponent(input);
        }
        return "";
    }
});
//cspServiceModule.service('cspService', function () {
//    //linkTo: function (link) {
//    //    window.open(link, '_system');
//    //};
//    this.setFocus = function (elementid) {
//        console.log("cspService.setFocus");
//        document.getElementById(elementid).focus();
//    };
//});

cspModule.directive('ngFileChanged', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function ($scope, element, attrs, ngModel) {
            var onChangeHandler = $parse(attrs.ngFileChanged);
            element.bind('change', function (e) {
                $scope.$apply(function () {
                    if (ngModel) {
                        ngModel.$setViewValue(element.val());
                        ngModel.$render();
                    }
                    if (onChangeHandler) onChangeHandler($scope, { $event: e, files: e.target.files });
                });
            });
        }
    }
}]);