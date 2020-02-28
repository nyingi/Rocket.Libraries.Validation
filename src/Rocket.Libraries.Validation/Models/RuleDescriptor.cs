namespace Rocket.Libraries.Validation.Models
{
    internal class RuleDescriptor
    {
        //public Func<bool> FailureCondition { get; set; }

        public bool RuleFailed { get; set; }

        public string MessageOnFailure { get; set; }

        public bool TerminateValidationOnFailure { get; set; }
    }
}