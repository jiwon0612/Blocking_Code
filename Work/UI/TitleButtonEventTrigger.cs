using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TitleButtonEventTrigger : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI btnText;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float sizeMultiply = 1.3f;
         
        private Vector3 _originSize;
        private Sequence _seq;

        private void Awake()
        {
            _originSize = transform.localScale;
        }

        private void OnDestroy()
        {
            _seq.Kill();
        }

        public void MouseOver()
        {
            if (_seq != null && _seq.IsActive())
            {
                _seq.Kill();
            }
            
            if(!_seq.IsActive())
                _seq = DOTween.Sequence();

            _seq.Append(transform.DOScale(_originSize * sizeMultiply, duration));
        }

        public void MouseOut()
        {
            if (_seq != null && _seq.IsActive())
            {
                _seq.Kill();
            }
            
            if(!_seq.IsActive())
                _seq = DOTween.Sequence();


            _seq.Append(transform.DOScale(_originSize * 1, duration));
        }
    }
}