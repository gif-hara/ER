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

        public T CurrentState { get; private set; }

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
            this.CurrentState = invalidState;
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
            var previousState = this.CurrentState;
            if(this.states.ContainsKey(this.CurrentState))
            {
                this.states[this.CurrentState].onExit?.Invoke(this.nextState);
            }

            this.CurrentState = this.nextState;

            if(this.states.ContainsKey(this.CurrentState))
            {
                this.states[this.CurrentState].onEnter?.Invoke(previousState);
            }
            this.nextState = this.invalidState;

            this.onChangedState.OnNext(CurrentState);
        }

        public void Dispose()
        {
            this.onChangedState.Dispose();
        }
    }
}
