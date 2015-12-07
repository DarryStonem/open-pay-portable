﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Openpay;
using Openpay.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using OpenPay;
using System.Threading.Tasks;

namespace OpenpayTest
{
    [TestClass]
    public class WebhooksTest
    {
        [TestMethod]
        public void TestWebhooks_Create_Get_Verify_Get_List_Delete()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Webhook webhook = new Webhook();
                webhook.Url = "http://postcatcher.in/catchers/54ed226514a1a60300001ab8";
                webhook.AddEventType("charge.refunded");
                webhook.AddEventType("charge.failed");

                Webhook webhookCreated = await openpayAPI.WebhooksService.Create(webhook);
                Assert.IsNotNull(webhookCreated.Id);
                Assert.IsNotNull(webhookCreated.Status);
                Assert.AreEqual("verified", webhookCreated.Status);

                Webhook webhookGet = await openpayAPI.WebhooksService.Get(webhookCreated.Id);
                Assert.IsNotNull(webhookGet.Id);
                Assert.IsNotNull(webhookGet.Status);
                Assert.AreEqual("verified", webhookGet.Status);
                Assert.AreEqual(2, webhookGet.EventTypes.Count);

                //openpayAPI.WebhooksService.Verify(webhookGet.Id, this.GetVerificationCode(webhookGet.Url));

                webhookGet = await openpayAPI.WebhooksService.Get(webhookCreated.Id);
                Assert.IsNotNull(webhookGet.Id);
                Assert.IsNotNull(webhookGet.Status);
                Assert.AreEqual("verified", webhookGet.Status);
                Assert.AreEqual(2, webhookGet.EventTypes.Count);

                List<Webhook> webhooksList = await openpayAPI.WebhooksService.List();
                Assert.AreEqual(2, webhooksList.Count);

                openpayAPI.WebhooksService.Delete(webhookGet.Id);
            }).GetAwaiter().GetResult();

        }

        private string GetVerificationCode(string url_webhook)
        {
            string url = url_webhook + "?inspect";

            WebRequest req = (WebRequest)WebRequest.Create(url);
            req.Method = "POST";
            if (req is HttpWebRequest)
            {
                ((HttpWebRequest)req).UserAgent = "Openpay .NET v1";
            }
            //req.Credentials = credential;
            req.PreAuthenticate = false;
            req.Timeout = 60 * 1000;
            req.ContentType = "application/json";

            // Obteniendo respuesta
            string result = null;

            using (WebResponse resp = (WebResponse)req.GetResponse())
            {
                result = GetResponseAsString(resp);
            }
            return result.Substring(result.IndexOf("verification_code") + 28, 8);
        }

        private string GetResponseAsString(WebResponse response)
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
    }
}

