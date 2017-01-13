using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketPriceCalculator.Repositories;
using BasketPriceCalculator.Service;

namespace BasketPriceCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Count().Equals(0))
                    throw new ArgumentNullException("args");
                var itemList = args.ToList();

                PrintOutput(itemList, new PrintOutput(), 
                    new PriceRepository(), new PromotionRepository());
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occurred. " + ex.Message);
            }
        }

        public static void PrintOutput(List<string> args, IPrint writer, IPriceRepository priceRep, IPromotionRepository promoRep)
        {
            var output = new StringBuilder();
            PriceCalculatorService service = new PriceCalculatorService(priceRep, promoRep);
            service.SetBasket(args);
            service.CalculatePromotions();
            writer.WriteLine(string.Format("Subtotal: {0}", service.SubTotal.ToString("c")));
            writer.WriteLine(service.PromotionText);
            writer.WriteLine("Total: " + service.Total.ToString("c"));
            writer.ReadLine();




        }
    }
}
