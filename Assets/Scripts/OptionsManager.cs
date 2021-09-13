using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] AudioMixer _masterMix;
    // Start is called before the first frame update
    void Start()
    {
        _masterMix.SetFloat("Master Volume",PlayerPrefs.GetFloat("Master Volume"));
        _masterMix.SetFloat("Music Volume",PlayerPrefs.GetFloat("Music Volume"));
        _masterMix.SetFloat("SFX Volume",PlayerPrefs.GetFloat("SFX Volume"));
    }

}
