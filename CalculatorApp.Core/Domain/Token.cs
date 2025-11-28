namespace CalculatorApp.Core.Domain;

/// <summary>
/// トークンの基底クラス
/// </summary>
internal abstract class Token
{
    public abstract string ToDisplayString();
}

/// <summary>
/// 数値トークン（不変型）
/// 数値を文字列として保持し、不変性を保証します
/// </summary>
internal class NumberToken : Token
{
    private const string InitialValue = "0";
    private const string NegativeZero = "-0";
    private const string NegativeSign = "-";
    private const string DecimalPoint = ".";
    
    private readonly string _value;

    /// <summary>
    /// 数値トークンを作成します
    /// </summary>
    /// <param name="value">数値の文字列表現</param>
    public NumberToken(string value)
    {
        _value = value;
    }

    /// <summary>
    /// 数字を追加した新しいトークンを返します
    /// </summary>
    /// <param name="digit">追加する数字</param>
    /// <returns>数字が追加された新しいNumberToken</returns>
    public NumberToken AppendDigit(string digit)
    {
        if (_value == InitialValue || _value == NegativeZero)
        {
            if (_value == NegativeZero)
            {
                return new NumberToken(NegativeSign + digit);
            }
            return new NumberToken(digit);
        }
        return new NumberToken(_value + digit);
    }

    /// <summary>
    /// 小数点を追加した新しいトークンを返します
    /// </summary>
    /// <returns>小数点が追加された新しいNumberToken、または既に小数点がある場合は現在のインスタンス</returns>
    public NumberToken AppendDecimal()
    {
        if (!_value.Contains(DecimalPoint))
        {
            return new NumberToken(_value + DecimalPoint);
        }
        return this;
    }

    /// <summary>
    /// 負の符号のみかどうかを判定します
    /// </summary>
    /// <returns>負の符号のみの場合true</returns>
    public bool IsNegativeSignOnly()
    {
        return _value == NegativeSign;
    }

    /// <summary>
    /// 数値のdecimal値を取得します
    /// </summary>
    /// <returns>数値のdecimal表現</returns>
    /// <exception cref="FormulaException">数値の解析に失敗した場合</exception>
    public decimal GetValue()
    {
        if (_value == NegativeSign || string.IsNullOrEmpty(_value))
        {
            return 0m;
        }
        
        if (!decimal.TryParse(_value, out var result))
        {
            throw new FormulaException($"無効な数値フォーマット: {_value}");
        }
        
        return result;
    }

    public override string ToDisplayString()
    {
        return _value;
    }
}

/// <summary>
/// 演算子トークン
/// </summary>
internal class OperatorToken(Operator op) : Token
{
    public Operator Op { get; } = op;

    public override string ToDisplayString()
    {
        return Op.ToSymbol();
    }
}
