using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chonks.UnityEditor {
    [Serializable]
    public class ListChunksInterpreter : IUnityEditorInterpreter {

        [SerializeField] SaveChunk[] _chunks = new SaveChunk[0];

        private Vector2 _scollPosition = Vector2.zero;
        private bool _isDirty = false;
        private Dictionary<int, List<string>> _changeTracking = new Dictionary<int, List<string>>();

        public void DrawInspectorGUI() {
            var label = $"{nameof(ListChunksInterpreter)}{(_isDirty ? "*" : "")}";
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label);
            EditorGUI.indentLevel++;
            _scollPosition = EditorGUILayout.BeginScrollView(_scollPosition, GUILayout.Height(200));

            int idx = 0;
            foreach (var chunk in _chunks) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(chunk.Name);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                foreach (var entry in chunk.Data) {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(entry.Key);
                    EditorGUILayout.Space();
                    var value = EditorGUILayout.TextArea(entry.Value, GUILayout.Height(32));
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();

                    if (value != entry.Value) {
                        if (_changeTracking.ContainsKey(idx)) {
                            _changeTracking[idx].Add(entry.Key);
                        } else {
                            _changeTracking.Add(idx, new List<string>() { entry.Key });
                        }
                        _chunks[idx].Data[entry.Key] = value;
                        _isDirty = true;
                    }
                }
                idx++;
            }
            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        public SaveChunk[] ApplyModifications(SaveChunk[] chunks) {
            foreach (var entry in _changeTracking) {
                var modifiedChunk = _chunks[entry.Key];
                foreach (var chunk in chunks) {
                    if (chunk.Name == modifiedChunk.Name) {
                        foreach (var modifiedKey in entry.Value) {
                            if (chunk.Data.ContainsKey(modifiedKey)) {
                                chunk.Data[modifiedKey] = modifiedChunk.Data[modifiedKey];
                            }
                        }
                    }
                }
            }
            return chunks;
        }

        public bool IsDirty() => _isDirty;

        public void ProcessChunks(SaveChunk[] chunks) {
            _changeTracking.Clear();
            _chunks = chunks;
            _isDirty = false;
        }
    }
}
