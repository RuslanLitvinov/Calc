using System;

namespace CalcClasses
{
    /// <summary>
    /// Получение и передача сведений о математической операции.
    /// Операция Action - это математическое действия с левым и правым операндом
    /// Из действий - только парсинг операндов
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Какая операция: *,/,+,-,...
        /// </summary>
        string type;
        string operand1Str;
        public decimal? operant1Dec;
        string operand2Str;
        public decimal? operant2Dec;
        //private string resultStr;
        string expression;
        /// <summary>
        /// Исходное выражение операции, каким оно было в строке.
        /// Нужно например, что подменить в исходной строке результатом
        /// </summary>
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value.Trim().ToLower(); }
        }
        public string Operand1Str
        {
            get { return operand1Str; }
            set { operand1Str = value; }
        }
        public string Operand2Str
        {
            get { return operand2Str; }
            set { operand2Str = value; }
        }
        public Action(string expression)
        {
            this.expression = expression;
        }
        public void ParseOperands()
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
    }
}
