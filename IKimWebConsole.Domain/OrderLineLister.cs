using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class OrderLineLister
    {
        public List<OrderLine> List { get; set; } = new List<OrderLine>();

        public OrderLine SearchCriteria { get; set; } = new OrderLine();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
