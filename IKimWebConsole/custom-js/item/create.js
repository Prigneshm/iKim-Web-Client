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


    function calculateTotal() {

        var cgst = parseFloat($('#CGST').val()) || 0;
        var sgst = parseFloat($('#SGST').val()) || 0;

        var gst = sgst + cgst;

        $('#GST').val(gst.toFixed(2));
    }

    $(document).on('input', '#CGST, #SGST', function () {
        calculateTotal();
    });

});

function fnValidation() {

    var isValid = true;

    var name = $.trim($("#Name").val());
    var hSNCode = $.trim($("#HSNCode").val());
    var unitOfMeasureId = $.trim($("#UnitOfMeasureId").val());
    var cGST = $.trim($("#CGST").val());
    var sGST = $.trim($("#SGST").val());
    var gST = $.trim($("#GST").val());

    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $("#Name").next("span").text("");
    }

    // hSNCode
    if (hSNCode == null || hSNCode == "" || hSNCode == undefined) {
        $("#HSNCode").next("span").text("HSN Code is required!");
        isValid = false;
    } else {
        $("#HSNCode").next("span").text("");
    }

    // unitOfMeasureId
    if (unitOfMeasureId == null || unitOfMeasureId == "" || unitOfMeasureId == undefined) {
        $("#UnitOfMeasureId").next("span").text("Unit of Measure is required!");
        isValid = false;
    } else {
        $("#UnitOfMeasureId").next("span").text("");
    }

    // CGST
    if (cGST == null || cGST == "" || cGST == undefined) {
        $("#CGST").next("span").text("CGST is required!");
        isValid = false;
    } else {
        $("#CGST").next("span").text("");
    }

    // SGST
    if (sGST == null || sGST == "" || sGST == undefined) {
        $("#SGST").next("span").text("SGST is required!");
        isValid = false;
    } else {
        $("#SGST").next("span").text("");
    }

    // GST
    if (gST == null || gST == "" || gST == undefined) {
        $("#GST").next("span").text("GST is required!");
        isValid = false;
    } else {
        $("#GST").next("span").text("");
    }

    return isValid;
}