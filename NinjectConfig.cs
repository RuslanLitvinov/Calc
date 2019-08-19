using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CalcClasses
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IMathString>().To<MathString>();
            Bind<IOperatorString>().To<OperatorString>();
            Bind<CalculatorString>().ToSelf();
        }
    }
}
