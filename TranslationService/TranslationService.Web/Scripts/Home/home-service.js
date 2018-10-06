'use strict';

var app = angular.module('demo');

app.service('homeService', function ($resource) {
    var cultureApi = $resource('/api/culture', {}, {
        query: { method: 'get', isArray: true }
    });

    var translationApi = $resource('/api/translation/:id', {}, {
        query: { method: 'get', isArray: true },
        post: {method: 'post'}
    });

    var getCulture = function () {
        return cultureApi.query().$promise;
    }

    var getTranslation = function (id) {
        return translationApi.query({ id: id}).$promise;
    }

    var saveTranslation = function (id, translations) {
        return translationApi.post({ cultureId: id, translations: translations}).$promise;
    }

    return {
        getCulture: getCulture,
        getTranslation: getTranslation,
        saveTranslation: saveTranslation
    }

});