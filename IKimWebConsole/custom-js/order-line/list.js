$(function () {
    $(document).off('click', '.add-new-item').on('click', '.add-new-item', fnCreate);

    $(document).off('click', '.icon-fulfillment-log').on('click', '.icon-fulfillment-log', function (e) {
        e.preventDefault();

        const orderLineId = $(this).data('id');
        const itemId = $(this).data('itemid');
        const qty = $(this).data('qty'); 

        if (orderLineId) fnGetFulfillmentLog(orderLineId, itemId, qty);
    });

    $(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });

    $(document).off('change', '.search-dropdown').on('change', '.search-dropdown', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction) {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });
});


function fnCreate() {
    fnHandleAjaxRequest({
        url: '/OrderLine/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-item').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

//function fnCreate() {
//    var orderId = $('#OrderId').val();

//    fnHandleAjaxRequest({
//        url: '/OrderLine/Create',
//        data: { orderId: orderId },
//        onSuccess: function (html) {
//            $('#divCreate').html(html);
//            $('#modal-item').modal('show');
//        }
//    });
//}

function fnGetFulfillmentLog(orderLineId, itemId, qty) {
    fnHandleAjaxRequest({
        url: `/OrderLine/GetFulfillmentLog`,
        data: { orderLineId: orderLineId, itemId: itemId, quantityRequested: qty },
        onSuccess: function (html) {            
            $('#divFulfillmentLog').empty().html(html);
            $('#modal-fulfillment-log').modal('show');
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/OrderLine/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-item').modal('show');
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/OrderLine/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.NotificationType, response.NotificationMsg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToastr("Failed to delete order.");
        }
    });
}