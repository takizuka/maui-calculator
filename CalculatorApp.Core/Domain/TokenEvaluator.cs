namespace CalculatorApp.Core.Domain;

/// <summary>
/// トークンリストを評価するクラス
/// 演算子の優先順位を考慮して計算
/// </summary>
internal class TokenEvaluator(List<Token> tokens)
{
    private int _position = 0;

    public decimal Evaluate()
    {
        if (tokens.Count == 0)
        {
            return 0;
        }

        return ParseExpression();
    }

    /// <summary>
    /// 式をパース（加算・減算）
    /// </summary>
    private decimal ParseExpression()
    {
        decimal result = ParseTerm();

        while (_position < tokens.Count && tokens[_position] is OperatorToken op)
        {
            if (op.Op == Operator.Add || op.Op == Operator.Subtract)
            {
                _position++;
                result = op.Op.Evaluate(result, ParseTerm());
            }
            else
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 項をパース（乗算・除算）
    /// </summary>
    private decimal ParseTerm()
    {
        decimal result = ParseFactor();

        while (_position < tokens.Count && tokens[_position] is OperatorToken op)
        {
            if (op.Op == Operator.Multiply || op.Op == Operator.Divide)
            {
                _position++;
                result = op.Op.Evaluate(result, ParseFactor());
            }
            else
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 因子をパース（数値または負の数）
    /// </summary>
    private decimal ParseFactor()
    {
        if (_position >= tokens.Count)
        {
            throw new EvaluationException($"位置 {_position} で式が予期せず終了しました", _position);
        }

        var token = tokens[_position];

        if (token is NumberToken numberToken)
        {
            _position++;
            return numberToken.GetValue();
        }

        throw new EvaluationException($"位置 {_position} で予期しないトークン: {token.GetType().Name}", _position);
    }
}
