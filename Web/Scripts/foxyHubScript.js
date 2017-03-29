$(function () {

    $.connection.hub.start().done();

    $("#updateBtn").on("click", function () {
        $.get("http://foxy.local/api/home/data/adverts");
        $(this).removeClass("btn-success");
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-primary");
        $(this).attr("disabled", "disabled");
    });

    var foxyHub = $.connection.foxyHub;

    foxyHub.client.newParserStatus = function (status) {
        $("#updateBtn").text(status);
        if (status == "Done!") {
            $("#updateBtn").removeClass("btn-primary");
            $("#updateBtn").addClass("btn-success");
            $("#updateBtn").attr("disabled", false);
        }
        else if (status.indexOf("Error!") > -1) {
            $("#updateBtn").removeClass("btn-primary");
            $("#updateBtn").addClass("btn-danger");
            $("#updateBtn").attr("disabled", false);
        }
        else {
            $("#updateBtn").removeClass("btn-success");
            $("#updateBtn").removeClass("btn-danger");
            $("#updateBtn").addClass("btn-primary");
            $("#updateBtn").attr("disabled", "disabled");
        }
    }

    $(".price").each(function() {
        $(this).text($(this).text().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + "p.");
    });
})