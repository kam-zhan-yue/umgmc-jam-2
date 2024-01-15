using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class RealitySprite : RealityObject
{
    [NonSerialized, ShowInInspector, ReadOnly] private SpriteRenderer _spriteRenderer;
    [NonSerialized, ShowInInspector, ReadOnly] private Collider2D _collider;

    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public override void SetVisibleInsideMask()
    {
        _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public override void SetVisibleOutsideMask()
    {
        _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }

    public override void ActivateCollider()
    {
        _collider.enabled = true;
    }

    public override void DeactivateCollider()
    {
        _collider.enabled = false;
    }
}
