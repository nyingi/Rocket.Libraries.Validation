namespace Rocket.Libraries.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rocket.Libraries.Validation.Exceptions;
    using Rocket.Libraries.Validation.Models;

    public class DataValidator
    {
        private List<RuleDescriptor> _expectedStates = new List<RuleDescriptor>();

        /// <summary>
        /// Queues a function that should be considered a validation failure if it evaluates to true
        /// Also associates an error message with the validation and finally states whether or not
        /// failure of this condition should prevent further validation of subsequent rules
        /// </summary>
        /// <param name="failureCondition">Func that when it evaluates to true, validation is considered to have failed</param>
        /// <param name="messageOnFailure">Message to be given when validation fails</param>
        /// <param name="terminateValidationOnFailure">Should failing of this rule cause further validation to be canceled?</param>
        /// <returns>Instance of self to allow chaining of multiple calls to data validator</returns>
        [Obsolete("If passed long-lived variables in the closure, this method may result in unnecessary usage of memory dependant on the lifetime of captured variables")]
        public virtual DataValidator AddFailureCondition(Func<bool> failureCondition, string messageOnFailure, bool terminateValidationOnFailure)
        {
            return AddFailureCondition(failureCondition(), messageOnFailure, terminateValidationOnFailure);
        }

        public virtual DataValidator AddFailureCondition<TValue>(TValue value, FailureConditionDescriptor<TValue> failureConditionDescriptor)
        {
            return AddFailureCondition(value, failureConditionDescriptor.FailureCondition, failureConditionDescriptor.MessageOnFailure, failureConditionDescriptor.TerminateValidationOnFailure);
        }

        public virtual DataValidator AddFailureCondition<TValue>(TValue value, Func<TValue, bool> failureCondition, string messageOnFailure, bool terminateValidationOnFailure)
        {
            _expectedStates.Add(new RuleDescriptor
            {
                MessageOnFailure = messageOnFailure,
                RuleFailed = failureCondition(value),
                TerminateValidationOnFailure = terminateValidationOnFailure,
            });
            return this;
        }

        public virtual DataValidator AddFailureCondition(bool ruleFailed, string messageOnFailure, bool terminateValidationOnFailure)
        {
            _expectedStates.Add(new RuleDescriptor
            {
                MessageOnFailure = messageOnFailure,
                RuleFailed = ruleFailed,
                TerminateValidationOnFailure = terminateValidationOnFailure,
            });
            return this;
        }

        /// <summary>
        /// Queues a function that should be considered a validation failure if it evaluates to true
        /// Also associates an error message with the validation and finally states whether or not
        /// failure of this condition should prevent further validation of subsequent rules
        /// </summary>
        /// <param name="failureCondition">Func that when it evaluates to true, validation is considered to have failed</param>
        /// <param name="messageOnFailure">Message to be given when validation fails</param>
        /// <param name="terminateValidationOnFailure">Should failing of this rule cause further validation to be canceled?</param>
        /// <returns>Instance of self to allow chaining of multiple calls to data validator</returns>
        public virtual DataValidator AddAsyncFailureCondition(Func<Task<bool>> failureCondition, string messageOnFailure, bool terminateValidationOnFailure)
        {
            _expectedStates.Add(new RuleDescriptor
            {
                MessageOnFailure = messageOnFailure,
                RuleFailed = AsyncHelpers.RunSync<bool>(() => failureCondition()),
                TerminateValidationOnFailure = terminateValidationOnFailure,
            });
            return this;
        }

        /// <summary>
        /// Creates a rule and evaluates it immediately. An exception is immediately thrown if the failure condition is met
        /// </summary>
        /// <param name="failureCondition">Func that when it evaluates to true, validation is considered to have failed</param>
        /// <param name="messageOnFailure">Message to be given when validation fails</param>
        /// <returns>Instance of self to allow chaining of multiple calls to data validator</returns>
        [Obsolete("If passed long-lived variables in the closure, this method may result in unnecessary usage of memory dependant on the lifetime of captured variables")]
        public virtual DataValidator EvaluateImmediate(Func<bool> failureCondition, string messageOnFailure)
        {
            new DataValidator()
                .AddFailureCondition(failureCondition, messageOnFailure, true)
                .ThrowExceptionOnInvalidRules();
            return this;
        }

        public virtual DataValidator EvaluateImmediate<TValue>(TValue value, Func<TValue, bool> failureCondition, string messageOnFailure)
        {
            return EvaluateImmediate(value, failureCondition, messageOnFailure);
        }

        public virtual DataValidator EvaluateImmediate(bool RuleFailed, string messageOnFailure)
        {
            new DataValidator()
                .AddFailureCondition(RuleFailed, messageOnFailure, true)
                .ThrowExceptionOnInvalidRules();
            return this;
        }

        /// <summary>
        /// Runs the failure rules and returns any error messages but does not trigger an exception
        /// </summary>
        /// <returns>List of string contains any error messages returned</returns>
        public virtual List<string> GetInvalidStateMessages()
        {
            var messages = new List<string>();
            foreach (var rule in _expectedStates)
            {
                var errorMessage = GetInvalidRuleMessage(rule);
                AppendErrorMessageIfNotEmpty(messages, errorMessage);
                if (ValidationShouldTerminate(rule.TerminateValidationOnFailure, errorMessage))
                {
                    break;
                }
            }

            return messages;
        }

        public static void Throw(string errorMessage)
        {
            new DataValidator().EvaluateImmediate(() => true, errorMessage);
        }

        public static DataValidator Evaluate(Func<bool> failureCondition, string messageOnFailure)
        {
            return new DataValidator().EvaluateImmediate(failureCondition, messageOnFailure);
        }

        /// <summary>
        /// Evaluates all the failure conditions and throws an exception with a collection of all the failure messages (if any)
        /// discovered.
        /// </summary>
        public virtual void ThrowExceptionOnInvalidRules()
        {
            var errorMessages = GetInvalidStateMessages();
            var shouldThrow = errorMessages.Count > 0;
            if (shouldThrow)
            {
                var errors = errorMessages.Select(a => new Error { Message = a }).ToList();
                ClearGlobalErrors();
                var exception = new IncorrectDataException();
                exception.Errors.AddRange(errors);
                throw exception;
            }
        }

        private void ClearGlobalErrors()
        {
            _expectedStates = new List<RuleDescriptor>();
        }

        private bool ValidationShouldTerminate(bool terminateOnFailure, string errorMessage)
        {
            return terminateOnFailure && !string.IsNullOrEmpty(errorMessage);
        }

        private void AppendErrorMessageIfNotEmpty(List<string> messages, string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                return;
            }

            messages.Add(errorMessage);
        }

        private string GetInvalidRuleMessage(RuleDescriptor rule)
        {
            if (rule.RuleFailed)
            {
                return rule.MessageOnFailure;
            }

            return string.Empty;
        }
    }
}