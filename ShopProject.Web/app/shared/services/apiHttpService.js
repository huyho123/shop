/// <reference path="/assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiHttpService', apiHttpService);
    apiHttpService.$inject = ['$http', 'notificationService'];
    function apiHttpService($http, notificationService) {
        return {
            get: get,
            post: post,
            put: put,
            del: del

        }

        function get(url, params, success, failure) {
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failure(error);
            });
        }

        function post(url, data, success, failure) {
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required.');
                }

                failure(error);

            });
        }

        function put(url, data, success, failure) {
            $http.put(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required.');
                }

                failure(error);

            });
        }

        function del(url, data, success, failure) {
            $http.delete(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status == 401) {
                    notificationService.displayError('Authenticate is required.');
                }

                failure(error);

            });
        }
    }
})(angular.module('shopproject.common'));