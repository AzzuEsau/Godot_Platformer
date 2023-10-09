using Godot;
using System;

public partial class PathFolloable : Path2D {
	#region Variables
		[Export] protected CharacterBody2D platofrmCharacter;
		[Export] protected PathFollow2D pathFollow2D;
		[Export] protected float speed = 0.2F;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {
			MoveElement(delta);
		}
	#endregion

	#region My Methods
		protected void MoveElement(double delta) {
			platofrmCharacter.GlobalPosition = pathFollow2D.GlobalPosition;

			// Change the position following the progress of the pathFollow
			if(pathFollow2D.ProgressRatio < 1) pathFollow2D.ProgressRatio += speed * (float)delta;
			else pathFollow2D.ProgressRatio = 0;
		}
	#endregion

	#region Events
	#endregion
}
