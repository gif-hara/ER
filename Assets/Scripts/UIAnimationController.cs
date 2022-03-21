using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator = default;

        private static readonly int In = Animator.StringToHash("UI_In");

        private static readonly int Out = Animator.StringToHash("UI_Out");

        public void Play(bool isIn)
        {
            PlayInternal(isIn, 0.0f);
        }

        public void PlayImmediate(bool isIn)
        {
            PlayInternal(isIn, 1.0f);
        }

        private void PlayInternal(bool isIn, float normalizedTime)
        {
            if (isIn)
            {
                this.animator.Play(In, 0, normalizedTime);
            }
            else
            {
                this.animator.Play(Out, 0, normalizedTime);
            }
        }
    }
}
