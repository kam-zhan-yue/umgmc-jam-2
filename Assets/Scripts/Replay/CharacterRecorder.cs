using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRecorder : MonoBehaviour
{
    private Recorder _recorder;

    private void Awake()
    {
        _recorder = GetComponent<Recorder>();
    }
    private void LateUpdate()
    {
        ReplayData data = new PlayerReplayData(transform.position);
        _recorder.RecordFrame(data);
    }
}
