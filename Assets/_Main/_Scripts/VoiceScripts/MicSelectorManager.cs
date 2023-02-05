using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.Unity;
using Photon.Voice;

public class MicSelectorManager : MonoBehaviour
{
    public Recorder _rec;
    public TMP_Dropdown _dropdown;
    private void Awake()
    {
        var list = new List<string>(Microphone.devices);
        _dropdown.AddOptions(list);
    }

    public void SetMic(int i)
    {
        var mic = Microphone.devices[i];
        _rec.MicrophoneDevice = new DeviceInfo(mic);
    }
    public void MicOff()
    {
        Destroy(this);
    }
}
