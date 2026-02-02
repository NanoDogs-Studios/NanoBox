using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nanodogs.Nanobox.UI
{
    public class NbIconRenderer : MonoBehaviour
    {
        public static NbIconRenderer Instance;

        public Camera iconCamera;
        public RenderTexture rt;

        void Awake() => Instance = this;

        public Sprite Render(GameObject prefab)
        {
            // 1. Instantiate
            var go = Instantiate(prefab);
            SetLayerRecursively(go, LayerMask.NameToLayer("Icon"));

            // 2. Frame
            FrameObject(go);

            // 3. HARD reset render target
            iconCamera.targetTexture = null;
            iconCamera.targetTexture = rt;

            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = prev;

            // 4. Render
            iconCamera.Render();

            // 5. Read pixels
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            // 6. Cleanup
            Destroy(go);

            return Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }

        void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform t in obj.transform)
                SetLayerRecursively(t.gameObject, layer);
        }

        void FrameObject(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;

            Bounds bounds = renderers[0].bounds;
            foreach (var r in renderers)
                bounds.Encapsulate(r.bounds);

            float size = bounds.extents.magnitude;
            iconCamera.transform.position = bounds.center + new Vector3(-1, 1, -size * 2f);
            iconCamera.transform.LookAt(bounds.center);
        }
    }

}
