Array.prototype.in_array = function (p_val) {
    for (var i = 0, l = this.length; i < l; i++) {
        if (this[i] == p_val) {
            return true;
        }
    }
    return false;
}


$(function () {

    $("body").data("s", { resized: false });
    $("body").data("_global", { closeMenuTM: 0, changeMenuItemTM: 0, menuActiveList: false, targetElement: false, isHover: false, playButtonInited: false, player: false });
    preload();
});

function init() {

    $("#tabs").organicTabs({
        "speed": 200
    });


    $('.slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.slider-nav',
        swipe: false
    });
    $('.slider-nav').slick({
        //arrows: false,
        slidesToShow: 4,
        slidesToScroll: 1,
        asNavFor: '.slider-for',
        dots: true,
        //centerMode: true,
        focusOnSelect: true,
        swipe: false,
        responsive: [
   {
       breakpoint: 980,
       settings: {
           slidesToShow: 3,
           swipe: true,
           infinite: false
       }
   },
   {
       breakpoint: 801,
       settings: {
           slidesToShow: 2,
           swipe: true,
           infinite: false
       }
   },
   {
       breakpoint: 641,
       settings: {
           slidesToShow: 4,
           swipe: true,
           infinite: false
       }
   },
    {
        breakpoint: 481,
        settings: {
            slidesToShow: 3,
            swipe: true,
            infinite: false,
            dots: true
        }
    },
        {
            breakpoint: 381,
            settings: {
                slidesToShow: 2,
                swipe: true,
                infinite: false,
                dots: true
            }
        }
        ]
    });

    $('#product-details-owl').owlCarousel({
        nav: true,
        items: 1,
        responsive: {
            641: {
                items: 2
            }
        }
        //nav: true
    });

    Selects.init();
    //initMenu();
    initVideo();
    applySizes();
    $(window).on('resize', applySizes);
    $(document).on("click", "#search-icon", toggleSearch);
    $(document).on("blur", "#search-start-text", toggleSearch);
    $(document).on("click", "#seacrh-start-button", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#ya-site-form0 .ya-site-form__submit").click();
    });

    $(document).on("keyup change", "#search-start-text", function (e) {

        $("#ya-site-form0 .ya-site-form__input-text").val($(this).val());
        if (e.type == "keyup" && e.keyCode == 13) {
            $("#ya-site-form0 .ya-site-form__submit").click();
        }

    });


    if ($("#map").is(":visible")) {
        initialize();
    }

    $("#product-slider").owlCarousel({
        items: 1,
        margin: 10,
        nav: true,
        loop: true
    });

    $("#top-page-slider").owlCarousel({
        items: 1,
        margin: 10,
        nav: true,
        loop: true
    });

    $("[data-select-block]").on("change", function () {
        var $this = $(this);
        var value = $this.val();
        var targetsSelector = $this.data("select-block");
        var newSection = $("#" + value);
        $(targetsSelector).hide();
        newSection.show();
        TweenMax.staggerFromTo(newSection.find(".products-list_item"), 0.5, { alpha: 0, scale: 0.8 }, { alpha: 1, scale: 1, ease: Back.easeOut }, 0.1);
    });



    $("[data-select-partners]").on("change", function () {
        var $this = $(this);
        //var value = Number($this.val());

        var value
        var options = document.getElementById("distributorsCoboBox").options;
        for (var i = 0; i < options.length; i++) {
            if (options[i].hasAttribute('selected')) {
                value =Number(options[i].getAttribute('value'));
                break;
            }
        }
        
        var partners = $($this.data("select-partners")).find(".partners-item");
        var hideCollection = [];
        var showCollection = [];
        partners.removeClass("target");
        partners.find(".partners-item_desc-country span").removeClass("active");
        partners.each(function (index) {
            var el = partners.eq(index);
            //console.log(el.data("countries").in_array(value));
            if (el.data("countries").in_array(value)) {
                showCollection.push(el)
                el.find(".country-" + value).addClass("active");
                el.addClass("target");
            }
            else {

                hideCollection.push(el);
            }
        });
        if (value > 0) {
            TweenMax.staggerFromTo(hideCollection, 0.5, { alpha: 1, scale: 1 }, { alpha: 0, scale: 0.7, ease: Back.easeOut }, 0.1);
            TweenMax.staggerFromTo(showCollection, 0.5, { alpha: 0, scale: 0.7 }, { alpha: 1, scale: 1, ease: Back.easeOut }, 0.1);
        }
        else {
            TweenMax.staggerFromTo(partners, 0.5, { alpha: 0, scale: 0.7 }, { alpha: 1, scale: 1, ease: Back.easeOut }, 0.1);
        }


    });

    $("[data-select-retailers]").on("change", function () {
        var $this = $(this);
        //var value = $this.val();
        
        var value
        var options = document.getElementById("retailersComboBox").options;
        for (var i = 0; i < options.length; i++) {
            if (options[i].hasAttribute('selected')) {
                value = options[i].getAttribute('value');
                break;
            }
        }

        var targetsSelector = $("#retailers-list");
        var newSection = (value === "all") ? targetsSelector.find(".inner-category") : $("#" + value);
        if (newSection.size() == 1) {
            targetsSelector.find(".inner-category").hide();
        }

        newSection.show();
        TweenMax.staggerFromTo(newSection.find(".partners-item"), 0.5, { alpha: 0, scale: 0.8 }, { alpha: 1, scale: 1, ease: Back.easeOut }, 0.1);
    });

    $(".category-navigation").css({
        top: ($(window).height() - $(".category-navigation").outerHeight()) / 2
    });

    if ($('#point-nav').offset()) {

        $('#point-nav').onePageNav({
            currentClass: 'active',
            changeHash: false,
            scrollSpeed: 750,
            scrollThreshold: 0.5,
            filter: '',
            easing: 'swing',
            scrollChange: function (item) {
                var menu = $("#point-nav");
                menu.removeClass("dark").removeClass("white").addClass(item.data("classname"));
            },
            end: function () {
                var menu = $("#point-nav");
                menu.removeClass("dark").removeClass("white").addClass($('#point-nav .active').data("classname"));
            }
        });

        $('.tooltip-link').tooltip({
            container: 'body',
            placement: 'left'
        });
    }

    if ($('#scroll-up').offset()) {

        if ($(window).scroll(0)) {
            $('#scroll-up').hide();
        }

        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                $('#scroll-up').fadeIn();
            } else {
                $('#scroll-up').fadeOut();
            }
        });

        $('#scroll-up').on("click", function () {
            $('body,html').animate({
                scrollTop: 0
            }, 400);
            return false;
        });
    }

    if ($('#lamp').is(":visible")) {

        var lampEffect = ("#effect-lamp .effect");
        var Waves = ("#radio-waves span");
        TweenMax.set(lampEffect, { alpha: 0, scale: 0.1 });
        TweenMax.set(Waves, { alpha: 0 });
        $("#lamp").on("mouseenter", function () {
            var tl = new TimelineMax();
            tl.staggerFromTo(Waves, 0.35, { alpha: 0 }, { alpha: 1, ease: Power2.easeOut }, 0.05, 0.3);
            tl.to(lampEffect, 1, { alpha: 1, scale: 1, ease: Power2.easeOut });
        });
        $("#lamp").on("mouseleave", function () {
            TweenMax.to(Waves, 0.35, { alpha: 0, ease: Power2.easeOut }, 0.3);
            TweenMax.to(lampEffect, 0.5, { alpha: 0, scale: 0.1, ease: Power2.easeOut });
        });

    }

    if ($('.speaker-button').offset()) {
        $('.speaker-button a').on("click", function (e) {
            e.preventDefault();
            if ($('#speaker-gallery img').attr('src') != $(this).attr('href')) {
                $(this).parents(".speaker-button").find(".active").removeClass("active");
                $(this).addClass("active");
                $('#speaker-gallery img').attr('src', $(this).attr('href'));
            }

        });
    }

    $("#mobile-menu-toggler").on("click", function (e) {
        e.preventDefault();
        $(this).toggleClass("active");
        $("#menu").toggle();
    });

    $(document).on("click", "div.product-card .product-card_image", rotateProductCard);
    $(document).on("click", "[data-url]", toggleProductColor);

    $(document).on('submit', '.ajax-form', function (event) {
        event.preventDefault();
        loader("show");
        var $this = $(this);
        var data = $this.serialize();
        var url = $this.attr("action");
        $.post(url, data, function (data) {
            loader("hide");
            swal({
                title: "Thank you!",
                text: "You are subscribed now!",
                type: "success",
                allowOutsideClick: true,
                confirmButtonColor: "#3bb077"
            });

            if (!!data && !!data.errors && !!data.errors.length) {
                console.log(data.errors);
            }
            else {

            }
        });
    });
}

