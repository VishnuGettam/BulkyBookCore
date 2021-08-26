 var CategoryTable;

$(document).ready(function () {
    LoadCategories();
});

function LoadCategories() {
    CategoryTable = $('#tableCategory').DataTable({
        "ajax": {
            "url": "Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class=text-center>
                            <a class="btn btn-sm btn-primary" href="Category/UpSert/${data}" style="cursor:pointer" >Edit</a>
                            <a class="btn btn-sm btn-danger" onclick="DeleteCategory('Category/Delete/${data}')" asp-route-id="@cat.Id">Delete</a>
                            </div>
                        `;
                },
                "width": "40%"
            }
        ]
    });
}

function DeleteCategory(url) {
    

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
                        CategoryTable.ajax.reload();
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