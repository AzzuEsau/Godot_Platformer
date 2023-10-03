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

		protected bool canChangeDirection = true;
		protected int direction = 1;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			rayTimer.Timeout += RayTimer_Timeout;
		}

		public override void _Process(double delta) {

		}
    #endregion

	#region States Methods
		public override void Update(double delta) {
			if(canChangeDirection) {
				if (rayCastWall.IsColliding() || !rayCastFloor.IsColliding()) { 
					canChangeDirection = false;
					rayTimer.Start();
					direction *= -1;
				}
			}
			Flip();

			animationPlayer.Play(character.walkAnimation);
		}

		public override void PhysicsUpdate(double delta) {
			float ySpeed = character.IsOnFloor() ? character.Velocity.Y :character. Velocity.Y + GameResources.Gravity;
			character.Velocity = new Vector2(direction * character.getSpeed(), ySpeed);
			character.MoveAndSlide();
		}

		public override void Enter() {
			// throw new NotImplementedException();
		}

		public override void Exit() {
			// throw new NotImplementedException();
		}
	#endregion

    #region My Methods
		private void Flip() {
			rayCasts.Scale = new Vector2(-direction, rayCasts.Scale.Y);
			sprite.FlipH = direction == 1;
		}
    #endregion

    #region Events
		private void RayTimer_Timeout() {
			canChangeDirection = true;
		}
    #endregion
}
