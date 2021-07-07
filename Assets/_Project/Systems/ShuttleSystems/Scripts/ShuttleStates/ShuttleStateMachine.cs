using System;
using System.Collections.Generic;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine.Assertions;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class ShuttleStateMachine: IPersistable
    {
        public IReadonlyObservable<State> CurrentState => _currentState;
        private readonly Observable<State> _currentState = new Observable<State>();
        private readonly Dictionary<Type, State> _states = new Dictionary<Type, State>();
        private Shuttle _shuttle;

        public ShuttleStateMachine(Shuttle shuttle)
        {
            _shuttle = shuttle;
            _states.Add(typeof(LandedState), new LandedState(this));
            _states.Add(typeof(TakeOffState), new TakeOffState(this));
            _states.Add(typeof(FlyingState), new FlyingState(this));
            _states.Add(typeof(LandingState), new LandingState(this));
            _states.Add(typeof(ShutdownState), new ShutdownState(this));

            SetState<LandedState>();
        }
        public void SetState<T>()
        {
            if (_currentState.Value is T) return;
            _currentState.Value?.Leave(_shuttle);
            _currentState.Value = _states[typeof(T)];
            _currentState.Value.Enter(_shuttle);
        }

        public object CaptureState() => _currentState.Value.GetType().AssemblyQualifiedName;

        public void RestoreState(object state)
        {
            var stateName = (string) state;
            Assert.IsNotNull(stateName);
            var stateType = Type.GetType(stateName);
            Assert.IsNotNull(stateType);
            _currentState.Value = _states[stateType];
            _currentState.Value.Enter(_shuttle);
        }

        public abstract class State {
            protected readonly ShuttleStateMachine _shuttleStateMachine;

            protected State(ShuttleStateMachine shuttleStateMachine) {
                _shuttleStateMachine = shuttleStateMachine;
            }

            public virtual void Enter(Shuttle shuttle) { }

            public virtual void Leave(Shuttle shuttle) { }
        }
    }
}
