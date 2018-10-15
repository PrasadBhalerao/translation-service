'use strict';

var app = angular.module('homeApp');


app.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}]);

app.controller('homeController', function ($scope, $http, $timeout, $interval, $window, $location, hotRegisterer, homeService) {
   
    $scope.rowHeaders = true;
    $scope.colHeaders = true;

    $scope.settings = {
        contextMenu: ['row_below', 'remove_row'],
        colHeaders: ['Translation key', 'Translation value'],
        columns: [
            {
                data: 'Key',
                editor: false
            },
            {
                data: 'Value',
                editor: 'text'
            }],
        cells: function (row, col, prop) {
            var cellProperties = {};
            //allow editing translation key for new entries
            if (prop == "Key" && $scope.data[row] && $scope.data[row]['TranslationKeyId'] == null) {
                cellProperties.editor = 'text';
            }
            return cellProperties;
        },
        colWidths: [500, 600],
        minSpareRows: 1
    };

    $scope.culture = {};
    $scope.culture.selected = {
        KeyID: 50,
        CultureName: "en-US",
        CultureCode: "0x0409",
        DisplayName: "English - United States"
    };

    homeService.getCulture().then(function (result) {
        $scope.cultures = result;
    });

    $scope.data = {};
    $scope.getTranslation = function () {
        homeService.getTranslation($scope.culture.selected.KeyID).then(function (response) {
            $scope.data = response;
        });
    };

    $scope.getTranslation();

    $scope.save = function () {
        var data = _.filter($scope.data, function (item) {
            return item.Key != null || item.Value != null;
        });
        homeService.saveTranslation($scope.culture.selected.KeyID, data).then(function (response) {
            $scope.getTranslation();
        },
            function (error) {
                if (error.data && error.data.ExceptionMessage)
                    $scope.validationResult = error.data.ExceptionMessage;
            });
    };

    var baseUrl = $location.$$protocol + '://' + $location.$$host + ":" + $location.$$port + "/";
    $scope.generateReport = function (isJson) {
        homeService.generateReport($scope.culture.selected.KeyID, isJson).then(function (response) {
            if (response) {
                $window.location.assign(baseUrl + response.Path);
            }
        });
    };

    $scope.showFileUploaderDiv = false;

    $scope.showFileUploader = function () {
        $("fileUploader").animate({ right: '250px', opacity: '0.5' });
        $scope.showFileUploaderDiv = !$scope.showFileUploaderDiv;
    };

    $scope.cancelUpload = function () {
        $scope.showFileUploaderDiv = false;
    }

    var formdata = new FormData();
    $scope.getTheFiles = function ($files) {
        formdata = new FormData();
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });
    };

    //upload files
    $scope.uploadFiles = function () {
        homeService.uploadReport($scope.culture.selected.KeyID, formdata)
            .then(function () {
                $scope.getTranslation();
                alert("Successfully uploaded file!");
                $scope.showFileUploaderDiv = !$scope.showFileUploaderDiv;
            },
            function (error) {
                if (error.data && error.data.ExceptionMessage)
                    $scope.validationResult = error.data.ExceptionMessage;
            });
    }
});