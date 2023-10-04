using Godot;
using System;
using System.Collections;

public partial class PigEnemy : Character {
	#region Variables
		[Export] private RayCast2D playerDetector;	
		[Export] private LifeComponent lifeComponent;	

		private Player player;

		[ExportGroup("States")]	
			[Export] private FiniteStateMachine finiteStateMachine;
			[Export] private WalkAround walkAroundState;
			[Export] private RunForPlayer runForPlayerState;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			runForPlayerState.LostPlayer += RunForPlayer_LostPlayer;
			lifeComponent.OnHealthChange += LifeComponent_OnHealthChange;
		}

		public override void _Process(double delta) {
			if(CanSeepPlayer()) {
				runForPlayerState.EmitSignal(State.SignalName.Transition, runForPlayerState, runForPlayerState.Name);
			}
		}

		public override void _PhysicsProcess(double delta) {

		}
    #endregion

    #region My Methods
		public async void testOfTimerInAwait() {
			await ToSignal(GetTree().CreateTimer(3), Timer.SignalName.Timeout);
		}

		private bool CanSeepPlayer() {
			if(playerDetector.IsColliding()) {
				Node2D collision = (Node2D)playerDetector.GetCollider();
				return collision.IsInGroup("Player");
			}
			return false;
		}
    #endregion

    #region Events
		private void RunForPlayer_LostPlayer() => walkAroundState.EmitSignal(State.SignalName.Transition, walkAroundState, walkAroundState.Name);

		private void LifeComponent_OnHealthChange(int currentLife, int damageAmount) {
			if(damageAmount > 0) runForPlayerState.EmitSignal(State.SignalName.Transition, runForPlayerState, runForPlayerState.Name);
		}	
    #endregion
}
