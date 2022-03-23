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
    public sealed class MenuButtonElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label = default;

        [SerializeField]
        private Button button = default;

        public TextMeshProUGUI Label => this.label;

        public Button Button => this.button;
    }
}
