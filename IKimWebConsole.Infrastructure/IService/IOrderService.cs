using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IOrderService
    {
        Task<Domain.OrderLister> GetAllAsync(Domain.OrderLister mLister);

        Task<Domain.Order> CreateAsync(Domain.Order mOrder);

        Task<Domain.Order> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
