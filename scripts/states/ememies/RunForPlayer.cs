using Godot;
using System;

public partial class RunForPlayer : State {
	#region Variables
		[Export] private Character character;
		[Export] AnimationPlayer animationPlayer;


		[Export] Node2D rayCasts;
		[Export] Sprite2D sprite;

		private Player player;
	#endregion

	#region Signals
		[Signal] public delegate void LostPlayerEventHandler();
	#endregion

	#region Godot Methdos
    #endregion

	#region States
		public override void Enter() {
			Global global = (Global)GetNode(GameResources.GlobalAutoload);
			if(global != null)
				player = global.GetPlayer();
		}

		public override void Exit() {
			player = null;
		}

		public override void Update(double delta) {
			if(player == null) return;

			Vector2 directionToPlayer = character.GlobalPosition.DirectionTo(player.GlobalPosition);
			animationPlayer.Play(character.runAngryAnimation);
			if(directionToPlayer.X < 0) character.direction = -1;
			else if (directionToPlayer.X > 0) character.direction = 1;
			Flip();

			float distanceToPlayer = character.GlobalPosition.DistanceTo(player.GlobalPosition);
			GD.Print("Distancia: " + distanceToPlayer.ToString());
			if(distanceToPlayer > character.GetViewDistance())
				EmitSignal(SignalName.LostPlayer);
		}

		public override void PhysicsUpdate(double delta) {
			float ySpeed = character.IsOnFloor() ? character.Velocity.Y :character. Velocity.Y + GameResources.Gravity;
			character.Velocity = new Vector2(character.direction * character.GetSpeed() * 1.5f, ySpeed);
			character.MoveAndSlide();
		}
	#endregion

    #region My Methods
		private void Flip() {
			rayCasts.Scale = new Vector2(-character.direction, rayCasts.Scale.Y);
			sprite.FlipH = character.direction == 1;
		}
    #endregion

    #region Events
    #endregion
}

