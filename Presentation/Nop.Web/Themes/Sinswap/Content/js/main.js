$(function () {
    var payoffs = ["We are all born sinners.", "Take your pleasure seriously."];
    var rand = Math.floor(Math.random() * 2);
    $(".payoff").text(payoffs[rand]);

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
