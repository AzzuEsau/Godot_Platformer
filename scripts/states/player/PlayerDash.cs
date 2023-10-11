using Godot;
using System;

public partial class PlayerDash : State {
	#region Variables
		[Export] private Player player;
		[Export] private Sprite2D sprite;

		private float direction;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region States
		public override void Enter() {
			player.Velocity = new Vector2(0, 0);
			player.canChangeState = false;
			direction = sprite.FlipH ? -1 : 1;
			StartTimer();
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
		private async void StartTimer() {
			await ToSignal(GetTree().CreateTimer(.2F), Timer.SignalName.Timeout);
			player.dashWaitTime = player.maxDashWaitTime;
			player.canChangeState = true;
		}

		private void AnimatePlayer() {

		}

		private void MovePlayer() {
			player.Velocity = new Vector2(direction * player.dashSpeed, 0);
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
