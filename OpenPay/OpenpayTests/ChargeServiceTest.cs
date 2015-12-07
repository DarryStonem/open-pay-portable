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
    public class ChargeServiceTest
    {
        [TestMethod]
        public void TestChargeToCustomerWithSourceId()
        {
            Task.Run(async () =>
            {
                string customer_id = "adyytoegxm6boiusecxm";
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.SourceId = "kwkoqpg6fcvfse8k8mg2";
                request.Description = "Testing from .Net";
                request.Amount = new Decimal(9.99);

                Charge charge = await openpayAPI.ChargeService.Create(customer_id, request);

                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.AreEqual("completed", charge.Status);

                Charge charge2 = await openpayAPI.ChargeService.Get(customer_id, charge.Id);
                Assert.IsNotNull(charge2);
                Assert.AreEqual(charge.Id, charge2.Id);
                Assert.AreEqual(charge.Amount, charge2.Amount);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestChargeToCustomerWithCard()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net";
                request.Amount = new Decimal(9.99);

                Charge charge = await openpayAPI.ChargeService.Create("adyytoegxm6boiusecxm", request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.AreEqual("completed", charge.Status);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestChargeToCustomerWithCard_metdatata_USD()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net";
                request.Amount = new Decimal(9.99);
                request.Metadata = new Dictionary<string, string>();
                request.Metadata.Add("test_key1", "pruebas");
                request.Metadata.Add("test_key2", "123456");
                request.Currency = "USD";

                Charge charge = await openpayAPI.ChargeService.Create("adyytoegxm6boiusecxm", request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.AreEqual("completed", charge.Status);
                Assert.IsNotNull(charge.Metadata);
                Assert.IsNotNull(charge.ExchangeRate);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestChargeToCustomer_AndCapture()
        {
            Task.Run(async () =>
            {
                String customer_id = "adyytoegxm6boiusecxm";
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.SourceId = "kwkoqpg6fcvfse8k8mg2";
                request.Description = "Testing from .Net";
                request.Amount = new Decimal(9.99);
                request.Capture = false;

                Charge charge = await openpayAPI.ChargeService.Create(customer_id, request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.AreEqual("in_progress", charge.Status);

                Charge chargeCompleted = await openpayAPI.ChargeService.Capture(customer_id, charge.Id, null);
                Assert.IsNotNull(chargeCompleted);
                Assert.AreEqual("completed", chargeCompleted.Status);
                Assert.AreEqual(charge.Amount, chargeCompleted.Amount);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestChargeToCustomerWithSourceId_AndRefund()
        {
            Task.Run(async () =>
            {
                String customer_id = "adyytoegxm6boiusecxm";
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.SourceId = "kwkoqpg6fcvfse8k8mg2";
                request.Description = "Testing from .Net";
                request.Amount = new Decimal(9.99);

                Charge charge = await openpayAPI.ChargeService.Create(customer_id, request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.AreEqual("completed", charge.Status);

                Charge chargeWithrefund = await openpayAPI.ChargeService.Refund(customer_id, charge.Id, "refund desc");
                Assert.IsNotNull(chargeWithrefund);
                Assert.IsNotNull(chargeWithrefund.Refund);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToCustomerWithBankAccount()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "bank_account";
                request.Description = "Testing from .Net [BankAccount]";
                request.Amount = new Decimal(9.99);
                request.DueDate = new DateTime(2015, 12, 6, 11, 50, 0);

                Charge charge = await openpayAPI.ChargeService.Create("adyytoegxm6boiusecxm", request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.IsNotNull(charge.PaymentMethod);
                Assert.AreEqual("in_progress", charge.Status);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToCustomerWithStore()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "store";
                request.Description = "Testing from .Net [STORE]";
                request.Amount = new Decimal(9.99);
                request.DueDate = new DateTime(2015, 12, 6, 11, 50, 0);

                Charge charge = await openpayAPI.ChargeService.Create("adyytoegxm6boiusecxm", request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.IsNotNull(charge.PaymentMethod);
                Assert.IsNotNull(charge.PaymentMethod.Reference);
                Assert.AreEqual("in_progress", charge.Status);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToCustomerWithBitcoin()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);

                ChargeRequest request = new ChargeRequest();
                request.Method = "bitcoin";
                request.Description = "Testing from .Net [BITCOIN]";
                request.Amount = new Decimal(9.99);

                Charge charge = await openpayAPI.ChargeService.Create("adyytoegxm6boiusecxm", request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CreationDate);
                Assert.IsNotNull(charge.PaymentMethod);
                Assert.IsNotNull(charge.PaymentMethod.AmountBitcoins);
                Assert.IsNotNull(charge.PaymentMethod.PaymentAddress);
                Assert.IsNotNull(charge.PaymentMethod.PaymentUrlBip21);
                Assert.IsNotNull(charge.PaymentMethod.ExchangeRate);
                Assert.AreEqual("bitcoin", charge.PaymentMethod.Type);
                Assert.AreEqual("charge_pending", charge.Status);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToMerchant()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net with new card";
                request.Amount = new Decimal(9.99);

                Customer customer = new Customer();
                customer.Name = "Openpay";
                customer.LastName = "Test";
                customer.PhoneNumber = "1234567890";
                customer.Email = "noemail@openpay.mx";

                request.Customer = customer;

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNull(charge.CardPoints);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);

                Charge charge2 = await openpayAPI.ChargeService.Get(charge.Id);
                Assert.IsNotNull(charge2);
                Assert.IsNull(charge2.CardPoints);
                Assert.AreEqual(charge.Id, charge2.Id);
                Assert.AreEqual(charge.Amount, charge2.Amount);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToMerchantWithPointsSmall()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net with new card";
                request.Amount = new Decimal(9.99);
                request.UseCardPoints = true;

                Customer customer = new Customer();
                customer.Name = "Openpay";
                customer.LastName = "Test";
                customer.PhoneNumber = "1234567890";
                customer.Email = "noemail@openpay.mx";

                request.Customer = customer;

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CardPoints);
                Assert.AreEqual(charge.CardPoints.Amount, new Decimal(9.99));

                Charge charge2 = await openpayAPI.ChargeService.Get(charge.Id);
                Assert.IsNotNull(charge2);
                Assert.IsNotNull(charge2.CardPoints);
                Assert.AreEqual(charge.Id, charge2.Id);
                Assert.AreEqual(charge.Amount, charge2.Amount);
                Assert.AreEqual(charge2.CardPoints.Amount, new Decimal(9.99));
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToMerchantWithPointsBig()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net with new card";
                request.Amount = new Decimal(29.99);
                request.UseCardPoints = true;

                Customer customer = new Customer();
                customer.Name = "Openpay";
                customer.LastName = "Test";
                customer.PhoneNumber = "1234567890";
                customer.Email = "noemail@openpay.mx";

                request.Customer = customer;

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.CardPoints);
                Assert.AreEqual(charge.CardPoints.Amount, new Decimal(22.5));

                Charge charge2 = await openpayAPI.ChargeService.Get(charge.Id);
                Assert.IsNotNull(charge2);
                Assert.IsNotNull(charge2.CardPoints);
                Assert.AreEqual(charge.Id, charge2.Id);
                Assert.AreEqual(charge.Amount, charge2.Amount);
                Assert.AreEqual(charge2.CardPoints.Amount, new Decimal(22.5));
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToMerchantAndGetByOrderId()
        {
            Task.Run(async () =>
            {
                Random random = new Random();
                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.OrderId = random.Next(0, 10000000).ToString();
                request.Description = "Testing from .Net with new card";
                request.Amount = new Decimal(9.99);

                Customer customer = new Customer();
                customer.Name = "Openpay";
                customer.LastName = "Test";
                customer.PhoneNumber = "1234567890";
                customer.Email = "noemail@openpay.mx";

                request.Customer = customer;

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);

                SearchParams search = new SearchParams();
                search.OrderId = request.OrderId;
                List<Charge> charges = await openpayAPI.ChargeService.List(search);
                Assert.AreEqual(1, charges.Count);
                Assert.AreEqual(charge.Id, charges[0].Id);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestChargeToMerchantAndRefund()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "card";
                request.Card = GetCardInfo();
                request.Description = "Testing from .Net with new card";
                request.Amount = new Decimal(9.99);

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);

                Charge refund = await openpayAPI.ChargeService.Refund(charge.Id, "Merchant Refund");
                Assert.IsNotNull(refund);
                Assert.IsNotNull(refund.Refund);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestMerchantChargeWithStore()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "store";
                request.Description = "Testing from .Net [STORE]";
                request.Amount = new Decimal(9.99);

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.AreEqual("in_progress", charge.Status);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestMerchantChargeWithBitcoin()
        {
            Task.Run(async () =>
            {
                ChargeRequest request = new ChargeRequest();
                request.Method = "bitcoin";
                request.Description = "Testing from .Net [BITCOIN]";
                request.Amount = new Decimal(9.99);

                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Charge charge = await openpayAPI.ChargeService.Create(request);
                Assert.IsNotNull(charge);
                Assert.IsNotNull(charge.Id);
                Assert.IsNotNull(charge.PaymentMethod);
                Assert.IsNotNull(charge.PaymentMethod.AmountBitcoins);
                Assert.IsNotNull(charge.PaymentMethod.PaymentAddress);
                Assert.IsNotNull(charge.PaymentMethod.PaymentUrlBip21);
                Assert.IsNotNull(charge.PaymentMethod.ExchangeRate);
                Assert.AreEqual("bitcoin", charge.PaymentMethod.Type);
                Assert.AreEqual("charge_pending", charge.Status);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestMerchantList()
        {
            Task.Run(async () =>
            {
                SearchParams search = new SearchParams();
                search.CreationLte = new DateTime(2014, 1, 7);
                search.Amount = 9.99M;
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                List<Charge> charges = await openpayAPI.ChargeService.List(search);
                Assert.AreEqual(3, charges.Count);
                foreach (Charge charge in charges)
                {
                    Assert.AreEqual(true, charge.Conciliated);
                }
            }).GetAwaiter().GetResult();

        }

        public Card GetCardInfo()
        {
            Card card = new Card();
            card.CardNumber = "5555555555554444";
            card.HolderName = "Juanito Pérez Nuñez";
            card.Cvv2 = "123";
            card.ExpirationMonth = "01";
            card.ExpirationYear = "18";
            return card;
        }
    }
}
