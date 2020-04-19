using Assets.Pixelation.Example.Scripts;
using UnityEngine;

namespace Assets.Pixelation.Scripts
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Pixelation")]
    public class Pixelation : ImageEffectBase
    {
        [Range(64.0f, 512.0f)] public float BlockCount = 128;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            float k = Camera.main.aspect;
            Vector2 count = new Vector2(BlockCount, BlockCount/k);
            Vector2 size = new Vector2(1.0f/count.x, 1.0f/count.y);
            //
            material.SetVector("BlockCount", count);
            material.SetVector("BlockSize", size);
            Graphics.Blit(source, destination, material);
        }
    }
}