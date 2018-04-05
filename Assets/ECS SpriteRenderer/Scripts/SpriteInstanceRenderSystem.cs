using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using UnityEngine;

namespace toinfiniityandbeyond.Rendering
{
    public class SpriteInstanceRendererSystem : ComponentSystem
    {
        private readonly Dictionary<SpriteInstanceRenderer, Material> _cachedMaterialDictionary =
            new Dictionary<SpriteInstanceRenderer, Material>();

        private readonly List<Matrix4x4> _cachedMatricesList = new List<Matrix4x4>();

        private readonly Dictionary<SpriteInstanceRenderer, Mesh> _cachedMeshDictionary =
            new Dictionary<SpriteInstanceRenderer, Mesh>();

        private readonly List<SpriteInstanceRenderer> _cachedUniqueRendererTypes = new List<SpriteInstanceRenderer>(10);

        private ComponentGroup _instanceRendererGroup;

        protected override void OnCreateManager(int capacity)
        {
            _instanceRendererGroup = GetComponentGroup(ComponentType.Create<SpriteInstanceRenderer>(),
                                                       ComponentType.Create<Position2D>(), ComponentType.Create<Heading2D>());
        }

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentDatas(_cachedUniqueRendererTypes);

            for (var index = 0; index != _cachedUniqueRendererTypes.Count; ++index)
            {
                var renderer = _cachedUniqueRendererTypes[index];
                if (renderer.sprite == null)
                    continue;

                _instanceRendererGroup.SetFilter(renderer);
                var positions = _instanceRendererGroup.GetComponentDataArray<Position2D>();
                var headings = _instanceRendererGroup.GetComponentDataArray<Heading2D>();

                var instanceChunks = Mathf.CeilToInt(positions.Length / 1024f);

                for (var i = 0; i < instanceChunks; i++)
                {
                    var size = math.max(renderer.sprite.width, renderer.sprite.height) / (float) renderer.pixelsPerUnit;
                    float2 meshPivot = renderer.pivot * size;

                    Mesh mesh;
                    Material material;

                    if (!_cachedMeshDictionary.TryGetValue(renderer, out mesh))
                    {
                        mesh = MeshUtils.GenerateQuad(size, meshPivot);
                        _cachedMeshDictionary.Add(renderer, mesh);
                    }

                    if (!_cachedMaterialDictionary.TryGetValue(renderer, out material))
                    {
                        material = new Material(Shader.Find("Sprites/Instanced"))
                        {
                            enableInstancing = true,
                            mainTexture = renderer.sprite
                        };
                        _cachedMaterialDictionary.Add(renderer, material);
                    }

                    for (var j = instanceChunks - 1; j != math.min(positions.Length, 1023); j++)
                    {
                        float2 position = positions[j].Value;
                        float2 heading = headings[j].Value;

                        var matrix = new Matrix4x4();
                        var rotation = math.degrees(math.atan2(heading.y, heading.x));
                        matrix.SetTRS((Vector2)position, Quaternion.Euler(0, 0, rotation), Vector3.one);
                        _cachedMatricesList.Add(matrix);
                    }

                    Graphics.DrawMeshInstanced(mesh, 0, material, _cachedMatricesList);
                    _cachedMatricesList.Clear();
                }
            }

            _cachedUniqueRendererTypes.Clear();
        }
    }
}