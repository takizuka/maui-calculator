using CalculatorApp.ViewModels;

namespace CalculatorApp.Tests;

public class CalculatorEdgeCaseTests
{
    [Fact(DisplayName = "0除算はエラーを返す")]
    public void DivisionByZero_ShouldReturnError()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("1");
        vm.OperationCommand.Execute("/");
        vm.NumberCommand.Execute("0");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("除算エラー", vm.Display);
    }

    [Fact(DisplayName = "浮動小数点の計算精度が正しい")]
    public void FloatingPointPrecision_ShouldBeCorrect()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("0.1");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("0.2");
        vm.CalculateCommand.Execute(null);
        // 0.1 + 0.2 is notoriously 0.30000000000000004 in double
        // We expect the calculator to handle this gracefully, maybe by formatting
        Assert.Equal("0.3", vm.Display); 
    }

    [Fact(DisplayName = "複数の小数点は無視される")]
    public void MultipleDecimalPoints_ShouldBeIgnored()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("1");
        vm.DecimalCommand.Execute(null);
        vm.NumberCommand.Execute("2");
        vm.DecimalCommand.Execute(null); // Should be ignored
        vm.NumberCommand.Execute("3");
        Assert.Equal("1.23", vm.Display);
    }

    [Fact(DisplayName = "先頭の0は累積しない")]
    public void LeadingZeros_ShouldNotAccumulate()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("0");
        vm.NumberCommand.Execute("0");
        vm.NumberCommand.Execute("7");
        Assert.Equal("7", vm.Display);
    }

    [Fact(DisplayName = "小数点で開始した場合は0が付与される")]
    public void DecimalAtStart_ShouldPrefixZero()
    {
        var vm = new CalculatorViewModel();
        vm.DecimalCommand.Execute(null);
        vm.NumberCommand.Execute("5");
        Assert.Equal("0.5", vm.Display);
    }
    
    [Fact(DisplayName = "小数点で開始する数値の計算が正しく機能する")]
    public void OperationWithDecimalAtStart_ShouldWork()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("1");
        vm.OperationCommand.Execute("+");
        vm.DecimalCommand.Execute(null);
        vm.NumberCommand.Execute("5");
        vm.CalculateCommand.Execute(null);
        Assert.Equal("1.5", vm.Display);
    }
}
