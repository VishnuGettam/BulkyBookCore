var coverTypeTable;

$(document).ready(function () {
    LoadCoverType()
})

function LoadCoverType() {
    coverTypeTable = $('#tableCoverType').DataTable({
        "ajax": {
            "url": "CoverType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class=text-center>
                            <a class="btn btn-sm btn-primary" href="CoverType/UpSert/${data}" style="cursor:pointer" >Edit</a>
                            <a class="btn btn-sm btn-danger" onclick="DeleteCoverType('CoverType/Delete/${data}')" asp-route-id="@cat.Id">Delete</a>
                            </div>

                        `
                }
            }

        ]
    })
}

function DeleteCoverType(url) {
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
                        coverTypeTable.ajax.reload();
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