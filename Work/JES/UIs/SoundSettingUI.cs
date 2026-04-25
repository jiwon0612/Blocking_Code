using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Work.JES.UIs
{
    [Serializable]
    public struct SoundValues
    {
        public float master, music, sfx;
    }

    public class SoundSettingUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
        private string jsonKey = "SoundValues";

        private void Awake()
        {
            masterSlider.value = 1;
            musicSlider.value = 1;
            sfxSlider.value = 1;
            masterSlider.onValueChanged.AddListener(HandleMasterSliderChange);
            musicSlider.onValueChanged.AddListener(HandleMusicSliderChange);
            sfxSlider.onValueChanged.AddListener(HandleSfxSliderChange);
        }

        private void Start()
        {
            LoadSaveData();
        }

        private void HandleSfxSliderChange(float value) =>
            SetSliderValue("SFX", value);

        private void HandleMusicSliderChange(float value) =>
            SetSliderValue("BGM", value);

        private void HandleMasterSliderChange(float value) =>
            SetSliderValue("MASTER", value);

        private void SetSliderValue(string key, float value) =>
            audioMixer.SetFloat(key, Mathf.Log10(value) * 20);

        private void OnDestroy()
        {
            SaveData();
        }

        private void LoadSaveData()
        {
            string jsonData = PlayerPrefs.GetString(jsonKey);

            if (string.IsNullOrEmpty(jsonData))
            {
                masterSlider.value = 1;
                musicSlider.value = 1;
                sfxSlider.value = 1;
                return;
            }

            SoundValues soundValues = JsonUtility.FromJson<SoundValues>(jsonData);

            masterSlider.value = soundValues.master;
            HandleMasterSliderChange(soundValues.master);

            musicSlider.value = soundValues.music;
            HandleMusicSliderChange(soundValues.music);

            sfxSlider.value = soundValues.sfx;
            HandleSfxSliderChange(soundValues.sfx);
        }

        private void SaveData()
        {
            SoundValues soundValues = new SoundValues
            {
                master = masterSlider.value,
                music = musicSlider.value,
                sfx = sfxSlider.value
            };

            string jsonData = JsonUtility.ToJson(soundValues);
            PlayerPrefs.SetString(jsonKey, jsonData);
            PlayerPrefs.Save();
        }
    }
}