using Godot;
using System;

public partial class PlayerAir : State {
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
			if(player.jumpsLeft == player.MaxJumps) player.jumpsLeft -= 1;
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
			// Check if the animation is double jump and if is playing
			bool isDoubleJumping = animator.AssignedAnimation == GameResources.doubleJumpAnimation && animator.CurrentAnimationPosition < animator.CurrentAnimationLength;

			if (!player.isHurted) {
				if(player.isJumping) animator.Play(GameResources.doubleJumpAnimation);

				else if (!isDoubleJumping) {
					if(player.Velocity.Y < 0) animator.Play(GameResources.jumpAnimation);
					else animator.Play(GameResources.fallAnimation);
				}
			}

			if(player.direction != 0) sprite.FlipH = player.direction < 0;
		}


		private void MovePlayer() {
			player.ApplyGravity();
			float ySpeed = player.Velocity.Y;

			// Apply the jump effect
			if(player.isJumping && player.jumpsLeft > 0) {
				player.jumpsLeft -= 1;
				player.Velocity = new Vector2(player.direction * player.speed, 0);
				ySpeed = player.Velocity.Y - player.GetJumpSpeed();
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
