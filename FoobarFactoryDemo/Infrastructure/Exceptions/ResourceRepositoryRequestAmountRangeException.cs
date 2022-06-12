namespace FoobarFactoryDemo.Infrastructure.Exceptions;

public class ResourceRepositoryRequestAmountRangeException : Exception
{
    public ResourceRepositoryRequestAmountRangeException(int minimum, int maximum)
    {
        this.Minimum = minimum;
        this.Maximum = maximum;
    }

    public int Minimum { get; private init; }

    public int Maximum { get; private init; }

    public override string Message => $"An invalid range was given to the resource repository (minimum: {this.Minimum}; maximum: {this.Maximum}).";
}