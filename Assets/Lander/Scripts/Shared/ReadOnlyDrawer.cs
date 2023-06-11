using UnityEngine;
using UnityEditor;
using System;

namespace Lander.Shared
{

    /// <summary>
    /// This class contain custom drawer for ReadOnly attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Unity method for drawing GUI in Editor
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }


    /// <summary>Атрибут для отображения свойства в редакторе без возможности редактирования.</summary>
    public sealed class ReadonlyAttribute : PropertyAttribute
    {
    }
}