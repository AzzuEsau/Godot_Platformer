using Godot;
using System;

public partial class MainMenu : Node {
	#region Variables
		[Export] private Button playButton;
		[Export] private Button settingsButton;
		[Export] private Button exitButton;
		[Export] CanvasLayer SettingsLayer;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			playButton.Pressed += PlayButton_Pressed;
			settingsButton.Pressed += SettingsButton_Pressed;
			exitButton.Pressed += ExitButton_Pressed;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
		private void PlayButton_Pressed() => GetTree().ChangeSceneToFile(GameResources.levels[1]);
		// private void SettingsButton_Pressed() => DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		private void SettingsButton_Pressed() => SettingsLayer.Visible = true;
		private void ExitButton_Pressed() => GetTree().Quit();
	#endregion
}
