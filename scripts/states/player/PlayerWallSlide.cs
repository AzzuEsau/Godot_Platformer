using Godot;
using System;

public partial class PlayerWallSlide : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animator;
		[Export] private Sprite2D sprite;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
			player.Velocity = Vector2.Zero;
			player.jumpsLeft = 1;
		}

		public override void Exit() {
			player.jumpsLeft = 0;
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
			if (!player.isHurted) animator.Play(GameResources.wallSlideAnimation);
		}

		private void MovePlayer() {
			float ySpeed = player.Velocity.Y + (GameResources.Gravity / 2);
			player.Velocity = new Vector2(player.direction * player.speed, ySpeed);
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
