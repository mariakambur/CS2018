using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class BinaryHelper
    {
        public BinaryHelper()
        {

        }

        public List<int> ConvertToBinary(int value)
        {
            List<int> binaryList = new List<int>(8) { 0, 0, 0, 0, 0, 0, 0, 0 };
            int counter = 0;
            if (value >= 0)
            {
                binaryList[0] = 0;
            }
            else
            {
                binaryList[0] = 1;
            }


            value = Math.Abs(value);
            do
            {
                if (counter == 8 - 1)
                {

                    break;
                }

                binaryList[binaryList.Count - 1 - counter] = value % 2;
                value = value / 2;
                counter++;
            } while (value != 0);

            return binaryList;
        }

        public List<int> ConvertToHelpCode(int value)
        {
            List<int> binaryList = ConvertToBinary(value);
            if (binaryList[0] == 0)
            {
                return binaryList;
            }
            var helpCodeList = new List<int>(15);

            //invert

            var invertList = new List<int>(8) { 1, 0, 0, 0, 0, 0, 0, 0 };
            for (int index = 1; index < binaryList.Count; index++)
            {
                invertList[index] = (binaryList[index] == 0 ? 1 : 0);
            }
            helpCodeList = Add(first: invertList, second: ConvertToBinary(1));
            return helpCodeList;

        }
        public List<int> ConvertToNegative(List<int> binaryList)
        {
            
            var helpCodeList = new List<int>(binaryList.Count);

            //invert

            var invertList = new List<int>(binaryList.Count) { 1, 0, 0, 0, 0, 0, 0, 0 };
            for (int index = 1; index < binaryList.Count; index++)
            {
                invertList[index] = (binaryList[index] == 0 ? 1 : 0);
            }
            helpCodeList = Add(first: invertList, second: ConvertToBinary(1));
            return helpCodeList;

        }
        public List<int> Add(List<int> first, List<int> second)
        {
            var result = new List<int>(8) { 0, 0, 0, 0, 0, 0, 0, 0 };

            int temporary = 0;
            for (int index = first.Count - 1; index >= 1; index--)
            {
                int sum = first[index] + second[index] + temporary;
                if (sum <= 1)
                {
                    result[index] = sum;
                    temporary = 0;
                }
                else if (sum == 2)
                {
                    result[index] = 0;
                    temporary = 1;
                }
                else if (sum == 3)
                {
                    result[index] = 1;
                    temporary = 1;
                }
                if(temporary == 1)
                {
                    var a = 1;
                }
            }

            
            return result;
        }
        public List<int> AddWithoutSign(List<int> first, List<int> second)
        {
            var result = new List<int>(16) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int temporary = 0;
            for (int index = first.Count - 1; index >= 0; index--)
            {
                int sum = first[index] + second[index] + temporary;
                if (sum <= 1)
                {
                    result[index] = sum;
                    temporary = 0;
                }
                else if (sum == 2)
                {
                    result[index] = 0;
                    temporary = 1;
                }
                else if (sum == 3)
                {
                    result[index] = 1;
                    temporary = 1;
                }
                if (temporary == 1)
                {
                    var a = 1;
                }
            }

            
            return result;
        }

        public List<int> WriteToRegister(List<int> source ,List<int> register)
        {
            int temporary = 0;
            for (int index = source.Count - 1; index >= 1; index--)
            {
                int sum = source[index] + register[index] + temporary;
                if (sum <= 1)
                {
                    register[index] = sum;
                    temporary = 0;
                }
                else if (sum == 2)
                {
                    register[index] = 0;
                    temporary = 1;
                }
                else if (sum == 3)
                {
                    register[index] = 1;
                    temporary = 1;
                }
                
            }

            register[0] = temporary;
            return register;
        }
        //в прямом коде
        public List<int> Multiply(List<int> first, List<int> second)
        {

            //сравнить знаки
            Console.WriteLine($"second : {ToScreen(second)}");

            Console.WriteLine($"first : {ToScreen(first)}");


            int sign = Convert.ToInt16(Convert.ToBoolean(first[0]) ^ Convert.ToBoolean(second[0]));

            var register = new List<int>(15) {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 1; i < second.Count ; i++)
            {
              
                if (second[second.Count - 1] == 1)
                {
                   register =  WriteToRegister(first, register);

                    Console.WriteLine($"addto register : {ToScreen(register)}");

                }

                //shift the multiplier

                second = Shift(second);


                register = Shift(register);
                Console.WriteLine($"shift register : {ToScreen(register)}");
                Console.WriteLine($"first register : {ToScreen(first)}");

                Console.WriteLine($"shift mult : { ToScreen(second)}");


            }
            //register = Shift(register);

            int res = ConvertRegisrtyToInt(register);
            res = sign == 0 ? res * 1 : res * -1;

            // register[0] = sign;
            Console.WriteLine($"register  {ToScreen(register)}");
            Console.WriteLine($"int value {res}");

            register[0] = sign;
            return register;
        }

        public List<int> Devision(int first, int second)
        {
            var f = ConvertToBinary(first);
            var s = ConvertToBinary(second);



            Console.WriteLine($"first : {ToScreen(f)}");
            Console.WriteLine($"second : {ToScreen(s)}");


            //var Devidend = new List<int>(f) { 0, 1, 1, 1 };
            //сравнить знаки - применить в самом конце
            int sign = Convert.ToInt16(Convert.ToBoolean(f[0]) ^ Convert.ToBoolean(s[0]));
            f[0] = 0;
            s[0] = 0;

            var Devidend = f;

            // reminder в два раза больше делимого
            var Reminder = new List<int>(16) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var Quotient = new List<int>(8) {0, 0, 0, 0, 0,0,0,0 };
            //var Divisor  = new List<int>(8) { 0, 0, 1, 0, 0, 0, 0, 0 };

            var Divisor = new  List<int>(16) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            
            //place devider in Reminder
            for (int i = 0; i < Devidend.Count; i++)
            {
                 //или убрать + 
                Reminder[i + Devidend.Count] = Devidend[i];
            }
            for (int i = 0; i < s.Count; i++)
            {
                Divisor[i] = s[i];
            }


            for (int i = 0; i < Devidend.Count + 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"iteration {i}");
                Console.WriteLine($"Devidend  {ToScreen(Devidend)}");
                Console.WriteLine($"Reminder  {ToScreen(Reminder)}");
                Console.WriteLine($"Divisor   {ToScreen(Divisor)}");
                Console.WriteLine($"Quotient  {ToScreen(Quotient)}");

                //Subtract the Divisor register from the 
                //Remainder register, and place the result
                //in the Remainder register.

                //make divisor negative
                var div = Divisor.Select((x) => { if (x == 1) return 0; else return 1; }).ToList();
                var negativeDivisor = AddWithoutSign(div, new List<int>(16) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
                Reminder = AddWithoutSign(Reminder, negativeDivisor);

                if(Reminder[0] == 0)
                {
                   Quotient= ShiftLeft(Quotient);
                }
                else
                {
                    Reminder = AddWithoutSign(Divisor, Reminder);
                    Quotient= ShiftLeftWithZero(Quotient);
                }
                Divisor = Shift(Divisor);
            }
            Console.WriteLine();
            Console.WriteLine($"Devidend  {ToScreen(Devidend)}");
            Console.WriteLine($"Reminder  {ToScreen(Reminder)}");
            Console.WriteLine($"Divisor   {ToScreen(Divisor)}");
            Console.WriteLine($"Quotient  {ToScreen(Quotient)}");
            int multSign = sign == 0 ? 1 : -1;
            Console.WriteLine($"Devidend  {first.ToString()} Devider {second.ToString()}");
            Console.WriteLine($"Result    {(multSign * ConvertToInt(Quotient)).ToString()} Reminder {ConvertToInt(Reminder).ToString()}");


            return Quotient;
        }

        public List<int> Shift(List<int> source)
        {
            //первый разряд - знак.не трогаем
            int prev = source[0];
            int next = source[0];
            source[0] = 0;
            for (int i = 1; i < source.Count; i++)
            {

                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
        public List<int> ShiftLeft(List<int> source)
        {  
            int  prev = 1;int next = 0;
            for (int i = source.Count -1; i >=0 ; i--)
            {
                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
        public List<int> ShiftLeftWithZero(List<int> source)
        {
            int prev = 0; int next = 0;
            for (int i = source.Count-1; i >= 0; i--)
            {
                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
        public int ConvertToInt(List<int> helpCodeList)
        {
            int tempOf = 0;
            if (helpCodeList[0] != 0)
            {
                helpCodeList = Add(first: helpCodeList, second: ConvertToBinary(-1));
                for (int index = 1; index < helpCodeList.Count; index++)
                {
                    helpCodeList[index] = (helpCodeList[index] == 0 ? 1 : 0);
                }
            }

            int value = 0;
            for (int i = helpCodeList.Count - 1; i >= 1; i--)
            {
                value += helpCodeList[i] * (int)Math.Pow(2, helpCodeList.Count - 1 - i);
            }
            return value;

        }
        public int ConvertRegisrtyToInt(List<int> helpCodeList)
        {
            int tempOf = 0;
            if (helpCodeList[0] != 0)
            {
                helpCodeList = WriteToRegister(ConvertToBinary(-1),helpCodeList);
                for (int index = 1; index < helpCodeList.Count; index++)
                {
                    helpCodeList[index] = (helpCodeList[index] == 0 ? 1 : 0);
                }
            }

            int value = 0;
            for (int i = helpCodeList.Count - 1; i >= 1; i--)
            {
                value += helpCodeList[i] * (int)Math.Pow(2, helpCodeList.Count - 1 - i);
            }
            return value;

        }
        public string ToScreen(List<int> value)
        {
            string number = "";
            foreach (var bit in value)
            {
                number += bit.ToString();
            }
            return number;
        }
    }

}
