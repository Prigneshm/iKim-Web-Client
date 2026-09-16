using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Service;
using System.Web.Mvc;
using Unity.Mvc5;
using Unity;

namespace IKimWebConsole
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IDropdownService, DropdownService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IFulfillmentLogService, FulfillmentLogService>();
            container.RegisterType<IHttpClientService, HttpClientService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IOrderLineService, OrderLineService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IStockItemService, StockItemService>();
            container.RegisterType<IStockService, StockService>();
            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IUserService, UserService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}