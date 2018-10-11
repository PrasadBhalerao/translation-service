'use strict';

var app = angular.module('homeApp');

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
        colWidths:[500, 600]
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
        homeService.saveTranslation($scope.culture.selected.KeyID, $scope.data).then(function (response) {
            $scope.data = response;
        },
            function (error) {
                if (error.data && error.data.ExceptionMessage)
                    $scope.validationResult = error.data.ExceptionMessage;
            });
    };

    var baseUrl = $location.$$protocol + '://' + $location.$$host + ":" + $location.$$port + "/";
    $scope.generateReport = function () {
        homeService.generateReport($scope.culture.selected.KeyID).then(function (response) {
            if (response) {
                $window.location.assign(baseUrl + response.Path);
            }
        });
    };
});
ty