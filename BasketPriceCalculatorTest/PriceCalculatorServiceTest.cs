using System;
using System.Collections.Generic;
using BasketPriceCalculator.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasketPriceCalculatorTest
{
    [TestClass]
    public class PriceCalculatorServiceTest
    {
        PriceCalculatorService _priceCalculatorService;
        [TestInitialize]
        public void TestInitialize()
        {
            var mockPrice = MockDataSetup.SetMockPrices();
            var mockPromotion = MockDataSetup.SetMockPromotions();
            _priceCalculatorService = new PriceCalculatorService(mockPrice.Object, mockPromotion.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Set_Basket_with_Null()
        {
            _priceCalculatorService.SetBasket(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Set_Basket_with_No_items()
        {
            _priceCalculatorService.SetBasket(new List<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.InvalidDataException))]
        public void Set_Basket_with_Invalid_Item()
        {
            _priceCalculatorService.SetBasket(new List<string>() { "Potato" });
        }
        [TestMethod]
        public void Set_Basket_with_Check_Items_Count()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk" });
            //assert
            Assert.AreEqual(2, _priceCalculatorService.LineItems.Count);
        }

        [TestMethod]
        public void Check_Basket_SubTotal()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk" });
            //assert
            Assert.AreEqual(2.30, _priceCalculatorService.SubTotal);
        }

        [TestMethod]
        public void Check_Basket_No_Promotion_Message()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Milk" });
            _priceCalculatorService.CalculatePromotions();
            //assert
            Assert.AreEqual("(No offers available)", _priceCalculatorService.PromotionText);
        }


        [TestMethod]
        public void Check_Basket_Total_For_Simple_PriceCut_Promotion()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk" });
            _priceCalculatorService.CalculatePromotions();
            //assert
            Assert.AreEqual(2.20, double.Parse(_priceCalculatorService.Total.ToString("#.##")));
        }

        [TestMethod]
        public void Check_Basket_Valid_Promotion_Message()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk" });
            _priceCalculatorService.CalculatePromotions();
            //assert
            Assert.IsTrue(_priceCalculatorService.PromotionText.Contains("10% discount on apples"));
        }

        [TestMethod]
        public void Check_Basket_Total_For_GroupSave_Promotion()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk", "Soup", "Bread", "Soup" });
            _priceCalculatorService.CalculatePromotions();
            //assert
            Assert.AreEqual(3.90, double.Parse(_priceCalculatorService.Total.ToString("#.##")));
        }

        [TestMethod]
        public void Check_Basket_Total_For_Missed_GroupSave_Promotion_On_Quantity()
        {
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Apples", "Milk", "Soup", "Bread" });
            _priceCalculatorService.CalculatePromotions();
            //assert
            Assert.AreEqual(3.65, double.Parse(_priceCalculatorService.Total.ToString("#.##")));
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.InvalidDataException))]
        public void Check_Basket_Invalid_Promotion()
        {
            //arrange
            _priceCalculatorService = new PriceCalculatorService(MockDataSetup.SetMockPrices().Object, MockDataSetup.SetMockInvalidPromotions().Object);
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Milk" });
            _priceCalculatorService.CalculatePromotions();

        }

        [TestMethod]
        public void Out_of_Date_Range_Promotion_Ignored()
        {
            //arrange
            _priceCalculatorService = new PriceCalculatorService(MockDataSetup.SetMockPrices().Object, MockDataSetup.SetMockOutOfRangePromotions().Object);
            //act
            _priceCalculatorService.SetBasket(new List<string>() { "Milk" });
            _priceCalculatorService.CalculatePromotions();
            Assert.AreEqual(1.30, _priceCalculatorService.Total);

        }
    }
}
