using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {


            
            IEEE754 first = new IEEE754("-20");
            Console.WriteLine("-20 {0}", first.ToString());
            IEEE754 second = new IEEE754("145");
            Console.WriteLine("145 {0}", second.ToString());

            IEEE754 result = new IEEE754("125");
            Console.WriteLine("125 {0}", result.ToString());
            Console.WriteLine("START ADDITION");
            first.Add(first, second);

            Console.WriteLine("================================================");

            first = new IEEE754("50");
            Console.WriteLine("50 {0}", first.ToString());
             second = new IEEE754("73");
            Console.WriteLine("73 {0}", second.ToString());

            result = new IEEE754("123");
            Console.WriteLine("123 {0}", result.ToString());
            Console.WriteLine("START ADDITION");
            first.Add(first, second);

            Console.WriteLine("================================================");


            BinaryHelper helper = new BinaryHelper();
            Console.WriteLine("Division as is:");

            int firstNumber = int.Parse(Console.ReadLine());
            int secondNumber = int.Parse(Console.ReadLine());
            var res = helper.Devision(firstNumber, secondNumber);
            Console.WriteLine("Multiplication shift right");

            firstNumber = int.Parse(Console.ReadLine());
            secondNumber = int.Parse(Console.ReadLine());
            res = helper.Multiply(helper.ConvertToBinary(firstNumber), helper.ConvertToBinary(secondNumber));


            string asss = helper.ToScreen(res);
            Console.WriteLine($"Binary format (help code) {asss}");
            Console.ReadKey();
        }


    }
}
