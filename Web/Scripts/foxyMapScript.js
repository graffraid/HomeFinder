$(function () {
    var myMap;
    ymaps.ready(init);

    function init() {
        myMap = new ymaps.Map('map', {
            center: [51.716730, 39.172982],
            zoom: 16
        });

        myMap.controls.add('zoomControl', { left: 5, top: 5 });
    }
});