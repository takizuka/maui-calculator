namespace CalculatorApp.Core.Domain;

/// <summary>
/// 数式の評価中に発生する例外
/// </summary>
public class FormulaException : Exception
{
    public FormulaException(string message) : base(message)
    {
    }

    public FormulaException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// 無効な入力が指定された際に発生する例外
/// </summary>
public class InvalidInputException(string message) : FormulaException(message)
{
}

/// <summary>
/// 数式の評価に失敗した際に発生する例外
/// </summary>
public class EvaluationException(string message, int position) : FormulaException(message)
{
    public int Position { get; } = position;
}
