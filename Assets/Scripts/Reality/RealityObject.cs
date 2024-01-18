using Common;
using UnityEngine;

public abstract class RealityObject : MonoBehaviour
{
    public abstract void SetMask(SpriteMaskInteraction maskInteraction);
    public abstract void ActivateCollider();
    public abstract void DeactivateCollider();
}