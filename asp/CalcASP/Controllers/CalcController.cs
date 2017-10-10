using CalcASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalcASP.Controllers
{
    public interface IStateManager<T>
    {
        void save(string name, T state);
        T load(string name);
    }


    public class SessionStateManager<T> : IStateManager<T>
    {
        public virtual void save(string name, T state)
        {
            HttpContext.Current.Session[name] = state;
        }
        public virtual T load(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }
    }
    
    [HandleError]
    public class CalcController : Controller
    {
        ILogger logger = new Logger(typeof(CalcController));

        protected IStateManager<CalcModel> stateManager = new SessionStateManager<CalcModel>();

        public void setStateManager(IStateManager<CalcModel> manager)
        {
            stateManager = manager;
        }



        public ActionResult Index()
        {
            var model = new CalcModel();

            stateManager.save("model", model);

            //Session["model"] = model;
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(string button)
        {
            //CalcModel model = (CalcModel)Session["model"];
            CalcModel model = stateManager.load("model");
            ProcessInput(button, model);
            return View("Index", model);
        }
        
        public ActionResult Author()
        {
            return View();
        }



        public void ProcessInput(string arg, CalcModel _calc)
        {
            logger.LogInfoMessage("Process Input: \"" + arg + "\"");
            string argLowerCase = arg.ToLower();
            switch (argLowerCase)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    logger.LogInfoMessage("Process Digit: \"" + arg + "\"");
                    _calc.ProcessDigit(argLowerCase);
                    break;
                case ".":

                    logger.LogInfoMessage("Process Point");
                    _calc.ProcessPoint();
                    break;
                case "C":
                case "c":
                    logger.LogInfoMessage("Process Cleaning");
                    _calc.clean();
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                case "=":
                    logger.LogInfoMessage("Process Arithmetic Operation: \"" + arg + "\"");
                    _calc.ProcessArithOperation(argLowerCase);
                    break;
                case "sqrt":
                case "%":
                case "+/-":
                    logger.LogInfoMessage("Process NonArithmetic Operation: \"" + arg + "\"");
                    _calc.ProcessNonArithOperation(argLowerCase);
                    break;
            }
        }
    }
}