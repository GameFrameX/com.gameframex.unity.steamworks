using GameFrameX.Editor;
using GameFrameX.SteamWorks.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.SteamWorks.Editor
{
    [CustomEditor(typeof(SteamWorksComponent))]
    internal sealed class SteamWorksComponentInspector : ComponentTypeComponentInspector
    {
        private SerializedProperty m_appId;

        private readonly GUIContent m_appIdGUIContent = new GUIContent("Steam的AppId");

        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(ISteamWorksManager));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_appId, m_appIdGUIContent);
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }

        protected override void Enable()
        {
            base.Enable();
            m_appId = serializedObject.FindProperty("m_appId");
        }
    }
}