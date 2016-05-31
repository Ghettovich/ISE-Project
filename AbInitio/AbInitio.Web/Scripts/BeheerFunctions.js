$(document).ready(function () {

});

$(document).on("click", ".details", function (e) {

    if ($(this).parent("tr").attr("data-href").has("data-href")) {
        window.location = $(this).parent("tr").attr("data-href");        
    }

    
});


$(document).on("click", ".delete_link", function () {

    $("#relatieid").attr("value", $(this).attr('data-relatieid'));

    if ($(this).has("data-persoonid")) {
        $("#persoonid").attr("value", $(this).attr('data-persoonid'));
    }

});

$("#wijzigenRechten").click(function () {
    var c = confirm("weet u zeker dat u deze collaborateur wilt wijzigen?");
    if (c == false) {
        $(location).attr('href', 'Index')
        return c;
    }


});

$("#verwijderenRechten").click(function () {
    var c = confirm("weet u zeker dat u deze gebruiker wilt verwijderen uit jouw collaborateur?");
    if (c == false) {
        $(location).attr('href', 'Index')
        return c;
    }
});


