$(function () {
    if ($('.topSellersSlide').length) {
        $('.topSellersSlide').slick({
            prevArrow: "<button type='button' class='slick-prev'><i class='las la-angle-left'></i></button>",
            nextArrow: "<button type='button' class='slick-next'><i class='las la-angle-right'></i></button>",
            speed: 300,
            slidesToShow: 5,
            slidesToScroll: 5,
            dots: false,
            arrows: true,
            infinite: true,
            centerMode: true,
            centerPadding: '10px',
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4,
                        dots: false,
                        arrows: true,
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
                        arrows: false,
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
                        arrows: false,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1.7,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 475,
                    settings: {
                        slidesToShow: 1.4,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 375,
                    settings: {
                        slidesToShow: 1.2,
                        slidesToScroll: 1,
                        dots: false,
                        arrows: false,
                        infinite: true,
                        centerMode: true,
                        centerPadding: '10px'
                    }
                },
                {
                    breakpoint: 300,
                    settings: {
                        slidesToShow: 1.1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                        arrows: false,
                        centerMode: true
                    }
                }
            ]
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

    $.stellar();
});
