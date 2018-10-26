using System;

namespace Rocket.Libraries.Validation.Services
{
    internal class RuleDescriptor
    {
        public Func<bool> FailureCondition { get; set; }
        public string MessageOnFailure { get; set; }
        public bool TerminateValidationOnFailure { get; set; }
    }
}