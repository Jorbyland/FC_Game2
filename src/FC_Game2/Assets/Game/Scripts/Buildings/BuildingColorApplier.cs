using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [DisallowMultipleComponent]
    public class BuildingColorApplier : MonoBehaviour
    {
        [System.Serializable]
        public class RendererOverride
        {
            public Renderer renderer;
            public int paletteIndex = 0;
        }

        [Header("Palette global (assigner la texture et sa taille)")]
        public Texture2D paletteTexture;
        public int paletteSize = 256;

        [Header("Default mapping")]
        public int defaultExteriorIndex = 0;
        public int defaultInteriorIndex = 1;
        public int defaultFloorIndex = 2;

        [Header("Per-renderer overrides (optional)")]
        public RendererOverride[] overrides;

        private readonly Dictionary<Renderer, MaterialPropertyBlock> _blocks = new();

        const string PROP_PALETTE = "_PaletteTex";
        const string PROP_PALSIZE = "_PaletteSize";
        const string PROP_INDEX = "_PaletteIndex";

        public void ApplyPreset(BuildingColorPreset preset)
        {
            if (preset == null) return;
            defaultExteriorIndex = preset.exteriorIndex;
            defaultInteriorIndex = preset.interiorIndex;
            defaultFloorIndex = preset.floorIndex;
            ApplyToChildren();
        }

        [ContextMenu("Apply To Children")]
        public void ApplyToChildren()
        {
            if (paletteTexture == null)
            {
                Debug.LogWarning("Palette texture not set on BuildingColorApplier.", this);
                return;
            }

            // Collect all renderers under this building
            var renderers = GetComponentsInChildren<Renderer>(true);

            // apply overrides first
            var overrideMap = new Dictionary<Renderer, int>();
            if (overrides != null)
            {
                foreach (var o in overrides)
                    if (o != null && o.renderer != null)
                        overrideMap[o.renderer] = o.paletteIndex;
            }

            foreach (var r in renderers)
            {
                int index = ResolveIndexForRenderer(r, overrideMap);
                SetRendererPaletteIndex(r, index);
            }
        }

        int ResolveIndexForRenderer(Renderer r, Dictionary<Renderer, int> overrideMap)
        {
            if (overrideMap != null && overrideMap.ContainsKey(r))
                return overrideMap[r];

            // heuristic by name: if name contains "Exterior", "Outside"
            string n = r.gameObject.name.ToLowerInvariant();
            if (n.Contains("exterior") || n.Contains("outside") || n.Contains("outer"))
                return defaultExteriorIndex;
            if (n.Contains("interior") || n.Contains("inside") || n.Contains("inner"))
                return defaultInteriorIndex;
            if (n.Contains("floor") || n.Contains("ground"))
                return defaultFloorIndex;

            // fallback: exterior
            return defaultExteriorIndex;
        }

        void SetRendererPaletteIndex(Renderer r, int index)
        {
            if (!_blocks.TryGetValue(r, out var block))
            {
                block = new MaterialPropertyBlock();
                _blocks[r] = block;
            }
            r.GetPropertyBlock(block);
            block.SetTexture(PROP_PALETTE, paletteTexture);
            block.SetFloat(PROP_PALSIZE, Mathf.Max(1, paletteSize));
            block.SetFloat(PROP_INDEX, Mathf.Max(0, index));
            r.SetPropertyBlock(block);
        }
    }
}
