using Godot;
using System;

public partial class HurtableComponent : Area2D {
	#region Variables
		[ExportGroup("Required")]
			[Export] private int damageAmount = 1;
	#endregion

	#region Signals
		[Signal] public delegate void HurtEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			this.AreaEntered += HurtableComponentAreaEntered_AreaEntered;
		}
	#endregion

	#region My Methods
		public int GetDamage() => damageAmount;
	#endregion

	#region Events
		private void HurtableComponentAreaEntered_AreaEntered(Area2D area) {
			if(!(area is DamageableComponent)) return;

			DamageableComponent damageable = (DamageableComponent)area;
			damageable.MakeDamage(damageAmount);

			EmitSignal(SignalName.Hurt);
		}
	#endregion
}
