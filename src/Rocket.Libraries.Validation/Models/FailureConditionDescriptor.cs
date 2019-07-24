using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation.Models
{
    public abstract class FailureConditionDescriptor<TValue>
    {
        public abstract Func<TValue, bool> FailureCondition { get; }

        public abstract string MessageOnFailure { get; set; }

        public virtual bool TerminateValidationOnFailure { get; set; } = false;
    }
}