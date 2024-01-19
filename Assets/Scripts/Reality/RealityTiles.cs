using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealityTiles : RealityObject
{
    public Timeline timeline;
    [NonSerialized, ShowInInspector, ReadOnly] private TilemapRenderer _tilemapRenderer;
    [NonSerialized, ShowInInspector, ReadOnly] private TilemapCollider2D _tilemapCollider2D;

    public override void SetMask(SpriteMaskInteraction maskInteraction)
    {
        _tilemapRenderer.maskInteraction = maskInteraction;
    }

    public override void ActivateCollider()
    {
        if (_tilemapCollider2D != null)
        {
            _tilemapCollider2D.enabled = true;
        }
    }

    public override void DeactivateCollider()
    {
        if (_tilemapCollider2D != null)
        {
            _tilemapCollider2D.enabled = false;
        }
    }

    private void OnValidate()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }
}
