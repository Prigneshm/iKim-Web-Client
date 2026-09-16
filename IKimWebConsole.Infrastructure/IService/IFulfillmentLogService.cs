using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IFulfillmentLogService
    {
        Task<Domain.FulfillmentLog> CreateAsync(Domain.FulfillmentLog mFulfillmentLog);

        Task<Domain.FulfillmentLog> GetAsync(int id);
    }
}
