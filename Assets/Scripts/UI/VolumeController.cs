using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{

    public enum Setting
    {
        VolumeSfx, VolumeMusic, VolumeUI
    }

    public Setting SettingName;

    public Text value;

    static readonly float delta = .2f;

    private float setting;
    // Start is called before the first frame update
    void Start()
    {
        setting = Mathf.Clamp(PlayerPrefs.GetFloat(SettingName.ToString(), 1), 0, 1);
        UpdatePercent();
    }

    public void ClickPlus()
    {
        if (setting >= 1)
        {
            SoundController.PlayError();
            return;
        }
        setting = Mathf.Min(setting + delta, 1);
        PlayerPrefs.SetFloat(SettingName.ToString(), setting);
        SoundController.OnVolumesUpdated();
        MusicManager.OnVolumesUpdated(SettingName);
        UpdatePercent();
        SoundController.playMenuChange();
    }

    public void UpdatePercent()
    {
        value.text = ((int)(setting * 100)) + "%";
    }

    public void ClickMinus()
    {
        if (setting <= 0)
        {
            SoundController.PlayError();
            return;
        }
        setting = Mathf.Max(setting - delta, 0);
        PlayerPrefs.SetFloat(SettingName.ToString(), setting);
        SoundController.OnVolumesUpdated();
        MusicManager.OnVolumesUpdated(SettingName);
        UpdatePercent();
        SoundController.playMenuChange();
        
    }

}
