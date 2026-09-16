using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using System.Collections.Generic;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Dynamic;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class OrderController : Controller
    {
        #region Declaration

        private readonly IOrderService _service;
        private readonly IDropdownService _dropdownService;

        public OrderController(IOrderService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        #endregion

        #region Order
        public ActionResult Index()
        {
            return View(new Domain.OrderLister());
        }

        public ActionResult List(Domain.OrderLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            return PartialView("_Create", new Domain.Order());
        }

        [HttpPost]
        public ActionResult Create(Domain.Order mOrder)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrder = StaticMethods.SetAuditOnUpsert(mOrder);
                mOrder = _service.CreateAsync(mOrder).GetAwaiter().GetResult();

                if (mOrder != null && mOrder.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Order has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving order!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mOrder);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mOrder = new Domain.Order();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrder = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_Create", mOrder);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Order has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting order!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}