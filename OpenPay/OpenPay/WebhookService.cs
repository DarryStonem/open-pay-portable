using Openpay;
using Openpay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPay
{
    public class WebhookService : OpenpayResourceService<Webhook, Webhook>
    {

        public WebhookService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "webhooks";
        }

        internal WebhookService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "webhooks";
        }

        public async Task<Webhook> Create(Webhook webhook)
        {
            return await base.Create(null, webhook);
        }

        public void Verify(string webhook_id, string verification_code)
        {
            string url = GetEndPoint(null, webhook_id) + "/verify" + "/" + verification_code;
            this.httpClient.Post<Webhook>(url);
        }

        public async Task<Webhook> Get(string webhook_id)
        {
            return await base.Get(null, webhook_id);
        }

        public void Delete(string webhook_id)
        {
            base.Delete(null, webhook_id);
        }

        public async Task<List<Webhook>> List()
        {
            return await base.List(null, null);
        }

    }
}
