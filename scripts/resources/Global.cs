using Godot;
using System;

public partial class Global : Node {
	#region Variables
		[Export] private int fruitsCollected = 0;
		[Export] private Player player;

		[Export] private AudioStreamPlayer fruitAudio;
	#endregion

	#region Signals
		[Signal] public delegate void FruitsCollectedChangedEventHandler(int fruits);
		[Signal] public delegate void FruitCollectedEventHandler(int plusFruits);
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			FruitCollected += This_FruitCollected;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
		public void SetPlayer(Player player) =>  this.player = player;
		public Player GetPlayer() =>  player;
	#endregion

	#region Events
		private void This_FruitCollected(int plusFruits) {
			fruitsCollected += plusFruits;
			fruitAudio.Play();
			EmitSignal(SignalName.FruitsCollectedChanged, fruitsCollected);
		}
	#endregion
}
