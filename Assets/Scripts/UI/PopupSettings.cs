using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PopupSettings")]
public class PopupSettings : ScriptableObject
{
    private const int BUTTON_ID = 126;
    public AudioClip button;
    
    public void PlayButton()
    {
        MMSoundManagerPlayOptions options = MMSoundManagerPlayOptions.Default;
        options.ID = BUTTON_ID;
        options.Loop = false;
        options.Location = Vector3.zero;
        options.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManager.Instance.PlaySound(button, options);
    }
}