$(document).ready(function () {

});



$(document).on("click", ".persoon_row", function () {

    window.location = $(this).attr("data-href");
    
});
