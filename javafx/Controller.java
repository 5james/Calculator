package application;

import java.math.BigDecimal;
import java.math.MathContext;

import javafx.application.Platform;
import javafx.beans.property.StringProperty;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.Node;
import javafx.scene.control.Button;
import javafx.scene.control.TextField;
import javafx.scene.layout.GridPane;

public class Controller {

	private enum Operators {
		NONE, ADD, SUBTRACTION, MULTIPLICATION, DIVISION
	}

	private Operators operator = Operators.NONE;

	private Boolean point = false;
	private Boolean newInputNumber = false;
	private Boolean continueCalculation = false;

	private BigDecimal previousNumber = new BigDecimal(0);

	@FXML
	public void initialize() {
		Platform.runLater(new Runnable() {
			@Override
			public void run() {
				updateDisplay("0");

				adjustFont(grid.getChildren());

				grid.widthProperty().addListener(event -> adjustFont(grid.getChildren()));
				grid.heightProperty().addListener(event -> adjustFont(grid.getChildren()));
			}
		});
	}

	@FXML
	private TextField display;

	@FXML
	private GridPane grid;

	@FXML
	public void ClickDigit(ActionEvent event) {
		String current = getDisplayed().getValue();
		String digit = ((Button) event.getSource()).getText();

		if (current.equals("0") || newInputNumber) {
			updateDisplay(digit);
			newInputNumber = false;
		} else {
			updateDisplay(current + digit);
		}
	}

	@FXML
	public void ClickPoint(ActionEvent event) {
		String current = display.textProperty().getValue();
		if (!point && !current.contains(".") && !newInputNumber) {
			point = true;
			updateDisplay(current + ".");
		} else if (newInputNumber) {
			updateDisplay("0.");
			newInputNumber = false;
		}
	}

	@FXML
	public void ClickClean(ActionEvent event) {
		clean();
		setButtonClickable(grid.getChildren(), false);
	}

	@FXML
	public void ClickNonArithOperation(ActionEvent event) {
		String operatorStr = ((Button) event.getSource()).getText();
		
		try {
			switch (operatorStr) {
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
			default:
				;
			}
		} catch (ArithmeticException | NumberFormatException e) {
			System.out.println(e.getMessage());
			updateDisplay("ERR");
			setButtonClickable(grid.getChildren(), true);
		}
	}

	@FXML
	public void ClickArithOperation(ActionEvent event) {
		point = false;
		String operatorStr = ((Button) event.getSource()).getText();
		calculate();
		try {
			previousNumber = new BigDecimal(getDisplayed().getValue());
		} catch (NumberFormatException e) {
			previousNumber = new BigDecimal(0);
		}
		switch (operatorStr) {
		case "+":
			operator = Operators.ADD;
			break;
		case "-":
			operator = Operators.SUBTRACTION;
			break;
		case "*":
			operator = Operators.MULTIPLICATION;
			break;
		case "/":
			operator = Operators.DIVISION;
			break;
		default:
			operator = Operators.NONE;
		}
		// updateDisplay(previousNumber.toString());
		// System.out.println(previousNumber.toString() + " " + operatorStr);

		newInputNumber = true;
	}

	private void setButtonClickable(ObservableList<Node> buttons, Boolean clickable) {
		for (Node btn : buttons) {
			if (Button.class.equals(btn.getClass())) {
				if (!((Button) btn).getText().toUpperCase().equals("C")) {
					((Button) btn).setDisable(clickable);
				}
			}
		}
	}

	private void calculate() {
		try {
			switch (operator) {
			case ADD:
				add();
				break;
			case SUBTRACTION:
				subtract();
				break;
			case MULTIPLICATION:
				multiply();
				break;
			case DIVISION:
				divide();
				break;
			default:
				// operator = Operators.NONE;
			}
		} catch (ArithmeticException | NumberFormatException e) {
			System.out.println(e.getMessage());
			updateDisplay("ERR");
			setButtonClickable(grid.getChildren(), true);
		}
	}

	private void divide() throws ArithmeticException {
		if (!newInputNumber || continueCalculation) {
			updateDisplay(
					previousNumber.divide(new BigDecimal(getDisplayed().getValue()), MathContext.DECIMAL64).toString());
			continueCalculation = false;
		}
	}

	private void multiply() throws ArithmeticException {
		if (!newInputNumber || continueCalculation) {
			updateDisplay(previousNumber
					.multiply(new BigDecimal(getDisplayed().getValue(), MathContext.DECIMAL64).stripTrailingZeros())
					.toString());
			continueCalculation = false;
		}
	}

	private void subtract() throws ArithmeticException {
		if (!newInputNumber || continueCalculation) {
			updateDisplay(previousNumber
					.subtract(new BigDecimal(getDisplayed().getValue(), MathContext.DECIMAL64).stripTrailingZeros())
					.toString());
			continueCalculation = false;
		}
	}

	private void add() throws ArithmeticException {
		if (!newInputNumber || continueCalculation) {
			updateDisplay(previousNumber
					.add(new BigDecimal(getDisplayed().getValue(), MathContext.DECIMAL64).stripTrailingZeros())
					.toString());
			continueCalculation = false;
		}
	}

	private void updateDisplay(String str) {
		getDisplayed().set(str);
	}

	private StringProperty getDisplayed() {
		return display.textProperty();
	}

	private void changeSign() {
		BigDecimal value = new BigDecimal(getDisplayed().getValue());
		updateDisplay(value.negate().toString());
	}

	private void calculateSqrt() throws ArithmeticException {
		Double newValue = new Double(StrictMath.sqrt(new Double(getDisplayed().getValue())));
		if (!newValue.isNaN()) {
		updateDisplay(newValue.toString());
		continueCalculation = true;
		}
		else {
			throw new ArithmeticException("Square root from number, that is less than 0");
		}
	}

	private void calculatePercent() {
		BigDecimal value = new BigDecimal(getDisplayed().getValue());
		BigDecimal factor = new BigDecimal("0.01");
		if (!previousNumber.equals(new BigDecimal("0"))) {
			factor = factor.multiply(previousNumber);
		}

		updateDisplay(value.multiply(factor).stripTrailingZeros().toString());
		continueCalculation = true;
	}

	private void clean() {
		updateDisplay("0");
		previousNumber = new BigDecimal(0);
		newInputNumber = false;
		point = false;
	}

	private void adjustFont(ObservableList<Node> nodes) {
		Double FontSizeHeight = new Double((grid.getHeight() / 28));
		Double FontSizeWidth = new Double((grid.getHeight() / 39));
		Double desiredFontSize = Math.min(FontSizeHeight, FontSizeWidth);
		//System.out.println(desiredFontSize.toString());

		for (Node node : nodes) {
			if (Button.class.equals(node.getClass())) {
				((Button) node).setStyle("-fx-font-size: " + desiredFontSize + ";");
			}

			if (TextField.class.equals(node.getClass())) {
				((TextField) node).setStyle("-fx-font-size: " + (desiredFontSize * 1.25) + ";");
			}
		}
	}
}