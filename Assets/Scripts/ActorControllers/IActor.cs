using ER.EquipmentSystems;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActor
    {
        ActorEvent Event { get; }

        Transform transform { get; }

        GameObject gameObject { get; }

        Animator Animator { get; }

        ActorAnimationParameter AnimationParameter { get; }

        ActorMotionController MotionController { get; }

        ActorStateController StateController { get; }

        ActorDirectorController DirectorController { get; }

        EquipmentController GetEquipmentController(HandType handType);
    }
}
