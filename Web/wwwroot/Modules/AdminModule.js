document.addEventListener("DOMContentLoaded", function () {
    var navLinks = document.querySelectorAll('li.nav-item a.nav-link');

    function isActiveLink(link) {
        return link.href === window.location.href;
    }

    navLinks.forEach(function (link) {

        if (isActiveLink(link)) {
            link.classList.add('active', 'text-primary');
        }

        link.addEventListener("click", function () {

            navLinks.forEach(function (navLink) {
                if (navLink === 'nav-link active') {
                    navLink.classList.remove('active', 'text-primary');
                }
            });
        });
    });
});