function loader(dir) {
    if (dir == "show") {
        $("#mask").show();
        TweenMax.to($("#mask"), 0.5, { alpha: 1, ease: Power2.easeOut });
    }
    else if (dir == "hide") {

        TweenMax.to($("#mask"), 0.5, { alpha: 0, ease: Power2.easeOut, onComplete: function () { $("#mask").hide(); } });
    }
}

function applySizes() {

    if ($("#point-nav.about-point-nav").is(":visible")) {
        $("#point-nav.about-point-nav").css("left", $("#header-logo-container").offset().left); //align of point nav at the about page relative to logotype
    }

    var w = $(window).width();
    var menuContainer = $("#menu");
    var mobileToggler = $("#mobile-menu-toggler");

    $("#menu #search-start-text").removeAttr("placeholder");
    $("#menu .search-field").removeClass("closed");

    if (w <= 1120) {

        $("body").data("s").resized = true;
        var search = $(".header-aside_search .search-field").clone(true);

        menuContainer.append(search);
        mobileToggler.show();

        if ($("#mobile-menu-toggler").hasClass("active")) {
            menuContainer.show();
        } else {
            menuContainer.hide();
        }
        $("#touch-menu li").off("click");
        $("#touch-menu li").on('click', function (e) {

            //e.preventDefault();
            e.stopPropagation();
            var toggler = $(this);
            var section = $(this).find(".toggle-section").eq(0);
            if (toggler.hasClass("opened")) {
                toggler.removeClass("opened");
                section.hide();
            }
            else {
                toggler.addClass("opened");
                section.show();
            }

        });

    } else if (w > 1120) {

        menuContainer.find(".search-field").remove();
        mobileToggler.hide();
        $("#touch-menu li").off("click");
        $("#touch-menu ul, #touch-menu li").attr("style", "");
        initMenu();
        menuContainer.show();

    }
}


