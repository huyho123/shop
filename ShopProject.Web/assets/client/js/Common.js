var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {

        // Event Search.
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            }
        })._renderItem = function (ul, item) {
            return $("<li>")
              .append("<a>" + item.label + "</a>")
              .appendTo(ul);
        };
         //Event Cart.
        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));

            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        toastr.success('Thêm sản phẩm thành công!');
                        //window.location.href = "/gio-hang.html";
                    }
                    else {
                        toastr.warning(response.message);
                    }
                }
            });
        });
        $('#btnLogout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#frmLogout').submit();
        });
    }
}
common.init();