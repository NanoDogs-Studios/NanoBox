using System.Collections.Generic;
using UnityEngine;

namespace Nanodogs.Nanobox.Map
{
    /// <summary>
    /// Procedurally generates and manages an infinite grid of flat grass plane chunks around a player.
    ///
    /// Notes:
    /// - Player assignment is intentionally left for you (see player field).
    /// - This creates GameObjects at runtime. Consider swapping to GPU instancing later if needed.
    /// </summary>
    public class NanoBoxInfiniteGrassManager : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Assign/resolve this at runtime (left for you).")]
        public Transform player;

        [Header("Chunk Settings")]
        [Tooltip("World size of one grass chunk in meters.")]
        public float chunkSize = 40f;

        [Tooltip("How many chunks to keep loaded in each direction (radius). Radius 2 => 5x5 chunks.")]
        [Min(0)]
        public int chunkRadius = 2;

        [Tooltip("Y position of the grass plane chunks.")]
        public float groundY = 0f;

        [Tooltip("Optional: rotate the plane to be horizontal if your mesh isn't already.")]
        public bool rotatePlaneToHorizontal = true;

        [Header("Rendering")]
        [Tooltip("Grass material to apply to each chunk renderer.")]
        public Material grassMaterial;

        [Tooltip("Optional: Assign a custom mesh (e.g., a subdivided plane). If null, Unity's built-in Plane is used.")]
        public Mesh chunkMesh;

        [Tooltip("If true, sets material mainTextureScale so tiling stays consistent across chunk sizes.")]
        public bool autoTextureTiling = true;

        [Tooltip("How many texture tiles per chunk (only used when autoTextureTiling is true).")]
        public float tilesPerChunk = 1f;

        [Header("Performance")]
        [Tooltip("If true, only updates when the player crosses into a new chunk.")]
        public bool updateOnlyOnChunkChange = true;

        [Tooltip("Optional: parent for chunk objects. Defaults to this transform.")]
        public Transform chunkParent;

        // Internal state
        private readonly Dictionary<Vector2Int, GameObject> _chunks = new();
        private readonly HashSet<Vector2Int> _neededThisFrame = new();
        private Vector2Int _lastPlayerChunk = new(int.MinValue, int.MinValue);

        private void Awake()
        {
            if (chunkParent == null) chunkParent = transform;
        }

        private void Start()
        {
            // Player assignment intentionally omitted.
            // Example (later): player = NanoBoxGameManager.GetLocalPlayerGameObject().transform;

            if (player != null)
            {
                _lastPlayerChunk = WorldToChunk(player.position);
                RebuildAroundPlayer(force: true);
            }
        }

        private void Update()
        {
            if (player == null) return;

            var currentChunk = WorldToChunk(player.position);

            if (updateOnlyOnChunkChange)
            {
                if (currentChunk != _lastPlayerChunk)
                {
                    _lastPlayerChunk = currentChunk;
                    RebuildAroundPlayer(force: false);
                }
            }
            else
            {
                // Always ensure correct set is loaded.
                _lastPlayerChunk = currentChunk;
                RebuildAroundPlayer(force: false);
            }
        }

        private void RebuildAroundPlayer(bool force)
        {
            // Compute the set of chunks we want loaded.
            _neededThisFrame.Clear();

            for (int dz = -chunkRadius; dz <= chunkRadius; dz++)
            {
                for (int dx = -chunkRadius; dx <= chunkRadius; dx++)
                {
                    var key = new Vector2Int(_lastPlayerChunk.x + dx, _lastPlayerChunk.y + dz);
                    _neededThisFrame.Add(key);

                    if (!_chunks.ContainsKey(key))
                    {
                        var go = CreateChunk(key);
                        _chunks.Add(key, go);
                    }
                    else if (force)
                    {
                        // Re-apply settings (useful if you tweak params at runtime in the inspector).
                        ApplyChunkSettings(_chunks[key], key);
                    }
                }
            }

            // Unload chunks we no longer need.
            // (Avoid modifying dictionary while iterating it.)
            if (_chunks.Count > _neededThisFrame.Count)
            {
                List<Vector2Int> toRemove = null;

                foreach (var kvp in _chunks)
                {
                    if (!_neededThisFrame.Contains(kvp.Key))
                    {
                        toRemove ??= new List<Vector2Int>();
                        toRemove.Add(kvp.Key);
                    }
                }

                if (toRemove != null)
                {
                    for (int i = 0; i < toRemove.Count; i++)
                    {
                        var k = toRemove[i];
                        if (_chunks.TryGetValue(k, out var go))
                        {
                            if (go != null) Destroy(go);
                            _chunks.Remove(k);
                        }
                    }
                }
            }
        }

