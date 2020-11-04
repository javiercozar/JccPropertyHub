using System.Collections.Generic;

namespace JccPropertyHub.Domain.Core.Dto {
    public abstract class Response {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}