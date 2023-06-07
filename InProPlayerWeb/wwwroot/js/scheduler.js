$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        errorModal = $('#errorModal'),
        title = $("[name='title']"),
        form = $('form'),
        LoopType = $("[name='LoopType']"),
        LoopType1 = $(".LoopType1"),
        LoopType2 = $(".LoopType2");

    form.submit(function (e) {
        if (title.val().trim() === "") {
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
                console.log(select);
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
});