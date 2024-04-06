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
            window.location.href = `/Admin/Category/DeleteCategory?Id=${id}`;
            Swal.fire({
                title: 'Deleted!',
                text: "The Category has been deleted.",
                icon: 'success',
            })
        }
    })
}

function Edit(CategoryId, title, IsAvailable) {
    IsAvailableLower = IsAvailable.toLowerCase();
    $('#CategoryId').val(CategoryId);
    $('#CategoryTitle').val(title);
    $('#IsAvailable').val(IsAvailableLower);
    $('#modalTitle').text('Edit Category');
    $('#categoryForm').attr('action', '/Admin/Category/EditCategory');
    $('#btnSave').removeClass('btn-success').addClass('btn-info rounded-pill').text('Edit Category');
}

function Reset() {
    $('#modalTitle').text('Add New Category');
    $('#btnSave').text('Save changes');
    $('#btnSave').removeClass('btn-info').addClass('btn-success rounded-pill');
    $('#CategoryId').val('');
    $('#CategoryTitle').val('');
    $('#IsAvailable').val('true');
}