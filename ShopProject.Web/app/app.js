/// <reference path="/assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopproject',
        ['shopproject.products',
         'shopproject.productCategories',
         'shopproject.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "app/components/home/homeView.html",
            controller:"homeController"
        });
        $urlRouterProvider.otherwise('/admin');
    }
})();