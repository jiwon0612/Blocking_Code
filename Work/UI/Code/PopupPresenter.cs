using R3;
using UnityEngine;

namespace UI
{
    public class PopupPresenter : MonoBehaviour
    {
        [SerializeField] private PopupView view;
        [SerializeField] private PopupModelSO model;

        private void Start()
        {
            Debug.Assert(model != null, "PopupModelSO is not assigned in PopupPresenter.");
            model.PopupData
                .Subscribe(data => view.SetPopup(data))
                .AddTo(this);
        }
    }
}
