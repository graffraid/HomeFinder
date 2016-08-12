$(document).ready(function () {

    $("#buildingform").validate({
        rules: {
            street: { required: true },
            shortstreet: { required: true },
            homenumber: { required: true }
        },
        errorPlacement: function (error, element) {
        }
    });

    $("#newbuilding").on("click", function () {
        if ($('#buildingform').valid()) {
            postBuilding();
        }
    });
});