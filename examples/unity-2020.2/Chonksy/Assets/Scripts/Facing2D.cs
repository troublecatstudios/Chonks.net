using Chonks;
using Chonks.Unity;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Chonksy/Facing2D")]
public class Facing2D : MonoBehaviour, ISaveStore {
    public SpriteRenderer Renderer;

    private Vector3 _lastPosition;

    public class FacingSaveState {
        public Vector3 LastPosition { get; set; }
        public bool Flipped { get; set; }
    }

    public List<SaveState> CreateSaveStates() {
        return new List<SaveState>() {
            new SaveState() {
                ChunkName = gameObject.name,
                Data = new FacingSaveState {
                    LastPosition = _lastPosition,
                    Flipped = Renderer.flipX
                }
            }
        };
    }

    public string GetStoreIdentifier() {
        return $"{gameObject.name}_Facing2D";
    }

    public void ProcessChunkData(string chunkName, ChunkDataSegment data) {
        var state = data.As<FacingSaveState>();
        _lastPosition = state.LastPosition;
        Renderer.flipX = state.Flipped;
    }

    private void Awake() {
        UnitySaveManager.Instance.Register(this);
    }

    private void Start() {
        _lastPosition = transform.position;
    }

    void Update() {
        var delta = _lastPosition - transform.position;
        if (delta.x < -0.1f) {
            Renderer.flipX = false;
            _lastPosition = transform.position;
        }

        if (delta.x > 0.1f) {
            Renderer.flipX = true;
            _lastPosition = transform.position;
        }
    }
}