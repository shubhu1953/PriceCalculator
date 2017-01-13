using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketPriceCalculator
{
    public interface IPrint
    {
        void WriteLine(string s);
        void ReadLine();
    }

    // Use this console writer for your live code
    public class PrintOutput : IPrint
    {
        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }



        public void ReadLine()
        {
            Console.ReadLine();
        }
    }
}
