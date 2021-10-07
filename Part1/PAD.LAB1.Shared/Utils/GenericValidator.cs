using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Shared.Utils
{
    // Add generic validator in factories
    public static class GenericValidator
    {
        public static void CheckIfEmptyString(string variableContent, string variableName)
        {
            if (string.IsNullOrEmpty(variableContent))
            {
                throw new Exception($"{ variableName } is null or empty!");
            }
        }
    }
}
