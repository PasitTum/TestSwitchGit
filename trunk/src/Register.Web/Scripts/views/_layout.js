var paras = getUrlParams(location.href);
if (paras && paras["testtypeid"]) {
    sessionStorage.setItem("TestTypeID", paras["testtypeid"]);
}
if (sessionStorage.getItem("TestTypeID")) $("#TestTypeID").val(sessionStorage.getItem("TestTypeID"));

$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        if ($(window).width() <= 768) {
            var footerWidth = $(".footer").width();
            if ($('#sidebar').hasClass('active')) {
                $(".footer").width(footerWidth + 250);
            } else {
                $(".footer").width(footerWidth - 250);
            }
        }
    });

    var url = window.location.pathname;
    $('a.menu-item').each(function () {
        var item = $(this);
        var href = item.attr("href");
        href = href.split('?');
        if (url == HOME_URL && url == href[0]) {
            item.parent().addClass('active');
        } else {
            if (url.startsWith(href[0]) && href[0] != HOME_URL) {
                item.parent().addClass('active');
            }
        }
    });
});