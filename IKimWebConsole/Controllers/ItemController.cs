using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class ItemController : Controller
    {
        private readonly IItemService _service;
        private readonly IDropdownService _dropdownService;

        public ItemController(IItemService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.ItemLister());
        }

        public ActionResult List(Domain.ItemLister mLister)
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
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            ViewBag.Categories = _dropdownService.GetCategoryOptionsAsync().GetAwaiter().GetResult();
            var mItem = new Domain.Item();
            var mPricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
            if (mPricingTiers != null)
            {
                foreach (var item in mPricingTiers)
                {
                    mItem.Prices.Add(new Domain.ItemPrice() { PricingTierId = Convert.ToInt32(item.Value), PricingTierName = item.Text });
                }
            }
            return PartialView("_Create", mItem);
        }

        [HttpPost]
        public ActionResult Create(Domain.Item mItem)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mItem = StaticMethods.SetAuditOnUpsert(mItem);
                mItem = _service.CreateAsync(mItem).GetAwaiter().GetResult();

                if (mItem != null && mItem.Id > 0)
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
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Categories = _dropdownService.GetCategoryOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mItem);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mItem = new Domain.Item();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mItem = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Categories = _dropdownService.GetCategoryOptionsAsync().GetAwaiter().GetResult();
                ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mItem);
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


    }
}