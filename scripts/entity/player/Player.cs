using Godot;
using System;

public partial  class Player : CharacterBody2D {
	#region Variables
		#region Exports
			[Export] private AnimationPlayer animator;
			[Export] private Sprite2D sprite;

			[Export] private Label fruitsLabel;
		#endregion

		private float direction;
		private float speed = 200;
		private float jumpSpeed = 250;
		private bool isJumping;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			Global global = (Global)GetNode(GameResources.GlobalAutoload);
			global.SetPlayer(this);
			global.FruitsCollectedChanged += Global_FruitsCollectedChanged;
		}

		public override void _Process(double delta) {
			AnimatePlayer();
		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
			MovePlayer();
		}
    #endregion

    #region My Methods
		private void ReadInput() {
			direction = Input.GetAxis(GameResources.KeyMoveLeft, GameResources.KeyMoveRight);
			isJumping = Input.IsActionJustPressed(GameResources.KeyJump) && IsOnFloor();
		}

		private void MovePlayer() {
			float ySpeed = isJumping ? Velocity.Y - jumpSpeed : Velocity.Y + GameResources.Gravity;
			Velocity = new Vector2(direction * speed, ySpeed);

			MoveAndSlide();
		}

		private void AnimatePlayer() {
			if(direction != 0) animator.Play("walk");
			else animator.Play("idle");

			if(direction != 0) sprite.FlipH = direction < 0;
		}
    #endregion

    #region Events
		private void Global_FruitsCollectedChanged(int fruits) {
			fruitsLabel.Text = "FRUTAS: " + fruits.ToString();
		}
    #endregion
}
