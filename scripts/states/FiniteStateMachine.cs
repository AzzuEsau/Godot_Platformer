using Godot;
using System;
using System.Collections.Generic;

public partial class FiniteStateMachine : Node {
	#region Variables
		[Export] State initialState;
		Dictionary<string, State> states = new Dictionary<string, State>();
		State currentState;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			// Fill the dictionary
			foreach (Node child in GetChildren())
				if(child is State) {
					State newState = (State)child;
					states.Add(child.Name, newState);
					// Make the state machine to listen for the change
					newState.Transition += NewState_Transition;
				}

			// Set the first state
			if(initialState != null) {
				initialState.Enter();
				currentState = initialState;
			}
		}

		public override void _Process(double delta) {
			// Activate the _Process of the currentstate
			if (currentState != null)
				currentState.Update(delta);
		}

		public override void _PhysicsProcess(double delta) {
			// Activate the _PhysicsProcess of the currentstate
			if (currentState != null)
				currentState.PhysicsUpdate(delta);
		}
    #endregion

    #region Events
		private void NewState_Transition(State state, string stateName) {
			// Check if the state is the actual state to do not enter the same state again
			if(state == currentState) return;

			// Try to get the state of the dictionary
			State newState = states[stateName.ToLower()];
			if(newState == null) return;

			// Switch between two states
			currentState.Exit();
			newState.Enter();
		}
    #endregion
}