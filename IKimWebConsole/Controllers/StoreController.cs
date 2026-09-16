using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _service;
        private readonly IDropdownService _dropdownService;
        private readonly IAddressService _addressService;

        public StoreController(IStoreService service, IDropdownService dropdownService, IAddressService addressService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _addressService = addressService;
        }

        public ActionResult Index()
        {
            return View(new Domain.StoreLister());
        }

        public ActionResult List(Domain.StoreLister mLister)
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
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
            ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
            ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();
            return PartialView("_Create", new Domain.Store());
        }


         [HttpPost]
        public ActionResult Create(Domain.Store mStore)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStore = StaticMethods.SetAuditOnUpsert(mStore);
                mStore = _service.CreateAsync(mStore).GetAwaiter().GetResult();

                if (mStore != null && mStore.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Store has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving store!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_Create", mStore);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mStore = new Domain.Store();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStore = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_Create", mStore);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Store has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting store!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }


        #region Address

        [HttpGet]
        public ActionResult GetAddress(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                mAddress = _addressService.GetAsync(id).GetAwaiter().GetResult();
                mAddress = mAddress ?? new Domain.Address();
                mAddress.StoreId = id;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }


        [HttpPost]
        public ActionResult SaveAddress(Domain.Address mAddress)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAddress = StaticMethods.SetAuditOnUpsert(mAddress);
                mAddress = _addressService.CreateAsync(mAddress).GetAwaiter().GetResult();

                if (mAddress != null && mAddress.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Address has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Address!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }

        #endregion
    }
}