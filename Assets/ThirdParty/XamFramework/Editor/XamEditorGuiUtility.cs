using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Xam.Editor
{
	public static class XamEditorGuiUtility
	{
		public static void ScriptFieldLayout( SerializedObject serializedObject, params GUILayoutOption[] options )
		{
			ScriptField( EditorGUILayout.GetControlRect( options ), serializedObject );
		}

		public static void ScriptField( Rect position, SerializedObject serializedObject )
		{
			GUI.enabled = false;
			{
				SerializedProperty scriptProperty = serializedObject.FindProperty( "m_Script" );
				EditorGUI.PropertyField( position, scriptProperty, true );
			}
			GUI.enabled = true;
		}

		public static Texture2D CreateTexture( int width, int height, Color color )
		{
			Color[] pixels = new Color[width * height];

			for ( int i = 0; i < pixels.Length; i++ )
			{
				pixels[i] = color;
			}

			Texture2D result = new Texture2D( width, height );
			result.SetPixels( pixels );
			result.Apply();

			return result;
		}
	}
}