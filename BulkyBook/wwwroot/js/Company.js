var CompanyTable;

$(document).ready(function () {
    LoadCompanies();
});

function LoadCompanies() {
    CompanyTable = $('#tableCompany').DataTable({
        "ajax": {
            "url": "Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "streetAddress", "width": "20%" },
            { "data": "city", "width": "40%" },
            { "data": "state", "width": "40%" },
            { "data": "postalCode", "width": "40%" },
            { "data": "phoneNumber", "width": "40%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (value) {
                    let checked = (value == true) ? "checked" : null;
                    if (value) {
                        return `
                            <div class=text-center> 
                              <input type="checkbox" checked="${checked}" disabled />
                            </div>
                        `;
                    }
                    else {
                        return '';
                    }
                },
                "width":"30%"

            },

            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center"  style="width:150px" >
                            <a class="btn btn-sm btn-primary" href="Company/UpSert/${data}" style="cursor:pointer" >Edit</a>
                            <a class="btn btn-sm btn-danger" onclick="DeleteCompany('Company/DeleteCompany/${data}')" asp-route-id="@cat.Id">Delete</a>
                            </div>
                        `;
                },
                "width": "100%"
            }
        ]
    });
}

function DeleteCompany(url) {
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
                        CompanyTable.ajax.reload();
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