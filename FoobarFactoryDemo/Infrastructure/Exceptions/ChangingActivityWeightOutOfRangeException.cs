namespace FoobarFactoryDemo.Infrastructure.Exceptions;

public class ChangingActivityWeightOutOfRangeException : Exception
{
    public ChangingActivityWeightOutOfRangeException(double value)
    {
        this.Value = value;
    }

    public double Value { get; private init; }

    public override string Message => $"Erroneous value '{this.Value}' was set for the changing activity weight. Expected to be greater than 0 and less than 1";
}