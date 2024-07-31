namespace MultiMart.Application.Common.Specification;

public class LambdaSpecification<T> : Specification<T>
{
    public LambdaSpecification()
    {
    }

    private LambdaSpecification(Action<Specification<T>> configure)
    {
        configure(this);
    }

    public static LambdaSpecification<T> Create(Action<Specification<T>> configure)
    {
        return new LambdaSpecification<T>(configure);
    }
}

public class LambdaSpecification<T, TResult> : Specification<T, TResult>
{
    public LambdaSpecification()
    {
    }

    private LambdaSpecification(Action<Specification<T, TResult>> configure)
    {
        configure(this);
    }

    public static LambdaSpecification<T, TResult> Create(Action<Specification<T, TResult>> configure)
    {
        return new LambdaSpecification<T, TResult>(configure);
    }
}