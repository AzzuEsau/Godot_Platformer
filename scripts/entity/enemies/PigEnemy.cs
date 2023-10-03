using Godot;
using System;
using System.Collections;

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
		public async void testOfTimerInAwait() {
			await ToSignal(GetTree().CreateTimer(3), Timer.SignalName.Timeout);
		}
    #endregion

    #region Events
    #endregion
}
