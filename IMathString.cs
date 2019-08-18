using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcClasses
{
    /// <summary>
    /// Методы персинга текстовой строки.
    /// </summary>
    public interface IMathString
    {
        /// <summary>
        /// Возвращает список операторов, которые присутствуют в выражении
        /// </summary>
        List<Operator> GetOperators(string expression, List<Operator> allOperators);
        int OperatorIndexOf(string expression, string operatorType);
        int OperatorLastIndexOf(string expression, string operatorType);
        Operator GetOperator(string expression, int operPos, List<Operator> allOperators);
        Action GetActionByOperator(string expression, int operPos, List<Operator> allOperators);
        /// <summary>
        /// Возвращает присутствующие (из allOperators) операторы, их первое появление.
        /// Коллекция отсортирована по возврастанию позиции операторов.
        /// </summary>
        List<Operator> GetFirstOperatorsSortPositionAsc(string expression, List<Operator> allOperators);
        /// <summary>
        /// Возвращает присутствующие (из allOperators) операторы, их последнее появление.
        /// Коллекция отсортирована по убыванию позиции операторов в выражении.
        /// </summary>
        List<Operator> GetLastOperatorsSortPositionDesc(string expression, List<Operator> allOperators);
        /// <summary>
        /// Осуществляет некоторые проверки входного выражения
        /// </summary>
        void ValidateExpression(string expr);
        /// <summary>
        /// Выделяет подстроку - выражение первых скобок, вместе соскобками (!).
        /// Выражение скобки нужно возвращать со скобками и в точности в том виде каким оно стоит в
        /// исходном выражении. Это важно, что бы можно было узнать и подменить результатом от расчета
        /// Принцип: найти первую открывающую скобку и от нее будет просто найти закрывающую.
        /// Правильная открывающая когда перед ней: нач.строки/знак арифм.операции/откр.скобка
        /// </summary>
        string GetFirstBracketExpression(string expression, List<Operator> allOperators);
        /// <summary>
        string GetFuncExpression(string expression, List<Operator> allOperators);

    }
}
