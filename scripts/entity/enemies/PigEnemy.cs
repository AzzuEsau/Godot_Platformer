using Godot;
using System;

public partial class PigEnemy : Character {
	#region Variables
		// [Export] private int vairbaleInEditor;
		[Export] private FiniteStateMachine finiteStateMachine;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			
		}

		public override void _Process(double delta) {
		}

		public override void _PhysicsProcess(double delta) {
		}
    #endregion

    #region My Methods
    #endregion

    #region Events
    #endregion
}
