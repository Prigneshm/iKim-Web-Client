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

    var itemId = $.trim($("#ItemId").val());
    var quantity = $.trim($("#Quantity").val());

    //itemId
    if (itemId == null || itemId == "" || itemId == undefined) {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    } else {
        $("#ItemId").next("span").text("");
    }

    //quantity
    if (quantity == null || quantity == "" || quantity == undefined) {
        $("#Quantity").next("span").text("Quantity is required!");
        isValid = false;
    } else {
        $("#Quantity").next("span").text("");
    }

    return isValid;
}


