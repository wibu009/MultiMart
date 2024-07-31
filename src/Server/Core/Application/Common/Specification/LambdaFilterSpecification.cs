namespace MultiMart.Application.Common.Specification;

public class LambdaFilterSpecification<T, TResult> : Specification<T, TResult>
{
    public LambdaFilterSpecification(BaseFilter filter, Action<Specification<T, TResult>> configure)
    {
        configure(this);
        Query.SearchBy(filter);
    }

    public static LambdaFilterSpecification<T, TResult> Create(BaseFilter filter, Action<Specification<T, TResult>> configure)
    {
        return new LambdaFilterSpecification<T, TResult>(filter, configure);
    }
}

public class LambdaFilterSpecification<T> : Specification<T>
{
    public LambdaFilterSpecification(BaseFilter filter, Action<Specification<T>> configure)
    {
        configure(this);
        Query.SearchBy(filter);
    }

    public static LambdaFilterSpecification<T> Create(BaseFilter filter, Action<Specification<T>> configure)
    {
        return new LambdaFilterSpecification<T>(filter, configure);
    }
}