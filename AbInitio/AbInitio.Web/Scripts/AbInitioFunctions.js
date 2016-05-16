$(document).ready(function () {


    $(".gebruiker_stambomen > a").click(function () {
        $.ajax($(this).attr("href")).done(function (result) {
            $(".stamboompersonen").replaceWith(result);
        }); return false;
    });

});


$(document).on("click", ".stamboom_row", function () {
    window.location = $(this).attr("data-href");
});

