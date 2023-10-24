var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/user/getall'},
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                data: { id:"id", lockoutEnd:"lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (data.IsBlocked) {
                        return `
                        <div class="text-center">
                             <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white text-nowrap" style="cursor:pointer; ">
                                    <i class="bi bi-lock-fill"></i>  Block
                                </a> 
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white text-nowrap" style="cursor:pointer;">
                                    <i class="bi bi-unlock-fill"></i>  Unblock
                                </a>
                        </div>
                    `
                    }

                   
                },
                "width": "15%"
            }
        ]
    });
}


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/Block_UnBlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}