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
    public class ChargeService : OpenpayResourceService<ChargeRequest, Charge>
    {

        public ChargeService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "charges";
        }

        internal ChargeService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "charges";
        }

        public async Task<Charge> Refund(string charge_id, string description)
        {
            return await this.Refund(null, charge_id, description);
        }

        public async Task<Charge> Refund(string customer_id, string charge_id, string description)
        {
            if (charge_id == null)
                throw new ArgumentNullException("charge_id cannot be null");
            string ep = GetEndPoint(customer_id, charge_id) + "/refund";
            RefundRequest request = new RefundRequest();
            request.Description = description;
            return await this.httpClient.Post<Charge>(ep, request);
        }

        public async Task<Charge> Capture(string charge_id, Decimal? amount)
        {
            return await this.Capture(null, charge_id, amount);
        }

        public async Task<Charge> Capture(string customer_id, string charge_id, Decimal? amount)
        {
            if (charge_id == null)
                throw new ArgumentNullException("charge_id cannot be null");
            string ep = GetEndPoint(customer_id, charge_id) + "/capture";
            CaptureRequest request = new CaptureRequest();
            request.Amount = amount;
            return await this.httpClient.Post<Charge>(ep, request);
        }

        public async Task<Charge> Create(ChargeRequest charge_request)
        {
            return await base.Create(null, charge_request);
        }

        public new async Task<Charge> Create(string customer_id, ChargeRequest charge_request)
        {
            return await base.Create(customer_id, charge_request);
        }

        public new async Task<Charge> Get(string customer_id, string charge_id)
        {
            return await base.Get(customer_id, charge_id);
        }

        public async Task<Charge> Get(string charge_id)
        {
            return await base.Get(null, charge_id);
        }

        public new async Task<List<Charge>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }

        public async Task<List<Charge>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }

    }
}
