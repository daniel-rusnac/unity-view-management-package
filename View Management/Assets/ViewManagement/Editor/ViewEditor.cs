using System;
using UnityEditor;
using UnityEngine;

namespace ViewManagement
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : Editor
    {
        private View view;

        private void OnEnable()
        {
            view = (View)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                DrawButtons();
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Show"))
                {
                    view.Show();
                }

                if (GUILayout.Button("Hide"))
                {
                    view.Hide();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}