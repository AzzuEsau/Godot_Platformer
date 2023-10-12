using Godot;
using System;
using Godot.Collections;

public partial class FileSystem : Node {
	#region Variables
		private const string SAVE_FILE = "user://SAVEFILE.SAVE";
		// [Export] private int vairbaleInEditor;

		public static string fullScreenKey = "fullScreen";
		public static string screenResolutionKey = "screenRes";
		public static string masterVolumeKey = "masterVolume";
		public static string musicVolumeKey = "musicVolume";
		public static string sfxVolumeKey = "sfxVolume";

		public bool isLoaded = false;

		public Dictionary<string, int> gameData = new Dictionary<string, int>() {
			{"playerLifes", 3},

			// Settings
			{fullScreenKey, 1},
			{screenResolutionKey, 1},
			{masterVolumeKey, -10},
			{musicVolumeKey, -10},
			{sfxVolumeKey, -10},
		};
	#endregion

	#region Signals
		[Signal] public delegate void LoadedDataEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			Load();
		}

		public override void _Process(double delta) {

		}

		public override void _ExitTree() {
			SaveData();
			base._ExitTree();
		}
    #endregion

    #region My Methods
    public void Load() {
			FileAccess file = FileAccess.Open(SAVE_FILE, FileAccess.ModeFlags.Read);
			if(file == null)
				SaveData();
			else {
				Dictionary<string, int> loadedFile = (Dictionary<string, int>)file.GetVar();

				// Check if a new field is added to our game data and add it to our file
				foreach(var key in gameData.Keys) 
					if(!loadedFile.Keys.Contains(key)) 
						loadedFile.Add(key, gameData[key]);
				gameData = loadedFile;
				SaveData();
			}
			isLoaded = true;
			EmitSignal(SignalName.LoadedData);
		}

		public void SaveData() {
			FileAccess file = FileAccess.Open(SAVE_FILE, FileAccess.ModeFlags.Write);
			file.StoreVar(gameData, true);
			file.Close();
		}
	#endregion

	#region Get And Set
		public int Number => gameData["number"];
		public int GetPlayerLifes() => gameData["playerLifes"];
		public void SetPlayerLifes(int newLifes) => gameData["playerLifes"] = newLifes;
	#endregion

	#region Events
	#endregion
}
