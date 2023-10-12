using Godot;
using System;

public partial class Global : Node {
	#region Variables
		[Export] private int fruitsCollected = 0;
		[Export] private Player player;

		[Export] private AudioStreamPlayer fruitAudio;

		[Export] public int playerLifes = 0;

		private FileSystem fileSystem;
	#endregion

	#region Signals
		[Signal] public delegate void FruitsCollectedChangedEventHandler(int fruits);
		[Signal] public delegate void FruitCollectedEventHandler(int plusFruits);
	#endregion

	#region Godot Methdos
		public override async void _Ready() {
			FruitCollected += This_FruitCollected;
			fileSystem = GetNode<FileSystem>(GameResources.FileSystemAutoload);

			// Wait until the file is loaded
			await ToSignal(fileSystem, FileSystem.SignalName.LoadedData);
			AddLifes(fileSystem.GetPlayerLifes());
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
		public void SetPlayer(Player player) =>  this.player = player;
		public Player GetPlayer() =>  player;

		public void SaveGame() {
			fileSystem.SetPlayerLifes(playerLifes);
			fileSystem.SaveData();
		}

		public void AddLifes(int plusLifes) {
			playerLifes += plusLifes;

			if(plusLifes < 0 && playerLifes > 0) 
				GetTree().ReloadCurrentScene();

			if(playerLifes <= 0) {
				playerLifes = GameResources.resetLifes;
				GetTree().ChangeSceneToFile(GameResources.levels[0]);
			}


			if(playerLifes > GameResources.maxLifes) playerLifes = GameResources.maxLifes;

			fileSystem.SetPlayerLifes(playerLifes);
			fileSystem.SaveData();
		}
	#endregion

	#region Events
		private void This_FruitCollected(int plusFruits) {
			fruitsCollected += plusFruits;
			fruitAudio.Play();
			EmitSignal(SignalName.FruitsCollectedChanged, fruitsCollected);
		}
	#endregion
}
