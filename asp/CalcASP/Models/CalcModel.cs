using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalcASP.Models
{
    public class CalcModel
    {
        ILogger logger = new Logger(typeof(CalcModel));

        public CalcModel()
        {
            Display = "0";
            point = false;
            newInputNumber = false;
            continueCalculation = false;
            IsDisabled = false;
            ErrorReason = "None";
            Separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

        }

        public CalcModel(string displayed)
        {
            Display = displayed;
            point = false;
            newInputNumber = false;
            continueCalculation = false;
            IsDisabled = false;
            ErrorReason = "None";
            Separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        }

        private enum Operators
        {
            NONE, ADD, SUBTRACTION, MULTIPLICATION, DIVISION
        }
        
        private Operators operator_;
        private bool point;
        private bool newInputNumber;
        private bool continueCalculation;
        private decimal previousNumber = new decimal(0);

        public bool IsDisabled { get; set; }
        public string ErrorReason { get; set; }
        public string Display { get; set; }
        public string Separator { get; set; }


        public virtual void ProcessArithOperation(string arg)
        {

            logger.LogInfoMessage("Will process arithmetic operator: \"" + arg + "\"");
            point = false;
            if (calculate())
            {
                logger.LogInfoMessage("Calculated successfully");
                try
                {
                    logger.LogInfoMessage("Will set previousNumber to" + Display);
                    previousNumber = Convert.ToDecimal(Display);
                }
                catch (Exception e)
                {
                    if (e is FormatException || e is OverflowException)
                    {
                        logger.LogException(e);
                        Display = "ERR";
                        IsDisabled = true;
                        ErrorReason = e.Message;
                        return;
                    }
                }
                switch (arg)
                {
                    case "+":
                        logger.LogInfoMessage("Processing adding");
                        operator_ = Operators.ADD;
                        break;
                    case "-":
                        logger.LogInfoMessage("Processing subtraction");
                        operator_ = Operators.SUBTRACTION;
                        break;
                    case "*":
                        logger.LogInfoMessage("Processing multiplication");
                        operator_ = Operators.MULTIPLICATION;
                        break;
                    case "/":
                        logger.LogInfoMessage("Processing dividing");
                        operator_ = Operators.DIVISION;
                        break;
                    case "=":
                        logger.LogInfoMessage("Processing =");
                        operator_ = Operators.NONE;
                        break;
                    default:
                        logger.LogWarningMessage("Processing \"" + arg + "\" and as ARITHMETIC operator");
                        operator_ = Operators.NONE;
                        break;

                }

                newInputNumber = true;
            }
            else
            {
                logger.LogWarningMessage("Did not calculate previous operation, so couldn't set new operation to calculate");
            }
        }

        public virtual void ProcessNonArithOperation(string argLowerCase)
        {

            logger.LogInfoMessage("Will process non arithmetic operator: \"" + argLowerCase + "\"");
            try
            {
                switch (argLowerCase)
                {
                    case "%":
                        logger.LogInfoMessage("Processing %");
                        point = false;
                        newInputNumber = true;
                        calculatePercent();
                        break;
                    case "sqrt":
                        logger.LogInfoMessage("Processing sqrt");
                        point = false;
                        newInputNumber = true;
                        calculateSqrt();
                        break;
                    case "+/-":
                        logger.LogInfoMessage("Processing +/-");
                        changeSign();
                        break;
                    default:
                        logger.LogWarningMessage("Processing \"" + argLowerCase + "\" and as ARITHMETIC operator");
                        break;

                }
            }
            catch (Exception e)
            {
                if (e is FormatException ||
                    e is OverflowException ||
                    e is DivideByZeroException ||
                    e is ArithmeticException
                    )
                {
                    Console.WriteLine(e.Message);
                    Display = "ERR";
                    IsDisabled = true;
                    ErrorReason = e.Message;
                    return;
                }
            }
        }


        public virtual void ProcessDigit(string digit)
        {
            logger.LogInfoMessage("Will process digit: \"" + digit + "\"");
            if (Display == "0" || newInputNumber)
            {
                logger.LogInfoMessage(digit + " will be first number (new number)");
                Display = digit;
                newInputNumber = false;
            }
            else
            {
                logger.LogInfoMessage(digit + " will next digit (after previous digit)");
                Display += digit;
            }
        }

        public virtual void ProcessPoint()
        {
            if (!point && !Display.Contains(Separator))
            {
                point = true;
                Display += Separator;
            }
            else if (newInputNumber)
            {
                Display = "0" + Separator;
                newInputNumber = false;
            }
        }

        private bool calculate()
        {
            try
            {
                switch (operator_)
                {
                    case Operators.ADD:
                        add();
                        break;
                    case Operators.SUBTRACTION:
                        substract();
                        break;
                    case Operators.MULTIPLICATION:
                        multiply();
                        break;
                    case Operators.DIVISION:
                        divide();
                        break;
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
                Display = "ERR";
                IsDisabled = true;
                ErrorReason = e.Message;
                return false;
            }
            return true;
        }

        private void divide()
        {
            if (!newInputNumber || continueCalculation)
            {
                decimal currentNumber = Convert.ToDecimal(Display);
                Console.WriteLine(previousNumber.ToString() + "/" + currentNumber.ToString());
                previousNumber /= currentNumber;
                Display = previousNumber.ToString();
                continueCalculation = false;
            }
        }

        private void multiply()
        {
            if (!newInputNumber || continueCalculation)
            {
                decimal currentNumber = Convert.ToDecimal(Display);
                Console.WriteLine(previousNumber.ToString() + "*" + currentNumber.ToString());
                previousNumber *= currentNumber;
                Display = previousNumber.ToString();
                continueCalculation = false;
            }
        }

        private void substract()
        {
            if (!newInputNumber || continueCalculation)
            {
                decimal currentNumber = Convert.ToDecimal(Display);
                Console.WriteLine(previousNumber.ToString() + "-" + currentNumber.ToString());
                previousNumber -= currentNumber;
                Display = previousNumber.ToString();
                continueCalculation = false;
            }
        }

        private void add()
        {
            if (!newInputNumber || continueCalculation)
            {
                decimal currentNumber = Convert.ToDecimal(Display);
                Console.WriteLine(previousNumber.ToString() + "+" + currentNumber.ToString());
                previousNumber += currentNumber;
                Display = previousNumber.ToString();
                continueCalculation = false;
            }
        }

        public virtual void clean()
        {
            Display = "0";
            previousNumber = new decimal(0);
            newInputNumber = false;
            point = false;
            IsDisabled = false;
            operator_ = Operators.NONE;
            ErrorReason = "None";
        }

        private void calculateSqrt()
        {
            double newValue = Math.Sqrt(Convert.ToDouble(Display));
            if (!Double.IsNaN(newValue))
            {
                Display = newValue.ToString();
                continueCalculation = true;
            }
            else
            {
                //throw new ArithmeticException("Square root from number, that is less than 0!");
                throw new ArithmeticException("Niedozwolona próba pierwiastkowania liczby ujemnej!");
            }
        }

        private void changeSign()
        {
            decimal current = Convert.ToDecimal(Display);
            current *= -1;
            Display = current.ToString();
        }

        private void calculatePercent()
        {
            decimal value = Convert.ToDecimal(Display);
            decimal factor = new decimal(0.01);
            if (!(previousNumber == 0))
            {
                factor *= previousNumber;
            }
            value = value * factor;
            Display = value.ToString();
            continueCalculation = true;
        }
    }
}