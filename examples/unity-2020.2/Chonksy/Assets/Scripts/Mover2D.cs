using Chonks;
using Chonks.Unity;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Chonksy/Mover2D")]
public class Mover2D : MonoBehaviour, ISaveStore {
    public float MovementSpeed = 5f;
    public LayerMask CollisionLayerMask;
    public float WallCheckDistance = 4f;
    public float GroundCheckDistance = 1f;

    private Vector2 _direction = Vector2.right;

    public class MoverSaveState {
        public Vector2 Direction { get; set; }
        public Vector3 Position { get; set; }
    }

    public List<SaveState> CreateSaveStates() {
        return new List<SaveState>() {
            new SaveState() {
                ChunkName = gameObject.name,
                Data = new MoverSaveState {
                    Direction = _direction,
                    Position = transform.position
                }
            }
        };
    }

    public string GetStoreIdentifier() => $"{gameObject.name}_Mover2D";

    public void ProcessChunkData(string chunkName, ChunkDataSegment data) {
        var state = data.As<MoverSaveState>();
        _direction = state.Direction;
        transform.position = state.Position;
    }

    private void Awake() {
        UnitySaveManager.Instance.Register(this);
    }

    void Update() {
        var rayStart = transform.position + Vector3.up;
        var wallCheck = Physics2D.Raycast(rayStart, _direction, WallCheckDistance, CollisionLayerMask);
#if UNITY_EDITOR
        Debug.DrawRay(rayStart, _direction * WallCheckDistance, wallCheck.collider ? Color.red : Color.green);
#endif
        if (wallCheck.collider) {
            _direction *= -1;
        }

        transform.position += (Vector3)_direction * MovementSpeed * Time.deltaTime;
    }
}
