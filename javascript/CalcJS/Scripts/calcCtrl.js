app.controller("calcCtrl", function ($scope) {

    /* ---VARIABLES--- */
    /* PUBLIC */
    $scope.Display = "0";
    $scope.IsDisabled = false;
    $scope.ErrorReason = "";

    /* PRIVATE */
    var OperatorsEnum = {
        NONE: 1,
        ADD: 2,
        SUBTRACTION: 3,
        MULTIPLICATION: 4,
        DIVISION: 5
    };

    var operator = OperatorsEnum.NONE;
    var point = false;
    var newInputNumber = false;
    var continueCalculation = false;
    var previousNumber = Number(0);



    /* ---FUNCTIONS--- */
    /* PUBLIC */
    $scope.processInput = function (arg) {
        var str = arg.toLowerCase();
        switch (str) {
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
                processDigit(Number(str));
                break;
            case ".":
                processPoint();
                break;
            case "C":
            case "c":
                clean();
                break;
            case "+":
            case "-":
            case "*":
            case "/":
            case "=":
                processArithOperation(str);
                break;
            case "sqrt":
            case "%":
            case "+/-":
                processNonArithOperation(str);
                break;
        }
        console.log("end of input");
    }

    /* PRIVATE */
    function processDigit(digit) {
        console.log('digit' + digit);
        if ($scope.Display == "0" || newInputNumber) {
            $scope.Display = digit;
            newInputNumber = false;
        }
        else {
            $scope.Display = $scope.Display.toString().concat(digit);
        }
    }

    function processPoint() {
        console.log('point');
        if (point == false && $scope.Display.toString().includes(".") == false && newInputNumber == false) {
            console.log("aaa");
            point = true;
            $scope.Display = $scope.Display.toString().concat(".");
        }
        else if (newInputNumber) {
            console.log("bbb");
            $scope.Display = "0.";
            newInputNumber = false;
        }
    }

    function calculate() {
        try {
            switch (operator) {
                case OperatorsEnum.ADD:
                    console.log('add');
                    add();
                    break;
                case OperatorsEnum.SUBTRACTION:
                    console.log('-');
                    substract();
                    break;
                case OperatorsEnum.MULTIPLICATION:
                    console.log("m");
                    multiply();
                    break;
                case OperatorsEnum.DIVISION:
                    console.log("d");
                    divide();
                    break;
            }
        }
        catch (e) {
            console.log("err1");
            $scope.Display = "ERR";
            $scope.IsDisabled = true;
            $scope.ErrorReason = e;
            return false;
        }
        return true;
    }

    function divide() {
        if (!newInputNumber || continueCalculation) {
            var currentNumber = Number($scope.Display);
            previousNumber /= currentNumber;
            if (!Number.isFinite(previousNumber)) {
                throw "Niedozwolone dzielenie przez 0!"
            }
            if (Number.isNaN(previousNumber)) {
                throw "Niedozwolona operacja!"
            }
            $scope.Display = previousNumber.toString();
            continueCalculation = false;
        }
    }

    function multiply() {
        if (!newInputNumber || continueCalculation) {
            var currentNumber = Number($scope.Display);
            previousNumber *= currentNumber;
            if (Number.isNaN(previousNumber)) {
                throw "Niedozwolona operacja!"
            }
            $scope.Display = previousNumber.toString();
            continueCalculation = false;
        }
    }

    function substract() {
        if (!newInputNumber || continueCalculation) {
            var currentNumber = Number($scope.Display);
            previousNumber -= currentNumber;
            if (Number.isNaN(previousNumber)) {
                throw "Niedozwolona operacja!"
            }
            $scope.Display = previousNumber.toString();
            continueCalculation = false;
        }
    }

    function add() {
        if (!newInputNumber || continueCalculation) {
            var currentNumber = Number($scope.Display);
            previousNumber += currentNumber;
            if (Number.isNaN(previousNumber)) {
                throw "Niedozwolona operacja!"
            }
            $scope.Display = previousNumber.toString();
            continueCalculation = false;
        }
    }

    function clean() {
        console.log('clean');
        $scope.Display = "0";
        previousNumber = Number(0);
        newInputNumber = false;
        point = false;
        $scope.IsDisabled = false;
        continueCalculation = false;
        operator = OperatorsEnum.NONE;
        $scope.ErrorReason = "";
    }

    function calculateSqrt() {
        var newValue = Math.sqrt(Number($scope.Display));
        if (!Number.isNaN(newValue)) {
            $scope.Display = newValue.toString();
            continueCalculation = true;
        }
        else {
            //throw new ArithmeticException("Square root from number, that is less than 0!");
            throw "Niedozwolona próba pierwiastkowania liczby ujemnej!";
        }
    }

    function changeSign() {
        var current = Number($scope.Display);
        current *= -1;
        $scope.Display = current.toString();
    }

    function calculatePercent() {
        var value = Number($scope.Display);
        var factor = Number(0.01);
        if (!(previousNumber == 0)) {
            factor *= previousNumber;
        }
        value = value * factor;
        $scope.Display = value.toString();
        continueCalculation = true;
    }

    function processArithOperation(arg) {
        console.log('ar');
        point = false;
        if (calculate()) {
            try {
                previousNumber = Number($scope.Display);
            }
            catch (e) {
                console.log("err2");
                $scope.Display = "ERR";
                $scope.IsDisabled = true;
                $scope.ErrorReason = e;
                return;
            }
            switch (arg) {
                case "+":
                    operator = OperatorsEnum.ADD;
                    break;
                case "-":
                    operator = OperatorsEnum.SUBTRACTION;
                    break;
                case "*":
                    operator = OperatorsEnum.MULTIPLICATION;
                    break;
                case "/":
                    operator = OperatorsEnum.DIVISION;
                    break;
                case "=":
                    operator = OperatorsEnum.NONE;
                    break;
                default:
                    operator = Operators.NONE;
                    break;

            }

            newInputNumber = true;
        }
        else {
            console.log("Did not calculate previous operation, so couldn't set new operation to calculate");
        }
    }

    function processNonArithOperation(arg) {
        console.log('nonar');
        try {
            switch (arg) {
                case "%":
                    point = false;
                    newInputNumber = true;
                    calculatePercent();
                    break;
                case "sqrt":
                    point = false;
                    newInputNumber = true;
                    calculateSqrt();
                    break;
                case "+/-":
                    changeSign();
                    break;
            }
        }
        catch (e) {
            console.log("err3");
            $scope.Display = "ERR";
            $scope.IsDisabled = true;
            $scope.ErrorReason = e;
            return;
        }
    }
});