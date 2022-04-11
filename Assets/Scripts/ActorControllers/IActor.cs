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
        string Id { get; }

        CompositeDisposable Disposables { get; }

        Transform transform { get; }

        GameObject gameObject { get; }

        Animator Animator { get; }

        Rigidbody2D Rigidbody2D { get; }

        ActorAnimationMediator AnimationParameter { get; }

        ActorMotionController MotionController { get; }

        ActorStateController StateController { get; }

        ActorStatusController StatusController { get; }

        ActorInteractableStageGimmickController InteractableStageGimmickController { get; }

        ActorEquipmentController EquipmentController { get; }

        ActorInventoryController InventoryController { get; }
        
        ActorBodyController BodyController { get; }
        
        ActorAnimationController AnimationController { get; }

        IMessageBroker Broker { get; }
    }
}
