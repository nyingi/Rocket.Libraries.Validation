namespace Rocket.Libraries.Validation.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Rocket.Libraries.Validation.Extensions;
    using Rocket.Libraries.Validation.Models;

    public class IncorrectDataException : Exception
    {
        public IncorrectDataException()
        {
        }

        public IncorrectDataException(string message)
            : base(message)
        {
        }

        public IncorrectDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public List<Error> Errors { get; } = new List<Error>();

        public override string Message => this.ToString();

        public override string ToString()
        {
            if (Errors == null || Errors.Count == 0)
            {
                return base.ToString();
            }
            else
            {
                var error = $"{Errors.Count} validation error(s) occured";
                Errors.ForEvery((e, i) =>
                {
                    error += $"{Environment.NewLine}{i}). {e.Message}";
                });
                return error;
            }
        }
    }
}