namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface IMapper<TSource, TTarget> {
        TTarget MapFrom(TSource source);
    }
}