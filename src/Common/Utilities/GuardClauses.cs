using System;

namespace TariffComparison.Common.Utilities
{
    public class GuardClauses
    {
        public static void IsNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue is null)
                throw new ArgumentNullException(nameof(argumentValue));
        }
    }
}
