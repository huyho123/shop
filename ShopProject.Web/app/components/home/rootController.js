﻿(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$location', '$window', 'authData', 'loginService', '$scope', 'authenticationService'];

    function rootController($location,$window, authData, loginService, $scope, authenticationService) {

        $scope.logOut = function () {
            loginService.logOut();            
            $location.path('/login');
            $window.location.reload();
        }
        $scope.authentication = authData.authenticationData;
        //$scope.sideBar = "/app/shared/views/sideBar.html";
        //authenticationService.validateRequest();
    }
})(angular.module('shopproject'));