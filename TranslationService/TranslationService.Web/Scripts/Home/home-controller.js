'use strict';

var app = angular.module('demo', ['ngSanitize', 'ui.select', 'ngHandsontable', 'ngResource']);

app.controller('DemoCtrl', function ($scope, $http, $timeout, $interval, homeService) {
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
        ]
    };

    $scope.culture = {};
    homeService.getCulture().then(function (result) {
        $scope.cultures = result;
    });

    $scope.data = {};
    $scope.cultureChanged = function () {
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
});
