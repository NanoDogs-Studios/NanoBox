using System.Collections.Generic;
using UnityEngine;

namespace Nanodogs.Nanobox.Map
{
    public class NanoBoxInfiniteGrassManager : MonoBehaviour
    {
        [Header("Player (assign later)")]
        public Transform player;

        [Header("Chunks")]
        public float chunkSize = 40f;     // world meters per chunk
        public int radius = 2;            // radius in chunks (2 => 5x5)
        public float groundY = 0f;

        [Header("Rendering")]
        public Material grassMaterial;
        public float tilesPerChunk = 4f;  // “ProBuilder-ish” tiling amount per chunk
        public bool randomUVRotation = true;

        [Header("UVs")]
        [Tooltip("World meters per texture tile (smaller = denser grass)")]
        public float uvScale = 4f;

        [Header("Perf")]
        public int prewarmPool = 64;

        readonly Dictionary<Vector2Int, Chunk> active = new();
        readonly Queue<GameObject> pool = new();
        readonly HashSet<Vector2Int> needed = new();

        Mesh[] uvRotMeshes; // 0/90/180/270
        Vector2Int lastChunk = new(int.MinValue, int.MinValue);

        class Chunk
        {
            public GameObject go;
            public Renderer r;
            public int rot; // 0..3
        }

        void Awake()
        {
            BuildUVRotMeshes();
            Prewarm(prewarmPool);
        }

        void Start()
        {
            if (player != null)
            {
                lastChunk = WorldToChunk(player.position);
                Rebuild(force: true);
            }
        }

        void Update()
        {
            if (!player) return;

            var c = WorldToChunk(player.position);
            if (c == lastChunk) return;

            lastChunk = c;
            Rebuild(force: false);
        }

        // -------------------------

        void Rebuild(bool force)
        {
            needed.Clear();

            for (int dz = -radius; dz <= radius; dz++)
                for (int dx = -radius; dx <= radius; dx++)
                {
                    var key = new Vector2Int(lastChunk.x + dx, lastChunk.y + dz);
                    needed.Add(key);

                    if (!active.ContainsKey(key))
                        active[key] = Spawn(key);
                    else if (force)
                        Apply(active[key], key, active[key].rot);
                }

            // Despawn unneeded
            if (active.Count <= needed.Count) return;

            List<Vector2Int> remove = null;
            foreach (var kv in active)
                if (!needed.Contains(kv.Key))
                {
                    remove ??= new List<Vector2Int>();
                    remove.Add(kv.Key);
                }

            if (remove == null) return;

            for (int i = 0; i < remove.Count; i++)
            {
                var k = remove[i];
                Despawn(active[k]);
                active.Remove(k);
            }
        }

        Chunk Spawn(Vector2Int coord)
        {
            var go = (pool.Count > 0) ? pool.Dequeue() : CreateGO();
            go.name = $"GrassChunk_{coord.x}_{coord.y}";
            go.SetActive(true);

            var c = new Chunk { go = go, r = go.GetComponent<Renderer>() };
            int rot = randomUVRotation ? Random.Range(0, 4) : 0;
            c.rot = rot;

            Apply(c, coord, rot);
            return c;
        }

        void Despawn(Chunk c)
        {
            c.go.SetActive(false);
            pool.Enqueue(c.go);
        }

        void Apply(Chunk c, Vector2Int coord, int rot)
        {
            c.go.transform.position = ChunkToWorld(coord);

            float s = chunkSize / 10f; // Unity Plane is 10x10 units
            c.go.transform.localScale = new Vector3(s, 1f, s);

            // Mesh (UV rotation)
            var mf = c.go.GetComponent<MeshFilter>();
            mf.sharedMesh = uvRotMeshes[rot];

            var mr = c.r;
            if (grassMaterial)
                mr.sharedMaterial = grassMaterial;

            // ----- UV SCALE (ProBuilder-style) -----
            // How many tiles fit across this chunk in world space
            float tiles = chunkSize / Mathf.Max(0.0001f, uvScale);

            var mpb = new MaterialPropertyBlock();
            mr.GetPropertyBlock(mpb);
            mpb.SetVector("_MainTex_ST", new Vector4(tiles, tiles, 0f, 0f));
            mr.SetPropertyBlock(mpb);
        }

        // -------------------------
        // Pool / Creation
        // -------------------------

        void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var go = CreateGO();
                go.SetActive(false);
                pool.Enqueue(go);
            }
        }

        GameObject CreateGO()
        {
            var go = new GameObject("Grass Chunk");
            go.transform.SetParent(transform, false);

            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();

            // default mesh
            mf.sharedMesh = uvRotMeshes[0];
            if (grassMaterial) mr.sharedMaterial = grassMaterial;

            return go;
        }

        // -------------------------
        // Coords
        // -------------------------

        Vector2Int WorldToChunk(Vector3 p)
            => new Vector2Int(Mathf.FloorToInt(p.x / chunkSize), Mathf.FloorToInt(p.z / chunkSize));

        Vector3 ChunkToWorld(Vector2Int c)
            => new Vector3((c.x + 0.5f) * chunkSize, groundY, (c.y + 0.5f) * chunkSize);

        // -------------------------
        // UV rotation meshes (0/90/180/270)
        // -------------------------

        void BuildUVRotMeshes()
        {
            // Grab Unity plane mesh once.
            var temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
            var baseMesh = temp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(temp);

            uvRotMeshes = new Mesh[4];
            uvRotMeshes[0] = baseMesh;
        }
    }
}
