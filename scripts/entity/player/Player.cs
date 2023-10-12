using Godot;
using System;
using System.Threading.Tasks;

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
				[Export] private PlayerDash dashState;
			[ExportGroup("UI")]
				[Export] private Label fruitsLabel;
				[Export] private TextureProgressBar hpBar;
				[Export] private AnimationPlayer guiAnimation;
		
			[ExportGroup("Components")]
				[Export] private HurtableComponent hurtableComponent;
				[Export] private LifeComponent lifeComponent;


			private static string guiStartAnimation = "transitionAnim";
			private Global global;
		#endregion

		public float direction;
		public bool isJumping;
		public float speed = 200;
		private float jumpSpeed = 250;


		public bool isHurted = false;
		public bool canChangeState = true;

		public bool isDashing;
		public float dashSpeed = 600F;
		public float dashWaitTime;
		public float maxDashWaitTime = 1F;



		#region Jump Juicy Configurations
			private bool triedToJump = false; 
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

			UpdateHPBar(lifeComponent.GetCurrentLifePercent());

			global = GetNode<Global>(GameResources.GlobalAutoload);
			global.SetPlayer(this);
			global.FruitsCollectedChanged += Global_FruitsCollectedChanged;

			guiAnimation.Play(guiStartAnimation);
		}

		public override void _Process(double delta) {
			PlayCoyoteTime(delta);
			PlayJumpBuffer(delta);
			PlayDashTime(delta);
		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
			SetState();
		}

		public override void _ExitTree() {
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
			direction = Input.GetAxis(GameResources.KeyMoveLeft, GameResources.KeyMoveRight);

			isJumping = false;
			if(jumpsLeft > 0) isJumping = Input.IsActionJustPressed(GameResources.KeyJump);
			else triedToJump = Input.IsActionJustPressed(GameResources.KeyJump);

			if(dashWaitTime < 0) isDashing = Input.IsActionJustPressed(GameResources.KeyDash);
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

		private void PlayDashTime(double delta) {
			if(dashWaitTime < 0) return;

			dashWaitTime -= (float)delta;
		}

		private void UpdateHPBar(float percent) => hpBar.Value = percent + 10;

		public async Task PlayTransition() {
			guiAnimation.PlayBackwards(guiStartAnimation);
			GetTree().Paused = true;
			await ToSignal(guiAnimation,  AnimationPlayer.SignalName.AnimationFinished);
			GetTree().Paused = false;
		}
    #endregion

	#region Getters And Setter
		public float GetJumpSpeed() => jumpSpeed;	
 
		private void SetState() {
			if(!canChangeState) return;

			if(isDashing) dashState.EmitSignal(State.SignalName.Transition, dashState, dashState.Name);
			else if(finiteStateMachine.IsCurrentState(wallSlideState) && isJumping) wallJumpState.EmitSignal(State.SignalName.Transition, wallJumpState, wallJumpState.Name);
			else if (coyoteTime < 0) {
				if(IsOnWall() && direction != 0) wallSlideState.EmitSignal(State.SignalName.Transition, wallSlideState, wallSlideState.Name);
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
			fruitsLabel.Text = fruits.ToString();
		}

		private void HurtableComponent_Hurt(DamageableComponent hurted) {
			SmallJump();
		}

		private async void LifeComponent_OnDeath() {
			finiteStateMachine.Stop();
			audioHurt.Play();
			animator.Play(GameResources.hurtAnimation);
			await ToSignal(animator, AnimationPlayer.SignalName.AnimationFinished);
			await PlayTransition();

			global.playerLifes -= 1;
			global.SaveGame();

			GetTree().ReloadCurrentScene();
		}

		private async void LifeComponent_OnHealthChange(LifeComponent lifeComponentDamaged, int damageTaken, Node2D sourceNode) {
			UpdateHPBar(lifeComponentDamaged.GetCurrentLifePercent());

			if(damageTaken > 0) {
				isHurted = true;
				audioHurt.Play();
				await ToSignal(lifeComponent, LifeComponent.SignalName.OnAnimationFinished);
				isHurted = false;
			}
		}
    #endregion
}
