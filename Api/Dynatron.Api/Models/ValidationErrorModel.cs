using FluentValidation.Results;
using System.Text;

namespace Dynatron.Api.Models
{
    public class ValidationErrorModel
    {
        public ValidationErrorModel(string message) 
        {
            Message = message;
        }

        public ValidationErrorModel(List<ValidationFailure> validationFailures)
        {
            if(validationFailures == null)
                throw new ArgumentNullException(nameof(validationFailures));

            var sb = new StringBuilder();
            validationFailures.ForEach(e => sb.AppendLine(e.ErrorMessage));
            Message = sb.ToString();
        }

        public string Message { get; }
    }
}
