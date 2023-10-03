using Godot;
using System;

public partial class Character : CharacterBody2D {
	#region Variables
		// [Export] private int vairbaleInEditor;
		[Export] protected int speed;
		[Export] protected int life;
		[Export] protected AnimationPlayer animationPlayer;


		public string idleAnimation = "idle";
		public string hurtAnimation = "hurt";
		public string walkAnimation = "walk";
		public string runAngryAnimation = "run_angry";
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

	#region My Methods
		public int getSpeed() => speed;
	#endregion

	#region Events
	#endregion
}
