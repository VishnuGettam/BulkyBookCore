var userListTable;

$(document).ready(function () {
    LoadUsers();
})

function LoadUsers() {
    userListTable = $('#tableUsers').DataTable({
        "ajax": {
            "url": "User/GetUsers"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var todayDate = new Date().getTime();
                    var lockoutDate = new Date(data.lockoutEnd).getTime();
                    if (lockoutDate > todayDate) {
                        return `
                             <a class="btn btn-sm btn-danger" onclick="LockUnLock('${data.id}','Unlock')">
                            <i class="fas fa-unlock-alt"></i>Unlock</a>
                        `;
                    } else {
                        return `
                            <a class="btn btn-sm btn-success" onclick="LockUnLock('${data.id}','lock')">
                            <i class="fas fa-lock"></i>Lock </a>

                     `
                    }
                }
            }
        ]
    });
}

function LockUnLock(id, stat) {
    var data = { userId: id, status: stat };
    $.ajax({
        type: "post",
        url: "User/LockUnLock",
        data: data,
        success: function (data) {
            toastr["success"](data.result);
            userListTable.ajax.reload();
        }
    })
}