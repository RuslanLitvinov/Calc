using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcClasses
{
    /// <summary>
    /// Сведения о математических операциях (обозначение, выполнение) в текстовой строке.
    /// TODO: реализовать выполенение тригонометрический и прочих самодельных функций так же через метод 
    ///    ExecuteAction - и представление из через объект action
    /// </summary>
    public class OperatorString : IOperatorString
    {
        /// <summary>
        /// '+', '-', '*', '/', и др. определяемые разработчиком
        /// </summary>
        private List<Operator> operators;
        public OperatorString()
        {
            this.InitOperators();
        }
        public List<Operator> Operators
        {
            get
            {
                return operators; 
            }
        }
        public static Operator DefaultNotFoundOperator()
        {
            return new Operator() { position = -1, prior = -1 };
        }
        public virtual void InitOperators()
        {
            this.operators = new List<Operator>
            {
                new Operator() { type = "*", prior = 0 },
                new Operator() { type = "/", prior = 0 },
                new Operator() { type = "+", prior = 1 },
                new Operator() { type = "-", prior = 1 }
              // ...
            };
        }
        public virtual string CalcFunction(string funcExpr)
        {
            // TODO: по мере необходимости реализовать подсчет различных функций (триг. и др.)
            return funcExpr;   //    !!!
        }
        public virtual string ExecuteAction(Action action)
        {
            if (action == null)
            {
                return null;              // !!!
            }
            if (string.IsNullOrEmpty(action.Type))
            {
                return action.Expression;        // !!!
            }

            action.ParseOperands();
            decimal? resultDec = null;

            switch (action.Type)
            {
                case "*":    // не через вызов соответствующего метода (которые можно переопределить) для простоты
                    if (ValidateTwoOperands(action))
                    {
                        resultDec = action.operant1Dec * action.operant2Dec;
                    }
                    break;
                case "/":
                    if (ValidateTwoOperands(action))
                    {
                        resultDec = action.operant1Dec / action.operant2Dec;
                    }

                    break;
                case "+":
                    if (ValidateTwoOperands(action))
                    {
                        resultDec = action.operant1Dec + action.operant2Dec;
                    }
                    break;
                case "-":
                    if (ValidateTwoOperands(action))
                    {
                        resultDec = action.operant1Dec - action.operant2Dec;
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Для операции '{action.Type}' не реализовано исполнение.");

            }

            if (resultDec != null)
            {
                return resultDec.ToString();       // !!!
            }
            else
            {
                return action.Expression;         // !!!
            }

        }
        public bool ValidateTwoOperands(Action action)
        {
            if (action.operant1Dec == null || action.operant2Dec == null)
            {
                Console.WriteLine($"Для операции '{action.Type}' нет одного из операндов. Выражение <{action.Expression}> будет результатом.");
                return false;     // !!!
            }

            return true;    // !!!
        }

    }
}
