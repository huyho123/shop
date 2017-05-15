(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    //service $state dung de dieu huong trang
    //service $stateParams de lay id cua url

    productCategoryEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function productCategoryEditController($scope, apiHttpService, notificationService, $state, $stateParams, commonService) {
        $scope.productCategory = {
            Status: true
        }

        $scope.EditProductCategory = EditProductCategory;

        $scope.GetSeoTitle = GetSeoTitle;

        $scope.imageUpload = function (event) {
            var files = event.target.files; //FileList object

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
        }

        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.productCategory.Image = e.target.result;
                $scope.FileName = e.target.Name;
                //$("<span class=\"pip\" id=\"pip\">" + "<img class=\"imageThumb\" src=\"" + $scope.productCategory.Image + "\" title=\"" + $scope.FileName + "\"/>" +
                //   "<br/><span class=\"remove\">Remove image</span>" +
                //    "</span>").insertAfter("#files");

                //$(".remove").click(function () {

                //    $(this).parent(".pip").remove();

                //    //$("#files").val(''); co the su dung cach nay`.

                //    var control = $("#files");
                //    control.replaceWith(control.val('').clone(true));

                //    $scope.productCategory.Image = null;
                //});
            });
        }
      
        $(document).ready(function () {
     

                $(".remove").click(function () {

                    $(this).parent(".pip").remove();

                    //$("#files").val(''); co the su dung cach nay`.

                    var control = $("#files");
                    control.replaceWith(control.val('').clone(true));

                    $scope.productCategory.Image = null;
                });                                                         
        });
        // function chuyen doi Name toi ky tu SEO
        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }
        // Cau hinh url:
        // service $stateParams de lay id productCategoryID cua url.
        // 'api/productcategory/getbyid/' + $stateParams.productCategoryID
        // cau hinh thong tin tren dung voi ben back end web api
        // [Route("getbyid/{productCategoryID:int}")]
        // Ngoai ra cau hinh params o module.js o cong state url: "/edit_product_category/:productCategoryID",
        // du'ng voi uirouter o view page ui-rsef =  "edit_product_category({productCategoryID:item.ID})"

        function GetproductCategorybyID() {
            apiHttpService.get('api/productcategory/getbyid/' + $stateParams.productCategoryID, null, function (result) {
                $scope.productCategory = result.data;

            }, function (error) {
                notificationService.displayError('Load data fail.');
            });
        }

        function EditProductCategory() {
            apiHttpService.put('api/productcategory/edit', $scope.productCategory, function (result) {

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

        GetproductCategorybyID();
    }

})(angular.module('shopproject.productCategories'));