function toggleProductColor(e) {

    e.stopPropagation();
    var toggler = $(this);
    if (!toggler.hasClass("active")) {

        var card = toggler.parents(".product-card");
        var img = card.find(".product-card_image-cover img").eq(0);
        card.find(".active").removeClass("active");
        toggler.addClass("active");
        TweenMax.to(img, 0.3, { alpha: 0, scale: 0.9, ease: Power2.easeOut });
        productPreloaderShow(card.find(".product-card_image-cover"));
        img.off("load");
        img.on("load", function () {

            TweenMax.to(card.find(".spinner"), 0.3, { alpha: 0, ease: Power2.easeOut });
            TweenMax.to(img, 0.3, { alpha: 1, scale: 1, ease: Power2.easeOut, delay: 0.3 });
        });

        img.attr("src", toggler.data("url") + "&r=" + Math.random());
    }

}

function productPreloaderShow(container) {

    if (container.find(".spinner").size() > 0) {
        TweenMax.set(container.find(".spinner"), { alpha: 1 });
    }
    else {
        container.append($("#product-spinner .spinner").clone(true));
    }


}


function rotateProductCard(e) {

    e.stopPropagation();
    var product = $(this).parents(".product");
    var productCard = $(this).parents(".product-card");
    if (productCard.hasClass("front-side")) {
        rotateProductCardToBack(product);
    }
    else {
        rotateProductCardToFront(product);
    }





    //var p = el.parents(".wrapper-item");
    //var h = p.height() + 20;//20 padding item
    //var w = p.parents(".list-product").eq(0).width() + ($(window).width() <= 980 ? 0 : 20);
    //var wrapper = p.find(".wrapper-item");

    //if (p.hasClass("opened")) {

    //    p.removeClass("opened");
    //    TweenMax.to(wrapper, 0.5, { scale: 0.7, alpha: 0, ease: Power2.easeOut, onComplete: function () { wrapper.hide(); p.css("z-index", 0).find(".item").css("z-index", 1) } });
    //}
    //else {

    //    if ($(window).width() >= 768) {
    //        //alert($(window).width());
    //        p.addClass("opened").css("z-index", 3).find(".item").css("z-index", 3);
    //        TweenMax.set(wrapper, { display: "block", alpha: 0, scale: 0.7, width: w });
    //        h = wrapper.height() < h ? h : wrapper.height() + 20;
    //        wrapper.find(".item").css("height", h);
    //        TweenMax.to(wrapper, 0.5, { scale: 1, alpha: 1, ease: Power2.easeOut });
    //    }
    //}


}

function rotateProductCardToBack(parent) {

    var oldEl = parent.find(".front-side"),
        newEl = parent.find(".back-side");
    var oldHeight = oldEl.outerHeight(),
        newHeight = newEl.outerHeight();

    if (oldHeight < newHeight) {

        oldEl.data("_g", { height: oldHeight });
        TweenMax.to(oldEl, 0.35, { height: newHeight, ease: Circ.easeOut });

    }

    parent.css("z-index", 100);
    TweenMax.to(oldEl, 0.35, { rotationY: 90, scale: 0.8, ease: Circ.easeIn });
    TweenMax.set(oldEl, { alpha: 0, zIndex: 1, delay: 0.36 });
    TweenMax.set(newEl, { rotationY: -90, delay: 0.37, alpha: 1, scale: 0.8, zIndex: 2 });
    TweenMax.to(newEl, 0.35, { rotationY: 0, scale: 1, ease: Circ.easeOut, delay: 0.45 });

}

