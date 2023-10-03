using Godot;
using System;
using System.Threading.Tasks;

public partial class LifeComponent : Node {
	#region Variables
		[ExportGroup("Required")]
			[Export] private float maxLife;
			[Export] private Node parentNode;
			[Export] bool isHurtAnimationExits = false;

		[ExportGroup("Optionals")]
			[Export] private AnimationPlayer animationPlayer;
			[Export] private FiniteStateMachine finiteStateMachine;
		

		private float curretnlife;
	#endregion

	#region Signals
		[Signal] public delegate void OnHealthChangeEventHandler(int currentLife);
		[Signal] public delegate void OnDeathEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			curretnlife = maxLife;
		}
	#endregion

	#region My Methods
		public async Task<bool> OnHurt(int damageTaken) {
			curretnlife -= damageTaken;
			finiteStateMachine.Stop();

			EmitSignal(SignalName.OnHealthChange, curretnlife);

			if(animationPlayer != null && isHurtAnimationExits) {
				animationPlayer.Play(GameResources.hurtAnimation);
				await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
			}

			if(curretnlife > 0) finiteStateMachine.Play();
			else {
				EmitSignal(SignalName.OnDeath);
				parentNode.QueueFree();
			}

			return true;
		}

		public void OnHeal(int heal) {
			curretnlife += heal;
			if(curretnlife > maxLife) curretnlife = maxLife;
			EmitSignal(SignalName.OnHealthChange, curretnlife);
		}

		public float CurrentLifePercent => curretnlife * 100 / maxLife;
	#endregion

	#region Events
	#endregion
}
