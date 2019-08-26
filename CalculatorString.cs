using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcClasses
{
    /// <summary>
    /// Начальный функционал калькулятора из текстовой строки со скобками.
    /// Учитывает приоритет операций, например выражение 3*(2+3)/2-3*2 - должно правильно вычисляться.
    /// Для добавления новой операции над операндом (или 2-мя операндами) в выражение expression
    /// нужно унаследоваться от класса OperatorString 
    /// и добавить функционал в InitOperators, CalcFunction, ExecuteAction класса наследника,
    /// сконфигурировав NinjectConfig соответственно
    /// </summary>
    public class CalculatorString
    {
        private readonly IMathString mathStr;
        private readonly IOperatorString stringOperators;
        public CalculatorString(IMathString oMathStr, IOperatorString oStringOperators)
        {
            mathStr = oMathStr;
            stringOperators = oStringOperators;
        }

        /// <summary>
        /// Вычисляет значение выражения в строковом виде
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        ///  Строковое значение, а не decimal потому, что не все может быть реализовано: 
        ///    функция или непределенная переменная - вернет как задали
        /// </returns>
        public string Calculation(string expression)
        {
            mathStr.ValidateExpression(expression);

            // Вычисление скобки
            string bracketExpr = mathStr.GetFirstBracketExpression(expression, stringOperators.Operators);
            if (!string.IsNullOrEmpty(bracketExpr))
            {
                // Убирая скобки, помним, что могут быть еще и вложенные скобки и их нужно оставить
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
            string funcExpr = mathStr.GetFuncExpression(expression, stringOperators.Operators);
            if (!string.IsNullOrEmpty(funcExpr))
            {
                return Calculation(expression.Replace(funcExpr, stringOperators.CalcFunction(funcExpr)));     // !!!
            }

            // Операции с двумя операндами ( например простые арифметические операции)
            var action = GetNextAction(expression, stringOperators.Operators);
            if (action != null)
            {
                string actionResult = stringOperators.ExecuteAction(action);
                if (action.Expression.Trim().ToLower() == actionResult.Trim().ToLower())
                {  // то есть после очередной операции ничего не изменилось - завершение.
                    return expression;   // !!!
                }

                return Calculation(expression.Replace(action.Expression, actionResult) );   // !!!
            }

            // Остается число или переменная или выражение содержащее неопределенную переменную - просто вернуть
            return expression;   // !!!
        }
        /// <summary>
        /// Получает следующую операцию из выражения expression по приоритетам операций.
        /// Разработчик сам должен контролировать, чтобы к моменту вызова метода не осталось 
        /// непосчитанных скобок и функций
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Action GetNextAction(string expression, List<Operator> allOperators)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;        //         !!!
            }
            var exprOperators = mathStr.GetFirstOperatorsSortPositionAsc(expression, allOperators);
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

                    //currActionPosition = exprOperators.Where(oper => oper.prior == prior).DefaultIfEmpty(OperatorString.DefaultNotFoundOperator()).Min(oper => oper.firstPos);

                    // В некоторых случаях считать дальше нельзя. Так было с ф+2-12: операция ф+2 была невыполнима.
                    // это будет отработано во внешней процедуре - результат не изменится и вычисления прекратятся.
                    return mathStr.GetActionByOperator(expression, currOper.position, allOperators);
                }
            }

            throw new InvalidOperationException($"[GetNextAction] в выражении {expression} найдены операции, но первую получить не смогли, ни по какому приоритету!");
        }


    }
}
