using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayObject : ReplayObject
{
    public override void SetDataForFrame(ReplayData data)
    {
        PlayerReplayData playerReplayData = (PlayerReplayData)data;
        transform.position = playerReplayData.position;
    }
}
