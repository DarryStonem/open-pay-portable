using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPay
{
    public class CustomerService : OpenpayResourceService<Customer, Customer>
    {

        public CustomerService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "customers";
        }

        internal CustomerService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "customers";
        }

        public async Task<Customer> Create(Customer customer)
        {
            return await base.Create(null, customer);
        }

        public async Task<Customer> Update(Customer customer)
        {
            return await base.Update(null, customer);
        }

        public void Delete(string customer_id)
        {
            base.Delete(null, customer_id);
        }

        public async Task<Customer> Get(string customer_id)
        {
            return await base.Get(null, customer_id);
        }

        public async Task<List<Customer>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }
    }
}
