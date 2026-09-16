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
    public class StockItemController : Controller
    {
        private readonly IStockItemService _service;
        private readonly IStockService _stockService;
        private readonly IDropdownService _dropdownService;

        public StockItemController(IStockItemService service, IDropdownService dropdownService, IStockService stockService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _stockService = stockService;
        }

        public ActionResult Index(int id)
        {
            if (id <= default(int))
                return RedirectToAction("Index", "Stock");

            var mLister = new Domain.StockItemLister();
            mLister.SearchCriteria.StockId = id;
            TempData["StockId"] = id;
            return View(mLister);
        }

        public ActionResult List(Domain.StockItemLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.StockId = Convert.ToInt32(TempData.Peek("StockId"));
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();

                var store = _stockService.GetAsync(mLister.SearchCriteria.StockId).GetAwaiter().GetResult();
                ViewBag.StoreName = store.Store;
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
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create() 
        {
            var mStockItem = new Domain.StockItem();
            try
            {
                mStockItem.StockId = Convert.ToInt32(TempData.Peek("StockId"));                
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
            return PartialView("_Create", mStockItem);
        }

        [HttpPost]
        public ActionResult Create(Domain.StockItem mStockItem)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStockItem = StaticMethods.SetAuditOnUpsert(mStockItem);
                mStockItem = _service.CreateAsync(mStockItem).GetAwaiter().GetResult();

                if (mStockItem != null && mStockItem.Id > 0)
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
                ViewBag.Stocks = _dropdownService.GetStockOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mStockItem);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mStockItem = new Domain.StockItem();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStockItem = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Stocks = _dropdownService.GetStockOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Items = _dropdownService.GetItemOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mStockItem);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Item has been deleted successfully!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting item!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}