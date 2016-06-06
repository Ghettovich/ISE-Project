$(document).ready(function () {

    

    $("input[name='persoonid']").change(function () {
        $("#kekuleid").attr("value", $(this).parent("td.kekulecell").attr("data-kekulenr"));
    });

    $("#voegToe").click(function () {

        if ($("input[name='persoonid']:checked").val()) {

            var kekuleid = $("#kekuleid").attr("value");
            var stamboomid = $("input[name='stamboomid']").val();
            var form = $("#myForm");
            form.attr("action", "/Beheer/ToevoegenRelatie?" + "stamboomid=" + stamboomid + "?persoonid=" + persoonid + "?kekuleid=" + kekuleid);
            form.submit();
        }
        else {
            alert("selecteer een persoon.");
        }
    });

    $(".relatietypes").change(function () {

        if ($(this).val() == 1) {
            $("#nieuwkekuleid").attr("value", ($("#kekuleid").val() * 2));
        }
        else if ($(this).val() == 2) {
            $("#nieuwkekuleid").attr("value", (($("#kekuleid").val() * 2)+ 1));
        }

    });

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

$(".wijzigenRechten").click(function () {
    var c = confirm("weet u zeker dat u deze collaborateur wilt wijzigen?");
    if (c == false) {
        $(location).attr('href', 'Index')
        return c;
    }


});

$(".verwijderenRechten").click(function () {
    var c = confirm("weet u zeker dat u deze gebruiker wilt verwijderen uit jouw collaborateur?");
    if (c == false) {
        $(location).attr('href', 'Index')
        return c;
    }
});

$("#afschermIcon").click(function () {
    var c = confirm("Weet u zeker dat u de afscherming van deze stamboom wilt wijzigen?");
    if (c == false) {
        $(location).attr('href', 'Index')
        return c;
    }
});

function setHiddenField(elementId) {
    window.alert(document.getElementById(elementId).value);
    document.getElementById("Hidden").value = document.getElementById(elementId).value;
}