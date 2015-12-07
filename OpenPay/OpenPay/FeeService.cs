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
    public class FeeService : OpenpayResourceService<FeeRequest, Fee>
    {

        public FeeService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "fees";
        }

        internal FeeService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "fees";
        }

        public async Task<Fee> Create(FeeRequest request)
        {
            return await base.Create(null, request);
        }

        public async Task<List<Fee>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }
    }
}
