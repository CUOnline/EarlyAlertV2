$(function () {
    $("[id^=modal-action-").on('loaded.bs.modal', function (e) {
        $.validator.unobtrusive.parse(this);
    }).on('shown.bs.modal', function () {
        $(document).off('focusin.modal');
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });
});