using R3;
using UnityEngine;

namespace UI
{
    public class TimerPresenter : MonoBehaviour
    {
        [SerializeField] private TimerView view;
        [SerializeField] private TimerModelSO model;

        private void Start()
        {
            Debug.Assert(model != null, "TimerModelSO is not assigned in TimerPresenter.");
            model.Timer
                .Subscribe(time => view.UpdateTimer(time))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (model != null)
            {
                model.Dispose();
            }
        }
    }
}