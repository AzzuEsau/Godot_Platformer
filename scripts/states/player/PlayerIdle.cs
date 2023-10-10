using Godot;
using System;

public partial class PlayerIdle : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animator;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
		}

		public override void Exit() {
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}

		public override void PhysicsUpdate(double delta) {
			MovePlayer();
		}
	#endregion

    #region My Methods
		private void AnimatePlayer() {
			if (!player.isHurted) animator.Play("idle");
		}

		private void MovePlayer() {
			player.ApplyGravity();
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
