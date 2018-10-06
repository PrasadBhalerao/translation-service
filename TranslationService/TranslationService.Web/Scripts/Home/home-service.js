'use strict';

var app = angular.module('demo');

app.service('homeService', function ($resource) {
    var cultureApi = $resource('/api/culture', {}, {
        query: { method: 'get', isArray: true }
    });

    var translationApi = $resource('/api/translation/:id', {}, {
        query: { method: 'get', isArray: true }
    });

    var getCulture = function () {
        return cultureApi.query().$promise;
    }

    var getTranslation = function (id) {
        return translationApi.query({ id: id}).$promise;
    }

    return {
        getCulture: getCulture,
        getTranslation: getTranslation
    }

});