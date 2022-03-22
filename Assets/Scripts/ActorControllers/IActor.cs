using ER.EquipmentSystems;
using HK.Framework.EventSystems;
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
        CompositeDisposable Disposables { get; }

        Transform transform { get; }

        GameObject gameObject { get; }

        Animator Animator { get; }

        ActorAnimationParameter AnimationParameter { get; }

        ActorMotionController MotionController { get; }

        ActorStateController StateController { get; }

        ActorDirectorController DirectorController { get; }

        ActorStatusController StatusController { get; }

        ActorInteractableStageGimmickController InteractableStageGimmickController { get; }

        ActorEquipmentController EquipmentController { get; }

        ActorInventoryController InventoryController { get; }

        IMessageBroker Broker { get; }
    }
}
