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
    public class CardService : OpenpayResourceService<Card, Card>
    {
        public CardService(string api_key, string merchant_id, bool production = false)
            : base(api_key, merchant_id, production)
        {
            ResourceName = "cards";
        }

        internal CardService(OpenpayHttpClient opHttpClient)
            : base(opHttpClient)
        {
            ResourceName = "cards";
        }

        public async Task<Card> Create(Card card)
        {
            return await base.Create(null, card);
        }

        public new async Task<Card> Create(string customer_id, Card card)
        {
            return await base.Create(customer_id, card);
        }

        public new void Delete(string customer_id, string card_id)
        {
            base.Delete(customer_id, card_id);
        }

        public void Delete(string card_id)
        {
            base.Delete(null, card_id);
        }

        public new async Task<Card> Get(string customer_id, string card_id)
        {
            return await base.Get(customer_id, card_id);
        }

        public async Task<Card> Get(string card_id)
        {
            return await base.Get(null, card_id);
        }

        public new async Task<List<Card>> List(string customer_id, SearchParams filters = null)
        {
            return await base.List(customer_id, filters);
        }

        public async Task<List<Card>> List(SearchParams filters = null)
        {
            return await base.List(null, filters);
        }
    }
}
