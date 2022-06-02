using ER.ERBehaviour;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="ActorAIController"/>が利用する<see cref="IBehaviourData"/>
    /// </summary>
    public sealed class ActorHolderBehaviourData : IBehaviourData, IActorHolder
    {
        public Actor Actor { get; set; }
    }
}
