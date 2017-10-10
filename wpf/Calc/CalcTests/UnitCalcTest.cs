using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;
using Calc.Model;
using Calc.View;
using Calc.ViewModel;
using Rhino.Mocks;

namespace CalcTests
{
    [TestClass]
    public class UnitCalcTest
    {
        private MainWindowViewModel calcVM;

        private string separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

        [TestInitialize()]
        public void Initialize()
        {
            calcVM = new MainWindowViewModel();
        }


        [TestMethod()]
        public void TestMethodClean1()
        {
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("C");
            Assert.AreEqual(Convert.ToDecimal("0"), Convert.ToDecimal(calcVM.Display), "0");
        }

        [TestMethod()]
        public void TestMockMethodClean1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("3");
            calculator.clean();

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("C");

            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodClean2()
        {
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("C");
            Assert.AreEqual(Convert.ToDecimal("0"), Convert.ToDecimal(calcVM.Display), "0");
        }

        [TestMethod()]
        public void TestMockMethodClean2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("3");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessNonArithOperation("sqrt");
            calculator.clean();

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("C");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodClean3()
        {
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            calcVM.ButtonCommand.Execute("C");
            Assert.AreEqual(Convert.ToDecimal("0"), Convert.ToDecimal(calcVM.Display), "3/0=ERR and C = 0");
        }

        [TestMethod()]
        public void TestMockMethodClean3()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("3");
            calculator.ProcessArithOperation("/");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");
            calculator.clean();

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            calcVM.ButtonCommand.Execute("C");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodAdd1()
        {
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("5"), Convert.ToDecimal(calcVM.Display), "2+3=5");
        }

