using System;
using ER.ERBehaviour;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class InputActionExtensions
    {
        public static IObservable<InputAction.CallbackContext> OnPerformedAsObservable(this InputAction self)
        {
            return Observable.FromEvent<InputAction.CallbackContext>(x => self.performed += x, x => self.performed -= x);
        }

        public static IObservable<InputAction.CallbackContext> OnCanceledAsObservable(this InputAction self)
        {
            return Observable.FromEvent<InputAction.CallbackContext>(x => self.canceled += x, x => self.canceled -= x);
        }
    }
}
