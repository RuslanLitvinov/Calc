using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcClasses
{
    /// <summary>
    /// Реализует начальный функционал мат.выражения в строке.
    /// </summary>
    class StringExpression
    {
        string expression;
        public string Expression
        {
            get { return this.expression; }
            set { this.expression = value.Replace(" ", ""); }
        }
        //Array operators = new[] { '+', '-', '*', '/' };
        /// <summary>
        /// '+', '-', '*', '/', и др. определяемые разработчиком
        /// </summary>
        List<Operator> operators;
        Operator defaultNotFoundOperator;
        public StringExpression()
        {
            this.InitOperators();
        }
        private void InitOperators()
        {
            this.defaultNotFoundOperator = new Operator() { firstPos = -1, lastPos = -1, prior = -1 };

            this.operators = new List<Operator>();
            this.operators.Add(new Operator() { type = "*", prior = 0 });
            this.operators.Add(new Operator() { type = "/", prior = 0 });
            this.operators.Add(new Operator() { type = "+", prior = 1 });
            this.operators.Add(new Operator() { type = "-", prior = 1 });
            // ...
        }
        /// <summary>
        /// Возвращает первое появление в выражении expression каждого оператора (из this.operators), какие есть в выражении.
        /// Коллекция отсортирована по возврастанию позиции операторов в выражении.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// Null - если операторы не найдены
        /// </returns>
        public List<Operator> GetFirstOperatorsSortPositionAsc(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;               // !!!
            }

            var operatorsFirst = this.operators.Where(o => expression.ToUpper().IndexOf(o.type.ToUpper()) > -1).ToList();
            if (operatorsFirst == null || operatorsFirst.Count == 0)
            {
                return null;        // !!!
            }

            var oper = new Operator();
            for (int i = 0; i < operatorsFirst.Count; i++)
            {
                oper = operatorsFirst[i];
                oper.firstPos = expression.ToUpper().IndexOf(operatorsFirst[i].type.ToUpper());
                operatorsFirst[i] = oper;
            }

            operatorsFirst = operatorsFirst.OrderBy(o => o.firstPos).ToList();

            return operatorsFirst;     // !!!
        }
        /// <summary>
        /// Возвращает последнее появление в выражении expression каждого оператора (из this.operators), какие есть в выражении.
        /// Коллекция отсортирована по убыванию позиции операторов в выражении.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// Null - если операторы не найдены
        /// </returns>
        public List<Operator> GetLastOperatorsSortPositionDesc(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;               // !!!
            }

            var operatorsLast = this.operators.Where(o => expression.ToUpper().LastIndexOf(o.type.ToUpper()) > -1).ToList();
            if (operatorsLast == null || operatorsLast.Count == 0)
            {
                return null;        // !!!
            }

            var oper = new Operator();
            for (int i = 0; i < operatorsLast.Count; i++)
            {
                oper = operatorsLast[i];
                oper.lastPos = expression.ToUpper().LastIndexOf(operatorsLast[i].type.ToUpper());
                operatorsLast[i] = oper;
            }
            operatorsLast = operatorsLast.OrderByDescending(o => o.lastPos).ToList();

            return operatorsLast;     // !!!
        }
        /// <summary>
        ///  Вовзращает оператор из позиции operPos выражения.
        ///  Оператор может быть из нескольких символов.
        ///  Мы можем указать на любую позицию занятую оператором и функция должна узнать его.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operPos"></param>
        /// <returns></returns>
        public Operator GetOperator(string expression, int operPos)
        {
            // попробуем по простому- если оператор начинается с этой позиции
            var FindOperatots = this.operators.Where(o => expression.ToUpper().IndexOf(o.type.ToUpper(), operPos) == operPos).ToList();
            if (FindOperatots == null || FindOperatots.Count == 0)
            {
                // Todo: реализовать узнавание оператора, если мы указали на позицию занятую оператором, но не первую
                var defaultNotFoundOperator = this.defaultNotFoundOperator;
                return defaultNotFoundOperator;          // !!!
            }
            return FindOperatots[0];    // !!!
        }
        public Action GetActionByOperator(string expression, int operPos)
        {
            if (operPos == -1 || string.IsNullOrEmpty(expression))
            {
                return null;        //   !!!       
            }
            var action = new Action(expression);   // Операция, которая будет возвращена
            int actionBeginPosition = -1;
            int actionEndPosition = -1;

            // Какая операция стоит в позиции operPos выражения
            var currentOper = GetOperator(expression, operPos);
            if (currentOper.firstPos == -1)
            {
                throw new InvalidOperationException($"Не распознана операция в выражении '{expression}' в позиции '{operPos}'");
            }
            action.type = currentOper.type;

            // Левый операнд
            if (operPos > 0)
            {   // Признак начала левого операнда: нач.выраж/другая операция. Когда выполняется GetAction скобок уже не должно быть.
                string leftExpression = expression.Substring(0, operPos);   //operPos ведет счет от 0, но число элементов без текущего нам подходит
                var operatorsLeftLast = GetLastOperatorsSortPositionDesc(leftExpression);
                if (operatorsLeftLast == null)
                {  // слева от операции не найдено других операция: операнд слева вся левая строка
                    action.operand1Str = leftExpression;
                    actionBeginPosition = 0;
                }
                else
                {
                    var leftLastOperator = operatorsLeftLast[0];
                    actionBeginPosition = leftLastOperator.lastPos + leftLastOperator.type.Length;
                    //lastPos - в терминах короткого левого выражения
                    action.operand1Str = leftExpression.Substring(actionBeginPosition);  // и до конца строки leftExpression
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
                var operatorsRightFirst = GetFirstOperatorsSortPositionAsc(rightExpression);
                if (operatorsRightFirst == null)
                {  // операторов справа больше не найдено - все выражение справо будет вторым операндом
                    action.operand2Str = rightExpression;
                    actionEndPosition = expression.Length - 1;
                }
                else
                {
                    var rightFirstOperator = operatorsRightFirst[0];
                    // rightFirstOperator.firstPos - в терминах короткой правой стороны от знака операции
                    action.operand2Str = rightExpression.Substring(0, rightFirstOperator.firstPos);  // позиция будет на одну меньше чем порядковый номер - как раз взять без него
                    actionEndPosition = operPos + currentOper.type.Length + rightFirstOperator.firstPos - 1;
                }
            }
            else
            {
                actionEndPosition = expression.Length - 1;
            }
            // Console.WriteLine($"actionBeginPosition = {actionBeginPosition}, actionEndPosition={actionEndPosition} operPos={operPos}");
            // выражение дожно быть в точночти таким, как в строке и в том же регистре, что бы узнать и подменить
            action.expression = expression.Substring(actionBeginPosition, actionEndPosition - actionBeginPosition + 1);
            return action;        //   !!!
        }

    }
}
