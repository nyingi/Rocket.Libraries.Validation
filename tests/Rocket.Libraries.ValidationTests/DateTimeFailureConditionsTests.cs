using Rocket.Libraries.Validation;
using System;
using Xunit;

namespace Rocket.Libraries.ValidationTests
{
    public class DateTimeFailureConditionsTests
    {
        [Fact]
        public void DefaultDateTimeIsCaught()
        {
            var result = DateTimeFailureConditions.IsDefaultDate(default);
            Assert.True(result);
        }

        [Fact]
        public void ValidDateDoesNotRegisterAsDefaultDate()
        {
            var result = DateTimeFailureConditions.IsDefaultDate(DateTime.Now);
            Assert.False(result);
        }
    }
}