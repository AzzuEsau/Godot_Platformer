using Godot;
using System;

public partial class Camera : Camera2D {
	#region Variables
		// [Export] private int vairbaleInEditor;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			this.TopLevel = true;
		}

		public override void _PhysicsProcess(double delta) {
			GlobalPosition = new Vector2(((Node2D)GetParent()).GlobalPosition.X, 450);
		}
	#endregion

	#region My Methods
	#endregion

	#region Events
	#endregion
}
