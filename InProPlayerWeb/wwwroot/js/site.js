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
        choseID;

    var currentUrl = window.location.pathname; // 頁面路徑
    var segments = currentUrl.split('/'); // 分割路徑
    var Controller = segments[1]; // Controller 名稱

    tableRow.on("click", function () {
        tableRow.removeClass('active');
        $(this).addClass('active');

        choseID = $(this).data("id");
    });

    appendBtn.on("click", function () {
        location.href = "/" + Controller + "/Append";
    });

    editBtn.on("click", function () {
        if (!choseID || typeof (choseID) === 'undefined') {
            modalerrorTitle.html("錯誤!(Error!)");
            modelerrorBody.html("請選擇要修改的資料行");
            errorModal.modal('show');
            return false;
        } else {
            location.href = "/" + Controller + "/Edit/" + choseID;
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
            location.href = "/" + Controller + "/Delete/" + choseID;
        }
    });

    searchBtn.on("click", function () {
        location.href = "/" + Controller + "/Index/1?search=" + encodeURIComponent(search.val());
    });
});
