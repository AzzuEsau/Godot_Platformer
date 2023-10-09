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

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {

		}
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
			if (!player.isHurted) {
				if(!player.IsOnFloor()) {
					if(player.Velocity.Y < 0) animator.Play(GameResources.jumpAnimation);
					else animator.Play(GameResources.fallAnimation);
				}
				else animator.Play("idle");
			}
		}


		private void MovePlayer() {
			player.ApplyGravity();
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
