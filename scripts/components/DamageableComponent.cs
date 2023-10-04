using Godot;
using System;

public partial class DamageableComponent : Area2D {
	#region Variables
		[Export] private LifeComponent lifeComponent; 
		[Export] private Node2D parent; 
		[Export] private float invulnerableExtraTime = 0;
		private CollisionShape2D collisionObject;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			HelperUtilities.ValidateCheckNullValue(this, nameof(lifeComponent), lifeComponent);
			HelperUtilities.ValidateCheckNullValue(this, nameof(parent), parent);
			collisionObject = GetChild<CollisionShape2D>(0);
		}
	#endregion

	#region My Methods
		public async void TakeDamage(int damage) {
			if(collisionObject != null) collisionObject.SetDeferred("disabled", true); 

			if(lifeComponent != null && damage > 0) await lifeComponent.OnHurt(damage, parent);

			await ToSignal(GetTree().CreateTimer(invulnerableExtraTime), Timer.SignalName.Timeout);

			if(collisionObject != null) collisionObject.SetDeferred("disabled", false); 
		}
	#endregion

	#region Events
	#endregion
}
