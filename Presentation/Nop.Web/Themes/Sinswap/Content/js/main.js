$(function () {
    if ($('.topSellersSlide').length) {
        $('.topSellersSlide').slick({
            prevArrow: "<button type='button' class='slick-prev'><i class='las la-angle-left'></i></button>",
            nextArrow: "<button type='button' class='slick-next'><i class='las la-angle-right'></i></button>",
            dots: true,
            infinite: true,
            speed: 300,
            slidesToShow: 5,
            slidesToScroll: 5,
            arrows: true,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4,
                        infinite: true,
                        dots: true,
                        arrows: true
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3.1,
                        slidesToScroll: 3,
                        infinite: true,
                        dots: true,
                        arrows: false
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 2.1,
                        slidesToScroll: 2,
                        infinite: true,
                        dots: true,
                        arrows: false
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                        arrows: false,
                        centerMode: true
                    }
                },
                {
                    breakpoint: 475,
                    settings: {
                        slidesToShow: 1.4,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                        arrows: false,
                        centerMode: false
                    }
                },
                {
                    breakpoint: 375,
                    settings: {
                        slidesToShow: 1.2,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                        arrows: false,
                        centerMode: false
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
                        centerMode: false
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
});
