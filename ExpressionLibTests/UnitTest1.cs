using ExpressionLib;

namespace ExpressionLibTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
      int a = 2; 
      int b = 2; 

      int expected = a + b;
      int actual = LambdaParser.Addx(a, b); 
      Assert.Equal(expected, actual);
    }
}