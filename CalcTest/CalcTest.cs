using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace CalcClasses
{
    [TestClass]
    public class CalcTest
    {
        [TestMethod]
        public void Is_Constant()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "5";

            string expected = "5";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void JustLiterary()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "f";

            string expected = "f";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void OperationPriority()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "12+3*8/2+3*2/6";

            string expected = "25";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BracketPriority()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "12-3*(6-2)/4-7";

            string expected = "2";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DoubleBracket()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "7-3*((6-2))/5";

            string expected = "4,6";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ManyBracket()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "356*((55-(105-50))*3)";

            string expected = "0";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Exception_ExpectedCket()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "8-2*((6-2)/5";

            string actual = string.Empty;
            try
            {
                actual = calc.Calculation(expression);
            }
            catch(System.InvalidOperationException e)
            {
                //StringAssert.Contains(e.Message, "Не найдена закрывающая скобка в подвыражении"); 
                StringAssert.Contains(e.Message, "Не хватает '(' или ')' в выражении");
                return;            // !!!
            }

            Assert.Fail("Исключение должно быть, а его нет!");

        }
        [TestMethod]
        public void CanNotOperationWithNegativeNumber_NotException()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "-2+3";

            string expected = "-2+3";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void NotExecuteVarible_NotException()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "2+3+5*6+g";

            string expected = "35+g";
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void NotExecuteIfUnknownOperationFirst_NotException()
        {
            NinjectContext.SetUp();
            var calc = NinjectContext.Kernel.Get<CalculatorString>();

            string expression = "g-2-3+5*6";

            string expected = "g-2-3+30";   // Потому, что после умножения первой по приоритету стоит вычитание с 
                      //неизвестной переменной и от этого зависит дальнейший резальтат. Вычисления нужно остановить.
            string actual = calc.Calculation(expression);

            Assert.AreEqual(expected, actual);
        }
    }
}
