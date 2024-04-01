document.getElementById('registerForm').addEventListener('submit', function (e) {
    e.preventDefault();
    register();
});

function register() {
    var name = document.getElementById('Name').value;
    var email = document.getElementById('Email').value;
    var password = document.getElementById('Password').value;
    var confirmPassword = document.getElementById('ConfirmPassword').value;
    var address = document.getElementById('Address').value;
    var phone = document.getElementById('Phone').value;
    var city = document.getElementById('City').value;

    if (name === "" || email === "" || password === "" || address === "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Name, Email, password, and address are required!',
        });
        return;
    }

    if (password !== confirmPassword) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Password and Confirm Password do not match!',
        });
        return;
    }

    if (city === '') {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please select valid city!',
        });
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Account/Register',
        headers: {
            RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Include anti-forgery token
        },
        data: {
            Name: name,
            Address: address,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            Phone: phone,
            City: city
        },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registration has been Successfully',
                    text: 'Please try login!',
                });
                window.location.href = '/Home/Index';
            } else {
                // If login fails, show error message
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.message,
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'An error occurred while processing your request.',
            });
        }
    });
}

const container = document.getElementById('lottie-container');

if (container && typeof lottie !== 'undefined') {
    fetch('/assets/animation/Animation_Registration.json')
        .then(response => response.json())
        .then(animationData => {
            const anim = lottie.loadAnimation({
                container: container,
                renderer: 'svg',
                loop: true,
                autoplay: true,
                animationData: animationData,
            });
        })
        .catch(error => {
            console.error('Error loading animation data:', error);
        });
}
else {
    console.error("Container element not found or Lottie library not loaded.");
}