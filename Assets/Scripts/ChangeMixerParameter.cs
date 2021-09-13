using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeMixerParameter : MonoBehaviour
{
    [SerializeField] AudioMixer _masterMix;
    [SerializeField] string _parameterName;

    [Header("References")]
    [SerializeField] Slider _slider;

    private void OnEnable() {
        float temp = 1.0f;
        _masterMix.GetFloat(_parameterName,out temp);
        _slider.value = Mathf.InverseLerp(-40.0f,0.0f,temp);
    }
    private void OnDisable() {
        OnValueChanged();
        Debug.Log(PlayerPrefs.GetFloat(_parameterName));
    }

    public void OnValueChanged()
    {
        var val = Mathf.Lerp(-40.0f, 0.0f, _slider.value);
        _masterMix.SetFloat(_parameterName, val);
        PlayerPrefs.SetFloat(_parameterName, val);
        PlayerPrefs.Save();
    }
}
