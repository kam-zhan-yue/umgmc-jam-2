using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RealityNpc : RealityObject
{
    [NonSerialized, ShowInInspector, ReadOnly] private SpriteRenderer[] _spriteRenderers;
    [NonSerialized, ShowInInspector, ReadOnly] private BoxCollider2D _collider;

    public override void SetMask(SpriteMaskInteraction maskInteraction)
    {
        for (int i = 0; i < _spriteRenderers.Length; ++i)
        {
            _spriteRenderers[i].maskInteraction = maskInteraction;
        }
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

    private void OnValidate()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }
}
