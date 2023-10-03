using Godot;
using System;

public partial class WalkAround : State {
	#region Variables
		[Export] Character character;

		[Export] RayCast2D rayCastFloor;
		[Export] RayCast2D rayCastWall;
		[Export] Node2D rayCasts;
		[Export] Timer rayTimer;
		[Export] Sprite2D sprite;
		[Export] AnimationPlayer animationPlayer;
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

	#region States Methods
		public override void Update(double delta) {
			if(character.canChangeDirection) {
				if (rayCastWall.IsColliding() || !rayCastFloor.IsColliding()) { 
					character.canChangeDirection = false;
					rayTimer.Start();
					character.direction *= -1;
				}
			}
			animationPlayer.Play(character.walkAnimation);
			Flip();
		}

		public override void PhysicsUpdate(double delta) {
			float ySpeed = character.IsOnFloor() ? character.Velocity.Y :character. Velocity.Y + GameResources.Gravity;
			character.Velocity = new Vector2(character.direction * character.GetSpeed(), ySpeed);
			character.MoveAndSlide();
		}

		public override void Enter() {
			character.canChangeDirection = true;
			rayTimer.Timeout += RayTimer_Timeout;
		}

		public override void Exit() {
			rayTimer.Timeout -= RayTimer_Timeout;
		}
	#endregion

    #region My Methods
		private void Flip() {
			rayCasts.Scale = new Vector2(-character.direction, rayCasts.Scale.Y);
			sprite.FlipH = character.direction == 1;
		}
    #endregion

    #region Events
		private void RayTimer_Timeout() {
			character.canChangeDirection = true;
		}
    #endregion
}
