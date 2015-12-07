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
    public class PlanService : OpenpayResourceService<Plan, Plan>
    {

        public PlanService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "plans";
        }

        internal PlanService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "plans";
        }

        public async Task<Plan> Create(Plan plan)
        {
            return await base.Create(null, plan);
        }

        public async Task<Plan> Update(Plan plan)
        {
            return await base.Update(null, plan);
        }

        public void Delete(string plan_id)
        {
            base.Delete(null, plan_id);
        }

        public async Task<Plan> Get(string plan_id)
        {
            return await base.Get(null, plan_id);
        }

        public async Task<List<Plan>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }

        public async Task<List<Subscription>> Subscriptions(string plan_id, SearchParams filters = null)
        {
            string url = GetEndPoint(null, plan_id) + "/subscriptions";
            url = url + BuildParams(filters);
            return await this.httpClient.Get<List<Subscription>>(url);
        }
    }
}
