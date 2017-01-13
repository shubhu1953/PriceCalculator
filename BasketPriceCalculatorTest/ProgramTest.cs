using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketPriceCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BasketPriceCalculatorTest
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void Check_Console_output_Number_Of_Times()
        {
            var writer = new Mock<IPrint>();
            Program.PrintOutput(new List<string>() { "Milk", "Apples" }, writer.Object,
                MockDataSetup.SetMockPrices().Object, MockDataSetup.SetMockPromotions().Object);
            writer.Verify(w => w.WriteLine(It.IsAny<string>()), Times.Exactly(3));

        }
    }
}
