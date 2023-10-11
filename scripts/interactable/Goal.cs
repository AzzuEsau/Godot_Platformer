using Godot;
using System;

public partial class Goal : Area2D {
	#region Variables
		[Export] private int levelIndex = -1;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			HelperUtilities.ValidateCheckPositiveValue(this, nameof(levelIndex), levelIndex, true);
			this.BodyEntered += This_BodyEntered;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
		private async void This_BodyEntered(Node2D node2D) {
			if(node2D is Player) await ((Player)node2D).PlayTransition();
			GetTree().ChangeSceneToFile(GameResources.levels[levelIndex]);
		}
	#endregion
}
