// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    $(document).ready(function () {
        var currentPath = window.location.pathname; // Lấy đường dẫn URL hiện tại
        $(".navbar-nav .nav-item a").each(function () {
            var linkPath = $(this).attr("href");
            if (currentPath.indexOf(linkPath) >= 0) {
                $(this).parent().addClass("active"); // Thêm lớp active vào mục tương ứng
            }
        });
    });
