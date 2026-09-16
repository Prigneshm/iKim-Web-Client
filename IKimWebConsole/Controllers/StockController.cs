using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class StockController : Controller
    {
        private readonly IStockService _service;
        private readonly IDropdownService _dropdownService;

        public StockController(IStockService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.StockLister());
        }

        public ActionResult List(Domain.StockLister mLister)
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
                ViewBag.Status = _dropdownService.GetStatusOptions();
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            return PartialView("_Create", new Domain.Stock());
        }

        [HttpPost]
        public ActionResult Create(Domain.Stock mStock)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStock = StaticMethods.SetAuditOnUpsert(mStock);
                mStock = _service.CreateAsync(mStock).GetAwaiter().GetResult();

                if (mStock != null && mStock.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Stock has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving stock!";
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
            return PartialView("_Create", mStock);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mStock = new Domain.Stock();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStock = _service.GetAsync(id).GetAwaiter().GetResult();
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
            return PartialView("_Create", mStock);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Stock has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting stock!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}