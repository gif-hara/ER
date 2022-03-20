using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DamageLabelElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label = default;

        [SerializeField]
        private float destroySeconds = default;

        public void Setup(string text)
        {
            this.label.text = text;

            Observable.Timer(TimeSpan.FromSeconds(this.destroySeconds))
                .Subscribe(_ => Destroy(this.gameObject))
                .AddTo(this);
        }
    }
}