        private GameObject CreateChunk(Vector2Int chunkCoord)
        {
            // Create a chunk GameObject.
            var go = new GameObject($"GrassChunk_{chunkCoord.x}_{chunkCoord.y}");
            go.transform.SetParent(chunkParent, worldPositionStays: false);

            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();

            // Mesh setup
            if (chunkMesh != null)
            {
                mf.sharedMesh = chunkMesh;
            }
            else
            {
                // Built-in Plane primitive mesh is easiest to get by creating a temporary primitive.
                // We avoid keeping the primitive by extracting its mesh and destroying it.
                var temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
                mf.sharedMesh = temp.GetComponent<MeshFilter>().sharedMesh;
                Destroy(temp);
            }

            // Material
            if (grassMaterial != null)
            {
                // Use sharedMaterial to avoid duplicating materials per chunk.
                mr.sharedMaterial = grassMaterial;
            }

            ApplyChunkSettings(go, chunkCoord);
            return go;
        }

        private void ApplyChunkSettings(GameObject chunk, Vector2Int chunkCoord)
        {
            // Position the chunk by chunkCoord. X/Z map to x/y in Vector2Int.
            Vector3 pos = ChunkToWorld(chunkCoord);
            chunk.transform.position = pos;

            // Scale: Unity plane is 10x10 units by default.
            // To get chunkSize x chunkSize, scale by chunkSize/10.
            float planeScale = chunkSize / 10f;

            // Ensure horizontal orientation if your mesh is vertical.
            if (rotatePlaneToHorizontal)
            {
                chunk.transform.rotation = Quaternion.identity; // Unity Plane is already horizontal.
            }

            chunk.transform.localScale = new Vector3(planeScale, 1f, planeScale);

            // Optional: set texture tiling so the material tiles consistently per chunk.
            if (autoTextureTiling)
            {
                var mr = chunk.GetComponent<MeshRenderer>();
                if (mr != null && mr.sharedMaterial != null)
                {
                    // NOTE: Changing sharedMaterial affects all chunks using it.
                    // If you need per-chunk tiling, clone the material per chunk.
                    // For consistent global tiling, sharedMaterial is correct.
                    mr.sharedMaterial.mainTextureScale = new Vector2(tilesPerChunk, tilesPerChunk);
                }
            }
        }

        private Vector2Int WorldToChunk(Vector3 worldPos)
        {
            // Convert world position to chunk coordinate.
            // We consider chunks centered on their area, but floor division works fine.
            int cx = Mathf.FloorToInt(worldPos.x / chunkSize);
            int cz = Mathf.FloorToInt(worldPos.z / chunkSize);
            return new Vector2Int(cx, cz);
        }

        private Vector3 ChunkToWorld(Vector2Int chunkCoord)
        {
            // Place chunk so it aligns in a grid.
            // This places the plane centered in its chunk cell.
            float x = (chunkCoord.x + 0.5f) * chunkSize;
            float z = (chunkCoord.y + 0.5f) * chunkSize;
            return new Vector3(x, groundY, z);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Keep values sane.
            if (chunkSize < 1f) chunkSize = 1f;
            if (tilesPerChunk < 0.01f) tilesPerChunk = 0.01f;

            // If in play mode and tweaking values, refresh loaded chunks.
            if (Application.isPlaying && player != null)
            {
                _lastPlayerChunk = WorldToChunk(player.position);
                RebuildAroundPlayer(force: true);
            }
        }
#endif
    }
}
