using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameUIPresenter : UIPresenter
    {
        private void Awake()
        {
            GameEvent.OnRequestOpenIngameMenuSubject()
                .Subscribe(_ =>
                {
                    Debug.Log("TODO");
                })
                .AddTo(this);
        }
    }
}
