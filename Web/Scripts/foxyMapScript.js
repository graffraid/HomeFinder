$(function () {
    var myMap;

    function init() {
        var advLat = $("#advert").data("lat");
        var advLon = $("#advert").data("lon");
        var hintLat = $("#advert").data("lat");
        var hintLon = $("#advert").data("lon");

        if (advLat == null && advLon == null) {
            advLat = 39.172982;
            advLon = 51.716730;
        }

        myMap = new ymaps.Map('map', {
            center: [advLon, advLat],
            zoom: 16
        });

        myMap.controls.add('zoomControl', { left: 5, top: 5 });


        var myPlacemark = new ymaps.Placemark([hintLon, hintLat], {
            hintContent: "Вотан!"
        });

        myMap.geoObjects.add(myPlacemark);


        $(".building").each(function() {
            var hintLat = $(this).data("lat");
            var hintLon = $(this).data("lon");
            var addr = $($(this).children()[1]).html() + ',' + $($(this).children()[2]).html();

            var myPlacemark = new ymaps.Placemark([hintLon, hintLat], {
                hintContent: addr
            });
            myMap.geoObjects.add(myPlacemark);
        });

        myMap.hint.show(myMap.getCenter(), "", {
            // Опция: задержка перед открытием.
            showTimeout: 1500
        });
    }

    ymaps.ready(init);

    function setTypeAndPan(lat, lon) {
        myMap
            // Плавное перемещение центра карты в точку с новыми координатами.
            .panTo([lon, lat], {
                delay: 500
            });
    }

    $(".building").on("click", function() {
        var lat = $(this).data("lat");
        var lon = $(this).data("lon");
        setTypeAndPan(lat, lon);
    });
});