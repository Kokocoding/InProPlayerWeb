$(document).ready(function () {
    /* Home Js */
    $(".mBtn").on("click", function () {
        var onoff = "";
        if ($(this).hasClass("bi-toggle2-off")) {
            $(this).removeClass("bi-toggle2-off").addClass("bi-toggle2-on").html("開啟");
            onoff = true;
        } else if ($(this).hasClass("bi-toggle2-on")) {
            $(this).removeClass("bi-toggle2-on").addClass("bi-toggle2-off").html("關閉");
            onoff = false;
        }

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

    /* Player Js */


});
