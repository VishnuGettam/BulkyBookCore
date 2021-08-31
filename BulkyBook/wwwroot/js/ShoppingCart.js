var cartTable;

$(document).ready(function () {

    $('#tblShoppingCart').DataTable({
        "ajax": {
            "url": "ShoppingCart/GetAll"
        },
        "columns": [
            {
                "data": "product", "width": "20%",
                render: function (product) {

                    return ` ${product.title}    `;


                }

            },
            {
                "data": "count", "width": "20%", render: function (count) {
                    return `<input class="form-control" value="${count}" />`;
                }
            }
        ]
    })

    //LoadCartProducts();
})
