using ER.ERBehaviour;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class PlayableDirectorExtensions
    {
        public static void SetGenericBinding(this PlayableDirector self, string streamName, UnityEngine.Object value)
        {
            var binding = self.playableAsset.outputs.First(c => c.streamName == streamName);
            self.SetGenericBinding(binding.sourceObject, value);
        }

        public static IObservable<Unit> OnStoppedAsObservable(this PlayableDirector self)
        {
            return Observable.Defer(() =>
            {
                return Observable.FromEvent<PlayableDirector>(x => self.stopped += x, x => self.stopped -= x)
                .Take(1)
                .AsUnitObservable();
            });
        }
    }
}
