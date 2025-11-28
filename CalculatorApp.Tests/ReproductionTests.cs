using CalculatorApp.ViewModels;

namespace CalculatorApp.Tests;

public class ReproductionTests
{
    [Fact(DisplayName = "演算子の連続入力でオペランドが重複しない")]
    public void RepeatedOperators_ShouldNotDuplicateOperands()
    {
        var vm = new CalculatorViewModel();
        vm.NumberCommand.Execute("1");
        vm.OperationCommand.Execute("+");
        vm.OperationCommand.Execute("+");
        vm.OperationCommand.Execute("+");
        vm.NumberCommand.Execute("2");
        vm.CalculateCommand.Execute(null);
        
        // Current buggy behavior is 5
        // Expected behavior is 3
        Assert.Equal("3", vm.Display);
    }
}
