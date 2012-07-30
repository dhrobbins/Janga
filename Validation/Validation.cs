using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Janga.Validation
{
    public class Validation<T>
    {
        public T Value { get; set; }
        public string ArgName { get; set; }
        public bool ProceedOnFailure { get; set; }
        public bool IsValid { get; set; }
        public IList<string> ErrorMessages { get; set; }        

        public Validation(T value, string argName)
        {
            this.ArgName = argName;
            this.Value = value;
            this.ProceedOnFailure = false;

            //  Set to valid in order to allow for different chaining of validations.
            //  Each validator will set value relative to failure or success.
            this.IsValid = true;
            this.ErrorMessages = new List<string>();
        } 

        public Validation(T value, string argName, bool proceedOnFailure)
        {
            this.ArgName = argName;
            this.Value = value;
            this.ProceedOnFailure = proceedOnFailure;
            
            //  Set to valid in order to allow for different chaining of validations.
            //  Each validator will set value relative to failure or success.
            this.IsValid = true;
            this.ErrorMessages = new List<string>();
        }
    }
}
