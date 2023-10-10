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
				[Export] private PlayerIdle idleState;
				[Export] private PlayerMoving movingState;
				[Export] private PlayerAir airState;
				[Export] private PlayerWallSlide wallSlideState;
				[Export] private PlayerWallJump wallJumpState;
			[ExportGroup("UI")]
				[Export] private Label fruitsLabel;
				[Export] private ProgressBar hpBar;
		
			[ExportGroup("Components")]
				[Export] private HurtableComponent hurtableComponent;
				[Export] private LifeComponent lifeComponent;
		#endregion

		public float direction;
		public bool isJumping;
		public bool isHurted = false;

		private bool triedToJump = false; 

		public float speed = 200;
		private float jumpSpeed = 250;

		public bool canChangeState = true;

		#region Jump Juicy Configurations
			public int jumpsLeft = 2;
			private int maxJumps = 2;
			public float coyoteTime;
			private float coyoteTimeMax = 0.35F;
			public float jumpBuffer = 0F;
			private float jumpBufferMax = .25F;
			public int MaxJumps { get {return maxJumps;} private set {maxJumps = 1;}}
		#endregion
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
			PlayCoyoteTime(delta);
			PlayJumpBuffer(delta);
		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
			SetState();
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

		public void ApplyGravity() {
			Velocity = new Vector2(Velocity.X * direction, Velocity.Y + GameResources.Gravity);
		}

		private void ReadInput() {
			isJumping = false;
			direction = Input.GetAxis(GameResources.KeyMoveLeft, GameResources.KeyMoveRight);
			if(jumpsLeft > 0) isJumping = Input.IsActionJustPressed(GameResources.KeyJump);
			else triedToJump = Input.IsActionJustPressed(GameResources.KeyJump);
		}

		private void PlayCoyoteTime(double delta) {
			if(IsOnFloor()) coyoteTime = coyoteTimeMax;
			else coyoteTime -= (float)delta;
		}

		private void PlayJumpBuffer(double delta) {
			if(IsOnFloor()) return; 

			if(triedToJump) jumpBuffer = jumpBufferMax;
			else jumpBuffer -= (float)delta;
		}
    #endregion

	#region Getters And Setter
		public float GetJumpSpeed() => jumpSpeed;	

		private void SetState() {
			if(!canChangeState) return;


			if (coyoteTime < 0) {
				if(IsOnWall() && direction != 0) {
					if (isJumping) wallJumpState.EmitSignal(State.SignalName.Transition, wallJumpState, wallJumpState.Name);
					else wallSlideState.EmitSignal(State.SignalName.Transition, wallSlideState, wallSlideState.Name);
				}
				// Set air state when the coyote time expires
				else airState.EmitSignal(State.SignalName.Transition, airState, airState.Name); 
			} 
			// Set the movement state when the input is pressed
			else if(direction != 0 || isJumping || jumpBuffer > 0) movingState.EmitSignal(State.SignalName.Transition, movingState, movingState.Name);
			// Just set idle
			else idleState.EmitSignal(State.SignalName.Transition, idleState, idleState.Name);
		}
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
				isHurted = true;
				audioHurt.Play();
				await ToSignal(lifeComponent, LifeComponent.SignalName.OnAnimationFinished);
				isHurted = false;
			}
		}
    #endregion
}
