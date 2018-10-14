'use strict';

var app = angular.module('homeApp');

app.service('homeService', function ($resource, $http) {

    //culture api
    var cultureApi = $resource('/api/culture', {}, {
        query: { method: 'get', isArray: true }
    });

    var getCulture = function () {
        return cultureApi.query().$promise;
    }

    //translation api
    var translationApi = $resource('/api/translation/:id', {}, {
        query: { method: 'get', isArray: true },
        post: {method: 'post'}
    });

    var getTranslation = function (id) {
        return translationApi.query({ id: id }).$promise;
    }

    var saveTranslation = function (id, translations) {
        return translationApi.post({ cultureId: id, translations: translations }).$promise;
    }

    //report api
    var reportApi = $resource('/api/report/:id', {}, {
        get: { method: 'get' },
        post: { method: 'post' }
    });

    var generateReport = function (id, isJson) {
        return reportApi.get({ id: id, isJson: isJson }).$promise;
    }

    var uploadReport = function (id, formdata) {
        var request = {
            method: 'POST',
            url: '/api/report/?id=' + id,
            data: formdata,
            headers: {
                'Content-Type': undefined
            }
        };

        //send files
        return $http(request);
    }

    return {
        getCulture: getCulture,
        getTranslation: getTranslation,
        saveTranslation: saveTranslation,
        generateReport: generateReport,
        uploadReport: uploadReport
    }

});