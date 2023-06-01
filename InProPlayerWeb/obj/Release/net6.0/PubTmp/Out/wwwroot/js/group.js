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
        title = $("[name='title']"),
        form = $('form'),
        choseID;

    tableRow.on("click", function () {
        tableRow.removeClass('active');
        $(this).addClass('active');

        choseID = $(this).data("id");
    });

    appendBtn.on("click", function () {
        location.href = "/Group/Append";
    });

    editBtn.on("click", function () {
        if (!choseID || typeof (choseID) === 'undefined') {
            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("請選擇要修改的資料行");
            errorModal.modal('show');
            return false;
        } else {
            location.href = "/Group/Edit/" + choseID;
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
            location.href = "/Group/Delete/" + choseID;
        }
    });

    searchBtn.on("click", function () {
        location.href = "/Group/Index/1?search=" + encodeURIComponent(search.val());
    });

    form.submit(function (e) {
        if (title.val().trim() === "") {
            e.preventDefault(); // 阻止表单提交

            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("名稱請勿留空！");
            errorModal.modal('show');
            return false;
        }
    });
});