function rotateProductCardToFront(parent) {

    var oldEl = parent.find(".back-side"),
        newEl = parent.find(".front-side");

    if (newEl.data("_g") && newEl.data("_g").height < oldEl.outerHeight()) {

        TweenMax.to(newEl, 0.35, { height: newEl.data("_g").height, ease: Circ.easeIn });

    }

    TweenMax.to(oldEl, 0.35, { rotationY: 90, scale: 0.8, ease: Circ.easeIn });
    TweenMax.set(oldEl, { alpha: 0, zIndex: 1, delay: 0.36 });
    TweenMax.set(newEl, { rotationY: -90, delay: 0.37, alpha: 1, scale: 0.8, zIndex: 2 });
    TweenMax.to(newEl, 0.35, { rotationY: 0, scale: 1, ease: Circ.easeOut, delay: 0.45, onComplete: function () { parent.css("z-index", 1); } });

}


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
            var link = controller.$select.hasClass("lang-select") ? "<a href='" + $option.val() + "'>" : "";
            var link2 = controller.$select.hasClass("lang-select") ? "</a>" : "";
            html += "<li class='i-select-list__item " + selected + "' data-option-id='" + k + "'>" + link + $option.text() + link2 + "</a></li>";
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



function toggleSearch(e) {

    var search = $("#main-header .search-field");
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
var _menuDelay = 10;

function initMenu() {

    $("#menu .parent[role='menuitem']").each(function () {

        var $this = $(this);
        $this.on("click", function (e) {
            e.preventDefault();
            if ($this.hasClass("current")) {
                $this.removeClass("current");
                closeSubmenu();
            }
            else {
                $this.addClass("current");
                openSubmenu();
            }

        });

    });
}

function openSubmenu() {

    var targetElement = $("#menu .current").eq(0),
        haveChildren = targetElement.hasClass("parent"),
        submenu = $("#submenu");

    if (haveChildren) {
        var targetElement = $("#menu .current").eq(0), children = targetElement.find(".inner-menu").clone(true), submenu = $("#submenu");
        submenu.show();
        submenu.html(children);
        TweenMax.to(submenu, 0.1, { autoAlpha: 1, ease: Power2.easeIn, force3D: true, onComplete: function () { submenu.addClass("open") } });
        TweenMax.staggerFromTo(submenu.find("li"), 0.5, { alpha: 0, scale: 0.95, rotationX: -10 }, { alpha: 1, scale: 1, rotationX: 0, ease: Back.easeOut, delay: 0.05 }, 0.02);
    }

}

function closeSubmenu() {

    var submenu = $("#submenu");
    TweenMax.to(submenu, 0.2, { alpha: 0, onComplete: function () { submenu.removeClass("open").hide(); } });
}

function preload() {

    var l = new PxLoader();
    $("img").each(function () {

        var item = $(this);
        var pxImage = new PxLoaderImage(item.attr("src"));
        l.add(pxImage);
    });
    l.addProgressListener(function (e) {
        $("#mask-for-page-loading .scale").css("width", "" + Math.ceil((e.completedCount / e.totalCount) * 100) + "%");
    });
    l.addCompletionListener(function (e) {
        $("#mask-for-page-loading").fadeOut();
        init();
    });
    l.start();
}

function initVideo() {

    var videos = $(".video-wrap");

    videos.each(function (index) {

        var v = $(this);
        var btn = v.find(".video-button");
        v.on("click", function (e) {

            e.stopPropagation();
            toggleVideo(v);

        });

    })

}

function toggleVideo(video) {

    var playButton = video.find(".video-wrap_play"),
        pauseButton = video.find(".video-wrap_pause"),
        _video = video.find("video").get(0);

    $("body").data("_global").player = _video;
    if (_video.paused) {

        _video.play();
        playButton.addClass("video-wrap_hide");
        pauseButton.removeClass("video-wrap_show").css("z-index", 5);
    }
    else {
        _video.pause();
        pauseButton.addClass("video-wrap_show");
    }
    if (!$("body").data("_global").playButtonInited) {

        _video.onended = function () {
            playButton.removeClass("video-wrap_hide").css("z-index", 5);
            pauseButton.css("z-index", 4);
            $("body").data("_global").playButtonInited = true;
        }

    }
}

function playVideo(video) {

    var playButton = video.find(".video-wrap_play"),
        pauseButton = video.find(".video-wrap_pause"),
        _video = video.get(0);
    _video.play();
    playButton.addClass("video-wrap_hide");
    pauseButton.removeClass("video-wrap_show").css("z-index", 5);
}

function stopVideo(video) {

    var playButton = video.find(".video-wrap_play"),
        pauseButton = video.find(".video-wrap_pause"),
        _video = video.get(0);
    _video.pause();
    pauseButton.addClass("video-wrap_show");
}