using System;

namespace CalcClasses
{
    class Prog
    {
        static void Main()
        {
            var myCalc = new StringCalculator("");
            var expression = "";
            do
            {
                Console.WriteLine("Задайте выражение ('q' для выхода):");
                expression = Console.ReadLine();

                if (expression.Trim().ToLower() == "q")
                {
                    break;
                }

                if (string.IsNullOrEmpty(expression))
                {
                    Console.WriteLine("Пусто");
                    continue;        //  !!
                }

                myCalc.Expression = expression;
                try
                {
                    Console.WriteLine(myCalc.Execute());
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
