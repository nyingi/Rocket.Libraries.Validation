namespace Rocket.Libraries.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rocket.Libraries.Validation.Exceptions;
    using Rocket.Libraries.Validation.Models;
    using Rocket.Libraries.Validation.Extensions;
    using System.Collections.Immutable;

    public class DataValidator : IDisposable
    {
        private readonly List<RuleDescriptor> _expectedStates = new List<RuleDescriptor>();

        /// <summary>
        ///Takes in a boolean value which when equal to true, indicates that a rule failed. If the rule did fail, then the error message <paramref name="messageOnFailure"/> is added
        ///to list of errors to be returned when <see cref="ThrowExceptionOnInvalidRules"/> is called.
        /// </summary>
        /// <param name="ruleFailed">The function that is to be run as a test condition</param>
        /// <param name="messageOnFailure">The message to be  added to error list if <paramref name="ruleFailed"/> is true</param>
        /// <param name="terminateValidationOnFailure">Should failing of this rule cause further validation to be canceled?</param>
        /// <returns>Instance of self to allow chaining of multiple calls to data validator</returns>
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
        /// Takes in the a boolean value and checks if it is true.
        /// </summary>
        /// <param name="ruleFailed">The value to be evaluated</param>
        /// <param name="messageOnFailure">The message specific to this rule that'll be added the resultant error list if rule fails</param>
        public virtual void EvaluateImmediate(bool ruleFailed, string messageOnFailure)
        {
            EvaluateImmediate<object>(ruleFailed, messageOnFailure);
        }

        /// <summary>
        /// Takes in the a boolean value and checks if it is true.
        /// </summary>
        /// <param name="ruleFailed">The value to be evaluated</param>
        /// <param name="messageOnFailure">The message specific to this rule that'll be added the resultant error list if rule fails</param>
        /// <typeparam name="TResponse">Type to be returned. Useful for when you wish to use this method as the last line in a different method and you can use it to return a type that matches the caller method type</typeparam>
        /// <returns>The default value of type <typeparamref name="TResponse"/></returns>
        public virtual TResponse EvaluateImmediate<TResponse>(bool ruleFailed, string messageOnFailure)
        {
            using (var dataValidator = new DataValidator())
            {
                return dataValidator.AddFailureCondition(ruleFailed, messageOnFailure, true)
                .ThrowExceptionOnInvalidRules<TResponse>();
            }
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

        /// <summary>
        /// Forces an exception to be thrown. Takes in an error message to be added to the exception's error list
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public static void Throw(string errorMessage)
        {
            Throw(errorMessage);
        }

        /// <summary>
        /// Forces an exception to be thrown. Takes in an error message to be added to the exception's error list
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <typeparam name="TResponse">Type to be returned. Useful for when you wish to use this method as the last line in a different method and you can use it to return a type that matches the caller method type</typeparam>
        /// <returns>The default value of type <typeparamref name="TResponse"/></returns>
        public static TResponse Throw<TResponse>(string errorMessage)
        {
            using (var dataValidator = new DataValidator())
            {
                return dataValidator.EvaluateImmediate<TResponse>(true, errorMessage);
            }
        }

        /// <summary>
        /// Evaluates all the failure conditions and throws an exception with a collection of all the failure messages (if any)
        /// discovered.
        /// </summary>
        public virtual void ThrowExceptionOnInvalidRules()
        {
            ThrowExceptionOnInvalidRules<object>();
        }

        /// <summary>
        /// Evaluates all the failure conditions and throws an exception with a collection of all the failure messages (if any)
        /// discovered.
        /// </summary>
        /// <typeparam name="TResponse">Type to be returned. Useful for when you wish to use this method as the last line in a different method and you can use it to return a type that matches the caller method type</typeparam>
        /// <returns>The default value of type <typeparamref name="TResponse"/></returns>
        public virtual TResponse ThrowExceptionOnInvalidRules<TResponse>()
        {
            var errorMessages = GetInvalidStateMessages();
            var shouldThrow = errorMessages.Count > 0;
            if (shouldThrow)
            {
                var errors = errorMessages.Select(a => new Error(a)).ToList();
                errorMessages.MakeListEmpty((errorString) => { });
                ClearGlobalErrors();
                var exception = new FailedValidationException(errors);
                throw exception;
            }
            else
            {
                return default;
            }
        }

        private void ClearGlobalErrors()
        {
            _expectedStates.MakeListEmpty(a => a = null);
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

        public void Dispose()
        {
            ClearGlobalErrors();
        }
    }
}