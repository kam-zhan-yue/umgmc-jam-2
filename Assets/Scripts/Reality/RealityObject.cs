using Common;
using UnityEngine;

public abstract class RealityObject : MonoBehaviour
{
    public abstract void SetVisibleInsideMask();
    public abstract void SetVisibleOutsideMask();
    public abstract void ActivateCollider();
    public abstract void DeactivateCollider();
}