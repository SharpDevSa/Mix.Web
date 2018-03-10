$(function () {

    Selects();

    if ($("#map").is(":visible")) {
        initialize();
    }

    if ($(".owl-carousel").offset()) {
        $('.owl-carousel').owlCarousel({
            items: 1,
            margin: 10,
            nav: true
        });
    }

    if ($(".header-menu").offset()) {
        $('.header-menu').dcMegaMenu({
            rowItems: '4',
            speed: 'fast',
            effect: 'fade'
        });
    }
    if ($('#point-nav').offset()) {
        $('#point-nav').onePageNav({
            currentClass: 'active',
            changeHash: false,
            scrollSpeed: 750,
            scrollThreshold: 0.5,
            filter: '',
            easing: 'swing'
        });
    }
});


function Selects() {

    function init () {
        var $selects = $(".i-select select").not('.defered-select');
        var totalSelects = $selects.length;
        for (var k = 0; k < totalSelects; k++) {
            this._initSelect($selects.eq(k));
        }
    };
    function _initSelect($select) {
        var controller = { active: false };
        controller.$select = $select;
        controller.$iSelect = controller.$select.parents(".i-select");
        controller.$selectValue = controller.$iSelect.find(".i-select__value");

        $selects.$window.on('click', function (e) {
            var $target = $(e.target)
            if (!$target.is(controller.$select) && !$target.is(controller.$optionsBlock)) {
                controller.close();
            }
        });
        controller.$select.on("mousedown", function (e) {
            e.preventDefault();
            controller.toggle();
        });

        controller.$options = $select.find("option");
        var totalOptions = controller.$options.length;

        var html = "<ul class='i-select-list nonselectable'>";
        for (var k = 0; k < totalOptions; k++) {
            var $option = controller.$options.eq(k);
            var selected = $option.attr("selected") ? 'i-select-list__item_active' : '';
            html += "<li class='i-select-list__item " + selected + "' data-option-id='" + k + "'>" + $option.text() + "</li>";
        }
        html += "</ul>";

        controller.$optionsBlock = $(html);
        controller.$optionsBlock.insertAfter($select).hide();
        controller.$lis = controller.$optionsBlock.find("li");
        controller.$lis.on('click', function (e) {
            controller.$lis.removeClass("i-select-list__item_active");
            var $this = $(this).addClass("i-select-list__item_active");
            var $selectedOption = controller.$options.removeAttr("selected").eq(parseInt($this.attr("data-option-id")));
            $selectedOption.attr("selected", "");
            controller.$select.attr("value", $selectedOption.attr("value")).attr("title", $selectedOption.text());
            controller.$selectValue.text($selectedOption.text()).attr("title", $selectedOption.text()).removeClass('form-select__value_placeholder');

            controller.$select.trigger("change");

            TweenMax.fromTo(controller.$selectValue, 0.5, { alpha: 0 }, { alpha: 1 });
        });

        TweenMax.set(controller.$optionsBlock, { transformOrigin: "50% 0%", transformPerspective: 300, force3D: true });
        TweenMax.set(controller.$optionsBlock, { rotationX: -5, autoAlpha: 0 });

        controller.open = function () {
            if (this.active) {
                return;
            }
            this.active = true;
            this.$iSelect.addClass("form-select_opened");
            this.$optionsBlock.show();

            TweenMax.to(this.$optionsBlock, 0.35, { rotationX: 0, autoAlpha: 1 });
        }
        controller.close = function () {
            if (!this.active) {
                return;
            }
            this.active = false;
            this.$iSelect.removeClass("form-select_opened");
            TweenMax.to(this.$optionsBlock, 0.35, { rotationX: -5, autoAlpha: 0, onComplete: $selects._hide, onCompleteParams: [this.$optionsBlock] });
        }
        controller.toggle = function () {
            this.active ? this.close() : this.open();
        }

    };
    function _hide ($el) {
        $el.hide();
    }
} 




function initialize() {

    var MY_MAPTYPE_ID = "dk";
    var stylez = [
  {
      "stylers": [
        { "hue": "#ff0000" },
        { "saturation": -100 },
        { "lightness": 34 },
        { "gamma": 0.54 }
      ]
  }
    ];

    var myOptions = {
        zoom: 15,
        scrollwheel: false,
        mapTypeControlOptions: {
            mapTypeIds: [google.maps.MapTypeId.ROADMAP, MY_MAPTYPE_ID]
        },
        mapTypeId: MY_MAPTYPE_ID
    };
    var map = new google.maps.Map(document.getElementById("map"), myOptions);

    var styledMapOptions = {
        name: "«ДСК Коловрат»"
    }

    var jayzMapType = new google.maps.StyledMapType(
        stylez, styledMapOptions);

    map.mapTypes.set(MY_MAPTYPE_ID, jayzMapType);

    // Markers init
    var companyPos = new google.maps.LatLng(54.6564726, 39.5991569);

    var companyMarker = new google.maps.Marker({
        position: companyPos,
        animation: google.maps.Animation.DROP,
        icon : "media/img/map-marker.png",
        map: map,
        title: "«ДСК Коловрат»"
    });



    var companyString = '<div class="map_text"><strong>ООО «ДСК Коловрат»</strong><br />390015, Рязань, р-н Дягилево, ул. Коняева, 142<br /><strong>Телефоны:</strong><br />+7 4912 777 372<br />+7 4912 372 000<br />+7 910 623 74 47<br />nbk@372000.ru</div>';
    var companyInfowindow = new google.maps.InfoWindow({ content: companyString });
    map.setCenter(companyPos);
    window.onresize = function () { map.setCenter(companyPos); };
    google.maps.event.addListener(companyMarker, 'click', function () { companyInfowindow.open(map, companyMarker); });

}

