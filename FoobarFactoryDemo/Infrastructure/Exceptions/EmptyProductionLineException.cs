namespace FoobarFactoryDemo.Infrastructure.Exceptions;

public class EmptyProductionLineException : Exception
{
    public override string Message => "Unable to retrieve next completed activity as the production line is empty.";
}