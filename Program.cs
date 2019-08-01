using System;

namespace CalcClasses
{
    class Prog
    {
        /// <summary>
        /// Для тестирования - потом удалить
        /// </summary>
        /*static void Main()
        {
            var myCalc = new StringCalculator();
            var action = new Action("");
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
                    //Console.WriteLine(myCalc.GetBracketExpression(expression)); 
                    //Console.WriteLine("Задайте позицию:");
                    //int pos = int.Parse(Console.ReadLine());
                    action = myCalc.GetNextAction(myCalc.Expression);
                    Console.WriteLine($"Операция: {action.type}, операнд1: {action.operand1Str} операнд2: {action.operand2Str} выражение: {action.expression}"); ; ;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Неверно задано выражение:");
                    Console.WriteLine(e.ToString());

                }
            }  while(true);
        }*/
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
