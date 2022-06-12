namespace FoobarFactoryDemo.Infrastructure.Exceptions;

public class ActionableActivityWeightOutOfRangeException : Exception
{
    public override string Message => "The sum of all actionable activity weights should be greater than zero.";
}