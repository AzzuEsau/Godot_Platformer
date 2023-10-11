using Godot;
using System;
using System.Collections.Generic;

public partial class FiniteStateMachine : Node {
	#region Variables
		[Export] State initialState;
		Dictionary<string, State> states = new Dictionary<string, State>();
		State currentState;

		bool isPaused = false;
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
					states.Add(child.Name.ToString().ToLower(), newState);
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
			if (currentState != null && !isPaused)
				currentState.Update(delta);
		}

		public override void _PhysicsProcess(double delta) {
			// Activate the _PhysicsProcess of the currentstate
			if (currentState != null  && !isPaused)
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
			if(currentState != null)
				currentState.Exit();
			// Almost forgot... set the new state the current state
			currentState = newState;
			newState.Enter();
		}
    #endregion

	#region My Methods
		public State GetCurrentState() => currentState;
		public bool IsPaused() => isPaused;
		public bool IsCurrentState(State state) => currentState == state;

		public void Stop() {
			if(isPaused) return;

			isPaused = true;
			currentState.Exit();
		} 

		public void Play() {
			if(!isPaused) return;

			isPaused = false;
			currentState.Enter();
		}
	#endregion
}
