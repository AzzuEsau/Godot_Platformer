using Godot;
using System;
using System.Threading.Tasks;

public partial class LifeComponent : Node {
	#region Variables
		[ExportCategory("Required")]
			// Set an amount of max life of the prefab
			[Export] private float maxLife;
			// Bring the top node of the prefab
			[Export] private Node parentNode;
			// Check if the 
			[Export] bool isHurtAnimationExits = false;
			// Flag to know if the gameobject will be destroyed on 0 life
			[Export] bool doDestroy = true;

		[ExportGroup("Optionals")]
			[Export] private AnimationPlayer animationPlayer;
		

		private float curretnlife;
	#endregion

	#region Signals
		[Signal] public delegate void OnHealthChangeEventHandler(LifeComponent lifeComponentDamaged, int damageTaken, Node2D sourceNode);
		[Signal] public delegate void OnAnimationFinishedEventHandler();
		[Signal] public delegate void OnDeathEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxLife), maxLife, false);
			HelperUtilities.ValidateCheckNullValue(this, nameof(parentNode), parentNode);

			curretnlife = maxLife;
			EmitSignal(SignalName.OnHealthChange, this, 0, this);
		}
	#endregion

	#region My Methods
		public async Task<bool> OnHurt(int damageTaken, Node2D source) {
			curretnlife -= damageTaken;

			if(curretnlife <= 0) {
				EmitSignal(SignalName.OnDeath);
				if(doDestroy)
					parentNode.QueueFree();
			}
			else if(animationPlayer != null && isHurtAnimationExits) {
				EmitSignal(SignalName.OnHealthChange, this, damageTaken, source);
				animationPlayer.Play(GameResources.hurtAnimation);
				await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
			}
			EmitSignal(SignalName.OnAnimationFinished);


			return true;
		}

		public void OnHeal(int heal) {
			curretnlife += heal;
			if(curretnlife > maxLife) curretnlife = maxLife;
			EmitSignal(SignalName.OnHealthChange, this, -heal, this);
		}

		public float GetCurrentLifePercent() => curretnlife * 100 / maxLife;
		public float GetCurrentLife() => curretnlife;
	#endregion

	#region Events
	#endregion
}
