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
    debugger
    var isValid = true;

    var itemId = $.trim($("#ItemId").val());
    var quantityRequested = $.trim($("#QuantityRequested").val());

    // Item
    if (itemId == null || itemId == "" || itemId == undefined || itemId == "Select Item") {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    } else {
        $("#ItemId").next("span").text("");
    }

    // Quantity Requested
    if (quantityRequested == null || quantityRequested == "" || quantityRequested == undefined || quantityRequested == 0) {
        $("#QuantityRequested").next("span").text("Quantity Requested is required!");
        isValid = false;
    } else {
        $("#QuantityRequested").next("span").text("");
    }

    return isValid;
}