using System.Collections.Generic;

namespace Rocket.Libraries.Validation.FailureDescriptors
{
    public static class ObjectFailureConditions
    {
        public static bool IsDefault<TValue>(TValue value)
        {
            return EqualityComparer<TValue>.Default.Equals(default);
        }
    }
}