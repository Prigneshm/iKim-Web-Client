using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class FulfillmentLogService : IFulfillmentLogService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public FulfillmentLogService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.FulfillmentLog> CreateAsync(Domain.FulfillmentLog mFulfillmentLog)
        {
            return await _httpClientService.PostAsync<Domain.FulfillmentLog>(
                url: $"{_baseUrl}/FulfillmentLog",
                obj: mFulfillmentLog,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.FulfillmentLog> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.FulfillmentLog>(
                url: $"{_baseUrl}/FulfillmentLog/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

    }
}
