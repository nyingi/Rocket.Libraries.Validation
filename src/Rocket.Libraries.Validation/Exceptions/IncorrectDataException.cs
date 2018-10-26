using Rocket.Libraries.Validation.Extensions;
using Rocket.Libraries.Validation.Models;
using System;
using System.Collections.Generic;

namespace Rocket.Libraries.Validation.Exceptions
{
    public class IncorrectDataException : Exception
    {
        public List<Error> Errors { get; set; }

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

        public override string Message => this.ToString();
    }
}