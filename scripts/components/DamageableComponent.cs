using Godot;
using System;

public partial class DamageableComponent : Area2D {
	#region Variables
		[Export] private LifeComponent lifeComponent; 
		private CollisionShape2D collisionObject;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			collisionObject = GetChild<CollisionShape2D>(0);
		}
	#endregion

	#region My Methods
		public async void MakeDamage(int damage) {
			if(collisionObject != null) collisionObject.SetDeferred("disabled", true); 

			if(lifeComponent != null && damage > 0) await lifeComponent.OnHurt(damage);

			if(collisionObject != null) collisionObject.SetDeferred("disabled", false); 
		}
	#endregion

	#region Events
	#endregion
}
