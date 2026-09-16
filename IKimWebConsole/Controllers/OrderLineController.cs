using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Web.Mvc;
using System.Dynamic;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class OrderLineController : Controller
    {
        private readonly IOrderLineService _service;
        private readonly IFulfillmentLogService _fulfillmentLogService;
        private readonly IDropdownService _dropdownService;

        public OrderLineController(IOrderLineService service, IDropdownService dropdownService, IFulfillmentLogService fulfillmentLogService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _fulfillmentLogService = fulfillmentLogService;
        }

        public ActionResult Index(int id)
        {
            if (id <= default(int))
                 return RedirectToAction("Index", "Order");

            var mLister = new Domain.OrderLineLister();
            mLister.SearchCriteria.OrderId = id;
            TempData["OrderId"] = id;
            return View(mLister);
        }

        public ActionResult List(Domain.OrderLineLister mLister)
        {
            try
            {

                System.Threading.Thread.Sleep(1000);

                mLister.SearchCriteria.OrderId = Convert.ToInt32(TempData.Peek("OrderId"));
                TempData.Keep("OrderId");
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var mOrderLine = new Domain.OrderLine();
            try
            {
                mOrderLine.OrderId = Convert.ToInt32(TempData.Peek("OrderId"));
                TempData.Keep("OrderId");
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_Create", mOrderLine);
        }

        [HttpPost]
        public ActionResult Create(Domain.OrderLine mOrderLine)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrderLine = StaticMethods.SetAuditOnUpsert(mOrderLine);
                mOrderLine = _service.CreateAsync(mOrderLine).GetAwaiter().GetResult();

                if (mOrderLine != null && mOrderLine.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Item has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving item!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            finally
            {
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mOrderLine);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mOrderLine = new Domain.OrderLine();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrderLine = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_Create", mOrderLine);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Item has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting item!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        #region FulfillmentLog
        [HttpGet]
        public ActionResult GetFulfillmentLog(int orderLineId, int itemId, int quantityRequested)
        {
            ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();

            var model = new Domain.FulfillmentLog();
            model.OrderLineId = orderLineId;
            model.ItemId = itemId;
            model.QuantityRequested = quantityRequested;

            ViewBag.QuantityRequested = quantityRequested;

            return PartialView("_CreateFulfillmentLog", model);
        }

        [HttpPost]
        public ActionResult SaveFulfillmentLog(Domain.FulfillmentLog mFulfillmentLog)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mFulfillmentLog = StaticMethods.SetAuditOnUpsert(mFulfillmentLog);
                mFulfillmentLog = _fulfillmentLogService.CreateAsync(mFulfillmentLog).GetAwaiter().GetResult();

                if (mFulfillmentLog != null && mFulfillmentLog.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Fulfillment Log has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving fulfillment log!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_CreateFulfillmentLog", mFulfillmentLog);
        }

        #endregion
    }
}