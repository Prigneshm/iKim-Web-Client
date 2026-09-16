$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreateFulfillmentLog').submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        debugger;
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            debugger;
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

    var quantityFulfilled = $.trim($("#QuantityFulfilled").val());

    // QuantityFulfilled
    if (quantityFulfilled == null || quantityFulfilled == "" || quantityFulfilled == undefined) {
        $("#QuantityFulfilled").next("span").text("Quantity Fulfilled is required!");
        isValid = false;
    } else {
        $("#QuantityFulfilled").next("span").text("");
    }

    return isValid;
}