using Rocket.Libraries.Validation.Exceptions;
using Rocket.Libraries.Validation.Services;
using Xunit;

namespace Rocket.Libraries.ValidationTests
{
    public class DataValidatorTests
    {
        [Fact]
        public void SynchronousValidationWorks()
        {
            var validator = new DataValidator()
                .AddFailureCondition(true, "One is not greater than two", false);

            Assert.Throws<FailedValidationException>(() => validator.ThrowExceptionOnInvalidRules<object>());
        }
    }
}