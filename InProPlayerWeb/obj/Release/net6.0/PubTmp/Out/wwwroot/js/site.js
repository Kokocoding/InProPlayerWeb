$(document).ready(function () {
    var trunBtn = $(".turnBtn");

    trunBtn.on("click", function () {
        var onoff = "";
        if ($(this).hasClass("bi-toggle2-off")) {
            $(this).removeClass("bi-toggle2-off").removeClass("btn-primary").addClass("btn-outline-primary").addClass("bi-toggle2-on").html("&nbsp;開啟");
            onoff = true;
        } else if ($(this).hasClass("bi-toggle2-on")) {
            $(this).removeClass("bi-toggle2-on").removeClass("btn-outline-primary").addClass("btn-primary").addClass("bi-toggle2-off").html("&nbsp;關閉");
            onoff = false;
        }

        $(this).blur();

        $.ajax({
            type: "POST",
            url: "/Home/SendCmd",
            data: {
                type: $(this).data("type"),
                num: $(this).data("num"),
                onoff: onoff
            },
            dataType: "json",
            success: function (response) {
                console.log(response);
            },
            error: function (thrownError) {

            }
        });
    });
});
