/// <reference path="/assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiHttpService', apiHttpService);
    apiHttpService.$inject = ['$http'];
    function apiHttpService($http) {
        return {
            get: get
        }

        function get(url, params, success, failure) {
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failure(error);
            });
        }
    }
})(angular.module('shopproject.common'));