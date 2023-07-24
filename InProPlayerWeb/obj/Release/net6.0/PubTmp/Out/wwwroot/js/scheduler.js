$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        errorModal = $('#errorModal'),
        form = $('form'),
        LoopType = $("[name='LoopType']"),
        LoopType1 = $(".LoopType1"),
        LoopType2 = $(".LoopType2"),
        SchedulerName = $("[name='SchedulerName']"),
        Week = $("[name='week[]']"),
        dailyCheckbox = $("#flexCheckChecked-0");

    form.submit(function (e) {
        if (SchedulerName.val().trim() === "") {
            e.preventDefault(); // 阻止表单提交

            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("名稱請勿留空！");
            errorModal.modal('show');
            return false;
        }
    });

    LoopType.on("change", function () {
        var select = $(this).val();
        switch (select) {
            case "1":
                LoopType1.removeClass("LoopTypeNone");
                LoopType2.addClass("LoopTypeNone");
                break;
            case "2":
                LoopType2.removeClass("LoopTypeNone");
                LoopType1.addClass("LoopTypeNone");
                break;
            default:
                break;
        }
    });

    Week.on("change", function () {
        if (dailyCheckbox.prop("checked")) {
            for (var i = 1; i < 8; i++) {
                $("#flexCheckChecked-" + i).prop("disabled", true);
            }
        } else {
            for (var i = 1; i < 8; i++) {
                $("#flexCheckChecked-" + i).prop("disabled", false);
            }
        }

        var days = true;
        for (var i = 1; i < 8; i++) {
            if (!$("#flexCheckChecked-" + i).prop("checked")) {
                days = false;
            }
        }
        if (days){
            dailyCheckbox.prop("checked", true);
            for (var i = 1; i < 8; i++) {
                $("#flexCheckChecked-" + i).prop("disabled", true);
                $("#flexCheckChecked-" + i).prop("checked", false);
            }
        }
    });
});