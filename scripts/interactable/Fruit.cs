using Godot;
using System;

public partial class Fruit : Area2D {
	#region Variables
		// [Export] private int vairbaleInEditor;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			this.BodyEntered += Fruit_BodyEntered;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
		private void Fruit_BodyEntered(Node2D node) {
			if(node is Player) {
				Global global = (Global)GetNode(GameResources.GlobalAutoload);
				global.EmitSignal(Global.SignalName.FruitCollected, 1);
				QueueFree();
			}
		}
	#endregion
}
