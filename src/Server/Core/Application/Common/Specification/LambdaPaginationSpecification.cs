namespace MultiMart.Application.Common.Specification;

public class LambdaPaginationSpecification<T, TResult> : LambdaFilterSpecification<T, TResult>
{
    public LambdaPaginationSpecification(PaginationFilter filter, Action<Specification<T, TResult>> configure)
        : base(filter, spec =>
        {
            configure(spec);
            spec.Query.PaginateBy(filter);
        })
    {
    }

    public static LambdaPaginationSpecification<T, TResult> Create(PaginationFilter filter, Action<Specification<T, TResult>> configure)
    {
        return new LambdaPaginationSpecification<T, TResult>(filter, configure);
    }
}

public class LambdaPaginationSpecification<T> : LambdaFilterSpecification<T>
{
    public LambdaPaginationSpecification(PaginationFilter filter, Action<Specification<T>> configure)
        : base(filter, spec =>
        {
            configure(spec);
            spec.Query.PaginateBy(filter);
        })
    {
    }

    public static LambdaPaginationSpecification<T> Create(PaginationFilter filter, Action<Specification<T>> configure)
    {
        return new LambdaPaginationSpecification<T>(filter, configure);
    }
}
