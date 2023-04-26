using Assets.Scripts.GameInput;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(PlayerControlScheme))]
    public class PlayerInputControllerSchemeEditor : UnityEditor.Editor
    {
        SerializedProperty inputType;
        SerializedProperty moveUpKey;
        SerializedProperty moveDownKey;
        SerializedProperty moveLeftKey;
        SerializedProperty moveRightKey;
        
        void OnEnable()
        {
            inputType = serializedObject.FindProperty("inputType");
            moveUpKey = serializedObject.FindProperty("moveUpKey");
            moveDownKey = serializedObject.FindProperty("moveDownKey");
            moveLeftKey = serializedObject.FindProperty("moveLeftKey");
            moveRightKey = serializedObject.FindProperty("moveRightKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(inputType);

            if (inputType.enumValueIndex == (int)PlayerInputType.Keyboard)
            {
                EditorGUILayout.PropertyField(moveUpKey);
                EditorGUILayout.PropertyField(moveDownKey);
                EditorGUILayout.PropertyField(moveLeftKey);
                EditorGUILayout.PropertyField(moveRightKey);
            }

            // Draw other fields if needed
            // EditorGUI.BeginDisabledGroup(true);
            // // Draw other fields
            // EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}