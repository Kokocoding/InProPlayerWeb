$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        modalcheckTitle = $('.modal-check-title'),
        modelcheckBody = $('.modal-check-body'),
        errorModal = $('#errorModal'),
        checkModal = $('#checkModal'),
        tableRow = $(".table-row"),
        checkModelBtn = $(".checkModelBtn"),
        appendBtn = $(".append"),
        editBtn = $(".edit"),
        deleteBtn = $(".delete"),
        searchBtn = $("#searchBtn"),
        search = $("[name='search']"),
        month = $("[name='NPDayMonth']"),
        day = $("[name='NPDayDay']"),
        form = $('form'),
        DayName = $("[name='DayName']"),
        choseID;

    tableRow.on("click", function () {
        tableRow.removeClass('active');
        $(this).addClass('active');

        choseID = $(this).data("id");
    });

    appendBtn.on("click", function () {
        location.href = "/SchedulerNPDay/Append";
    });

    editBtn.on("click", function () {
        if (!choseID || typeof (choseID) === 'undefined') {
            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("請選擇要修改的資料行");
            errorModal.modal('show');
            return false;
        } else {
            location.href = "/SchedulerNPDay/Edit/" + choseID;
        }
    });

    deleteBtn.on("click", function () {
        modalcheckTitle.html("確認!(Confirm!)");
        modelcheckBody.html("確定要刪除資料?");
        checkModal.modal('show');
    });

    checkModelBtn.on("click", function () {
        checkModal.modal('hide');
        if (!choseID || typeof (choseID) === 'undefined') {
            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("請選擇要修改的資料行");
            errorModal.modal('show');
            return false;
        } else {
            location.href = "/SchedulerNPDay/Delete/" + choseID;
        }
    });

    searchBtn.on("click", function () {
        location.href = "/SchedulerNPDay/Index/1?search=" + encodeURIComponent(search.val());
    });

    month.on("change", function () {
        var year = new Date().getFullYear();
        var days = new Date(year, $(this).val(), 0).getDate();
        day.empty();
        for (i = 1; i <= days; i++) {
            day.append('<option value="' + i + '">' + i +'</option>');
        }    
    });

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

});