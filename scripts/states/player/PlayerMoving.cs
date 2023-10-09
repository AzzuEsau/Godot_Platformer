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
		}

		public override void Exit() {
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}

		public override void PhysicsUpdate(double delta) {
			MovePlayer();
			PlaySounds();
		}
	#endregion

    #region My Methods
		private void AnimatePlayer() {
			if (!player.isHurted) {
				if(!player.IsOnFloor()) {
					if(player.Velocity.Y < 0) animator.Play(GameResources.jumpAnimation);
					else animator.Play(GameResources.fallAnimation);
				}
				else animator.Play(GameResources.walkAnimation);
			}
			sprite.FlipH = player.direction < 0;
		}

		private void MovePlayer() {
			player.ApplyGravity();
			float ySpeed = player.isJumping ? player.Velocity.Y - player.GetJumpSpeed() : player.Velocity.Y;
			player.Velocity = new Vector2(player.direction * player.speed, ySpeed);

			player.MoveAndSlide();
		}

		private void PlaySounds() {
			if(player.isJumping) audioJump.Play();
		}
    #endregion

    #region Events
    #endregion
}
