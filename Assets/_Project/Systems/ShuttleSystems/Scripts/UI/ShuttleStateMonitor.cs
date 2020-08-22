using System;
using System.Collections.Generic;
using SpaceGame.ShuttleSystems.ShuttleStates;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Shuttle State Monitor")]
    public class ShuttleStateMonitor : MonoBehaviour {
        [SerializeField] private StateComparison<LandedState> _landed = new StateComparison<LandedState>();
        [SerializeField] private StateComparison<TakeOffState> _takeOff = new StateComparison<TakeOffState>();
        [SerializeField] private StateComparison<FlyingState> _flying = new StateComparison<FlyingState>();
        [SerializeField] private StateComparison<LandingState> _landing = new StateComparison<LandingState>();
        [SerializeField] private StateComparison<ShutdownState> _shutdown = new StateComparison<ShutdownState>();

        private Shuttle _shuttle = default;

        public void SetShuttle(Shuttle shuttle) {
            if (_shuttle) _shuttle.CurrentState.Unsubscribe(OnStateChange);
            _shuttle = shuttle;
            if (_shuttle) {
                _shuttle.CurrentState.Subscribe(OnStateChange);
            } else {
                OnStateChange(null);
            }
        }

        private void OnStateChange(ShuttleStateMachine.State newState) {
            _landed.StateChange(newState);
            _takeOff.StateChange(newState);
            _flying.StateChange(newState);
            _landing.StateChange(newState);
            _shutdown.StateChange(newState);
        }

        private void OnDestroy()
        {
            if (_shuttle != null) _shuttle.CurrentState.Unsubscribe(OnStateChange);
        }

        [Serializable]
        public class StateComparison<T> {
            private ShuttleStateMachine.State _oldState = default;

            [SerializeField] private UnityEvent _onEnter = default;
            [SerializeField] private UnityEvent _onLeave = default;
            [SerializeField] private List<GameObject> _enabledInState = new List<GameObject>();

            public void StateChange(ShuttleStateMachine.State newState) {
                if (_oldState == newState) return;
                if (newState is T) {
                    _onEnter.Invoke();
                    _enabledInState.ForEach(gameObject => gameObject.SetActive(true));
                } else {
                    _enabledInState.ForEach(gameObject => gameObject.SetActive(false));
                    if (_oldState is T) {
                        _onLeave.Invoke();
                    }
                }
                _oldState = newState;
            }
        }
    }
}