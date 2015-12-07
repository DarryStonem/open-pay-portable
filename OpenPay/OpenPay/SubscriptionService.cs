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
    public class SubscriptionService : OpenpayResourceService<Subscription, Subscription>
    {

        public SubscriptionService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "subscriptions";
        }

        internal SubscriptionService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "subscriptions";
        }

        public new async Task<Subscription> Create(string customer_id, Subscription subscription)
        {
            return await base.Create(customer_id, subscription);
        }

        public new async Task<Subscription> Update(string customer_id, Subscription subscription)
        {
            return await base.Update(customer_id, subscription);
        }

        public new void Delete(string customer_id, string subscription_id)
        {
            base.Delete(customer_id, subscription_id);
        }

        public new async Task<Subscription> Get(string customer_id, string subscription_id)
        {
            return await base.Get(customer_id, subscription_id);
        }

        public new async Task<List<Subscription>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }
    }
}
