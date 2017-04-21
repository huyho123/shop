(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    //$state cua uirouter
    productCategoryAddController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', 'commonService'];
    function productCategoryAddController($scope, apiHttpService, notificationService, $state, commonService) {
        $scope.AddProductCategory = AddProductCategory;
        $scope.productCategoryParent = [];
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true
        }
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        function AddProductCategory() {
            apiHttpService.post('api/productcategory/create', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' was upload');
                $state.go('product_categories');
            }, function (error) {
                notificationService.displayError('Please input filed !');
            });
        }

        function LoadParentProductCategory() {
            apiHttpService.get('api/productcategory/getParent', null, function (result) {
                $scope.productCategoryParent = result.data;

            });
        }

        LoadParentProductCategory();

    }
})(angular.module('shopproject.productCategories'));