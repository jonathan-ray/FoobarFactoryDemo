namespace FoobarFactoryDemo.Infrastructure.Exceptions;

public class ResourceRepositoryRequestAmountException : Exception
{
    public ResourceRepositoryRequestAmountException(int amount)
    {
        this.Amount = amount;
    }

    public int Amount { get; private init; }

    public override string Message => $"An invalid amount was given to the resource repository ({this.Amount}).";
}