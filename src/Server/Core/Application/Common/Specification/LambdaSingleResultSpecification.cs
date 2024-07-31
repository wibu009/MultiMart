namespace MultiMart.Application.Common.Specification;

public class LambdaSingleResultSpecification<T> : SingleResultSpecification<T>
{
    public LambdaSingleResultSpecification()
    {
    }

    private LambdaSingleResultSpecification(Action<SingleResultSpecification<T>> configure)
    {
        configure(this);
    }

    public static LambdaSingleResultSpecification<T> Create(Action<SingleResultSpecification<T>> configure)
    {
        return new LambdaSingleResultSpecification<T>(configure);
    }
}

public class LambdaSingleResultSpecification<T, TResult> : SingleResultSpecification<T, TResult>
{
    public LambdaSingleResultSpecification()
    {
    }

    private LambdaSingleResultSpecification(Action<SingleResultSpecification<T, TResult>> configure)
    {
        configure(this);
    }

    public static LambdaSingleResultSpecification<T, TResult> Create(Action<SingleResultSpecification<T, TResult>> configure)
    {
        return new LambdaSingleResultSpecification<T, TResult>(configure);
    }
}