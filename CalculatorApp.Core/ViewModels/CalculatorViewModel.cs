using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalculatorApp.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CalculatorApp.ViewModels;

public partial class CalculatorViewModel(ILogger<CalculatorViewModel>? logger = null) : ObservableObject
{
    [ObservableProperty]
    private string _display = "0";

    private readonly Formula _formula = new();
    private readonly ILogger<CalculatorViewModel>? _logger = logger;
    private bool _shouldResetDisplay = true;

    [RelayCommand]
    private void Number(string number)
    {
        if (_shouldResetDisplay)
        {
            _formula.Clear();
            _shouldResetDisplay = false;
        }
        
        _formula.AppendNumber(number);
        UpdateDisplay();
    }

    [RelayCommand]
    private void Operation(string op)
    {
        _shouldResetDisplay = false;
        if (OperatorExtensions.FromSymbol(op) is Operator operatorEnum)
        {
            _formula.AppendOperator(operatorEnum);
            UpdateDisplay();
        }
    }

    [RelayCommand]
    private void Calculate()
    {
        try 
        {
            decimal result = _formula.Evaluate();
            _formula.SetResult(result.ToString());
            UpdateDisplay();
        }
        catch (DivideByZeroException ex)
        {
            _logger?.LogWarning(ex, "除算エラーが発生しました");
            Display = "除算エラー";
            _formula.Clear();
        }
        catch (FormulaException ex)
        {
            _logger?.LogWarning(ex, "数式評価エラー: {Message}", ex.Message);
            Display = "Error";
            _formula.Clear();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "予期しないエラーが発生しました");
            Display = "Error";
            _formula.Clear();
        }
        finally
        {
            _shouldResetDisplay = true;
        }
    }

    [RelayCommand]
    private void Clear()
    {
        _formula.Clear();
        _shouldResetDisplay = true;
        UpdateDisplay();
    }

    [RelayCommand]
    private void Decimal()
    {
        if (_shouldResetDisplay)
        {
            _formula.Clear();
            _shouldResetDisplay = false;
        }
        
        _formula.AppendDecimal();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        Display = _formula.GetDisplayString();
    }
}
