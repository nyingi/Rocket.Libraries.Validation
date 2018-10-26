﻿using Rocket.Libraries.Validation.Exceptions;
using Rocket.Libraries.Validation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocket.Libraries.Validation.Services
{
    public class DataValidator
    {
        private List<RuleDescriptor> _rules = new List<RuleDescriptor>();
        private List<RuleDescriptor> _expectedStates = new List<RuleDescriptor>();

        /// <summary>
        /// Queues a function that should be considered a validation failure if it evaluates to true
        /// Also associates an error message with the validation and finally states whether or not
        /// failure of this condition should prevent further validation of subsequent rules
        /// </summary>
        /// <param name="failureCondition">Func that when it evaluates to true, validation is considered to have failed</param>
        /// <param name="messageOnFailure">Message to be given when validation fails</param>
        /// <param name="terminateValidationOnFailure">Should failing of this rule cause further validation to be canceled?</param>
        /// <returns></returns>
        public virtual DataValidator AddFailureCondition(Func<bool> failureCondition, string messageOnFailure, bool terminateValidationOnFailure)
        {
            _expectedStates.Add(new RuleDescriptor
            {
                MessageOnFailure = messageOnFailure,
                FailureCondition = failureCondition,
                TerminateValidationOnFailure = terminateValidationOnFailure
            });
            return this;
        }

        /// <summary>
        /// Creates a rule and evaluates it immediately. An exception is immediately thrown if the failure condition is met
        /// </summary>
        /// <param name="failureCondition">Func that when it evaluates to true, validation is considered to have failed</param>
        /// <param name="messageOnFailure">Message to be given when validation fails</param>
        /// <returns></returns>
        public virtual DataValidator EvaluateImmediate(Func<bool> failureCondition, string messageOnFailure)
        {
            new DataValidator()
                .AddFailureCondition(failureCondition, messageOnFailure, true)
                .ThrowExceptionOnInvalidRules();
            return this;
        }

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

        public virtual void ThrowExceptionOnInvalidRules()
        {
            var errorMessages = GetInvalidStateMessages();
            var shouldThrow = errorMessages.Count > 0;
            if (shouldThrow)
            {
                var errors = errorMessages.Select(a => new Error { Message = a }).ToList();
                ClearGlobalErrors();
                throw new IncorrectDataException
                {
                    Errors = errors
                };
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

        [Obsolete]
        private string GetErrorMessageOnRuleFailure(RuleDescriptor rule)
        {
            if (rule.FailureCondition() == false)
            {
                return rule.MessageOnFailure;
            }
            return string.Empty;
        }

        private string GetInvalidRuleMessage(RuleDescriptor rule)
        {
            if (rule.FailureCondition())
            {
                return rule.MessageOnFailure;
            }
            return string.Empty;
        }
    }
}