$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        errorModal = $('#errorModal'),
        title = $("[name='title']"),
        form = $('form');

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