using ER.ERBehaviour;
using System.Linq;
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
        public static void SetGenericBinding(this PlayableDirector self, string streamName, Object value)
        {
            var binding = self.playableAsset.outputs.First(c => c.streamName == streamName);
            self.SetGenericBinding(binding.sourceObject, value);
        }
    }
}
