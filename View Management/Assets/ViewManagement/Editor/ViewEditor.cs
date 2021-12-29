using UnityEditor;
using UnityEngine;
using ViewManagement.Components;

namespace ViewManagement
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : Editor
    {
        private bool isInitialized;
        private GUIStyle buttonStyle;
        private View view;

        private void Initialize()
        {
            if (isInitialized)
                return;

            buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
            view = (View)target;
            
            isInitialized = true;
        }

        public override void OnInspectorGUI()
        {
            Initialize();
            
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                DrawButtons();
            }
            
            DrawCallbacks(view.viewCallbacksController);
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

        private void DrawCallbacks(ViewCallbacksController callbacksController)
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.BeginHorizontal();
            {
                callbacksController.drawInitialize = GUILayout.Toggle(callbacksController.drawInitialize, new GUIContent("OnInitialize", "Event only once, when the ViewManagers is initialized."), buttonStyle);
                callbacksController.drawShow = GUILayout.Toggle(callbacksController.drawShow, new GUIContent("OnShow"), buttonStyle);
                callbacksController.drawShown = GUILayout.Toggle(callbacksController.drawShown, new GUIContent("OnShown"), buttonStyle);
                callbacksController.drawHide = GUILayout.Toggle(callbacksController.drawHide, new GUIContent("OnHide"), buttonStyle);
                callbacksController.drawHidden = GUILayout.Toggle(callbacksController.drawHidden, new GUIContent("OnHidden"), buttonStyle);
                callbacksController.drawExit = GUILayout.Toggle(callbacksController.drawExit, new GUIContent("OnExit"), buttonStyle);
            }
            EditorGUILayout.EndHorizontal();

            if (callbacksController.drawInitialize)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onInitialize)));
            if (callbacksController.drawShow)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onShow)));
            if (callbacksController.drawShown)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onShown)));
            if (callbacksController.drawHide)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onHide)));
            if (callbacksController.drawHidden)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onHidden)));
            if (callbacksController.drawExit)
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(view.viewCallbacksController)).FindPropertyRelative(nameof(callbacksController.onExit)));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}