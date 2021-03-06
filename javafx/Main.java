package application;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.stage.Stage;
import javafx.scene.Parent;
import javafx.scene.Scene;

public class Main extends Application {

	@Override
	public void start(Stage primaryStage) {
		try {
			Parent root = FXMLLoader.load(getClass().getResource("fxcalc.fxml"));
			Scene scene = new Scene(root, 400, 300);
			scene.getStylesheets().add(getClass().getResource("application.css").toExternalForm());
			primaryStage.setScene(scene);
			primaryStage.setTitle("FxCalcGuzek");
			primaryStage.show();
			primaryStage.setMinHeight(300);
			primaryStage.setMinWidth(400);
			primaryStage.setMaxHeight(900);
			primaryStage.setMaxWidth(1200);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public static void main(String[] args) {
		launch(args);
	}
}
