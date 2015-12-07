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
    public class BankAccountService : OpenpayResourceService<BankAccount, BankAccount>
    {

        public BankAccountService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "bankaccounts";
        }

        internal BankAccountService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "bankaccounts";
        }

        public new async Task<BankAccount> Create(string customer_id, BankAccount bankAccount)
        {
            return await base.Create(customer_id, bankAccount);
        }

        public async Task<BankAccount> Create(BankAccount bankAccount)
        {
            return await base.Create(null, bankAccount);
        }

        public new void Delete(string customer_id, string bankAccount_id)
        {
            base.Delete(customer_id, bankAccount_id);
        }

        public void Delete(string bankAccount_id)
        {
            base.Delete(null, bankAccount_id);
        }

        public new async Task<BankAccount> Get(string customer_id, string bankAccount_id)
        {
            return await base.Get(customer_id, bankAccount_id);
        }

        public async Task<BankAccount> Get(string bankAccount_id)
        {
            return await base.Get(null, bankAccount_id);
        }

        public new async Task<List<BankAccount>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }

        public async Task<List<BankAccount>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }
    }
}
