/// <reference path="productCategoryAddController.js" />
(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    //$state cua uirouter
    productCategoryAddController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', 'commonService'];
    function productCategoryAddController($scope, apiHttpService, notificationService, $state, commonService) {
        $scope.AddProductCategory = AddProductCategory;
        $scope.productCategoryParent = [];
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true,

        }

        $scope.tinymceOptions = {
            onChange: function (e) {
                // put logic here for keypress and cut/paste changes
            },
            // performance
            cleanup: true,
            verify_html: false,
            entity_encoding: "raw",
            cleanup_on_startup: true,
            // ket thuc performance
            height: '200px',
            inline: false,
            plugins: 'advlist autolink link image lists print preview code media table textcolor hr searchreplace wordcount ',
            //toolbar: "forecolor backcolor table image autolink preview charmap link searchreplace fontsizeselect fontselect ",
            toolbar: "fontselect | bold italic underline | forecolor | fontsizeselect | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | image searchreplace ",
            fontsize_formats: "8pt 10pt 12pt 13pt 14pt 18pt 24pt 36pt",          
            skin: 'lightgray',
            theme: 'modern',
            languague: 'vi',
            valid_elements: '+*[*]',
            file_browser_callback: RoxyFileBrowser,

            media_url_resolver: function (data, resolve/*, reject*/) {
                if (data.url.indexOf('YOUR_SPECIAL_VIDEO_URL') !== -1) {
                    var embedHtml = '<iframe src="' + data.url +
                    '" width="400" height="400" ></iframe>';
                    resolve({ html: embedHtml });
                } else {
                    resolve({ html: '' });
                }
            }
        }

        $scope.RoxyFileBrowser = RoxyFileBrowser;
        var roxyFileman = '/fileman/index.html?integration=tinymce4';
        function RoxyFileBrowser(field_name, url, type, win) {
            var cmsURL = roxyFileman;  // script URL - use an absolute path!
            if (cmsURL.indexOf("?") < 0) {
                cmsURL = cmsURL + "?type=" + type;
            }
            else {
                cmsURL = cmsURL + "&type=" + type;
            }
            cmsURL += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
            tinyMCE.activeEditor.windowManager.open({
                file: cmsURL,
                title: 'Insert File',
                width: 850, // Your dimensions may differ - toy around with them!
                height: 550,
                resizable: "yes",
                plugins: "media",
                inline: "yes", // This parameter only has an effect if you use the inlinepopups plugin!
                close_previous: "no"
            }, {
                window: win,
                input: field_name
            });
            return false;
        }


        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        // region: Upload image,se dua doi lai file upload.
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
                var filename = e.target;

                $("<span class=\"pip\">" +
           "<img class=\"imageThumb\" src=\"" + $scope.productCategory.Image + "\" title=\"" + filename.Name + "\"/>" +
           "<br/><span class=\"remove\">Remove image</span>" +
           "</span>").insertAfter("#files");

                $(".remove").click(function () {

                    $(this).parent(".pip").remove();

                    //$("#files").val(''); co the su dung cach nay`.

                    var control = $("#files");
                    control.replaceWith(control.val('').clone(true));

                    $scope.productCategory.Image = null;
                });
            });
        }

        // endregion: Upload image

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