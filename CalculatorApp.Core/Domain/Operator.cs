namespace CalculatorApp.Core.Domain;

public enum Operator
{
    Add,
    Subtract,
    Multiply,
    Divide
}

public static class OperatorExtensions
{
    public static string ToSymbol(this Operator op)
    {
        return op switch
        {
            Operator.Add => "+",
            Operator.Subtract => "-",
            Operator.Multiply => "*",
            Operator.Divide => "/",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }

    public static Operator? FromSymbol(string symbol)
    {
        return symbol switch
        {
            "+" => Operator.Add,
            "-" => Operator.Subtract,
            "*" => Operator.Multiply,
            "/" => Operator.Divide,
            _ => null
        };
    }

    public static decimal Evaluate(this Operator op, decimal left, decimal right)
    {
        return op switch
        {
            Operator.Add => left + right,
            Operator.Subtract => left - right,
            Operator.Multiply => left * right,
            Operator.Divide => right == 0 ? throw new DivideByZeroException() : left / right,
            _ => throw new InvalidOperationException("Unknown operator")
        };
    }
}
