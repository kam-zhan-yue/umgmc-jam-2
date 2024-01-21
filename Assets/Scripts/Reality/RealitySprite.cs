using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class RealitySprite : RealityObject
{
    [NonSerialized, ShowInInspector, ReadOnly] private SpriteRenderer _spriteRenderer;
    [NonSerialized, ShowInInspector, ReadOnly] private Collider2D _collider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
    
    public override void SetMask(SpriteMaskInteraction maskInteraction)
    {
        _spriteRenderer.maskInteraction = maskInteraction;
    }

    public override void ActivateCollider()
    {
        if(_collider)
            _collider.enabled = true;
    }

    public override void DeactivateCollider()
    {
        if(_collider)
            _collider.enabled = false;
    }
}
