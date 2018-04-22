using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{

    public class IEEE754
    {
        public int Sign { get; set; }
        public int[] Exponent { get; set; }

        public int Normalized { get; set; }
        public int[] Fraction { get; set; }

        public string NormPart { get; set; }

        public IEEE754()
        {

        }
        public IEEE754(string number, int expLength, int fracLength)
        {
            Exponent = new int[expLength];
            Fraction = new int[fracLength];

            if (number.Contains("Infinity"))
            {
                if (number.Contains("-"))
                {
                    MakeNegativeInfinity();
                }
                else
                {
                    MakePositiveInfinity();
                }
            }
            else
            {
                float num = 0;
                bool isNotNAN = Single.TryParse(number, out num);
                if (isNotNAN)
                {
                    ConvertFloatToIEE754(num);
                }
                else
                {
                    MakeNaN();
                }
            }


        }
        private void MakeNegativeInfinity()
        {
            Sign = 1;
            Normalized = 0;
            for (int i = 0; i < Exponent.Length; i++)
            {
                Exponent[i] = 1;
            }

        }
        private void MakePositiveInfinity()
        {
            Sign = 0;
            Normalized = 0;
            for (int i = 0; i < Exponent.Length; i++)
            {
                Exponent[i] = 1;
            }

        }
        private void MakeNaN()
        {
            Sign = 0;
            Normalized = 1;
            for (int i = 0; i < Exponent.Length; i++)
            {
                Exponent[i] = 1;
            }
            for (int i = 0; i < Fraction.Length; i++)
            {
                Fraction[i] = 1;
            }
        }
        public IEEE754(string number) : this(number, 8, 23)
        {

        }
        public IEEE754(float number, int expLength, int fracLength)
        {
            Exponent = new int[expLength];
            Fraction = new int[fracLength];
            ConvertFloatToIEE754(number);
        }
        public IEEE754(float number) : this(number, 8, 23)
        {

        }
        private void FindTheExponent(int power)
        {
            int init_bias = (int)Math.Round(Math.Pow(2, Exponent.Length - 1)) - 1;
            int bias = init_bias + power;
            if (bias < 0 || bias > init_bias * 2)
            {
                throw new ArgumentException();
            }

            string stringResult = Convert.ToString(bias, 2);
            while (Exponent.Length != stringResult.Length)
            {
                stringResult = stringResult.Insert(0, "0");
            }
            for (int i = 0; i < stringResult.Length; i++)
            {
                Exponent[i] = int.Parse(stringResult[i].ToString());
            }
        }
        public int[] ConvertToBinary(int value)
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

                binaryList[binaryList.Count - 1 - counter] = value % 2;
                value = value / 2;
                counter++;
            } while (value != 0);

            return binaryList.ToArray();
        }
        private void ConvertFloatToIEE754(float number)
        {
            try
            {

                int power = 0;
                if (number == 0)
                {
                    NormPart = String.Format("{0:F8}", (double)number) + "*2E" + (-126).ToString();
                    Normalized = 0;
                    return;
                }
                if (number > 0)
                {
                    Sign = 0;
                }
                else
                {
                    Sign = 1;
                }
                number = Math.Abs(number);

                Normalized = 1;
                if (number == Single.PositiveInfinity || number == Single.NegativeInfinity)
                {
                    throw new ArgumentException();
                }
                while (number >= 2)
                {
                    number = number / 2f;
                    power++;
                }
                while (number < 1)
                {
                    number = number * 2f;
                    power--;
                }
                NormPart = String.Format("{0:F9}", (double)number) + "*2E" + power.ToString();
                FindTheExponent(power);
                WriteTheFractionInBinaryForm(number);

            }
            catch (ArgumentException ex)
            {
                if (Sign == 0)
                {
                    MakePositiveInfinity();
                }
                else
                {
                    MakeNegativeInfinity();
                }
            }

        }
        private void WriteTheFractionInBinaryForm(float number)
        {
            float fraction = number - 1;
            float tempFraction = 0;
            for (int i = 0; i < Fraction.Length; i++)
            {
                if ((float)1 / (2 * Math.Pow(2, i)) > fraction - tempFraction)
                {
                    Fraction[i] = 0;
                }
                else
                {
                    tempFraction += (float)(1 / (2 * Math.Pow(2, i)));
                    Fraction[i] = 1;
                }

            }
        }
        public override string ToString()
        {

            string result = "";
            result = NormPart + Environment.NewLine;

            result += Sign.ToString() + "|";
            for (int i = 0; i < Exponent.Length; i++)
            {
                result += Exponent[i].ToString();
            }
            result += "|";
            result += Normalized.ToString() + "|";
            for (int i = 0; i < 23; i++)
            {
                result += Fraction[i].ToString();
            }
            return result;

        }

        public IEEE754 Add(IEEE754 first, IEEE754 second)
        {
            var result = new IEEE754();

            //first power

            int firstPower = FindPower(first);
            int secondPower = FindPower(second);

            Console.WriteLine($"First Power  { firstPower.ToString()}");
            Console.WriteLine($"Second Power  { secondPower.ToString()}");



            //abs (a) > abs (b)
            int diff = 0;
            if (firstPower >= secondPower)
            {
                diff = firstPower - secondPower;
                Console.WriteLine($"Abs(A) > Abs(B)");

            }
            else
            {
                Console.WriteLine($"Abs(A) < Abs(B) , swapping");

                //swapping
                IEEE754 temp = new IEEE754();
                temp.Exponent = first.Exponent;
                temp.Sign = first.Sign;
                temp.Normalized = first.Normalized;
                temp.Fraction = first.Fraction;


                first.Exponent = second.Exponent;
                first.Sign = second.Sign;
                first.Normalized = second.Normalized;
                first.Fraction = second.Fraction;

                second.Exponent = temp.Exponent;
                second.Sign = temp.Sign;
                second.Normalized = temp.Normalized;
                second.Fraction = temp.Fraction;

                int tmp = secondPower;
                secondPower = firstPower;
                firstPower = tmp;
                diff = firstPower - secondPower;
            }
            Console.WriteLine($"Diff Power  {diff.ToString()}");


            int resultPower = firstPower;
            result.Exponent = first.Exponent;


            //shift left mantisa of x2

            for (int i = 0; i < diff; i++)
            {
                if (i == 0)
                {
                    //shift with normalized bit.
                    second.Fraction = ShiftWithNormalized(second.Fraction);
                    second.Normalized = 0;
                }
                else
                {
                    second.Fraction = Shift(second.Fraction);
                }
            }
            Console.WriteLine($"Shifted Mantisa of Second  {second.ToString()}");

            if (first.Sign == second.Sign)
            {
                Console.WriteLine("Signs are equal");
                //add


                //diff
                int[] extFrFirst = new int[24];
                for (int i = 0; i < first.Fraction.Length; i++)
                {
                    extFrFirst[i + 1] = first.Fraction[i];
                }
                extFrFirst[0] = first.Normalized;

                int[] extFrSecond = new int[24];
                for (int i = 0; i < second.Fraction.Length; i++)
                {
                    extFrSecond[i + 1] = second.Fraction[i];
                }
                extFrSecond[0] = second.Normalized;
                var res = Add(extFrFirst, extFrSecond);
                
                int tmp = res.Item2;

              

                if (tmp == 1)
                {

                    result.Fraction = res.Item1;
                    Console.WriteLine($"Unnormalized  {result.ToString()}");

                    int[] extFr = Shift(res.Item1);

                    result.Fraction = new int[23];
                    result.Normalized = 1;
                    for (int i = 1; i < extFr.Length; i++)
                    {
                        result.Fraction[i - 1] = extFr[i];
                    }

                    firstPower += 1;
                    result.Exponent = Add(result.Exponent, new int[8] { 0, 0, 0, 0, 0, 0, 0, 1 }).Item1;
                }
                else
                {
                    result.Fraction = new int[23];
                    result.Normalized = res.Item1[0];
                    for (int i = 1; i < res.Item1.Length; i++)
                    {
                        result.Fraction[i - 1] = res.Item1[i];
                    }
                }
               

            }
            else
            {
                Console.WriteLine("Signs aren't equal");

                //diff
                int[] extFrFirst = new int[24];
                for (int i = 0; i < first.Fraction.Length; i++)
                {
                    extFrFirst[i + 1] = first.Fraction[i];
                }
                extFrFirst[0] = first.Normalized;

                int[] extFrSecond = new int[24];
                for (int i = 0; i < second.Fraction.Length; i++)
                {
                    extFrSecond[i + 1] = second.Fraction[i];
                }
                extFrSecond[0] = second.Normalized;

                //make second negative
                var div = extFrSecond.Select((x) => { if (x == 1) return 0; else return 1; }).ToArray();
                var negativeSecond = Add(div, new int[24] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
              
                var res = Add(extFrFirst, negativeSecond.Item1);
              
                if (res.Item1[0] == 0)
                {

                    //while not zero - normalize
                    int[] newFracton = res.Item1;
                    result.Fraction = newFracton;
                    Console.WriteLine($"Unnormalized  {result.ToString()}");

                    while (newFracton[0] == 0)
                    {
                        newFracton = ShiftLeftWithZero(newFracton);
                        result.Exponent = Add(result.Exponent, new int[24] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }).Item1;

                    }
                    result.Fraction = new int[23];
                    result.Normalized = 1;
                    for (int i = 1; i < newFracton.Length; i++)
                    {
                        result.Fraction[i - 1] = newFracton[i];
                    }


                }
                else
                {
                    result.Fraction = new int[23];
                    result.Normalized = 1;
                    for (int i = 1; i < res.Item1.Length; i++)
                    {
                        result.Fraction[i - 1] = res.Item1[i];
                    }
                }
            }
            Console.WriteLine($"Normalized Result  {result.ToString()}");

            return result;
        }
        public int[] Shift(int[] source)
        {
            int prev = 0; int next = 0;
            for (int i = 0; i < source.Length; i++)
            {
                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
           public int[] ShiftLeftWithZero(int[] source)
        {
            int prev = 0; int next = 0;
            for (int i = source.Length - 1; i >= 0; i--)
            {
                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
        public int[] ShiftWithNormalized(int[] source)
        {
            int prev = 1; int next = 0;
            for (int i = 0; i < source.Length; i++)
            {
                next = source[i];
                source[i] = prev;
                prev = next;
            }
            return source;
        }
        private int FindPower(IEEE754 number)
        {
            //calculate POWER 
            double power = 0;

            for (int i = number.Exponent.Length - 1; i >= 0; i--)
            {
                power += number.Exponent[i] == 1 ? Math.Pow(2f, number.Exponent.Length -1 - i) : 0;

            }
            return (int)power;
        }
        public Tuple<int[],int> Add(int[] first, int[] second)
        {

            var result = new int[first.Length];

            int temporary = 0;
            for (int index = first.Length - 1; index >= 0; index--)
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
                    result[index ] = 1;
                    temporary = 1;
                }

            }
            return new Tuple<int[], int>(result, temporary);
        }
    }
}
