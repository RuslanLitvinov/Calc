using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcClasses
{
    /// <summary>
    /// Персинг текстовой строки.
    /// Из знаний - только, что операнды операций ограничиваются другими операторами или границами строки (самих операторов не знает). 
    /// А так же, что чило открытых скобок должно быть равно числу закрытых. Все.
    /// </summary>
    public class MathString : IMathString
    {
        public  MathString()
        {
        }
        /// <summary>
        /// Возвращает список операторов, которые присутствуют в выражении
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="allOperators"></param>
        /// <returns></returns>
        public List<Operator> GetOperators(string expression, List<Operator> allOperators)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;               // !!!
            }

            var operatorsFirst = allOperators.Where(o => expression.ToUpper().IndexOf(o.type.ToUpper()) > -1).ToList();
            if (operatorsFirst == null || operatorsFirst.Count == 0)
            {
                return null;        // !!!
            }

            return operatorsFirst;          // !!!
        }
        public int OperatorIndexOf(string expression, string operatorType)
        {
            return expression.ToUpper().IndexOf(operatorType.ToUpper());
        }
        public int OperatorLastIndexOf(string expression, string operatorType)
        {
            return expression.ToUpper().LastIndexOf(operatorType.ToUpper());
        }
        public Operator GetOperator(string expression, int operPos, List<Operator> allOperators)
        {
            // попробуем по простому- если оператор начинается с этой позиции
            var FindOperatots = allOperators.Where(o => expression.ToUpper().IndexOf(o.type.ToUpper(), operPos) == operPos).ToList();
            if (FindOperatots == null || FindOperatots.Count == 0)
            {
                // Todo: реализовать узнавание оператора, если мы указали на позицию занятую оператором, но не первую
                return OperatorString.DefaultNotFoundOperator();          // !!!
            }
            var oper = FindOperatots[0];
            oper.position = operPos;

            return oper;    // !!!
        }
        /// <summary>
        /// На момент вызова GetActionByOperator скобой уже не должно быть.
        /// Т.е. сначала для выражения expression должен вызываться метод считающий и замещающий скобки.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operPos"></param>
        /// <param name="allOperators"></param>
        /// <returns></returns>
        public Action GetActionByOperator(string expression, int operPos, List<Operator> allOperators)
        {
            if (operPos == -1 || string.IsNullOrEmpty(expression))
            {
                return null;        //   !!!       
            }
            var action = new Action(expression);   // Операция, которая будет возвращена

            int actionBeginPosition = -1;
            int actionEndPosition = -1;

            // Какая операция стоит в позиции operPos выражения
            var currentOper = GetOperator(expression, operPos, allOperators );
            if (currentOper.position == -1)
            {
                throw new InvalidOperationException($"Не распознана операция в выражении '{expression}' в позиции '{operPos}'");
            }

            // Тип операции
            action.Type = currentOper.type;

            // Левый операнд
            if (operPos > 0)
            {   // Признак начала левого операнда: нач.выраж/другая операция. Когда выполняется GetActionByOperator скобок уже не должно быть.
                string leftExpression = expression.Substring(0, operPos);   //operPos ведет счет от 0, но число элементов без текущего нам подходит
                var operatorsLeftLast = GetLastOperatorsSortPositionDesc(leftExpression, allOperators);
                if (operatorsLeftLast == null)
                {  // слева от операции не найдено других операция: операнд слева вся левая строка
                    action.Operand1Str = leftExpression;
                    actionBeginPosition = 0;
                }
                else
                {
                    var leftLastOperator = operatorsLeftLast[0];
                    actionBeginPosition = leftLastOperator.position + leftLastOperator.type.Length;
                    //lastPos - в терминах короткого левого выражения
                    action.Operand1Str = leftExpression.Substring(actionBeginPosition);  // и до конца строки leftExpression
                }
            }
            else
            {
                actionBeginPosition = 0;
            }
            // Правый операнд
            if (operPos < expression.Length - 1)   // позиция символа будет на одну меньше его порядкового номера
            {
                String rightExpression = expression.Substring(operPos + currentOper.type.Length);
                var operatorsRightFirst = GetFirstOperatorsSortPositionAsc(rightExpression, allOperators);
                if (operatorsRightFirst == null)
                {  // операторов справа больше не найдено - все выражение справо будет вторым операндом
                    action.Operand2Str = rightExpression;
                    actionEndPosition = expression.Length - 1;
                }
                else
                {
                    var rightFirstOperator = operatorsRightFirst[0];
                    // rightFirstOperator.firstPos - в терминах короткой правой стороны от знака операции
                    action.Operand2Str = rightExpression.Substring(0, rightFirstOperator.position);  // позиция будет на одну меньше чем порядковый номер - как раз взять без него
                    actionEndPosition = operPos + currentOper.type.Length + rightFirstOperator.position - 1;
                }
            }
            else
            {
                actionEndPosition = expression.Length - 1;
            }
            // Console.WriteLine($"actionBeginPosition = {actionBeginPosition}, actionEndPosition={actionEndPosition} operPos={operPos}");
            // выражение дожно быть в точночти таким, как в строке и в том же регистре, что бы узнать и подменить
            action.Expression = expression.Substring(actionBeginPosition, actionEndPosition - actionBeginPosition + 1);
            return action;        //   !!!
        }
        /// <summary>
        /// Возвращает присутствующие (из allOperators) операторы, их первое появление.
        /// Коллекция отсортирована по возврастанию позиции операторов.
        /// </summary>
        /// <param name="expression"> Исходное выражение </param>
        /// <param name="allOperators"> Все известные операторы </param>
        /// <returns></returns>
        public List<Operator> GetFirstOperatorsSortPositionAsc(string expression, List<Operator> allOperators)
        {
            var operatorsFirst = GetOperators(expression, allOperators);
            if (operatorsFirst == null)
            {
                return null;        // !!!
            }

            // теперь необходимо получить позиции найденых операторов в выражении
            var oper = new Operator();
            for (int i = 0; i < operatorsFirst.Count; i++)
            {
                // позицию приходися присвоить через отдельный экземп. структуры, иначе у в коллекции только типы, менять нельзя
                oper = operatorsFirst[i];
                oper.position = OperatorIndexOf(expression, operatorsFirst[i].type);
                operatorsFirst[i] = oper;
            }

            operatorsFirst = operatorsFirst.OrderBy(o => o.position).ToList();

            return operatorsFirst;     // !!!
        }
        /// <summary>
        /// Возвращает присутствующие (из allOperators) операторы, их последнее появление.
        /// Коллекция отсортирована по убыванию позиции операторов в выражении.
        /// </summary>
        /// <param name="expression"> Исходное выражение </param>
        /// <param name="allOperators"> Все известные операторы </param>
        /// <returns></returns>
        public List<Operator> GetLastOperatorsSortPositionDesc(string expression, List<Operator> allOperators)
        {

            var operatorsLast = GetOperators(expression, allOperators);
            if (operatorsLast == null)
            {
                return null;        // !!!
            }

            // теперь необходимо получить позиции найденых операторов в выражении
            var oper = new Operator();
            for (int i = 0; i < operatorsLast.Count; i++)
            {
                // позицию приходися присвоить через отдельный экземп. структуры, иначе у в коллекции только типы, менять нельзя
                oper = operatorsLast[i];
                oper.position = OperatorLastIndexOf(expression, operatorsLast[i].type);
                operatorsLast[i] = oper;
            }
            operatorsLast = operatorsLast.OrderByDescending(o => o.position).ToList();

            return operatorsLast;     // !!!
        }
        /// <summary>
        /// Осуществляет некоторые проверки входного выражения
        /// </summary>
        public void ValidateExpression(string expr)
        {
            if (string.IsNullOrEmpty(expr))
            {
                return;         //    !!!
            }

            // Поверхностная проверка на скобки
            int braCount = 0;
            int cketCount = 0;
            // Пробежимся
            for (int i = 0; i < expr.Length; i++)
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
        /// <summary>
        /// Выделяет подстроку - выражение первых скобок, вместе соскобками (!).
        /// Выражение скобки нужно возвращать со скобками и в точности в том виде каким оно стоит в
        /// исходном выражении. Это важно, что бы можно было узнать и подменить результатом от расчета
        /// Принцип: найти первую открывающую скобку и от нее будет просто найти закрывающую.
        /// Правильная открывающая когда перед ней: нач.строки/знак арифм.операции/откр.скобка
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetFirstBracketExpression(string expression, List<Operator> allOperators)
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
                        || GetOperator(expression, i - 1, allOperators).position > -1
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
                throw new InvalidOperationException($"Не найдена закрывающая скобка в выражении <{expression}>");
            }
            return null;         //   !!!
        }
        /// <summary>
        /// Пока не реализована
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetFuncExpression(string expression, List<Operator> allOperators)
        {
            return null;    //   !!!
        }

    }
}
