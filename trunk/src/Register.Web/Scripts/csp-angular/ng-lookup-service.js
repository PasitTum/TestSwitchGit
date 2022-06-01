cspModule.service('LookupService', ['$http', function ($http) {
    var urlBase = API_ROOT + '/Master';
    var jsonUrl = WWW_ROOT + '/Json';

    this.getLookupExamCenters = function (testTypeID) {
        return $http.get(urlBase + "/ExamCenters/TestTypeID/" + testTypeID);
    };

    this.getLookupProvince = function () {
        var config = { cache:false };
        return $http.get(jsonUrl + "/provinces.json", config);
    };

    this.getLookupProvinceDB = function () {
        var config = { cache: false };
        return $http.get(urlBase + "/Provinces", config);
    };

    this.getLookupAmphur = function (provID) {
        return $http.get(urlBase + "/Provinces/" + provID + "/Amphurs");
    };

    this.getLookupTumbon = function (amphID) {
        return $http.get(urlBase + "/Amphurs/" + amphID + "/Tumbons");
    };

    this.getLookupPrefixes = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/prefixes.json", config);
    };

    this.getLookupPrefixesDB = function () {
        return $http.get(urlBase + "/Prefixes");
    };

    this.getLookupGender = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/genders.json", config);
    };

    this.getLookupGenderDB = function () {
        return $http.get(urlBase + "/Genders");
    };

    this.getLookupReligions = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/religions.json", config);
    };

    this.getLookupReligionsDB = function () {
        return $http.get(urlBase + "/Religions");
    };

    this.getLookupStatuses = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/statuses.json", config);
    };

    this.getLookupStatusesDB = function () {
        return $http.get(urlBase + "/Statuses");
    };

    this.getLookupExamSites = function (testTypeID) {
        return $http.get(urlBase + "/TestTypeID/" + testTypeID + "/ExamSite");
    };

    this.getLookupTeachClasses = function (testTypeID) {
        return $http.get(urlBase + "/TestTypeID/" + testTypeID + "/TeachClasses");
    };

    this.getLookupClassGroups = function (testTypeID) {
        return $http.get(urlBase + "/TestTypeID/" + testTypeID + "/ClassGroups");
    };

    this.getLookupClasses = function (testTypeID, classGroupID) {
        return $http.get(urlBase + "/TestTypeID/" + testTypeID + "/ClassGroupID/" + classGroupID + "/Classes");
    };

    this.getLookupCerts = function (testTypeID, classGroupID) {
        return $http.get(urlBase + "/TestTypeID/" + testTypeID + "/ClassGroupID/" + classGroupID + "/Certs");
    };

    this.getLookupEducationals = function (classLevelID) {
        return $http.get(urlBase + "/Educationals/" + classLevelID);
    };

    this.getLookupOCSCEducationals = function (testTypeID, classLevelID) {
        return $http.get(urlBase + "/OCSCEducationals/TestTypeID/" + testTypeID + "/ClassLevelID/" + classLevelID);
    };

    this.getLookupHighestEducationals = function (classLevelID) {
        return $http.get(urlBase + "/HighestEducationals/" + classLevelID);
    };

    this.getLookupSchoolGroup = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/school-groups.json", config);
    };

    this.getLookupSchoolGroupDB = function () {
        return $http.get(urlBase + "/SchoolGroups");
    };

    this.getLookupSchoolLocation = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/school-locations.json", config);
    };

    this.getLookupSchoolLocationDB = function () {
        return $http.get(urlBase + "/SchoolLocations");
    };

    this.getLookupSchool = function (schGrpID) {
        return $http.get(urlBase + "/Schools/SchoolGroup/" + schGrpID);
    };

    this.getLookupDegrees = function (regClassID, classLevelID) {
        return $http.get(urlBase + "/RegClassID/" + regClassID + "/ClassLevelID/" + classLevelID + "/Degrees");
    };

    this.getLookupMajors = function (regClassID, classLevelID, degreeID) {
        return $http.get(urlBase + "/RegClassID/" + regClassID + "/ClassLevelID/" + classLevelID + "/DegreeID/" + degreeID + "/Majors");
    };

    this.getLookupOccupation = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/occupations.json", config);
    };

    this.getLookupOccupationDB = function () {
        return $http.get(urlBase + "/Occupations");
    };

    this.getLookupEduYears = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/edu-years.json", config);
    };

    this.getLookupEduYearsDB = function () {
        return $http.get(urlBase + "/EduYears");
    };

    this.getLookupDefectiveHelp = function () {
        var config = { cache: false };
        return $http.get(jsonUrl + "/defectives.json", config);
    };

    this.getLookupDefectiveHelpDB = function () {
        return $http.get(urlBase + "/DefectiveHelps");
    };

    this.getLookupQuestionaires = function (testTypeID) {
        return $http.get(urlBase + "/Questionaires/TestTypeID/" + testTypeID);
    };

}]);