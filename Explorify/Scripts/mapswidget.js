var distanceWidget;
var geocodeTimer;
var profileMarkers = [];


$(function () {

    $("#Radius").change(function () {
        distanceWidget.set('map', null);

        distanceWidget = new DistanceWidget({
            map: map,
            distance: $("#Radius").val(), // Starting distance in km.
            maxDistance: 250, // Twitter has a max distance of 2500km.
            color: '#000000',
            activeColor: '#5599bb',
            sizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png',
            activeSizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png'
        });

        google.maps.event.addListener(distanceWidget, 'distance_changed',
            updateDistance);

        google.maps.event.addListener(distanceWidget, 'position_changed',
            updatePosition);

        map.fitBounds(distanceWidget.get('bounds'));

        updateDistance();
        updatePosition();
        addActions();
    });

    $('#Lat').change(function () {
        map.setCenter(new google.maps.LatLng($('#Lat').val(), $('#Lan').val()));
        distanceWidget.set('map', null);

        distanceWidget = new DistanceWidget({
            map: map,
            distance: $("#Radius").val(), // Starting distance in km.
            maxDistance: 250, // Twitter has a max distance of 2500km.
            color: '#000000',
            activeColor: '#5599bb',
            sizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png',
            activeSizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png'
        });

        google.maps.event.addListener(distanceWidget, 'distance_changed',
            updateDistance);

        google.maps.event.addListener(distanceWidget, 'position_changed',
            updatePosition);

        map.fitBounds(distanceWidget.get('bounds'));

        updateDistance();
        updatePosition();
        addActions();

    });


    $('#Lan').change(function () {
        map.setCenter(new google.maps.LatLng($('#Lat').val(), $('#Lan').val()));
        distanceWidget.set('map', null);

        distanceWidget = new DistanceWidget({
            map: map,
            distance: $("#Radius").val(), // Starting distance in km.
            maxDistance: 250, // Twitter has a max distance of 2500km.
            color: '#000000',
            activeColor: '#5599bb',
            sizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png',
            activeSizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png'
        });

        google.maps.event.addListener(distanceWidget, 'distance_changed',
            updateDistance);

        google.maps.event.addListener(distanceWidget, 'position_changed',
            updatePosition);

        map.fitBounds(distanceWidget.get('bounds'));

        updateDistance();
        updatePosition();
        addActions();
    });



});



    // Create the search box and link it to the UI element.
    var input = /** @type {HTMLInputElement} */(
        document.getElementById('Location'));
    //   map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    var searchBox = new google.maps.places.SearchBox(
        /** @type {HTMLInputElement} */(input));

    // Listen for the event fired when the user selects an item from the
    // pick list. Retrieve the matching places for that item.
    google.maps.event.addListener(searchBox, 'places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }
        place = places[0];
        var bounds = new google.maps.LatLngBounds();
        bounds.extend(place.geometry.location);
        map.fitBounds(bounds);
        map.setCenter(place.geometry.location);
        map.setZoom(8);
        updateDistanceWidget();

    });

    function updateDistanceWidget() {
        distanceWidget.set('map', null);

        distanceWidget = new DistanceWidget({
            map: map,
            distance: $("#Radius").val(), // Starting distance in km.
            maxDistance: 250, // Twitter has a max distance of 2500km.
            color: '#000000',
            activeColor: '#5599bb',
            sizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png',
            activeSizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png'
        });

        google.maps.event.addListener(distanceWidget, 'distance_changed',
            updateDistance);

        google.maps.event.addListener(distanceWidget, 'position_changed',
            updatePosition);

        map.fitBounds(distanceWidget.get('bounds'));

        updateDistance();
        updatePosition();
        addActions();
    }


    // Bias the SearchBox results towards places that are within the bounds of the
    // current map's viewport.
    google.maps.event.addListener(map, 'bounds_changed', function () {
        var bounds = map.getBounds();
        searchBox.setBounds(bounds);
    });


    distanceWidget = new DistanceWidget({
        map: map,
        distance: $("#Radius").val(), // Starting distance in km.
        maxDistance: 250, // Twitter has a max distance of 2500km.
        color: '#000000',
        activeColor: '#5599bb',
        sizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png',
        activeSizerIcon: 'https://cdn0.iconfinder.com/data/icons/entypo/79/resize2-32.png'
    });

    google.maps.event.addListener(distanceWidget, 'distance_changed',
            updateDistance);

    google.maps.event.addListener(distanceWidget, 'position_changed',
            updatePosition);

    map.fitBounds(distanceWidget.get('bounds'));

    updateDistance();
    updatePosition();
    addActions();

function updatePosition() {
    if (geocodeTimer) {
        window.clearTimeout(geocodeTimer);
    }

    // Throttle the geo query so we don't hit the limit
    geocodeTimer = window.setTimeout(function () {
        reverseGeocodePosition();
    }, 200);
}

function reverseGeocodePosition() {
    var pos = distanceWidget.get('position');
    var distance = distanceWidget.get('distance');

    $('#Lat').val(pos.lat());
    $('#Lan').val(pos.lng());
    $('#Radius').val(distance);
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': pos }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                $('#of').html('of ' + results[1].formatted_address);
                return;
            }
        }

        $('#of').html('of somewhere');
    });
}

function updateDistance() {
    var distance = distanceWidget.get('distance');
    var pos = distanceWidget.get('position');
    $('#Lat').val(pos.lat());
    $('#Lan').val(pos.lng());
    $('#Radius').val(distance);

    //$('#dist').html(distance.toFixed(2));
}

function addActions() {



    $('#close').click(function () {
        $('#cols').removeClass('has-cols');
        google.maps.event.trigger(map, 'resize');
        map.fitBounds(distanceWidget.get('bounds'));
        $('#results-wrapper').hide();

        return false;
    });
}


function clearMarkers() {
    for (var i = 0, marker; marker = profileMarkers[i]; i++) {
        marker.setMap(null);
    }
}

google.maps.event.addDomListener(window, 'load', init);