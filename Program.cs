using System;
//using Ninject.Core;
//using Ninject.Modules;
using Ninject;


namespace CalcClasses
{
    class Prog
    {
        static void Main()
        {
            //IModule = new InlineModule
            //IKernel kernel = new StandardKernel(new InlineModule());
            NinjectContext.SetUp();
            var myCalc = NinjectContext.Kernel.Get<CalculatorString>();
            //var myCalc = new CalculatorString();
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
