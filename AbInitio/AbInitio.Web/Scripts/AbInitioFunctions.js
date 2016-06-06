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

$(document).ready(function(){
    $('.precisie').on('change', function() {
        if (document.getElementById("geboortePrecisie").value == "tussen") {
            $('.geboortedatum').show();
            $('.geboortedatum2').show();
        } else if (document.getElementById("geboortePrecisie").value == "op" || document.getElementById("geboortePrecisie").value == "voor" || document.getElementById("geboortePrecisie").value == "na") {
            $('.geboortedatum').show();
            $('.geboortedatum2').hide();
            document.getElementById("geboortedatum2Text").value = "";
        } else {
            $('.geboortedatum').hide();
            document.getElementById("geboortedatumText").value = "";
            $('.geboortedatum2').hide();
            document.getElementById("geboortedatum2Text").value = "";
        }
    });

    $('.precisie').load('change', function () {
        if (document.getElementById("geboortePrecisie").value == "tussen") {
            $('.geboortedatum').show();
            $('.geboortedatum2').show();
        } else if (document.getElementById("geboortePrecisie").value == "op" || document.getElementById("geboortePrecisie").value == "voor" || document.getElementById("geboortePrecisie").value == "na") {
            $('.geboortedatum').show();
            $('.geboortedatum2').hide();
            document.getElementById("geboortedatum2Text").value = "";
        } else {
            $('.geboortedatum').hide();
            document.getElementById("geboortedatumText").value = "";
            $('.geboortedatum2').hide();
            document.getElementById("geboortedatum2Text").value = "";
        }
    });

    $('.precisieAv').on('change', function () {
        if (document.getElementById("datumPrecisie").value == "tussen") {
            $('.van').show();
            $('.tot').show();
        } else if (document.getElementById("datumPrecisie").value == "op" || document.getElementById("datumPrecisie").value == "voor" || document.getElementById("datumPrecisie").value == "na") {
            $('.van').show();
            $('.tot').hide();
            document.getElementById("totText").value = "";
        } else {
            $('.van').hide();
            document.getElementById("vanText").value = "";
            $('.tot').hide();
            document.getElementById("totText").value = "";
        }
    });

    $('.precisieAv').load('change', function () {
        if (document.getElementById("datumPrecisie").value == "tussen") {
            $('.van').show();
            $('.tot').show();
        } else if (document.getElementById("datumPrecisie").value == "op" || document.getElementById("datumPrecisie").value == "voor" || document.getElementById("datumPrecisie").value == "na") {
            $('.van').show();
            $('.tot').hide();
            document.getElementById("totText").value = "";
        } else {
            $('.van').hide();
            document.getElementById("vanText").value = "";
            $('.tot').hide();
            document.getElementById("totText").value = "";
        }
    });
});