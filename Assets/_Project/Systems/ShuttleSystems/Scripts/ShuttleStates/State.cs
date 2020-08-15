using System;
using System.Collections.Generic;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class FSM
    {
        public IReadonlyObservable<State> CurrentState => _currentState;
        private readonly Observable<State> _currentState = new Observable<State>();
        private readonly Dictionary<Type, State> _states = new Dictionary<Type, State>();

        public FSM(Shuttle shuttle) {
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

        public void Tick() {
            _currentState.Value?.Tick();
        }

        public abstract class State {
            protected readonly FSM _fsm;
            protected readonly Shuttle _shuttle;

            protected State(FSM fsm, Shuttle shuttle) {
                _fsm = fsm;
                _shuttle = shuttle;
            }
            public virtual void Enter() {
                _shuttle.Hull.OnDie += OnDie;
            }
            public virtual void Leave() {
                _shuttle.Hull.OnDie -= OnDie;
            }

            protected virtual void OnDie() {
                _fsm.SetState<ShutdownState>();
            }
            public virtual void Tick() { }
        }
    }
}