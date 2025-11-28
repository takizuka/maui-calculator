using System.Text;

namespace CalculatorApp.Core.Domain;

/// <summary>
/// 数式をトークンのリストとして管理する電卓クラス
/// </summary>
public class Formula
{
    private readonly List<Token> _tokens = [];

    /// <summary>
    /// 数字を現在の数式に追加します
    /// </summary>
    /// <param name="digit">追加する数字（0-9）</param>
    /// <exception cref="ArgumentException">digitが無効な場合</exception>
    public void AppendNumber(string digit)
    {
        if (string.IsNullOrEmpty(digit))
        {
            throw new InvalidInputException("数字が指定されていません");
        }
        
        // 数字または小数点のみを許可
        if (!digit.All(c => char.IsDigit(c) || c == '.'))
        {
            throw new InvalidInputException($"有効な数字を指定してください: {digit}");
        }
        
        // 最後のトークンが数値の場合は追加、それ以外は新しい数値トークンを作成
        if (_tokens.Count > 0 && _tokens[^1] is NumberToken lastNumber)
        {
            _tokens[^1] = lastNumber.AppendDigit(digit);
        }
        else
        {
            _tokens.Add(new NumberToken(digit));
        }
    }

    /// <summary>
    /// 演算子を現在の数式に追加します
    /// </summary>
    /// <param name="op">追加する演算子</param>
    public void AppendOperator(Operator op)
    {
        // トークンがない場合
        if (_tokens.Count == 0)
        {
            if (op == Operator.Subtract)
            {
                // 負の符号として数値トークンを開始
                _tokens.Add(new NumberToken("-"));
            }
            else
            {
                // 0を追加してから演算子
                _tokens.Add(new NumberToken("0"));
                _tokens.Add(new OperatorToken(op));
            }
            return;
        }

        var lastToken = _tokens[^1];

        // 最後が数値の場合
        if (lastToken is NumberToken numberToken)
        {
            // 負の符号のみの場合
            if (numberToken.IsNegativeSignOnly())
            {
                // "0" + 演算子に置き換え
                _tokens[^1] = new NumberToken("0");
                _tokens.Add(new OperatorToken(op));
                return;
            }

            // 通常の数値の場合は演算子を追加
            _tokens.Add(new OperatorToken(op));
        }
        // 最後が演算子の場合
        else if (lastToken is OperatorToken operatorToken)
        {
            if (op == Operator.Subtract)
            {
                // 乗算・除算の後の減算は負の符号として数値を開始
                if (operatorToken.Op == Operator.Multiply || operatorToken.Op == Operator.Divide)
                {
                    _tokens.Add(new NumberToken("-"));
                }
                else
                {
                    // 加算・減算は演算子を置き換え
                    _tokens[^1] = new OperatorToken(op);
                }
            }
            else
            {
                // その他の演算子は置き換え
                _tokens[^1] = new OperatorToken(op);
            }
        }
    }

    /// <summary>
    /// 小数点を現在の数値に追加します
    /// </summary>
    public void AppendDecimal()
    {
        // 最後のトークンが数値の場合は小数点を追加
        if (_tokens.Count > 0 && _tokens[^1] is NumberToken lastNumber)
        {
            _tokens[^1] = lastNumber.AppendDecimal();
        }
        else
        {
            // 数値がない場合は "0." を作成
            _tokens.Add(new NumberToken("0."));
        }
    }

    /// <summary>
    /// 数式をクリアして初期状態（0）に戻します
    /// </summary>
    public void Clear()
    {
        _tokens.Clear();
        _tokens.Add(new NumberToken("0"));
    }

    /// <summary>
    /// 計算結果を設定します
    /// </summary>
    /// <param name="result">計算結果の文字列</param>
    public void SetResult(string result)
    {
        _tokens.Clear();
        _tokens.Add(new NumberToken(result));
    }

    /// <summary>
    /// 数式の文字列表現を取得します
    /// </summary>
    /// <returns>数式の表示文字列</returns>
    public string GetDisplayString()
    {
        if (_tokens.Count == 0)
        {
            return "0";
        }

        var sb = new StringBuilder();
        foreach (var token in _tokens)
        {
            sb.Append(token.ToDisplayString());
        }
        return sb.ToString();
    }

    /// <summary>
    /// 数式を評価して結果を計算します
    /// </summary>
    /// <returns>計算結果</returns>
    /// <exception cref="DivideByZeroException">0で除算した場合</exception>
    /// <exception cref="EvaluationException">式の評価に失敗した場合</exception>
    public decimal Evaluate()
    {
        // トークンリストを式として評価
        var evaluator = new TokenEvaluator(_tokens);
        return evaluator.Evaluate();
    }
}
