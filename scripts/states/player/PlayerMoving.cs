using Godot;
using System;

public partial class PlayerMoving : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animator;
		[Export] private Sprite2D sprite;
		[Export] private AudioStreamPlayer2D audioJump;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
			player.jumpsLeft = player.MaxJumps;
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
			if (!player.isHurted) animator.Play(GameResources.walkAnimation);
			if(player.direction != 0) sprite.FlipH = player.direction < 0;
		}

		private void MovePlayer() {
			player.ApplyGravity();

			float ySpeed = player.Velocity.Y;
			if(player.isJumping || player.jumpBuffer > 0) {
				player.jumpsLeft -= 1;
				player.Velocity = new Vector2(player.direction * player.speed, 0);
				ySpeed = player.Velocity.Y - player.GetJumpSpeed();
				player.coyoteTime = 0;
				PlayJumpSound();
			}
			player.Velocity = new Vector2(player.direction * player.speed, ySpeed);

			player.MoveAndSlide();
		}

		private void PlayJumpSound() => audioJump.Play();
    #endregion

    #region Events
    #endregion
}
