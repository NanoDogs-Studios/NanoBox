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
            var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.layer = LayerMask.NameToLayer("Icon");

            iconCamera.Render();

            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            Destroy(go);

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }
    }

}
