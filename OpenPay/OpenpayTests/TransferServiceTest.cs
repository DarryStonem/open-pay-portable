using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System.Collections.Generic;
using OpenPay;
using System.Threading.Tasks;

namespace OpenpayTest
{
    [TestClass]
    public class TransferServiceTest
    {
        private static readonly string customer_id = "adyytoegxm6boiusecxm";

        [TestMethod]
        public void TesTransferCreate()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                TransferRequest request = new TransferRequest();
                request.CustomerId = "acjpcdct4tbyemirw7zo";
                request.Amount = 11.0m;
                request.Description = "Transfer Testing";

                Transfer transfer = await openpayAPI.TransferService.Create(customer_id, request);
                Assert.IsNotNull(transfer.Id);
                Assert.IsNotNull(transfer.CreationDate);

                Transfer transferGet = await openpayAPI.TransferService.Get(customer_id, transfer.Id);
                Assert.AreEqual(transfer.Amount, transferGet.Amount);
                Assert.IsNull(transferGet.OrderId);

                Assert.IsNotNull(transferGet.CreationDate);
                Assert.IsNotNull(transferGet.CustomerId);
                Assert.IsNotNull(transferGet.Description);
                Assert.IsNotNull(transferGet.Method);
                Assert.IsNotNull(transferGet.OperationType);
                Assert.IsNotNull(transferGet.Status);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TesTransferList()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                SearchParams filters = new SearchParams();
                filters.CreationLte = new DateTime(2014, 1, 8);
                filters.Amount = 10.0m;
                List<Transfer> transfers = await openpayAPI.TransferService.List(customer_id, filters);
                Assert.AreEqual(2, transfers.Count);
            }).GetAwaiter().GetResult();

        }
    }
}
