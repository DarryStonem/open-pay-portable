﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Openpay.Entities;
using Openpay.Entities.Request;
using Openpay.Utils;

namespace Openpay
{
    public abstract class OpenpayResourceService<T, R>
        where T : JsonObject
        where R : OpenpayResourceObject
    {
        protected OpenpayHttpClient httpClient;

        private static readonly string filter_date_format = "yyyy-MM-dd";

        private static readonly string filter_amount_format = "0.00";

        public OpenpayResourceService(string api_key, string merchant_id, bool production = false)
        {
            this.httpClient = new OpenpayHttpClient(api_key, merchant_id, production);
        }

        internal OpenpayResourceService(OpenpayHttpClient opHttpClient)
        {
            this.httpClient = opHttpClient;
        }

        protected String ResourceName { get; set; }

        protected String GetEndPoint(string customer_id, string resource_id = null)
        {
            string ep = "/" + ResourceName.ToLower();
            if (customer_id != null)
            {
                ep = String.Format("/customers/{0}" + ep, customer_id);
            }
            if (resource_id != null)
            {
                ep = ep + "/" + resource_id;
            }
            return ep;
        }

        protected async virtual Task<R> Create(string customer_id, T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("The object to create is null");
            string ep = GetEndPoint(customer_id);
            return await this.httpClient.Post<R>(ep, obj);
        }

        protected async Task<R> Update(string customer_id, R obj)
        {
            if (String.IsNullOrEmpty(obj.Id))
                throw new ArgumentNullException("resource_id");
            if (obj == null)
                throw new ArgumentNullException("Object is null");

            string ep = GetEndPoint(customer_id, obj.Id);
            return await this.httpClient.Put<R>(ep, obj);
        }

        protected virtual void Delete(string customer_id, string resource_id)
        {
            if (String.IsNullOrEmpty(resource_id))
                throw new ArgumentNullException("The id of the object cannot be null");
            string ep = GetEndPoint(customer_id, resource_id);
            this.httpClient.Delete(ep);
        }

        protected virtual async Task<R> Get(string customer_id, string resource_id)
        {
            string ep = GetEndPoint(customer_id, resource_id);
            return await this.httpClient.Get<R>(ep);
        }

        protected async Task<List<R>> List(string customer_id, SearchParams searchParams)
        {
            string url = GetEndPoint(customer_id);
            url = url + BuildParams(searchParams);
            return await this.httpClient.Get<List<R>>(url);
        }

        protected string BuildParams(SearchParams searchParams)
        {
            string url_params = string.Empty;
            if (searchParams != null)
            {
                if (searchParams.Offset < 0)
                    throw new ArgumentOutOfRangeException("offset");
                if (searchParams.Limit < 1 || searchParams.Limit > 100)
                    throw new ArgumentOutOfRangeException("limit");

                url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "limit", searchParams.Limit.ToString());
                url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "offset", searchParams.Offset.ToString());

                if (searchParams.OrderId != null)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "order_id", searchParams.OrderId);

                if (searchParams.Creation != DateTime.MinValue)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation", searchParams.Creation.ToString(filter_date_format));

                if (searchParams.CreationGte != DateTime.MinValue)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation[gte]", searchParams.CreationGte.ToString(filter_date_format));

                if (searchParams.CreationLte != DateTime.MinValue)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation[lte]", searchParams.CreationLte.ToString(filter_date_format));

                if (searchParams.Amount > 0)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount", searchParams.Amount.ToString(filter_amount_format));

                if (searchParams.AmountGte > 0)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount[gte]", searchParams.AmountGte.ToString(filter_amount_format));

                if (searchParams.AmountLte > 0)
                    url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount[lte]", searchParams.AmountLte.ToString(filter_amount_format));
            }
            return url_params;
        }
    }
}
