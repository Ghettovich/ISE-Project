$(document).ready(function () {

});

$(document).on("click", ".details", function (e) {

    if ($(this).parent("tr").attr("data-href").has("data-href")) {
        window.location = $(this).parent("tr").attr("data-href");        
    }

    
});


$(document).on("click", ".delete_link", function () {

    
    if ($(this).attr("data-persoonid")) {        
        $("#persoonid").attr("value", $(this).attr('data-persoonid'));
    }
    else if ($(this).attr("data-relatieid")) {
        $("#relatieid").attr("value", $(this).attr('data-relatieid'));
    }
    else if ($(this).attr("data-avrid")) {
        $("#avrid").attr("value", $(this).attr("data-avrid"));
    }

});

