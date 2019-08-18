using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcClasses
{
    /// <summary>
    /// Сведения о математических операциях (обозначение, выполнение) в текстовой строке.
    /// </summary>
    public interface IOperatorString
    {
        List<Operator> Operators { get; }
        void InitOperators();
        string CalcFunction(string funcExpr);
        string ExecuteAction(Action action);
        bool ValidateTwoOperands(Action action);

    }
}
