using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Settings : CanvasLayer {
	#region Variables
		[Export] private AnimationPlayer animation;
		[Export] private Button acceptButton;

		[ExportGroup("Settings Buttons")]
			[ExportSubgroup("Video")]
				[Export] private CheckBox fullScreen;
				[Export] private OptionButton screenResolution;
			[ExportSubgroup("Audio")]
				[Export] private Slider masterVolume;
				[Export] private Slider musicVolume;
				[Export] private Slider sfxVolume;


		private FileSystem fileSystem;

		private static string animationStart = "start";
		private static string animationFinish = "finish";
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override async void _Ready() {
			fileSystem = GetNode<FileSystem>(GameResources.FileSystemAutoload);



			this.VisibilityChanged += CanvasLayer_VisibilityChanged;
			acceptButton.Pressed += AcceptButton_ButtonPressed;


			fullScreen.Pressed += FullScreen_Pressed;
			screenResolution.ItemSelected += ScreenResolution_ItemSelected;
			masterVolume.ValueChanged += MasterVolume_ValueChanged;
			musicVolume.ValueChanged += MusicVolume_ValueChanged;
			sfxVolume.ValueChanged += SFXVolume_ValueChanged;

			if(!fileSystem.isLoaded) await ToSignal(fileSystem, FileSystem.SignalName.LoadedData);
			PreloadInfo();
		}

		public override void _Process(double delta) {

			}
	#endregion

	#region My Methods
		private void UpdateVolumes(int busIndex, double value) {
			AudioServer.SetBusVolumeDb(busIndex, (float)value);
		}

		private void PreloadInfo() {
			fullScreen.ButtonPressed = fileSystem.gameData[FileSystem.fullScreenKey] == 1;
			screenResolution.Selected = fileSystem.gameData[FileSystem.screenResolutionKey];

			masterVolume.Value = fileSystem.gameData[FileSystem.masterVolumeKey];
			musicVolume.Value = fileSystem.gameData[FileSystem.musicVolumeKey];
			sfxVolume.Value = fileSystem.gameData[FileSystem.sfxVolumeKey];


			ScreenResolution_ItemSelected(screenResolution.Selected);
			FullScreen_Pressed();
			MasterVolume_ValueChanged(masterVolume.Value);
			MusicVolume_ValueChanged(musicVolume.Value);
			SFXVolume_ValueChanged(sfxVolume.Value);
		}
	#endregion

	#region Events
		private void CanvasLayer_VisibilityChanged() {
			if(Visible) animation.Play(animationStart);
		}

		private async void AcceptButton_ButtonPressed() {
			animation.Play(animationFinish);
			await ToSignal(animation, AnimationPlayer.SignalName.AnimationFinished);
			Hide();
		}


		private void FullScreen_Pressed() {
			if(fullScreen.ButtonPressed) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

			screenResolution.Disabled = fullScreen.ButtonPressed;

			fileSystem.gameData[FileSystem.fullScreenKey] = fullScreen.ButtonPressed ? 1 : 0;
			fileSystem.SaveData();
		}


		private void ScreenResolution_ItemSelected(long index) {
			Vector2I size;
				switch (index) {
					case 0: size = new Vector2I(1152, 648); break;
					case 1: size = new Vector2I(640, 360); break;
					case 2: size = new Vector2I(1280, 720); break;
					case 3: size = new Vector2I(1920, 1080); break;
					default: size = new Vector2I(1152, 648); break;
				}
			DisplayServer.WindowSetSize(size);
			
			fileSystem.gameData[FileSystem.screenResolutionKey] = (int)index;
			fileSystem.SaveData();
		}

		private void MasterVolume_ValueChanged(double volume) {
			UpdateVolumes(0, volume);

			fileSystem.gameData[FileSystem.masterVolumeKey] = (int)volume;
			fileSystem.SaveData();
		}

		private void MusicVolume_ValueChanged(double volume) {
			UpdateVolumes(1, volume);
			fileSystem.gameData[FileSystem.musicVolumeKey] = (int)volume;
			fileSystem.SaveData();
		}

		private void SFXVolume_ValueChanged(double volume) {
			UpdateVolumes(2, volume);
			fileSystem.gameData[FileSystem.sfxVolumeKey] = (int)volume;
			fileSystem.SaveData();
		}

	#endregion
}
