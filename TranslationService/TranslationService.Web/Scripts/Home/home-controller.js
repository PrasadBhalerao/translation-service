'use strict';

var app = angular.module('demo', ['ngSanitize', 'ui.select', 'ngHandsontable', 'ngResource']);

app.controller('DemoCtrl', function ($scope, $http, $timeout, $interval, hotRegisterer, homeService) {
   $scope.myData = [
        {
            firstName: "Cox",
            lastName: "Carney",
            company: "Enormo",
            employed: true
        },
        {
            firstName: "Lorraine",
            lastName: "Wise",
            company: "Comveyer",
            employed: false
        },
        {
            firstName: "Nancy",
            lastName: "Waters",
            company: "Fuelton",
            employed: false
        }
    ];

    $scope.rowHeaders = true;
    $scope.colHeaders = true;

    $scope.settings = {
        contextMenu: [
            'row_above', 'row_below', 'remove_row'
        ],
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
        }
    };

    $scope.culture = {};
    homeService.getCulture().then(function (result) {
        $scope.cultures = result;
    });

    $scope.data = {};
    $scope.cultureChanged = function () {
        debugger;
        homeService.getTranslation($scope.culture.selected.KeyID).then(function (response) {
            $scope.data = response;
        });
    };

    var loadInitialCultureData = function () {
        var userCultureId = 50;
        homeService.getTranslation(userCultureId).then(function (response) {
            $scope.data = response;
        });
    };
    loadInitialCultureData();

    $scope.save = function () {
        homeService.saveTranslation(50, $scope.data).then(function (response) {
            $scope.data = response;
        });
    };
});