        [TestMethod()]
        public void TestMockMethodAdd1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("2");
            calculator.ProcessArithOperation("+");
            calculator.ProcessDigit("3");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodAdd2()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("1"), Convert.ToDecimal(calcVM.Display), "1+0=1");
        }

        [TestMethod()]
        public void TestMockMethodAdd2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessArithOperation("+");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodAdd3()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("-4"), Convert.ToDecimal(calcVM.Display), "1+(-5)=-4");
        }

        [TestMethod()]
        public void TestMockMethodAdd3()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessArithOperation("+");
            calculator.ProcessDigit("5");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodSubract1()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("50"), Convert.ToDecimal(calcVM.Display), "100-50=50");
        }

        [TestMethod()]
        public void TestMockMethodSubract1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("0");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("-");
            calculator.ProcessDigit("5");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }



        [TestMethod()]
        public void TestMethodSubract2()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("1"), Convert.ToDecimal(calcVM.Display), "1-0=0");
        }

        [TestMethod()]
        public void TestMockMethodSubract2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessArithOperation("-");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }



        [TestMethod()]
        public void TestMethodSubract3()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("15"), Convert.ToDecimal(calcVM.Display), "10-(-5)=15");
        }

        [TestMethod()]
        public void TestMockMethodSubract3()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("-");
            calculator.ProcessDigit("5");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("-");
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodMultiply1()
        {
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("18"), Convert.ToDecimal(calcVM.Display), "6*3=18");
        }

        [TestMethod()]
        public void TestMockMethodMultiply1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("6");
            calculator.ProcessArithOperation("*");
            calculator.ProcessDigit("3");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("3");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodMultiply2()
        {
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("0"), Convert.ToDecimal(calcVM.Display), "6*0=0");
        }

        [TestMethod()]
        public void TestMockMethodMultiply2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("6");
            calculator.ProcessArithOperation("*");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodMultiply3()
        {
            calcVM.ButtonCommand.Execute("8");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("-208"), Convert.ToDecimal(calcVM.Display), "8*(-26)=-208");
        }

        [TestMethod()]
        public void TestMockMethodMultiply3()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("8");
            calculator.ProcessArithOperation("*");
            calculator.ProcessDigit("2");
            calculator.ProcessDigit("6");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("8");
            calcVM.ButtonCommand.Execute("*");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodDivide1()
        {
            calcVM.ButtonCommand.Execute("7");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("=");

            Assert.AreEqual(Convert.ToDecimal("3" + separator + "5"), Convert.ToDecimal(calcVM.Display), "7/2=3.5");
        }

        [TestMethod()]
        public void TestMockMethodDivide1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("7");
            calculator.ProcessArithOperation("/");
            calculator.ProcessDigit("2");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;


            calcVM.ButtonCommand.Execute("7");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodDivide2()
        {
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("=");

            Assert.AreEqual(Convert.ToDecimal("-2" + separator + "6"), Convert.ToDecimal(calcVM.Display), "7/2=3.5");
        }

        [TestMethod()]
        public void TestMockMethodDivide2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("7");
            calculator.ProcessArithOperation("/");
            calculator.ProcessDigit("2");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;


            calcVM.ButtonCommand.Execute("7");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodErrorDivide1()
        {
            calcVM.ButtonCommand.Execute("7");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual("ERR", calcVM.Display.ToUpper(), "7/0=ERR");
        }

        [TestMethod()]
        public void TestMockMethodErrorDivide1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("7");
            calculator.ProcessArithOperation("/");
            calculator.ProcessDigit("0");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;


            calcVM.ButtonCommand.Execute("7");
            calcVM.ButtonCommand.Execute("/");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodSqrt1()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("4"), Convert.ToDecimal(calcVM.Display), "sqrt(16)=4");
        }

        [TestMethod()]
        public void TestMockMethodSqrt1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("6");
            calculator.ProcessNonArithOperation("sqrt");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodSqrtError1()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual("ERR", calcVM.Display, "sqrt(-16)=ERR");
        }

        [TestMethod()]
        public void TestMockMethodSqrtError1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("6");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessNonArithOperation("sqrt");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;


            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("sqrt");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodPercentage1()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("%");
            calcVM.ButtonCommand.Execute("=");
            Assert.AreEqual(Convert.ToDecimal("0" + separator + "16"), Convert.ToDecimal(calcVM.Display), "%(16)=0.16");
        }

        [TestMethod()]
        public void TestMockMethodPercentage1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("6");
            calculator.ProcessNonArithOperation("%");
            calculator.ProcessArithOperation("=");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("6");
            calcVM.ButtonCommand.Execute("%");
            calcVM.ButtonCommand.Execute("=");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodPercentage2()
        {
            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("%");
            Assert.AreEqual(Convert.ToDecimal("5"), Convert.ToDecimal(calcVM.Display), "5%(100)=5");
        }

        [TestMethod()]
        public void TestMockMethodPercentage2()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("5");
            calculator.ProcessArithOperation("+");
            calculator.ProcessDigit("1");
            calculator.ProcessDigit("0");
            calculator.ProcessDigit("0");
            calculator.ProcessNonArithOperation("%");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("5");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("%");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodPercentage3()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("%");
            Assert.AreEqual(Convert.ToDecimal("-12"), Convert.ToDecimal(calcVM.Display), "12%(-100)=-12");
        }

        [TestMethod()]
        public void TestMockMethodPercentage3()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("2");
            calculator.ProcessArithOperation("+");
            calculator.ProcessDigit("1");
            calculator.ProcessDigit("0");
            calculator.ProcessDigit("0");
            calculator.ProcessNonArithOperation("+/-");
            calculator.ProcessNonArithOperation("%");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute("+");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("+/-");
            calcVM.ButtonCommand.Execute("%");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestPoint1()
        {
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute(".");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("5");
            Assert.AreEqual(Convert.ToDecimal("12"+separator+"105"), Convert.ToDecimal(calcVM.Display), "12.105");
        }

        [TestMethod()]
        public void TestMockPoint1()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));

            calculator.ProcessDigit("1");
            calculator.ProcessDigit("2");
            calculator.ProcessPoint();
            calculator.ProcessDigit("1");
            calculator.ProcessDigit("0");
            calculator.ProcessDigit("5");

            mocks.ReplayAll();
            calcVM.Calculator = calculator;

            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("2");
            calcVM.ButtonCommand.Execute(".");
            calcVM.ButtonCommand.Execute("1");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("5");
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestMethodZeros1()
        {
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            calcVM.ButtonCommand.Execute("0");
            Assert.AreEqual("0", calcVM.Display, "0");
        }

        [TestMethod()]
        public void TestIsSqrttUsed()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));
            calculator.ProcessNonArithOperation("sqrt");
            mocks.ReplayAll();
            calcVM.Calculator = calculator;
            calcVM.ButtonCommand.Execute("sqrt");
            mocks.VerifyAll();
        }
        [TestMethod()]
        public void TestIsDigittUsed()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));
            calculator.ProcessDigit("1");
            mocks.ReplayAll();
            calcVM.Calculator = calculator;
            calcVM.ButtonCommand.Execute("1");
            mocks.VerifyAll();
        }
        [TestMethod()]
        public void TestIsAddUsed()
        {
            MockRepository mocks = new MockRepository();
            Calculator calculator = (Calculator)mocks.StrictMock(typeof(Calculator));
            calculator.ProcessArithOperation("+");
            mocks.ReplayAll();
            calcVM.Calculator = calculator;
            calcVM.ButtonCommand.Execute("+");
            mocks.VerifyAll();
        }

    }
}
