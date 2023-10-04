using Godot;
using System;

public partial class NormalState : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animator;
		[Export] private Sprite2D sprite;

		public bool isHurted = false;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
			isHurted = false;
		}

		public override void Exit() {
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}

		public override void PhysicsUpdate(double delta) {
			ReadInput();
			MovePlayer();
		}
	#endregion

    #region My Methods
		private void AnimatePlayer() {
			if (!isHurted) {
				if(player.direction != 0) animator.Play("walk");
				else animator.Play("idle");
			}

			if(player.direction != 0) sprite.FlipH = player.direction < 0;
		}

		private void ReadInput() {
			player.direction = Input.GetAxis(GameResources.KeyMoveLeft, GameResources.KeyMoveRight);
			player.isJumping = Input.IsActionJustPressed(GameResources.KeyJump) && player.IsOnFloor();
		}

		private void MovePlayer() {
			float ySpeed = player.isJumping ? player.Velocity.Y - player.GetJumpSpeed() : player.Velocity.Y;
			player.Velocity = new Vector2(player.direction * player.speed, ySpeed);

			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
