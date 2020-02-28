using Rocket.Libraries.Validation;
using Xunit;

namespace Rocket.Libraries.ValidationTests
{
    public class StringFailureConditionsTests
    {
        [Theory]
        [InlineData("1234567890", false)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", false)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", false)]
        [InlineData("123$4567890", true)]
        [InlineData("abcdefgh245ijklmnopqrstuvwxyz", false)]
        [InlineData("ABCDEFGHIJKLMNOPQ4646RSTUVWXYZ", false)]
        [InlineData("abcdefgh&&^ijklmnopqrstuvwxyz", true)]
        [InlineData("_ABCDEFGHIJKLMNOPQRSTUVWXYZ", true)]
        public void AlphaNumbericFailureConditionCatchesInvalidCharacter(string testValue, bool shouldFail)
        {
            var result = StringFailureConditions.ContainsNonAlphaNumericCharacters(testValue);
            Assert.Equal(shouldFail, result);
        }

        [Theory]
        [InlineData("1234567890", true)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", false)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", false)]
        [InlineData("123$4567890", true)]
        [InlineData("abcdefgh245ijklmnopqrstuvwxyz", true)]
        [InlineData("ABCDEFGHIJKLMNOPQ4646RSTUVWXYZ", true)]
        [InlineData("abcdefgh&&^ijklmnopqrstuvwxyz", true)]
        [InlineData("_ABCDEFGHIJKLMNOPQRSTUVWXYZ", true)]
        public void AlphabeticFailureConditionsCatchesNonAlphabets(string testValue, bool shouldFail)
        {
            var result = StringFailureConditions.ContainsNonAlphabeticCharacters(testValue);
            Assert.Equal(shouldFail, result);
        }

        [Theory]
        [InlineData("qwerty", 3, true)]
        [InlineData("qwerty", 6, false)]
        [InlineData("qwerty", 7, false)]
        public void MaxLengthConditionWorksProperly(string userValue, uint maxLength, bool shouldFail)
        {
            var result = StringFailureConditions.ExceedsLength(userValue, maxLength);
            Assert.Equal(shouldFail, result);
        }

        [Theory]
        [InlineData("qwerty", 7, true)]
        [InlineData("qwerty", 6, false)]
        [InlineData("qwerty", 5, false)]
        public void MinLengthConditionWorksProperly(string userValue, uint minLength, bool shouldFail)
        {
            var result = StringFailureConditions.ShorterThan(userValue, minLength);
            Assert.Equal(shouldFail, result);
        }

        [Theory]
        [InlineData("0123456789", false)]
        [InlineData("#123456789", true)]
        [InlineData("0123456789.1", false)]
        [InlineData("1234=4321", true)]
        public void NumericValuesOnlyTestWorks(string userValue, bool shouldFail)
        {
            var result = StringFailureConditions.DoesNotParseToDoubleNumber(userValue);
            Assert.Equal(shouldFail, result);
        }
    }
}