using Godot;
using System;

// [Tool]
public partial class Platform : Path2D {
	#region Variables
		[Export] private CharacterBody2D platofrmCharacter;
		[Export] private PathFollow2D pathFollow2D;
		[Export] private float speed = 0.2F;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {
			platofrmCharacter.GlobalPosition = pathFollow2D.GlobalPosition;

			// Update the progress
			if(pathFollow2D.ProgressRatio < 1) pathFollow2D.ProgressRatio += speed * (float)delta;
			else pathFollow2D.ProgressRatio = 0;
		}
	#endregion

	#region My Methods
	#endregion

	#region Events
	#endregion
}
