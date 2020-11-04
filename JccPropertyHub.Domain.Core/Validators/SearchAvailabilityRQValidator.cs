using System.Collections.Generic;
using System.Linq;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccPropertyHub.Domain.Core.Validators {
    public class SearchAvailabilityRqValidator : IValidator<SearchAvailabilityRq, ResponseValidator> {
        public ResponseValidator Validate(SearchAvailabilityRq request) {
            var errorList = new List<Error>();

            RequestIsNotNull(request, ref errorList);
            CheckDatesCriteria(request, ref errorList);

            return new ResponseValidator {
                IsValid = !errorList.Any(),
                Errors = errorList.AsEnumerable()
            };
        }

        private static void CheckDatesCriteria(SearchAvailabilityRq request, ref List<Error> errorList) {
            if (request.CheckOut < request.CheckIn) {
                errorList.Add(new Error {
                    ErrorType = ErrorType.BadRequestCriteria,
                    Issue = "CheckOut have to be less than CheckIn"
                });
            }
        }

        private static void RequestIsNotNull(SearchAvailabilityRq request, ref List<Error> errorList) {
            if (request == null) {
                errorList.Add(new Error {
                    ErrorType = ErrorType.RequestIsNull,
                    Issue = "Request can not be null"
                });
            }
        }
    }
}