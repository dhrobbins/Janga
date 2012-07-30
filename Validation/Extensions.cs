using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Janga.Validation
{    
    public static class Validation
    {
        public static Validation<T> Enforce<T>(this T item, string argName, bool proceedOnFailure)
        {
            return new Validation<T>(item, argName, proceedOnFailure);
        }

        public static Validation<T> Enforce<T>(this T item, string argName)
        {
            return new Validation<T>(item, argName);
        }
    }
}
