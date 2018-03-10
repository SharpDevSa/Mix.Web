function DotOrgMap(startLocation, addressId, lngId, latId) {
    if (typeof (google) === 'undefined') {
        return;
    }
    var self = this;
    this.zoom = 8;
    this.addressId = addressId;
    this.lngId = lngId;
    this.latId = latId;
    this.map = initMap(startLocation, addressId + "gmap", this.zoom);
    this.marker = initMarker(startLocation, this.map);
    this.geocoder = new google.maps.Geocoder();
    this.hasChanges = false;

    var changeHasChanges = function() {
        self.hasChanges = true;
    };

    google.maps.event.addListener(this.map, "click", changeHasChanges);
    google.maps.event.addListener(this.map, "dblclick", changeHasChanges);
    google.maps.event.addListener(this.map, "drag", changeHasChanges);
    google.maps.event.addListener(this.map, "zoom_changed", changeHasChanges);

    google.maps.event.addListener(this.map, "dblclick", function (event) {
        var location = event.latLng;
        self.setMarker(location);
        
        if (!$("[name='" + self.addressId+"']").val()) {
            self.geocoder.geocode({ location: location, region: "ru-RU" }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var address = results[0].formatted_address;
                    $("[name='" + self.addressId + "']").val(address);
                } else {
                    console.log("Geocode was not successful for the following reason: " + status);
                }
            });
        }

        event.stop();
    });

};

DotOrgMap.prototype.setMarker = function (location) {
    var self = this;
    //this.map.setZoom(this.zoom);
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
    $("#" + self.latId).text(location.lat());
    $("#" + self.lngId).text(location.lng());
    $("[name='"+self.latId + "']").val(location.lat());
    $("[name='"+self.lngId + "']").val(location.lng());
};

DotOrgMap.prototype.setAddress = function (address) {
    var self = this;
    this.geocoder.geocode({ 'address': address, region: "ru" }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var location = results[0].geometry.location;
            self.setMarker(location);
        } else {
            console.log("Geocode was not successful for the following reason: " + status);
        }
    });
};

var initMap = function (startLocation, elementId, zoom) {
    var center = startLocation;
    var mapOptions = {
        center: center,
        disableDoubleClickZoom: true,
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
