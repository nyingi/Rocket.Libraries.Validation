namespace Rocket.Libraries.Validation.FailureDescriptors
{
    public static class ObjectFailureConditions
    {
        public static bool IsDefault<TValue>(TValue value)
        {
            return value == default;
        }
    }
}