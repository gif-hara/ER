using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// ステートコントローラー
    /// </summary>
    public sealed class StateController<T> : IDisposable where T : Enum
    {
        private class StateInfo
        {
            public Action<T> onEnter;

            public Action<T> onExit;
        }

        private Dictionary<Enum, StateInfo> states = new Dictionary<Enum, StateInfo>();

        private T currentState;

        private T nextState;

        private T invalidState;

        /// <summary>
        /// ステートが切り替わった際のイベント
        /// </summary>
        private Subject<T> onChangedState = new Subject<T>();

        /// <summary>
        /// <inheritdoc cref="onChangedState"/>
        /// </summary>
        public IObservable<T> OnChangedStateAsObservable() => this.onChangedState;

        public StateController(T invalidState)
        {
            this.invalidState = invalidState;
            this.currentState = invalidState;
            this.nextState = invalidState;
        }

        public void Update()
        {
            if(this.nextState.GetHashCode() != this.invalidState.GetHashCode())
            {
                this.Change();
            }
        }

        public void Set(T value, Action<T> onEnter, Action<T> onExit)
        {
            Assert.IsFalse(this.states.ContainsKey(value), $"{value}は既に登録済みです");

            this.states.Add(value, new StateInfo
            {
                onEnter = onEnter,
                onExit = onExit
            });
        }

        public void ChangeRequest(T value)
        {
            this.nextState = value;
        }

        private void Change()
        {
            Assert.AreNotEqual(this.nextState, this.invalidState);
            var previousState = this.currentState;
            if(this.states.ContainsKey(this.currentState))
            {
                this.states[this.currentState].onExit?.Invoke(this.nextState);
            }

            this.currentState = this.nextState;

            if(this.states.ContainsKey(this.currentState))
            {
                this.states[this.currentState].onEnter?.Invoke(previousState);
            }
            this.nextState = this.invalidState;

            this.onChangedState.OnNext(currentState);
        }

        public void Dispose()
        {
            this.onChangedState.Dispose();
        }
    }
}
