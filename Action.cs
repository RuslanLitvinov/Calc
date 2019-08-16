using System;

namespace CalcClasses
{
    public class Action
    {
        // Элементы дынных публичные - для простоты
        /// <summary>
        /// Какая операция: *,/,+,-,...
        /// </summary>
        public string type;
        public string operand1Str;
        public decimal? operant1Dec;
        public string operand2Str;
        public decimal? operant2Dec;
        public decimal? resultDec;
        private string resultStr;
        public string expression;
        public Action(string expression)
        {
            this.expression = expression;
            this.resultStr = expression;
        }
        public string Result()
        {
            if (this.resultDec != null)
            {
                return this.resultDec.ToString();
            }
            else
            {
                return this.resultStr;  
            }
        }
        public void Execute()
        {
            if (string.IsNullOrEmpty(this.type))
            {
                return;
            }

            ParseOperands();

            switch (this.type.Trim().ToLower())
            {
                case "*":
                    if (this.ValidateTwoOperands())
                    {
                        this.resultDec = this.operant1Dec * this.operant2Dec;
                    }
                    break;
                case "/":
                    if (this.ValidateTwoOperands())
                    {
                        this.resultDec = this.operant1Dec / this.operant2Dec;
                    }

                    break;
                case "+":
                    if (this.ValidateTwoOperands())
                    {
                        this.resultDec = this.operant1Dec + this.operant2Dec;
                    }
                    break;
                case "-":
                    if (this.ValidateTwoOperands())
                    {
                        this.resultDec = this.operant1Dec - this.operant2Dec;
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Для операции '{this.type}' не реализовано исполнение.");
                  
            }
        }
        private void ParseOperands()
        {
            if (this.operant1Dec == null && !string.IsNullOrWhiteSpace(this.operand1Str))
            {
                try
                {
                    this.operant1Dec = decimal.Parse(this.operand1Str);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Операнд 1 = '{this.operand1Str}' непонятен и будет оставлен в выражении.");
                }
            }
            if (this.operant2Dec == null && !string.IsNullOrWhiteSpace(this.operand2Str))
            {
                try
                {
                    this.operant2Dec = decimal.Parse(this.operand2Str);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Операнд 2 = '{this.operand2Str}' непонятен и будет оставлен в выражении.");
                }
            }
        }
        private bool ValidateTwoOperands()
        {
            if (this.operant1Dec == null || this.operant2Dec == null)
            {
                Console.WriteLine($"Для операции '{this.type}' нет одного из операндов. Выражение {this.expression} будет результатом.");
                return false;     // !!!
            }

            return true;    // !!!
        }
    }
}
