﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Openpay;
using Openpay.Entities;
using System.Collections.Generic;
using OpenPay;
using System.Threading.Tasks;

namespace OpenpayTest
{
    [TestClass]
    public class PlantServiceTest
    {

        string customer_id = "adyytoegxm6boiusecxm";

        [TestMethod]
        public void TestCreateGeDelete()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Plan plan = new Plan();
                plan.Name = "Tv";
                plan.Amount = 99.99m;
                plan.RepeatEvery = 1;
                plan.RepeatUnit = "month";
                plan.StatusAfterRetry = "unpaid";
                plan.TrialDays = 0;
                Plan planCreated = await openpayAPI.PlanService.Create(plan);
                Assert.IsNotNull(planCreated.Id);
                Assert.IsNotNull(planCreated.CreationDate);
                Assert.AreEqual("active", planCreated.Status);

                Plan planGet = await openpayAPI.PlanService.Get(planCreated.Id);
                Assert.AreEqual(planCreated.Name, planGet.Name);

                openpayAPI.PlanService.Delete(planCreated.Id);
            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void TestUpdatePlan()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Plan plan = await openpayAPI.PlanService.Get("pxs6fx3asdaa7xg3ray4");
                Random rnd = new Random();
                string newName = plan.Name + rnd.Next(0, 10);
                plan.Name = newName;
                Plan plantUpdated = await openpayAPI.PlanService.Update(plan);
                Assert.AreEqual(newName, plantUpdated.Name);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestListSubscriptions()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                Plan plan = new Plan();
                plan.Name = "Tv";
                plan.Amount = 89.99m;
                plan.RepeatEvery = 1;
                plan.RepeatUnit = "month";
                plan.StatusAfterRetry = "unpaid";
                plan.TrialDays = 0;
                plan = await openpayAPI.PlanService.Create(plan);

                Card card = new Card();
                card.CardNumber = "345678000000007";
                card.HolderName = "Juanito Pérez Nuñez";
                card.Cvv2 = "1234";
                card.ExpirationMonth = "01";
                card.ExpirationYear = "17";

                Subscription subscription = new Subscription();
                subscription.PlanId = plan.Id;
                subscription.Card = card;
                subscription = await openpayAPI.SubscriptionService.Create(customer_id, subscription);

                List<Subscription> subscriptions = await openpayAPI.PlanService.Subscriptions(plan.Id);
                Assert.AreEqual(1, subscriptions.Count);

                openpayAPI.SubscriptionService.Delete(customer_id, subscription.Id);
                openpayAPI.PlanService.Delete(plan.Id);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void TestList()
        {
            Task.Run(async () =>
            {
                OpenpayAPI openpayAPI = new OpenpayAPI(Constants.API_KEY, Constants.MERCHANT_ID);
                List<Plan> plans = await openpayAPI.PlanService.List();
                Assert.IsTrue(plans.Count > 0);
            }).GetAwaiter().GetResult();

        }
    }
}
