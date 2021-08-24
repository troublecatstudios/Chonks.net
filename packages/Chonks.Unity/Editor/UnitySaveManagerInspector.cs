using Chonks.Unity;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chonks.UnityEditor {

    [CustomEditor(typeof(UnitySaveManager))]
    public class UnitySaveManagerInspector : Editor {
        [NonSerialized]
        private List<IUnityEditorInterpreter> _editorInterpreters = new List<IUnityEditorInterpreter>() {
            new ListChunksInterpreter()
        };

        private UnitySaveManager Manager => (UnitySaveManager)target;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            foreach (var i in _editorInterpreters) {
                i.DrawInspectorGUI();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Make Snapshot", GUILayout.Height(32))) {
                Manager.MakeSnapshot();
            }
            GUILayout.Space(20);
            if (GUILayout.Button("Apply Snapshot", GUILayout.Height(32))) {
                Manager.ApplySnapshot("Save1");
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnEnable() {
            foreach (var i in _editorInterpreters) {
                Manager.Register(i);
            }
        }

        private void OnDisable() {
            foreach (var i in _editorInterpreters) {
                Manager.Unregister(i);
            }
        }
    }
}
