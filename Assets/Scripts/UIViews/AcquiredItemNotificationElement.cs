using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AcquiredItemNotificationElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label = default;

        [SerializeField]
        private PlayableDirector director = default;

        [SerializeField]
        private PlayableAsset inAnimation = default;

        [SerializeField]
        private PlayableAsset outAnimation = default;

        [SerializeField]
        private float destroySeconds = default;

        public void Setup(string message)
        {
            this.label.text = message;
            this.director.Play(this.inAnimation);
            this.director.Evaluate();

            Observable.Timer(TimeSpan.FromSeconds(this.destroySeconds))
                .Subscribe(_ =>
                {
                    this.director.Play(this.outAnimation);
                    this.director.OnStoppedAsObservable()
                    .Subscribe(_ =>
                    {
                        Destroy(this.gameObject);
                    })
                    .AddTo(this);
                })
                .AddTo(this);
        }
    }
}
