$(function () {

    $("body").data("s", { resized: false});

    //PointNav.init();
    Selects.init();

    $(document).on("click", "#search-icon", toggleSearch);
    $(document).on("click", "#search-icon", toggleSearch);
    $(document).on("click", "#search-icon .search-field-icon", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#ya-site-form0 .ya-site-form__submit").click();
    });
    $(document).on("click", "#search-icon .search-field", function (e) { e.stopPropagation(); });

    $(document).on("blur", "#search-start-text", toggleSearch);
    $(document).on("keyup change", "#search-start-text", function () {
        $("#ya-site-form0 .ya-site-form__input-text").val($(this).val());
    });


    $(document).on("change keyup", ".count-cell input", function () {
        if (isNaN($(this).val()) || $(this).val() < 0) { $(this).val(1); }
    });

    if ($('.header-menu').offset()) {
        $('.header-menu').dcMegaMenu({ rowItems: '4', speed: 'fast', effect: 'fade' });
    }

    //menu();
    //$(window).on('resize', menu);

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
    

    $("#touch-icon").on("click tap", function () {
        var menu = $("#touch-menu");
        if (menu.hasClass("open")) {
            menu.slideUp().removeClass("open");
        }
        else {
            menu.slideDown().addClass("open");
        }
    });

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
	
	 if ($('#lamp').is(":visible")) {
        var lampEffect = ("#effect-lamp .effect");
        var Waves = ("#radio-waves span");
        TweenMax.set(lampEffect, { alpha: 0 });
        TweenMax.set(Waves, { alpha: 0 });
        $("#lamp").on("mouseenter", function () {
            var tl = new TimelineMax();
            tl.staggerFromTo(Waves, 0.35, { alpha: 0 }, { alpha: 1, ease: Power2.easeOut }, 0.05, 0.3);
            tl.to(lampEffect, 1, { alpha: 1, ease: Power2.easeOut });
        });
        $("#lamp").on("mouseleave", function () {
            TweenMax.to(Waves, 0.35, { alpha: 0, ease: Power2.easeOut }, 0.3);
            TweenMax.to(lampEffect, 0.5, { alpha: 0, ease: Power2.easeOut });
        });
	}


});


//function menu() {
//    var w = $(window).width();
//    if (w < 1120 && !$("body").data("s").resized) {
//        $("body").data("s").resized = true;
    
//    } else if (w >= 1120 && $("body").data("s").resized) {
//        $("body").data("s").resized = false;
       
//    }

//}




//function applySizes() {

//    var w = $(window).width();
//    if (w < 1120 && !$("body").data("s").resized) {
//        $("body").data("s").resized = true;
//        var childs = $(".header-menu-bottom ul li").clone(true);
//        $("#touch-menu").append("<ul id='cloned-menu'>");
//        var ul = $("#cloned-menu");
//        ul.append(childs)
//        var headerIcons = $(".header-aside_cart").clone(true);
//        $(".wrap-header-menu-top").append(headerIcons);
//    } else if (w >= 1120 && $("body").data("s").resized) {
//        $("body").data("s").resized = false;
//        $(".wrap-header-menu-top .header-aside_cart").remove();
//        $("#touch-menu").show();
//        $("#cloned-menu").remove();
//    }

//}


var Selects = {

    init: function () {
        var $selects = $(".i-select select").not('.defered-select');
        var totalSelects = $selects.length;
        for (var k = 0; k < totalSelects; k++) {
            this._initSelect($selects.eq(k));
        }
    },
    _initSelect: function ($select) {
        var controller = { active: false };

        controller.$select = $select;
        controller.$iSelect = controller.$select.parents(".i-select");
        controller.$selectValue = controller.$iSelect.find(".i-select__value");

        $(window).on('click', function (e) {
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
        try {
            //console.log($select.get(0).selectedIndex);
            controller.$selectValue.text($select.get(0).options[$select.get(0).selectedIndex].text);
        }
        catch (e) { console.log(e); }
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
            TweenMax.to(this.$optionsBlock, 0.35, { rotationX: -5, autoAlpha: 0, onComplete: Selects._hide, onCompleteParams: [this.$optionsBlock] });
        }
        controller.toggle = function () {
            this.active ? this.close() : this.open();
        }

    },
    _hide: function ($el) {
        $el.hide();
    }

}


//var PointNav = {
//    init: function () {
//        this.$element = $(".point-nav");
//        if (!this.$element.length) {
//            return;
//        }
//        var self = this;
//        var startOffset = this.$element.offset().top;
//        PointNav.$window.on("scroll", function () {
//            var scrolledBy = PointNav.$window.scrollTop()
//            if (scrolledBy > 860) {
//                self.$element.addClass("point-nav_dark");
//            } else {
//                self.$element.removeClass("point-nav_dark");
//            }
//            TweenMax.set(self.$element, { top: scrolledBy + startOffset })
//        })
//    }
//}




function toggleSearch(e) {

    var search = $("#search-icon .search-field");
    if (search.hasClass("closed")) {
        TweenMax.set(search, { alpha: 0, width: 0, display: "block" });
        TweenMax.to(search, 0.5, {
            alpha: 1, width: 240, ease: Power2.easeOut, onComplete: function () {
                search.removeClass("closed");
            }
        });

    }
    else {
        TweenMax.to(search, 0.5, {
            alpha: 0, width: 0, ease: Power2.easeOut, onComplete: function () {
                TweenMax.set(search, { display: "none" });
                search.addClass("closed");
            }
        });

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

