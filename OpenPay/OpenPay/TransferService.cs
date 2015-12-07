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
    public class TransferService : OpenpayResourceService<TransferRequest, Transfer>
    {

        public TransferService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "transfers";
        }

        internal TransferService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "transfers";
        }

        public new async Task<Transfer> Create(string customer_id, TransferRequest request)
        {
            return await base.Create(customer_id, request);
        }

        public new async Task<Transfer> Get(string customer_id, string transfer_id)
        {
            return await base.Get(customer_id, transfer_id);
        }

        public new async Task<List<Transfer>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }
    }
}
