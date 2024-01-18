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

    public override void SetVisibleInsideMask()
    {
        _tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public override void SetVisibleOutsideMask()
    {
        _tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }

    public override void ActivateCollider()
    {
        _tilemapCollider2D.enabled = true;
    }

    public override void DeactivateCollider()
    {
        _tilemapCollider2D.enabled = false;
    }

    private void OnValidate()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }
}
