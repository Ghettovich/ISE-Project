
$(document).ready(function () {

    $('.precisie').on('change', function () {
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
});