using Rocket.Libraries.Validation.Exceptions;
using Rocket.Libraries.Validation.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rocket.Libraries.ValidationTests
{
    public class DataValidatorTests
    {
        [Fact]
        public void SynchronousValidationWorks()
        {
            var validator = new DataValidator()
                .AddFailureCondition(() => true, "One is not greater than two", false);

            Assert.Throws<IncorrectDataException>(() => validator.ThrowExceptionOnInvalidRules());
        }

        [Fact]
        public void AsynchronousValidationWorks()
        {
            var throwingValidator = new DataValidator()
                .AddAsyncFailureCondition(() => Task.Run(() => true), "One is not greater than two", false);

            var nonThrowingValidator = new DataValidator()
                .AddAsyncFailureCondition(() => Task.Run(() => false), "One is not greater than two", false);

            nonThrowingValidator.ThrowExceptionOnInvalidRules();
            Assert.Throws<IncorrectDataException>(() => throwingValidator.ThrowExceptionOnInvalidRules());
        }
    }
}