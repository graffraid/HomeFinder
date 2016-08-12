function postBuilding() {

    var street = $("#street").val();
    var shortstreet = $("#shortstreet").val();
    var homenumber = $("#homenumber").val();
    var url = window.location.protocol + "//" + window.location.host + "/api/home/building";

    $("#newbuilding").addClass("disabled");

    $.post(
        url,
        { "Street": street, "ShortStreet": shortstreet, "No": homenumber },
        function (response) {
            location.reload();
        })
    .fail(function () {
            $("#newbuilding").removeClass("btn-default");
            $("#newbuilding").addClass("btn-danger");
            $("#newbuilding").html("Error");
        });
}