using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calc.Model;

namespace Calc.ViewModel
{
    /// <summary>
    /// Interaction logic for FontPicker.xaml
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            _selectedFont = "Arial";
            _selectedColor = "White";
            _calc = new Model.Calculator();
            ButtonCommand = new RelayCommand(
            new Action<object>(delegate (object arg)
            {
                ProcessInput(arg as string);
                OnPropertyChanged("Display");
                OnPropertyChanged("ButtonsDisabled");
            }));

            logger.LogInfoMessage("Created with Font: " + _selectedColor + " and Color: " + _selectedColor);
        }

        ILogger logger = new Logger(typeof(MainWindowViewModel));

        private Model.Calculator _calc;
        public string Display
        {
            get
            {
                //logger.LogInfoMessage("Get Display from Calculator(Model) object.");
                return _calc.Display;
            }
            set
            {
                if (value != _calc.Display)
                {
                    //logger.LogInfoMessage("Set Display from Calculator(Model) object.");
                    _calc.Display = value;
                    OnPropertyChanged("Display");
                }
                else
                {

                    //logger.LogInfoMessage("Don't Set Display from Calculator(Model) object.");
                }
            }
        }

        public Calculator Calculator
        {
            get
            {
                //logger.LogInfoMessage("Get Calculator(Model) object.");
                return _calc;
            }
            set
            {
                if (value != _calc)
                {
                    //logger.LogInfoMessage("Set Calculator(Model) object.");
                    _calc = value;
                    OnPropertyChanged("Calculator");
                }
                else
                {

                    //logger.LogInfoMessage("Don't Set Calculator(Model) object.");
                }
            }
        }


        public bool ButtonsDisabled
        {
            get
            {
                //logger.LogInfoMessage("Get IsDisabled from Calculator(Model) object.");
                return _calc.IsDisabled;
            }
            set
            {
                //logger.LogInfoMessage("Set IsDisabled from Calculator(Model) object.");
                _calc.IsDisabled = value;
                OnPropertyChanged("ButtonsDisabled");
            }
        }

        private string _selectedFont;
        public string SelectedFont
        {
            get
            {
                //logger.LogInfoMessage("Get Selected Font.");
                return _selectedFont;
            }
            set
            {
                //logger.LogInfoMessage("Set Selected Font.");
                _selectedFont = value;
                OnPropertyChanged("SelectedFont");
            }
        }

        private string _selectedColor;

        public string SelectedColor
        {
            get
            {
                //logger.LogInfoMessage("Get Selected Color.");
                return _selectedColor;
            }
            set
            {
                //logger.LogInfoMessage("Set Selected Color.");
                _selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }

        public ICommand ButtonCommand { get; set; }


        public void OnPropertyChanged(string propertyName)
        {
            //this.VerifyPropertyName(propertyName);

            logger.LogInfoMessage("PropertyChanged: \"" + propertyName + "\"");

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public void ProcessInput(string arg)
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
