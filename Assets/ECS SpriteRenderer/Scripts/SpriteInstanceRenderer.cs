using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace toinfiniityandbeyond.Rendering
{
    [Serializable]
    public struct SpriteInstanceRenderer : ISharedComponentData
    {
        public readonly Texture2D sprite;
        public readonly int pixelsPerUnit;
        public readonly float2 pivot;

        public SpriteInstanceRenderer(Texture2D sprite, int pixelsPerUnit, float2 pivot)
        {
            this.sprite = sprite;
            this.pixelsPerUnit = pixelsPerUnit;
            this.pivot = pivot;
        }
    }
}