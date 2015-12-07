using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Openpay;
using Openpay.Entities;
using System.Collections.Generic;
using OpenPay;
using OpenpayTest;
using System.Threading.Tasks;

namespace OpenpayTests
{
    [TestClass]
    public class BankAccountServiceTest
    {
        private static readonly string customer_id = "adyytoegxm6boiusecxm";

        [TestMethod]
        public void TestAsCustomer_CreateGetDelete()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                BankAccount bankAccount = new BankAccount();
                bankAccount.CLABE = "012298026516924616";
                bankAccount.HolderName = "Testing";
                BankAccount bankAccountCreated = await openpayAPI.BankAccountService.Create(customer_id, bankAccount);
                Assert.IsNotNull(bankAccountCreated.Id);
                Assert.IsNull(bankAccountCreated.Alias);
                Assert.AreEqual("012XXXXXXXXXX24616", bankAccountCreated.CLABE);

                BankAccount bankAccountGet = await openpayAPI.BankAccountService.Get(customer_id, bankAccountCreated.Id);
                Assert.AreEqual("012XXXXXXXXXX24616", bankAccountGet.CLABE);

                List<BankAccount> accounts = await openpayAPI.BankAccountService.List(customer_id);
                Assert.AreEqual(1, accounts.Count);

                openpayAPI.BankAccountService.Delete(customer_id, bankAccountGet.Id);
            }).GetAwaiter().GetResult();
        }

        // [TestMethod]
        public void TestAsMerchant_CreateGetDelete()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                string bank_account_id = "bypzo1cstk5xynsuzjxo";

                BankAccount bankAccountGet = await openpayAPI.BankAccountService.Get(bank_account_id);
                Assert.AreEqual("012XXXXXXXXXX24616", bankAccountGet.CLABE);

                openpayAPI.BankAccountService.Delete(bankAccountGet.Id);
            }).GetAwaiter().GetResult();
        }
    }
}
