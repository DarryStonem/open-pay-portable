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
    public class PayoutService : OpenpayResourceService<PayoutRequest, Payout>
    {

        public PayoutService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "payouts";
        }

        internal PayoutService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "payouts";
        }

        public async Task<Payout> Create(PayoutRequest payout_request)
        {
            return await base.Create(null, payout_request);
        }

        public new async Task<Payout> Create(string customer_id, PayoutRequest payout_request)
        {
            return await base.Create(customer_id, payout_request);
        }

        public new async Task<Payout> Get(string customer_id, string payout_id)
        {
            return await base.Get(customer_id, payout_id);
        }

        public async Task<Payout> Get(string payout_id)
        {
            return await base.Get(null, payout_id);
        }

        public new async Task<List<Payout>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }

        public async Task<List<Payout>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }

    }
}
