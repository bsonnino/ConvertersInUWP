using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerLib
{
    public interface ICustomerRepository
    {
        bool Add(Customer customer);
        bool Remove(Customer customer);
        bool Commit();
        Task<IEnumerable<Customer>> GetCustomersAsync();
    }
}
