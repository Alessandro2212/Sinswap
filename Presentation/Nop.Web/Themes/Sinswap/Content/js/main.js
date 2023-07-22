$(window).on("scroll", function () {
    $('.cartFoldout').slideUp(100).removeClass('active');
    $('body').removeClass('modalActive');
});

$(function () {

    // header init
    if ($('.cartFoldout').length && !$('.cartContainer').length) {
        $('.icon.cart').click(function (e) {
            e.preventDefault();
            $('body').toggleClass('modalActive');
            $('.cartFoldout').slideToggle('fast').toggleClass('active');

            //    $("body.modalActive").click(function (e) {
            //        console.log('body clicked:' + $(e.target).attr('class'));
            //        if (!$(e.target).hasClass("fa-shopping-cart")) {
            //            $('.cartFoldout').slideUp(100).removeClass('active');
            //            $('body').removeClass('modalActive');
            //        }
            //    });
            //    $(".cartFoldout, .btn.icon.cart, .fa.fa-shopping-cart").click(function (e) {
            //        console.log('cart clicked:' + $(e.target).attr('class'));
            //        e.stopPropagation();
            //    });
        })

        $('.close-button').click(function (e) {
            e.preventDefault();
            $('.cartFoldout').slideUp(100).removeClass('active');
            $('body').removeClass('modalActive');
        })
    }


    // Tabs init
    $('.nav-tabs .nav-link').click(function () {
        $(this).tab('show');
    })

    // Search init
    if ($('.service').length) {
        $('.search').click(function (e) {
            $('body').addClass('searchActive');
        });
    }

    if ($('.searchModal').length) {
        $('.searchModal').click(function (e) {
            if ($(e.target).closest('.searchContainer').length == 0) {
                $('body').removeClass('searchActive');
            }
        });
    }

    // news cards are fully clickeable
    if ($('.newsCard').length) {
        $('.newsCard').click(function (e) {
            var url = $(e.target).closest('.newsCard').find('a').attr('href');
            window.location.href = url;
        });
    }

    // Blog cards are fully clickeable
    if ($('.blogCard').length) {
        $('.blogCard').click(function (e) {
            var url = $(e.target).closest('.blogCard').find('a').attr('href');
            window.location.href = url;
        });
    }

    // Blog detail specific
    if ($('.blogPostContainer').length) {
        $(window).resize(function () {
            setAffixedElements();
        });

        setAffixedElements();
    }

    // Signup specific
    if ($('.vendorRegistration .swiper').length) {
        const registrationSwiper = new Swiper(".swiper", {
            //navigation: {
            //    nextEl: ".swipe-next",
            //},
            allowSlideNext: true,
            allowSlidePrev: false,
            allowTouchMove: false,
            followFinger: false,
            grabCursor: false
        });

        if ($(registrationSwiper.slides[registrationSwiper.activeIndex]).find('.firstField').length) {
            $(registrationSwiper.slides[registrationSwiper.activeIndex]).find('.firstField').focus();
        }

        registrationSwiper.on('slideChange', function () {
            // set icon bar
            $(".stepIconContainer li").removeClass('active');
            $(".stepIconContainer .icon" + registrationSwiper.activeIndex).addClass('active');

            // include fields in form validation
            $(registrationSwiper.slides[registrationSwiper.activeIndex]).find(".ignore").removeClass('ignore');

            // set tab index for subform
            $('.swiper-slide-active input').attr('tabindex', "-1");
            $('.swiper-slide-next input, .swiper-slide-next select').attr('tabindex', "1");
            if ($('.swiper-slide-next #txtcaptcha').length) {
                $('.swiper-slide-next #divcaptch').show();
            }

            // focus on first field of subform
            if ($('.swiper-slide-next .firstField').length) {
                //$('.swiper-slide-next .firstField').focus();
                var elemId = $('.swiper-slide-next .firstField').attr('id');
                console.log("focus on " + elemId);
                setTimeout(function () { $(elemId).focus(); }, 1000);
            }
        });

        $(".vendorRegistrationForm").validate({
            ignore: ".ignore",
            rules: {
                Password: {
                    required: true,
                    minlength: 8
                },
                ConfirmPassword: {
                    required: true,
                    minlength: 8,
                    equalTo: "[name='Password']"
                },
                Gender: {
                    required: true
                },
                FirstName: {
                    required: true
                },
                LastName: {
                    required: true
                },
                ShopName: {
                    required: true
                },
                StreetAddress: {
                    required: true
                },
                ZipPostalCode: {
                    required: true
                },
                City: {
                    required: true
                },
                County: {
                    required: true
                },
                AcceptPrivacyPolicyEnabled: {
                    required: true
                },
                txtcaptcha: {
                    required: true
                }
            },
            messages: {
                Password: {
                    required: "Sorry, I really need you to take the security of your shop more seriously",
                    minlength: "Like my girlfriend said: I think this is a little bit too short (at least 8 characters please)"
                },
                ConfirmPassword: {
                    required: "Please confirm your password so we're both sure you didn't make a typo",
                    minlength: "That doesn't even remotely look like the previous password",
                    equalTo: "The passwords are supposed to be the same... try harder"
                },
                Gender: {
                    required: "I know it's gotten complicated, but please choose the one that you like best"
                },
                FirstName: {
                    required: "How are we supposed to get to know you without a name?"
                },
                LastName: {
                    required: "Sorry Cher, last name is required here"
                },
                ShopName: {
                    required: "How would people even find your nameless shop?"
                },
                StreetAddress: {
                    required: "This is for legal reasons, we won't show up the the next BBQ, we promise"
                },
                ZipPostalCode: {
                    required: "No zip, no shop"
                },
                City: {
                    required: "Seriously, we need to know all this stuff"
                },
                County: {
                    required: "Yes, this field is also mandatory..."
                },
                AcceptPrivacyPolicyEnabled: {
                    required: "A relationship without trust is like a car without gas. You can stay in it, but it won't go anywhere"
                },
                txtcaptcha: {
                    required: "Sorry R2-D2, no market for robot panties..."
                }
            },
            errorPlacement: function (error, element) {
                if (element.is(":radio") || element.is(":checkbox")) {
                    error.appendTo('.swiper-slide-active .form-fields');
                }
                else {
                    error.insertAfter(element);
                }
            },
            submitHandler: function (form) {
                //console.log('submitting form');
                form.submit();
            }
        });

        $(".vendorRegistrationForm .swipe-next").click(function () {
            if ($(".vendorRegistrationForm").valid()) {
                registrationSwiper.slideNext();
            }
        });
    }

    function setAffixedElements() {
        if ($('.socialShareContainer').length) {
            $('.socialShareContainer').removeData('bs.affix').removeClass('affix affix-top affix-bottom');
            var elem = $(".socialShareContainer");
            var elemTop = elem.offset().top - 25;

            var container = $('.post-body');
            var containerBottom = $('body').outerHeight(true) - (container.offset().top + container.outerHeight(true));
            elem.affix({ offset: { top: elemTop, bottom: containerBottom } });
        }

        //if ($('.blogPostSidebar .advertisement').length) {
        //    var elem = $(".blogPostSidebar .advertisement");
        //    var elemTop = elem.offset().top + 42;

        //    var container = $('.blogPostSidebar');
        //    var containerBottom = $('body').outerHeight(true) - (container.offset().top + container.outerHeight(true));
        //    elem.affix({ offset: { top: elemTop, bottom: containerBottom } });
        //}
    }

    if ($('#productModal').length) {
        //$('.categoryCard').click(function (e) {
        //    e.preventDefault();
        //    console.log($(e.target).attr('data-url'));
        //    var prodUrl = "/en" + $(e.target).attr('data-url');
        //    $('#productModal .modal-body').load(prodUrl, function () {
        //        $('#productModal').modal({ show: true });
        //    });
        //});

        $('#productModal').on('show.bs.modal', function (e) {
            $('#productModal').find('.modal-body').html("");

            var prodUrl = $(e.relatedTarget).closest('.categoryCard').attr('data-url');

            $.get(prodUrl, function (data) {
                productContent = $(data).find('.productDetailContainer');
                $('#productModal').find('.modal-body').html(productContent);
            })
        });
    }

    /*if ($('#productVendorModal').length) {*/
    $('#productVendorModal').on('show.bs.modal', function (e) {
        $('#productVendorModal').find('.modal-body').html("");
        var prodVendorUrl = $(e.relatedTarget).closest('.categoryCard').attr('data-url');
        $.get(prodVendorUrl, function (data) {
            $('#productVendorModal').find('.modal-body').html(data);
            window.stop();
        })
    });
    //}

    if ($('.topSellersSlide').length) {
        $('.topSellersSlide').slick({
            prevArrow: "<button type='button' class='slick-prev'><i class='las la-angle-left'></i></button>",
            nextArrow: "<button type='button' class='slick-next'><i class='las la-angle-right'></i></button>",
            speed: 300,
            slidesToShow: 5,
            slidesToScroll: 5,
            dots: false,
            arrows: false,
            infinite: true,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 5,
                        dots: false,
                        arrows: false,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3.1,
                        slidesToScroll: 3,
                        dots: false,
                        arrows: true,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 2.1,
                        slidesToScroll: 2,
                        dots: false,
                        arrows: true,
                        infinite: true
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1.7,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true
                    }
                },
                {
                    breakpoint: 475,
                    settings: {
                        slidesToShow: 1.4,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true
                    }
                },
                {
                    breakpoint: 375,
                    settings: {
                        slidesToShow: 1.2,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true
                    }
                },
                {
                    breakpoint: 300,
                    settings: {
                        slidesToShow: 1.1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                        arrows: false
                    }
                }
            ]
        });
    }


    if ($('.categoryCard').length && !$('.vendorPremium').length) {
        $('.categoryCard').click(function (e) {
            window.location.href = $(e.target).find('a').attr('href');
        });
    }


    if ($('.vendorMediaSlide').length) {
        lightGallery(document.getElementById('lg_vendorMedia'), {
            speed: 500,
            download: false,
            counter: false,
            zoomFromOrigin: true,
        });

        $('.vendorMediaSlide').slick({
            prevArrow: "<button type='button' class='slick-prev'><i class='las la-angle-left'></i></button>",
            nextArrow: "<button type='button' class='slick-next'><i class='las la-angle-right'></i></button>",
            speed: 300,
            slidesToScroll: 3,
            dots: false,
            arrows: true,
            infinite: true,
            variableWidth: true,
            centerMode: true,
            //responsive: [
            //    {
            //        breakpoint: 1200,
            //        settings: {
            //            slidesToShow: 5,
            //            slidesToScroll: 1,
            //            dots: false,
            //            arrows: false,
            //            infinite: true,
            //            centerMode: true,
            //            centerPadding: '10px'
            //        }
            //    },
            //    {
            //        breakpoint: 992,
            //        settings: {
            //            slidesToShow: 3.1,
            //            slidesToScroll: 3,
            //            dots: false,
            //            arrows: true,
            //            infinite: true,
            //            centerMode: true,
            //            centerPadding: '10px'
            //        }
            //    },
            //    {
            //        breakpoint: 768,
            //        settings: {
            //            slidesToShow: 2.1,
            //            slidesToScroll: 2,
            //            dots: false,
            //            arrows: true,
            //            infinite: true
            //        }
            //    },
            //    {
            //        breakpoint: 576,
            //        settings: {
            //            slidesToShow: 1.7,
            //            slidesToScroll: 1,
            //            dots: false,
            //            arrows: false,
            //            infinite: true
            //        }
            //    },
            //    {
            //        breakpoint: 475,
            //        settings: {
            //            slidesToShow: 1.4,
            //            slidesToScroll: 1,
            //            dots: false,
            //            arrows: false,
            //            infinite: true
            //        }
            //    },
            //    {
            //        breakpoint: 375,
            //        settings: {
            //            slidesToShow: 1.2,
            //            slidesToScroll: 1,
            //            dots: false,
            //            arrows: false,
            //            infinite: true
            //        }
            //    },
            //    {
            //        breakpoint: 300,
            //        settings: {
            //            slidesToShow: 1.1,
            //            slidesToScroll: 1,
            //            infinite: true,
            //            dots: false,
            //            arrows: false
            //        }
            //    }
            //]
        });
    }


    if ($('.featuredContainer').length) {
        $('.featuredCard').hover(
            function () {
                $(this).closest('.featuredContainer').find('.featuredCard').addClass('hover');
            }, function () {
                $(this).closest('.featuredContainer').find('.featuredCard').removeClass('hover');
            }
        );
    }


    if ($('.blogListContainer').length) {
        $('.blogListContainer').isotope({
            itemSelector: '.blogItem',
            stamp: '.questionFormContainer',
            masonry: {
                columnWidth: '.blogItem',
            }
        });
    }


    $(".h-card").hover(
        function (e) {
            let elem = e.target.closest(".h-card");
            $(elem).addClass("hover");
        },
        function (e) {
            let elem = e.target.closest(".h-card");
            $(elem).removeClass("hover");
        }
    );


    $.stellar({
        horizontalScrolling: false
    });
});