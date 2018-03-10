function ProjectMap(startLocation, elementId) {
    if (typeof (google) === 'undefined') {
        return;
    }
    var self = this;
    this.zoom = 8;
    this.map = initMap(startLocation, elementId, this.zoom);
    this.marker = initMarker(startLocation, this.map);
    this.geocoder = new google.maps.Geocoder();

    google.maps.event.addListener(this.map, "dblclick", function (event) {
        var location = event.latLng;
        self.setMarker(location);
    });

};

ProjectMap.prototype.setMarker = function (location) {
    var self = this;
    this.map.setZoom(this.zoom);
    if (self.marker != null) {
        self.marker.setMap(null);
    }
    window.setTimeout(function () {
        self.map.panTo(location);
    }, 100);
    this.marker = new google.maps.Marker({
        position: location,
        map: this.map
    });
    $(".latitude").text(location.lat());
    $(".longitude").text(location.lng());
    $("#lat").val(location.lat());
    $("#lng").val(location.lng());
};

ProjectMap.prototype.setAddress = function (address) {
    var self = this;
    this.geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var location = results[0].geometry.location;
            self.setMarker(location);
        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
};

var initMap = function (startLocation, elementId, zoom) {
    var center = startLocation;
    if (center == null) {
        center = new google.maps.LatLng(55.7517, 37.6178); // координаты москвы
    }
    var mapOptions = {
        center: center,
        disableDoubleClickZoom: false,
        zoom: zoom,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    return new google.maps.Map(document.getElementById(elementId), mapOptions);
};

var initMarker = function (startLocation, map) {
    return new google.maps.Marker({
        position: startLocation,
        map: map
    });
};
