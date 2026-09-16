using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IOrderLineService
    {
        Task<Domain.OrderLineLister> GetAllAsync(Domain.OrderLineLister mLister);

        Task<Domain.OrderLine> CreateAsync(Domain.OrderLine mOrderLine);

        Task<Domain.OrderLine> GetAsync(int id);

        Task DeleteAsync(int id);






        //Task<List<Domain.OrderLine>> GetByAsync(int orderId);

        //Task<Domain.OrderLine> CreateAsync(Domain.OrderLine mOrderLine);

        //Task<Domain.OrderLine> GetAsync(int id);

        //Task DeleteAsync(int id);
    }
}
