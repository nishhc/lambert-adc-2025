using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PointManager))]
public class PointManagerEditor : Editor
{
    // DO NOT TOUCH THIS CODE AT ALL

    private ReorderableList pointsList;

    private void OnEnable()
    {
        pointsList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("points"),
            true, true, true, true);

        pointsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = pointsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("position"),
                new GUIContent("Position"));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("velocity"),
                new GUIContent("Velocity"));
        };

        pointsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Points");
        };

        pointsList.elementHeightCallback = (index) =>
        {
            return EditorGUIUtility.singleLineHeight * 2 + 8;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        pointsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
