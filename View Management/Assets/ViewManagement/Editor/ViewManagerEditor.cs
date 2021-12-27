using UnityEditor;
using UnityEngine;

namespace ViewManagement
{
    [CustomEditor(typeof(ViewManager))]
    public class ViewManagerEditor : Editor
    {
        private ViewManager viewManager;

        private void OnEnable()
        {
            viewManager = (ViewManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                DrawStackString();
                Repaint();
            }
            else
            {
                DrawButtons();
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("(Re)Load"))
                {
                    viewManager.LoadViews();
                    EditorUtility.SetDirty(viewManager);
                }

                if (GUILayout.Button("Clear"))
                {
                    viewManager.ClearViews();
                    EditorUtility.SetDirty(viewManager);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawStackString()
        {
            if (viewManager.activeViewsStack.Count == 0)
                return;

            GUIStyle style = new GUIStyle("label");
            style.richText = true;

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("View Stack", GetViewStackString(), style);
            }
            EditorGUILayout.EndHorizontal();
        }

        private string GetViewStackString()
        {
            string s = "";

            foreach (View view in viewManager.activeViewsStack)
            {
                string color = view.IsLocked ? "red" : view.IsShown ? "green" : "grey";
                s += $" <color={color}>{view.name}</color> <";
            }

            if (s.Length > 0)
            {
                s = s.Remove(s.Length - 1);
            }

            return s;
        }
    }
}