using Godot;
using System;

public partial  class Player : CharacterBody2D {
	#region Variables
		#region Exports
			[Export] private AnimationPlayer animator;
			[Export] private Sprite2D sprite;
			[Export] private AudioStreamPlayer2D audioHurt;

			[ExportGroup("Finite State Machine")]
				[Export] private FiniteStateMachine finiteStateMachine;
				[Export] private NormalState normalState;
				
			[ExportGroup("UI")]
				[Export] private Label fruitsLabel;
				[Export] private ProgressBar hpBar;
		
			[ExportGroup("Components")]
				[Export] private HurtableComponent hurtableComponent;
				[Export] private LifeComponent lifeComponent;
		#endregion

		public float direction;
		public bool isJumping;

		public float speed = 200;
		private float jumpSpeed = 250;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			hurtableComponent.Hurt += HurtableComponent_Hurt;
			lifeComponent.OnDeath += LifeComponent_OnDeath;
			lifeComponent.OnHealthChange += LifeComponent_OnHealthChange;

			hpBar.Value = lifeComponent.GetCurrentLifePercent();

			Global global = (Global)GetNode(GameResources.GlobalAutoload);
			global.SetPlayer(this);
			global.FruitsCollectedChanged += Global_FruitsCollectedChanged;
		}

		public override void _Process(double delta) {

		}

		public override void _PhysicsProcess(double delta) {
			ApplyGravity();
		}

		public override void _ExitTree() {
			Global global = (Global)GetNode(GameResources.GlobalAutoload);
			global.FruitsCollectedChanged -= Global_FruitsCollectedChanged;
		}
    #endregion

    #region My Methods
    private void SmallJump() {
			Velocity = new Vector2(Velocity.X, 0);
			Velocity = new Vector2(Velocity.X, Velocity.Y - jumpSpeed / 2);
		}

		private void ApplyGravity() {
			Velocity = new Vector2(Velocity.X, Velocity.Y + GameResources.Gravity);
		}

		public float GetJumpSpeed() => jumpSpeed;
    #endregion

    #region Events
		private void Global_FruitsCollectedChanged(int fruits) {
			if(fruitsLabel == null) return;
			fruitsLabel.Text = "FRUTAS: " + fruits.ToString();
		}

		private void HurtableComponent_Hurt(DamageableComponent hurted) {
			SmallJump();
		}

		private void LifeComponent_OnDeath() {
			GetTree().ReloadCurrentScene();
		}

		private async void LifeComponent_OnHealthChange(LifeComponent lifeComponentDamaged, int damageTaken, Node2D sourceNode) {
			hpBar.Value = lifeComponentDamaged.GetCurrentLifePercent();

			if(damageTaken > 0) {
				normalState.isHurted = true;
				audioHurt.Play();
				await ToSignal(lifeComponent, LifeComponent.SignalName.OnAnimationFinished);
				normalState.isHurted = false;
			}
		}
    #endregion
}
