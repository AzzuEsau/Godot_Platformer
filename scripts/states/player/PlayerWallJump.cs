using Godot;
using System;

public partial class PlayerWallJump : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animator;
		[Export] private Sprite2D sprite;
		[Export] private AudioStreamPlayer2D audioJump;

		private float direction;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
			player.canChangeState = false;
			player.jumpsLeft = 0;

			direction = player.direction;
			sprite.FlipH = direction > 0;

			audioJump.Play();
			WaitForAnimation();
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
				if(player.Velocity.Y < 0) animator.Play(GameResources.jumpAnimation);
				else animator.Play(GameResources.fallAnimation);
			}
		}

		private void MovePlayer() {
			player.Velocity = new Vector2(0, 0);

			float xVelocity = Mathf.Lerp(player.Velocity.X, player.Velocity.X - player.speed * direction, .9F);
			float yVelocity = Mathf.Lerp(player.Velocity.Y, player.Velocity.Y - player.GetJumpSpeed() * 0.75F, .9F);

			player.Velocity = new Vector2(xVelocity, yVelocity);


			player.MoveAndSlide();
		}

		private async void WaitForAnimation() {
			await ToSignal(GetTree().CreateTimer(.25F), Timer.SignalName.Timeout);
			player.canChangeState = true;
		}
    #endregion

    #region Events
    #endregion
}
