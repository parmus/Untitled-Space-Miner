using System;
using System.Collections.Generic;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class ShuttleStateMachine
    {
        public IReadonlyObservable<State> CurrentState => _currentState;
        private readonly Observable<State> _currentState = new Observable<State>();
        private readonly Dictionary<Type, State> _states = new Dictionary<Type, State>();

        public ShuttleStateMachine(Shuttle shuttle) {
            _states.Add(typeof(LandedState), new LandedState(this, shuttle));
            _states.Add(typeof(TakeOffState), new TakeOffState(this, shuttle));
            _states.Add(typeof(FlyingState), new FlyingState(this, shuttle));
            _states.Add(typeof(LandingState), new LandingState(this, shuttle));
            _states.Add(typeof(ShutdownState), new ShutdownState(this, shuttle));

            SetState<LandedState>();
        }
        public void SetState<T>()
        {
            if (_currentState.Value is T) return;
            _currentState.Value?.Leave();
            _currentState.Value = _states[typeof(T)];
            _currentState.Value.Enter();
        }

        public void Tick() => _currentState.Value?.Tick();

        public abstract class State {
            protected readonly ShuttleStateMachine _shuttleStateMachine;
            protected readonly Shuttle _shuttle;

            protected State(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) {
                _shuttleStateMachine = shuttleStateMachine;
                _shuttle = shuttle;
            }

            public virtual void Enter() { }

            public virtual void Leave() { }

            public virtual void Tick() { }
        }
    }
}