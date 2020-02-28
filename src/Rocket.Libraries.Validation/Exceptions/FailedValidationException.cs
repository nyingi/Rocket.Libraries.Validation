namespace Rocket.Libraries.Validation.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using Rocket.Libraries.Validation.Extensions;
    using Rocket.Libraries.Validation.Models;

    [Serializable]
    public class FailedValidationException : Exception, IDisposable
    {
        public FailedValidationException(List<Error> errors)
        {
            Errors = ImmutableList.CreateRange(errors);
        }

        public FailedValidationException(string message)
            : base(message)
        {
        }

        public FailedValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FailedValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue(nameof(Errors), Errors, typeof(List<Error>));
            base.GetObjectData(info, context);
        }

        public void Dispose()
        {
            Errors = Errors.Clear();
        }

        public ImmutableList<Error> Errors { get; private set; }

        public string FailureDescription
        {
            get
            {
                if (Errors == null || Errors.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    var stringBuilder = new StringBuilder($"{Errors.Count} validation error(s) occured");
                    for (var i = 0; i < Errors.Count; i++)
                    {
                        stringBuilder.Append($"{Environment.NewLine}{i}). {Errors[i].Message}");
                    }
                    return stringBuilder.ToString();
                }
            }
        }
    }
}