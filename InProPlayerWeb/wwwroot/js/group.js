$(document).ready(function () {
    var modalTitle = $('.modal-title'),
        modelBody = $('.modal-body'),
        myModal = $('#myModal'),
        tableRow = $(".table-row"),
        appendBtn = $(".append"),
        editBtn = $(".edit"),
        deleteBtn = $(".delete"),
        searchBtn = $("#searchBtn"),
        search = $("[name='search']"),
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
            modalTitle.html("錯誤!(Error!)");
            modelBody.html("請選擇要修改的資料行");
            myModal.modal('show');
            return false;
        } else {
            location.href = "/Group/Edit/" + choseID;
        }
    });

    deleteBtn.on("click", function () {
        if (!choseID || typeof (choseID) === 'undefined') {
            modalTitle.html("錯誤!(Error!)");
            modelBody.html("請選擇要修改的資料行");
            myModal.modal('show');
            return false;
        } else {
            location.href = "/Group/Delete/" + choseID;
        }
    });

    searchBtn.on("click", function () {
        location.href = "/Group/Index/1?search=" + encodeURIComponent(search.val());
    });
});