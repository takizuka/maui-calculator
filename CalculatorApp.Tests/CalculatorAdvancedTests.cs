using CalculatorApp.ViewModels;
using CalculatorApp.Core.Domain;

namespace CalculatorApp.Tests;

public class CalculatorAdvancedTests
{
    [Fact(DisplayName = "大きな数値の計算が正しく機能する")]
    public void VeryLargeNumber_ShouldHandleCorrectly()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("999999");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("1");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("1000000", vm.Display);
    }

    [Fact(DisplayName = "複雑な式が正しく評価される")]
    public void ComplexExpression_ShouldEvaluateCorrectly()
    {
        var vm = new CalculatorViewModel();
        // 1 + 2 * 3 - 4 / 2 = 1 + 6 - 2 = 5
        vm.NumberCommand.Execute("1");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("2");
        vm.OperationCommand.Execute("*");
        vm.NumberCommand.Execute("3");
        vm.OperationCommand.Execute("-");
        vm.NumberCommand.Execute("4");
        vm.OperationCommand.Execute("/");
        vm.NumberCommand.Execute("2");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("5", vm.Display);
    }

    [Fact(DisplayName = "非常に小さな小数の計算が正しく機能する")]
    public void VerySmallDecimal_ShouldHandleCorrectly()
    {
        var vm = new CalculatorViewModel();
        vm.DecimalCommand.Execute(null);
        vm.NumberCommand.Execute("001");
        vm.OperationCommand.Execute("+");
        vm.DecimalCommand.Execute(null);
        vm.NumberCommand.Execute("001");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("0.002", vm.Display);
    }

    [Fact(DisplayName = "無効な入力で例外が発生する")]
    public void InvalidInput_ShouldThrowException()
    {
        var formula = new Formula();
        
        // 空文字列
        Assert.Throws<InvalidInputException>(() => formula.AppendNumber(""));
        
        // 無効な文字
        Assert.Throws<InvalidInputException>(() => formula.AppendNumber("abc"));
        
        // 特殊文字
        Assert.Throws<InvalidInputException>(() => formula.AppendNumber("@"));
    }

    [Fact(DisplayName = "連続した演算が正しく機能する")]
    public void ChainedOperations_ShouldWork()
    {
        var vm = new CalculatorViewModel();
        // 10 - 5 + 3 = 8
        vm.NumberCommand.Execute("10");
        vm.OperationCommand.Execute("-");
        vm.NumberCommand.Execute("5");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("3");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("8", vm.Display);
    }

    [Fact(DisplayName = "計算後に新しい数値を入力できる")]
    public void AfterCalculation_CanInputNewNumber()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("5");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("3");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("8", vm.Display);
        
        // 新しい数値を入力
        vm.NumberCommand.Execute("9");
        Assert.Equal("9", vm.Display);
    }

    [Fact(DisplayName = "ゼロで始まる数値が正しく処理される")]
    public void NumberStartingWithZero_ShouldHandleCorrectly()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("0");
        vm.NumberCommand.Execute("0");
        vm.NumberCommand.Execute("5");
        Assert.Equal("5", vm.Display);
    }
}
