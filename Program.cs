using System;

namespace CalcClasses
{
    class Prog
    {
        static void Main()
        {
            var myCalc = new CalculatorString();
            do
            {
                Console.WriteLine("Задайте выражение ('q' для выхода):");
                string expression = Console.ReadLine();

                if (expression.Trim().ToLower() == "q")
                {
                    break;
                }

                try
                {
                    Console.WriteLine(myCalc.Calculation(expression));
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Неверно задано выражение:");
                    Console.WriteLine(e.ToString());

                }
                Console.WriteLine("");
            } while (true);
        }

    }
}
