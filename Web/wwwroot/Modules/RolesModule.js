document.getElementById('roleForm').addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = new FormData(this);

    fetch('@Url.Action("Roles", "Account")', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: data.message,
                    confirmButtonText: 'Ok',
                    timer: 2000
                }).then(function () {
                    window.location.href = '/Admin/Account/Roles';
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: data.message + ' ' + data.errors.join(' '),
                    footer: '<a href="">Why do I have this issue?</a>'
                });
            }
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                footer: '<a href="">Why do I have this issue?</a>'
            });
        });
});


function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/Account/DeleteRole?Id=${id}`;
            Swal.fire({
                title: 'Deleted!',
                text: "The Role has been deleted.",
                icon: 'success',
            })
        }
    })
}

function Edit(id, name) {
    document.getElementById("modalTitle").innerHTML = "Edit Role";
    document.getElementById("btnSave").value = "Update changes";
    document.getElementById("btnSave").classList.add("btn", "btn-info", "rounded-pill");
    document.getElementById("roleId").value = id;
    document.getElementById("roleName").value = name;
}

function Reset() {
    document.getElementById("modalTitle").innerHTML = "Add New Role";
    document.getElementById("btnSave").value = "Save changes";
    document.getElementById("btnSave").classList.add("btn", "btn-success", "rounded-pill");
    document.getElementById("roleId").value = "";
    document.getElementById("roleName").value = "";
}