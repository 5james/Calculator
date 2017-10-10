using CalcASP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web;
using Moq;
using System.Web.Mvc;
using CalcASP.Models;

namespace CalcASPTest
{
    [TestClass]
    public class UnitCalcASPTests
    {
        //private MockHttpSession session;
        //SessionStateManager<CalcModel> statemanager;
        //private string Display;

        private CalcController controller;
        private Mock<ControllerContext> context;
        private CalcModel model;

        private string separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


        [TestInitialize()]
        public void TestInit()
        {
            context = new Mock<ControllerContext>();
            //context = MockRepository.GenerateMock<ControllerContext>();
            controller = new CalcController();
            controller.ControllerContext = context.Object;

            model = new CalcModel();

            var statemanager = new Mock<SessionStateManager<CalcModel>>();

            statemanager.Setup(l => l.load("model")).Returns(model);
            statemanager.Setup(s => s.save("model", model));

            controller.setStateManager(statemanager.Object);

            controller.Index();
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual("0", model.Display);
        }

        [TestMethod]
        public void TestSimpleInput1()
        {
            controller.Index("1");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("1", model.Display);
        }

        [TestMethod]
        public void TestSimpleInput2()
        {
            controller.Index("1");
            controller.Index("2");
            controller.Index("3");
            controller.Index("4");
            controller.Index("5");
            controller.Index("6");
            controller.Index("7");
            controller.Index("8");
            controller.Index("9");
            controller.Index("0");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("1234567890", model.Display);
        }

        [TestMethod]
        public void TestSimpleClean1()
        {
            controller.Index("1");
            controller.Index("+");
            controller.Index("1");
            controller.Index("2");
            controller.Index("*");
            controller.Index("8");
            controller.Index("C");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("0", model.Display);
        }

        [TestMethod]
        public void TestSimpleAdd1()
        {
            controller.Index("1");
            controller.Index("+");
            controller.Index("1");
            controller.Index("=");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("2", model.Display);
        }

        [TestMethod]
        public void TestSimpleSubtraction1()
        {
            controller.Index("3");
            controller.Index("7");
            controller.Index("-");
            controller.Index("2");
            controller.Index("1");
            controller.Index("=");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("16", model.Display);
        }

        [TestMethod]
        public void TestSimpleMultiply1()
        {
            controller.Index("5");
            controller.Index("7");
            controller.Index("*");
            controller.Index("2");
            controller.Index("9");
            controller.Index("=");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("1653", model.Display);
        }

        [TestMethod]
        public void TestSimpleDivide1()
        {
            controller.Index("5");
            controller.Index("2");
            controller.Index("/");
            controller.Index("2");
            controller.Index("=");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("26", model.Display);
        }
        [TestMethod]
        public void TestSimpleErrorDivide1()
        {
            controller.Index("5");
            controller.Index("2");
            controller.Index("/");
            controller.Index("0");
            controller.Index("=");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("ERR", model.Display);
            Assert.AreEqual(true, model.IsDisabled);
        }

        [TestMethod]
        public void TestSimpleChangeSign1()
        {
            controller.Index("5");
            controller.Index("2");
            controller.Index("+/-");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("-52", model.Display);
        }

        [TestMethod]
        public void TestSimplePercent1()
        {
            controller.Index("5");
            controller.Index("5");
            controller.Index("%");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("0" + separator + "55", model.Display);
        }

        [TestMethod]
        public void TestSimpleSqrt1()
        {
            controller.Index("4");
            controller.Index("sqrt");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("2", model.Display);
        }

        [TestMethod]
        public void TestSimpleErrorSqrt1()
        {
            controller.Index("4");
            controller.Index("+/-");
            controller.Index("sqrt");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("ERR", model.Display);
            Assert.AreEqual(true, model.IsDisabled);
        }

        [TestMethod]
        public void TestSimpleComma1()
        {
            controller.Index("1");
            controller.Index(".");
            controller.Index("0");
            controller.Index("1");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("1" + separator + "01", model.Display);
        }

        [TestMethod]
        public void TestMultipleComma1()
        {
            controller.Index("5");
            controller.Index("5");
            controller.Index(".");
            controller.Index(".");
            controller.Index(".");
            controller.Index("5");
            controller.Index("5");
            //CalcModel test = (CalcModel)session["model"];
            Assert.AreEqual("55" + separator + "55", model.Display);
        }

        //private class MockHttpSession : HttpSessionStateBase
        //{
        //    Dictionary<string, object> _sessionDictionary = new Dictionary<string, object>();
        //    public override object this[string name]
        //    {
        //        get
        //        {
        //            return _sessionDictionary[name];
        //        }
        //        set
        //        {
        //            _sessionDictionary[name] = value;
        //        }
        //    }
        //}
    }
}
