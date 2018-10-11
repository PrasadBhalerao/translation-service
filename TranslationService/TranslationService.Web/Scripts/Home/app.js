'use strict';

var app = angular.module('homeApp', ['ngSanitize', 'ui.select', 'ngHandsontable', 'ngResource', 'ngRoute']);

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(|blob|):/);
}]);