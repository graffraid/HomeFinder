$(function () {
    $('#updateBtn').on('click', function () {

        $(this).attr("disabled", "disabled");

        $.connection.hub.start().done(function () {
            $.get('http://foxy.local/api/home/data/adverts');
        });
    });


    var foxyHub = $.connection.foxyHub;
    foxyHub.client.newParserStatus = function (status) {
        $('#updateBtn').text(status);
        if (status == 'Done!') {
            $('#updateBtn').removeClass('btn-primary');
            $('#updateBtn').addClass('btn-success');
            $('#updateBtn').attr("disabled", false);
        }
    }
})