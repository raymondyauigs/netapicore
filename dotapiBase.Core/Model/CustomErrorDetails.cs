using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace dotapiBase.Core.Model
{
    public class CustomErrorDetails:HttpValidationProblemDetails
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public new string? Detail { get; set; }

        [JsonPropertyName("errors")]
        public new IEnumerable<CustomError> Errors { get; } = new List<CustomError>();

        public CustomErrorDetails() { }

        public CustomErrorDetails(IEnumerable<CustomError> errors)
        {
            Errors = errors;
        }
        public CustomErrorDetails(ModelStateDictionary modelStateDictionary)
        {
            Errors = ConvertModelStateErrorsToCustomErrors(modelStateDictionary);
        }

        private List<CustomError> ConvertModelStateErrorsToCustomErrors(ModelStateDictionary modelStateDictionary)
        {
            List<CustomError> CustomErrors = new();

            foreach (var keyModelStatePair in modelStateDictionary)
            {
                var errors = keyModelStatePair.Value.Errors;
                switch (errors.Count)
                {
                    case 0:
                        continue;

                    case 1:
                        CustomErrors.Add(new CustomError { Code = null, Message = errors[0].ErrorMessage });
                        break;

                    default:
                        var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage));
                        CustomErrors.Add(new CustomError { Message = errorMessage });
                        break;
                }
            }

            return CustomErrors;
        }
    }
}
