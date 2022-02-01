using UnityEditor;
using UnityEngine;
using ViewManagement.Components;

namespace ViewManagement
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : Editor
    {
        private const string INITIALIZE_TOOLTIP = "Fired once, in Awake from the ViewManager.";
        private const string SHOW_TOOLTIP = "Fired every time the view is activated.";
        private const string SHOWN_TOOLTIP = "Fired when the the finished all show animations.";
        private const string HIDE_TOOLTIP = "Fired when the view starts the hide animation.";
        private const string HIDDEN_TOOLTIP = "Fired when the view finished all hide animations and is disabled.";
        private const string EXIT_TOOLTIP = "Fired when this is the only active view and 'Back Button' is pressed.";
        
        private bool _isInitialized;
        private GUIStyle _buttonStyle;
        private View _view;
        private SerializedProperty _viewCallbacksProperty;

        private void Initialize()
        {
            if (_isInitialized)
                return;

            _buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
            _view = (View)target;
            _viewCallbacksProperty = serializedObject.FindProperty(nameof(_view.viewCallbacksController));
            
            _isInitialized = true;
        }

        public override void OnInspectorGUI()
        {
            Initialize();
            
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                DrawButtons();
            }
            
            EditorGUI.BeginChangeCheck();
            {
                DrawCallbacks(_view.viewCallbacksController);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Show"))
                {
                    _view.Show();
                }

                if (GUILayout.Button("Hide"))
                {
                    _view.Hide();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCallbacks(ViewCallbacksController callbacksController)
        {
            serializedObject.UpdateIfRequiredOrScript();
            
            EditorGUILayout.BeginHorizontal();
            {
                callbacksController.drawInitialize = GUILayout.Toggle(callbacksController.drawInitialize, new GUIContent("OnInitialize", INITIALIZE_TOOLTIP), _buttonStyle);
                callbacksController.drawShow = GUILayout.Toggle(callbacksController.drawShow, new GUIContent("OnShow", SHOW_TOOLTIP), _buttonStyle);
                callbacksController.drawShown = GUILayout.Toggle(callbacksController.drawShown, new GUIContent("OnShown", SHOWN_TOOLTIP), _buttonStyle);
                callbacksController.drawHide = GUILayout.Toggle(callbacksController.drawHide, new GUIContent("OnHide", HIDE_TOOLTIP), _buttonStyle);
                callbacksController.drawHidden = GUILayout.Toggle(callbacksController.drawHidden, new GUIContent("OnHidden", HIDDEN_TOOLTIP), _buttonStyle);
                callbacksController.drawExit = GUILayout.Toggle(callbacksController.drawExit, new GUIContent("OnExit", EXIT_TOOLTIP), _buttonStyle);
            }
            EditorGUILayout.EndHorizontal();

            DrawCallback(callbacksController.drawInitialize, nameof(callbacksController.onInitialize));
            DrawCallback(callbacksController.drawShow, nameof(callbacksController.onShow));
            DrawCallback(callbacksController.drawShown, nameof(callbacksController.onShown));
            DrawCallback(callbacksController.drawHide, nameof(callbacksController.onHide));
            DrawCallback(callbacksController.drawHidden, nameof(callbacksController.onHidden));
            DrawCallback(callbacksController.drawExit, nameof(callbacksController.onExit));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCallback(bool draw, string propertyName)
        {
            if (_viewCallbacksProperty == null)
            {
                _isInitialized = false;
                Initialize();
            }
            
            if (draw)
            {
                EditorGUILayout.PropertyField(_viewCallbacksProperty.FindPropertyRelative(propertyName));
            }
        }
    }
}