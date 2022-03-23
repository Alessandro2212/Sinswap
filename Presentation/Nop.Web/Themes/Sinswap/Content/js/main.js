$(function () {


    if ($('.service').length) {
        $('.search').click(function (e) {
            $('body').toggleClass('searchActive');
        });
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
            dots: true,
            arrows: true,
            infinite: true,
            variableWidth: true,
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
