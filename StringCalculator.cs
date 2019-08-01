using System;
using System.Linq;

namespace CalcClasses
{
    /// <summary>
    /// Рализует начальных функционал калькулятора из текстовой строки со скобками.
    /// Учитывает приоритет операций. Например выражение 3*(2+3)/2-3*2 - должно правильно вычисляться.
    /// Для добавления новой операции над операндом (или 2-мя операндами) в выражение expression
    /// нужно:
    /// а) добавить ее обозначение (и приоритет) в метод InitOperators;
    /// б) реализацию в класс Action
    /// </summary>
    class StringCalculator : StringExpression
    {
        public StringCalculator(string expression):base()
        {
            this.Expression = expression;
        }
        public string Execute()
        {
            ValidateExpression(this.Expression);
            return Calculation(this.Expression);
        }
        /// <summary>
        /// Вычисляет значение выражения в строковом виде
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        ///  Строковое значение, а не decimal потому, что не все может быть реализовано: 
        ///    функция или непределенная переменная - вернет как задали
        /// </returns>
        private string Calculation(string expression)
        {
            // Вычисление скобки
            string bracketExpr = GetFirstBracketExpression(expression);
            if (!string.IsNullOrEmpty(bracketExpr))
            {
                string bracketExprForCalc = bracketExpr.Substring(1, bracketExpr.Length - 2);      // Получим выражение без скобок
                try
                {   // в скобках просто число
                    decimal nmbr = decimal.Parse(bracketExprForCalc);
                    return Calculation(expression.Replace(bracketExpr, bracketExprForCalc));        //  !!!
                }
                catch (FormatException)
                {
                    ;
                }
                string bracketExecResult = Calculation(bracketExprForCalc);
                if ('('+bracketExecResult+')' == bracketExpr)
                {
                    Console.WriteLine($"Выражение в скобках '{bracketExpr}' не может быть посчитано. Вычисления прекращаются");
                    return expression;       // !!!
                }
                return Calculation(expression.Replace(bracketExpr, bracketExecResult));        //  !!!
            }

            // тригонометрические и другие функции
            string funcExpr = GetFuncExpression(expression);
            if (!string.IsNullOrEmpty(funcExpr))
            {
                return Calculation(expression.Replace(funcExpr, CalcFunction(funcExpr)));     // !!!
            }

            // простые арифметические операции
            var action = GetNextAction(expression);
            if (action != null)
            {
                action.Execute();
                if (expression.Trim().ToLower() == action.Result().Trim().ToLower())
                {  // то есть после очередной операции ничего не изменилось - завершение.
                    return expression;   // !!!
                }

                return Calculation(expression.Replace(action.expression, action.Result() ) );   // !!!
            }

            // Остается число или переменная или выражение содержащее неопределенную переменную - просто вернуть
            return expression;   // !!!
        }
        /// <summary>
        /// Получает следующую операцию из выражения expression по приоритетам операций.
        /// Разработчик сам должен контролировать, когда вызывать GetSimpleActionExpr для
        /// выражения - что бы в нем к этому моменту не осталось непосчитанных скобок и функций
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Action GetNextAction(string expression)
        {
           if (string.IsNullOrEmpty(expression))
            {
                return null;        //         !!!
            }
            var exprOperators = GetFirstOperatorsSortPositionAsc(expression);
            if (exprOperators == null)
            {
                // Не обнаружено ни одной операции из известных программе операторов (в this.operators). Вернуть все выражение.
                return new Action(expression);      //     !!!
            }
            // Посмотрим какая операция по приоритету следующая

            for (int prior = 0; prior <= exprOperators.Max(oper => oper.prior); prior++)
            {
                foreach (Operator currOper in exprOperators.Where(oper => oper.prior == prior))
                {  // коллекция exprOperators отсортирована по возрастанию позиций в выражении
                   // так, что одного приоритета будут идти по порядку, как в выражении

                    //currActionPosition = exprOperators.Where(oper => oper.prior == prior).DefaultIfEmpty(this.defaultNotFoundOperator).Min(oper => oper.firstPos);

                    // В некоторых случаях считать дальше нельзя. Так было с ф+2-12: операция ф+2 была невыполнима.
                    // это будет отработано во внешней процедуре - результат не изменится.
                    return GetActionByOperator(expression, currOper.firstPos);
                }
            }

            throw new InvalidOperationException($"[GetActionExpr] в выражении {expression} найдены операции, но первую получить не смогли, ни по какому приоритету!");
        }

        private string CalcFunction(string funcExpr)
        {
            // TODO: по мере необходимости реализовать подсчет различных функций (триг. и др.)
            return funcExpr;   //    !!!
        }
        /// <summary>
        /// Пока не реализована
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string GetFuncExpression(string expression)
        {
            return null;    //   !!!
        }
        /// <summary>
        /// Выделяет подстроку - выражение первых скобок, вместе соскобками (!).
        /// Принцип: найти первую открывающую скобку и от нее будет просто найти закрывающую.
        /// Правильная открывающая когда перед ней: нач.строки/знак арифм.операции/откр.скобка
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetFirstBracketExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;            //  !!!
            }

            int startPos = expression.IndexOf('(');
            if (startPos == -1)
            {
                return null;             //  !!!
            }

            int braPos = -1;
            int cketPos = -1;
            int braCount = 0;    // Что бы правильно определить 
            int cketCount = 0;   // соответствующую закрывающую скобку

            for (int i = startPos; i < expression.Length; i++)
            {
                if (expression[i] == '(' && braPos == -1)
                {
                    if (i == 0
                        // т.е. если сразу перед скобкой стоит один наших операторов
                        || GetOperator(expression, i - 1).firstPos > -1
                        || expression[i - 1] == '(')
                    {   // нашли нужную открывающую скобку.
                        braPos = i;
                    }
                }
                //нужно теперь отыскать правильную закрывающую скобку
                if (braPos > -1)
                {   // теперь считаем
                    if (expression[i] == '(')
                    { braCount++; }

                    if (expression[i] == ')')
                    { cketCount++; }

                    if (cketCount == braCount)
                    {
                        cketPos = i;
                        return expression.Substring(braPos, cketPos - braPos + 1);       // !!!
                    }
                }
            }

            if (braPos > -1)
            {
                throw new InvalidOperationException($"Не найдена закрывающая скобка в подвыражении '{expression}'  выражения '{this.Expression}'");
            }
            return null;         //   !!!
        }
        /// <summary>
        /// Осуществляет некоторые проверки входного выражения
        /// </summary>
        private void ValidateExpression(string expr)
        {
            if (string.IsNullOrEmpty(expr))
            {
                return;         //    !!!
            }

            // Поверхностная проверка на скобки
            int braCount = 0;
            int cketCount = 0;
            // Пробежимся
            for (int i=0; i < expr.Length; i++)
            {
                if (expr[i] == '(')
                {
                    braCount++;
                }
                if (expr[i] == ')')
                {
                    cketCount++;
                }
            }
            // Смотрим
            if (braCount != cketCount)
            {
                throw new InvalidOperationException($"Не хватает '(' или ')' в выражении: '{expr}'");
            }

            // Другие проверки
        }

    }
}
