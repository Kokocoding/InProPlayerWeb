$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        errorModal = $('#errorModal'),
        month = $("[name='NPDayMonth']"),
        day = $("[name='NPDayDay']"),
        form = $('form'),
        DayName = $("[name='DayName']");

    init();

    function init() {
        var year = new Date().getFullYear();
        var days = new Date(year, month.val(), 0).getDate();
        day.empty();
        for (i = 1; i <= days; i++) {
            if (day.data("d") == i) {
                day.append('<option selected value="' + i + '">' + i + '</option>');
            } else {
                day.append('<option value="' + i + '">' + i + '</option>');
            }
        }
    }

    form.submit(function (e) {
        if (DayName.val().trim() === "") {
            e.preventDefault(); // 阻止表单提交

            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("名稱請勿留空！");
            errorModal.modal('show');
            return false;
        }
    });

    month.on("change", function () {
        var year = new Date().getFullYear();
        var days = new Date(year, $(this).val(), 0).getDate();
        day.empty();
        for (i = 1; i <= days; i++) {
            day.append('<option value="' + i + '">' + i + '</option>');
        }
    });

});