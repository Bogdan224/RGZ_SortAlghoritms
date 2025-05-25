using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGZ_SortAlghoritms.Models.Converters
{
    public static class ConverterFromString<T>
    {
        public static T ConvertFromString(string value)
        {
            var conversionMethod = typeof(T).GetMethod("op_Implicit", new[] { typeof(string) });
            if (conversionMethod != null)
            {
                return (T)conversionMethod.Invoke(null, new object[] { value });
            }
            throw new InvalidOperationException($"Тип {typeof(T)} не поддерживает неявное преобразование из string.");
        }
    }
}
