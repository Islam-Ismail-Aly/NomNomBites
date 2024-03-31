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
            window.location.href = `/Admin/Food/DeleteFood?Id=${id}`;
            Swal.fire({
                title: 'Deleted!',
                text: "The Food has been deleted.",
                icon: 'success',
            })
        }
    })
}

function Edit(id, title, description, price, rating, isAvailable, categoryId) {
    IsAvailableLower = isAvailable.toLowerCase();

    $('#FoodId').val(id);
    $('#Title').val(title);
    $('#Description').val(description);
    $('#Price').val(price);
    $('#Rating').val(rating);
    //$('#Image').val(image);
    $('#IsAvailable').val(IsAvailableLower);
    $('#CategoryId').val(categoryId);
    $('#modalTitle').text('Edit Food');
    $('#foodForm').attr('action', '/Admin/Food/EditFood');

    $('#btnSave').removeClass('btn-success').addClass('btn-info rounded-pill').text('Edit Food').click(function (e) {
        e.preventDefault(); // Prevent default form submission

        var formData = {
            Id: $('#FoodId').val(),
            Title: $('#Title').val(),
            Description: $('#Description').val(),
            Price: $('#Price').val(),
            //Image: $('#Image').val(),
            Rating: $('#Rating').val(),
            IsAvailable: $('#IsAvailable').val() === 'true',
            CategoryId: $('#CategoryId').val()
        };

        $.ajax({
            url: '/Admin/Food/EditFood',
            type: 'POST',
            data: formData,
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Updated!',
                    text: 'Food item has been updated successfully.'
                });

                location.reload();
            },
            error: function (xhr, status, error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error!',
                    text: 'An error occurred while editing the food item.'
                });
            }
        });
    });
}


function Reset() {
    $('#modalTitle').text('Add New Food');
    $('#btnSave').text('Save changes');
    $('#btnSave').removeClass('btn-info').addClass('btn-success rounded-pill');
    $('#FoodId').val('');
    $('#Title').val('');
    $('#Description').val('');
    $('#Price').val('');
    //$('#Image').val('');
    $('#Rating').val('');
    $('#IsAvailable').val('true');
    $('#CategoryId').val('');
}