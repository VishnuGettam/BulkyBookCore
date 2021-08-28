var productDataTable;

$(document).ready(function () {
    LoadProducts();
})

function LoadProducts() {
    productDataTable = $('#tableProduct').DataTable({
        "ajax": {
            "url": "Product/GetAll"
        },
        "columns": [
            {
                "data": "imageURL", "render": function (imgUrL) {
                    return `
                        <img src="data:image/png;base64,${imgUrL}" class="img" style="height:100px"  />
                            `
                }
            },
            { "data": "title", "width": "40%" },
            { "data": "description" },
            { "data": "isbn" },
            { "data": "author" },
            { "data": "listPrice" },
            { "data": "price" },
            { "data": "price50" },
            { "data": "price100" },

            { "data": "category.name" },
            { "data": "coverType.name" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class=text-center>
                            <a class="btn btn-sm btn-primary" href="Product/UpSert/${data}" style="cursor:pointer" >Edit</a>
                            <a class="btn btn-sm btn-danger" onclick="DeleteProduct('Product/Delete/${data}')" asp-route-id="@cat.Id">Delete</a>
                            </div>
                        `;
                },
                "width": "90%"
            }
        ]
    });
}

function DeleteProduct(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore ,Please confirm ",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((selected) => {
        if (selected) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.result) {
                        toastr["success"](data.message);
                        productDataTable.ajax.reload();
                    }
                    else {
                        toastr["error"](data.message);
                    }
                },
                error: function (error) {
                }
            })
        }
    })
}