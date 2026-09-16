$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            showToastr(notificationType, notificationMsg)
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#frmList').submit();
        }
    }
});

function fnValidation() {

    var isValid = true;

    var storeId = $.trim($("#StoreId").val());
    var date = $.trim($('#Date').val());

    // Date
    if (date == null || date == "" || date == undefined) {
        $("#Date").next("span").text("Date is required!");
        isValid = false;
    } else {
        $('#Date').next('span').text("");
    }

    // Store
    if (storeId == null || storeId == "" || storeId == undefined || storeId == "Select Store") {
        $("#StoreId").next("span").text("Store is required!");
        isValid = false;
    } else {
        $("#StoreId").next("span").text("");
    }

    return isValid;
}