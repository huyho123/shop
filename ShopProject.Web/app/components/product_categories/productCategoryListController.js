(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope','apiHttpService'];

    function productCategoryListController($scope, apiHttpService)
    {
        $scope.lstProductCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.getProductCategories = getProductCategories;
        $scope.search = search;
        function search() {
            getProductCategories();
        }
        function getProductCategories(page)
        {
            //neu page = null se gan = 0,bien page duoc truyen vao tu pagerdirective.html
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiHttpService.get('api/productcategory/getall', config, function (result) {
                $scope.lstProductCategories = result.data.Items;
                $scope.page = result.data.Page,
                $scope.pagesCount = result.data.TotalPages,
                $scope.totalCount = result.data.TotalCount

            }, function () {
                console.log('Load productcategory failed');
            });           
        }
        $scope.getProductCategories();
    }
})(angular.module('shopproject.productCategories'));