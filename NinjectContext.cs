using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcClasses
{
    /// <summary>
    /// Хранение ядра для всего проекта
    /// </summary>
    public static class NinjectContext
    {
        public static IKernel Kernel { get; private set; }
        /// <summary>
        /// Для получения один раз ядра на старт программы.
        /// </summary>
        /// <returns></returns>
        public static void SetUp()
        {
            Kernel = new StandardKernel(new NinjectConfig());
        }
    }
}
