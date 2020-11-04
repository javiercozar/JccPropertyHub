using System.Collections.Generic;
using JccPropertyHub.Domain.Core.Dto;

namespace JccPropertyHub.Domain.Core.Validators {
    public class ResponseValidator {
        public bool IsValid { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}