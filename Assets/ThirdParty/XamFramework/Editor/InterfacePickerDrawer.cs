using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Xam.Editor
{
	using Attributes;

	[CustomPropertyDrawer( typeof( InterfacePickerAttribute ) )]
	public class InterfacePickerDrawer : PropertyDrawer
	{
		private const string k_nullName = "Null";
		private const float k_editButtonWidth = 25;
		private const string k_editButtonIconName = "d_editicon.sml";

		private System.Type[] m_interfaceImplementerTypes = null;
		private string[] m_interfaceImplementerNames = null;
		private int m_selectedImplementorIdx = 0;

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.DrawRect( position, new Color( 0, 0, 0, 0.1f ) );
			position.height = EditorGUIUtility.singleLineHeight;

			Initialize( property );
			
			//position.width -= k_editButtonWidth;
			label.text += $" ({m_interfaceImplementerNames[m_selectedImplementorIdx]})";
			EditorGUI.PropertyField( position, property, label, true );
			
			OnGUI_InterfaceSelection( position, property );
		}

		private void Initialize( SerializedProperty property )
		{
			if ( m_interfaceImplementerTypes != null ) { return; }

			System.Type interfaceType = fieldInfo.FieldType;
			
			IEnumerable<System.Type> implementingTypes = System.AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany( a => a.GetTypes() )
				.Where( t => interfaceType != t && interfaceType.IsAssignableFrom( t ) );

			m_interfaceImplementerTypes = implementingTypes.ToArray();
			m_interfaceImplementerNames = new string[m_interfaceImplementerTypes.Length + 1];
			m_interfaceImplementerNames[0] = k_nullName;

			for ( int idx = 1; idx < m_interfaceImplementerNames.Length; ++idx )
			{
				m_interfaceImplementerNames[idx] = m_interfaceImplementerTypes[idx - 1].Name;
			}

			m_selectedImplementorIdx = 0;
			if ( !string.IsNullOrEmpty( property.managedReferenceFullTypename ) )
			{
				int lastDotIdx = property.managedReferenceFullTypename.LastIndexOf( '.' );
				string currentImplementerName = property.managedReferenceFullTypename.Substring( lastDotIdx + 1 );

				for ( int idx = 1; idx < m_interfaceImplementerNames.Length; ++idx )
				{
					if ( currentImplementerName == m_interfaceImplementerNames[idx] )
					{
						m_selectedImplementorIdx = idx;
						break;
					}
				}
			}
		}

		private void OnGUI_InterfaceSelection( Rect position, SerializedProperty property )
		{
			EditorGUI.BeginChangeCheck();

			//Rect buttonPos = new Rect( position );
			//buttonPos.x += position.width;
			//buttonPos.width = k_editButtonWidth;
			//buttonPos.height = EditorGUI.GetPropertyHeight( property, true );
			//GUI.Button( buttonPos, EditorGUIUtility.IconContent( k_editButtonIconName ) );
			
			position.y += EditorGUI.GetPropertyHeight( property, true );
			m_selectedImplementorIdx = EditorGUI.Popup( position, m_selectedImplementorIdx, m_interfaceImplementerNames );

			if ( EditorGUI.EndChangeCheck() )
			{
				if ( m_selectedImplementorIdx == 0 )
				{
					property.SetValue( null );
				}
				else
				{
					int typeIdx = m_selectedImplementorIdx - 1;
					System.Type targetType = m_interfaceImplementerTypes[typeIdx];

					object newInterface = System.Activator.CreateInstance( targetType );
					property.SetValue( newInterface );
				}
			}
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return EditorGUI.GetPropertyHeight( property, true ) + EditorGUIUtility.singleLineHeight;
		}
	}
}