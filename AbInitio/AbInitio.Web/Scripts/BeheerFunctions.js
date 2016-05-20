$(document).ready(function () {

});

$(document).on("click", ".persoon_row", function () {
    window.location = $(this).attr("data-href");    
});

$('#confirm-delete').on('show.bs.modal', function (e) {
    $(this).find('.delete_relatie').attr('href', $(e.relatedTarget).data('href'));
});

