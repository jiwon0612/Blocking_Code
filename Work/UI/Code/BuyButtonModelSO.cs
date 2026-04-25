using R3;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuyButtonModel", menuName = "SO/UIModels/BuyButtonModel")]
public class BuyButtonModelSO : ScriptableObject, IDisposable
{
    public int Cost = 1000; // 구매 비용

    public string BuyText = "구매";
    public string insufficientFundsText = "자금 부족";
    public string buildingText = "설치 중";

    public ReactiveProperty<bool> IsEnabled = new ReactiveProperty<bool>(true);

    private void OnEnable()
    {
        IsEnabled.Value = true; // 초기값 설정
    }

    public void Dispose()
    {
        IsEnabled?.Dispose();
    }
}
