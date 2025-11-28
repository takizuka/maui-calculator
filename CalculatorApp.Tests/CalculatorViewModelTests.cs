using CalculatorApp.ViewModels;

namespace CalculatorApp.Tests;

public class CalculatorViewModelTests
{
    [Fact(DisplayName = "初期状態は0が表示される")]
    public void InitialState_DisplayIsZero()
    {
        var vm = new CalculatorViewModel();
        Assert.Equal("0", vm.Display);
    }

    [Fact(DisplayName = "数字ボタンで表示が更新される")]
    public void NumberCommand_UpdatesDisplay()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("5");
        Assert.Equal("5", vm.Display);
        vm.NumberCommand.Execute("2");
        Assert.Equal("52", vm.Display);
    }

    [Fact(DisplayName = "足し算が正しく機能する")]
    public void Addition_Works()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("5");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("3");
        Assert.Equal("5+3", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("8", vm.Display);
    }

    [Fact(DisplayName = "引き算が正しく機能する")]
    public void Subtraction_Works()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("10");
        vm.OperationCommand.Execute("-");
        vm.NumberCommand.Execute("4");
        Assert.Equal("10-4", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("6", vm.Display);
    }

    [Fact(DisplayName = "掛け算が正しく機能する")]
    public void Multiplication_Works()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("6");
        vm.OperationCommand.Execute("*");
        vm.NumberCommand.Execute("7");
        Assert.Equal("6*7", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("42", vm.Display);
    }

    [Fact(DisplayName = "割り算が正しく機能する")]
    public void Division_Works()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("20");
        vm.OperationCommand.Execute("/");
        vm.NumberCommand.Execute("4");
        Assert.Equal("20/4", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("5", vm.Display);
    }

    [Fact(DisplayName = "演算子の優先順位が正しく機能する")]
    public void OperatorPrecedence_Works()
    {
        var vm = new CalculatorViewModel();
        // 1 + 2 * 3 = 7
        vm.NumberCommand.Execute("1");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("2");
        vm.OperationCommand.Execute("*");
        vm.NumberCommand.Execute("3");
        Assert.Equal("1+2*3", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("7", vm.Display);
    }

    [Fact(DisplayName = "クリアボタンで状態がリセットされる")]
    public void Clear_ResetsState()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("5");
        vm.ClearCommand.Execute(null);
        Assert.Equal("0", vm.Display);
    }


    [Fact(DisplayName = "負の数を入力できる")]
    public void InputNegativeNumber_AtStart()
    {
        var vm = new CalculatorViewModel();
        vm.OperationCommand.Execute("-");
        Assert.Equal("-", vm.Display);
        vm.NumberCommand.Execute("5");
        Assert.Equal("-5", vm.Display);
    }

    [Fact(DisplayName = "演算子の後に負の数を入力できる")]
    public void InputNegativeNumber_AfterOperator()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("5");
        vm.OperationCommand.Execute("+");
        vm.OperationCommand.Execute("-");
        Assert.Equal("5-", vm.Display);
        vm.NumberCommand.Execute("3");
        Assert.Equal("5-3", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("2", vm.Display); // 5 - 3 = 2
    }

    [Fact(DisplayName = "負の数同士の計算ができる")]
    public void Calculate_WithNegativeNumbers()
    {
        var vm = new CalculatorViewModel();
        vm.OperationCommand.Execute("-");
        vm.NumberCommand.Execute("5"); // -5
        vm.OperationCommand.Execute("*");
        vm.OperationCommand.Execute("-");
        vm.NumberCommand.Execute("3"); // -3
        Assert.Equal("-5*-3", vm.Display);
        vm.CalculateCommand.Execute(null);
        Assert.Equal("15", vm.Display); // -5 * -3 = 15
    }


}
