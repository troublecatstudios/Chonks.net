using Chonks;
using Chonks.Unity;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Chonksy/Animator2D")]
public class Animator2D : MonoBehaviour, ISaveStore {
    public SpriteRenderer Renderer;
    public List<Sprite> Sprites;
    public float FrameTime = 1f / 12f;

    private int _spriteIndex = 0;
    private float _frameTimer = 0;

    public class AnimatorSaveState {
        public int SpriteIndex { get; set; }
        public float FrameTimer { get; set; }
    }

    public string GetStoreIdentifier() {
        return $"{gameObject.name}_Animator2D";
    }

    public List<SaveState> GetSaveStates() {
        return new List<SaveState>() {
            new SaveState() {
                ChunkName = gameObject.name,
                Data = new AnimatorSaveState {
                    SpriteIndex = _spriteIndex,
                    FrameTimer = _frameTimer
                }
            }
        };
    }

    public void LoadChunkData(string chunkName, ChunkDataSegment data) {
        var state = data.As<AnimatorSaveState>();
        _spriteIndex = state.SpriteIndex;
        _frameTimer = state.FrameTimer;
    }

    private void Awake() {
        UnitySaveManager.Instance.Register(this);
    }

    void Update() {
        if (_frameTimer <= 0) {
            _spriteIndex++;
            if (_spriteIndex >= Sprites.Count) {
                _spriteIndex = 0;
            }
            Renderer.sprite = Sprites[_spriteIndex];
            _frameTimer = FrameTime;
        }
        _frameTimer -= Time.deltaTime;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (Renderer && Sprites != null && Sprites.Count > 0) {
            Renderer.sprite = Sprites[0];
        }
    }
#endif
}
