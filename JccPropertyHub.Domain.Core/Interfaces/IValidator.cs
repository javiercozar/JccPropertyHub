namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface IValidator<in TRequest, out TValidation> {
        TValidation Validate(TRequest request);
    }
}