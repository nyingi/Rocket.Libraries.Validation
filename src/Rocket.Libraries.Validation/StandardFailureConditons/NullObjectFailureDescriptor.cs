using Rocket.Libraries.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation.StandardFailureConditons
{
    public class NullObjectDescriptor : FailureConditionDescriptor<object>
    {
        public override Func<object, bool> FailureCondition => (obj) => obj == null;

        public override string MessageOnFailure { get; set; } = "Item is null";

        public override bool TerminateValidationOnFailure { get; set; } = true;
    }
}