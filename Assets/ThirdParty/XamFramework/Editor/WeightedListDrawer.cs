using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Xam.Editor
{
	using Xam.Utility.Randomization;

	[CustomPropertyDrawer( typeof( WeightedList ), true )]
	public class WeightedListDrawer : PropertyDrawer
	{
		public const string k_itemsPropertyName = "m_items";
		
		private Dictionary<string, ReorderablePropertyData> m_propertyPathToReorderableProperties = null;

		public WeightedListDrawer()
		{
			m_propertyPathToReorderableProperties = new Dictionary<string, ReorderablePropertyData>();
		}

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			ReorderablePropertyData reorderableProperty = GetReorderablePropertyData( property );

			OnGUI_ItemsList( position, property.displayName, reorderableProperty );

			if ( property.serializedObject.hasModifiedProperties )
			{
				NormalizeWeights( reorderableProperty.Property );

				property.serializedObject.ApplyModifiedProperties();
			}
		}

		private void OnGUI_ItemsList( Rect position, string headerName, ReorderablePropertyData reorderableProperty )
		{
			ReorderableList reorderable = reorderableProperty.Reorderable;
			SerializedProperty listProperty = reorderableProperty.Property;

			reorderable.drawHeaderCallback = ( Rect headerPos ) =>
			{
				EditorGUI.LabelField( headerPos, headerName );
			};

			reorderable.drawElementCallback = ( Rect elementPos, int index, bool isActive, bool isFocused ) =>
			{
				SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex( index );
				elementPos.height = EditorGUI.GetPropertyHeight( elementProperty );
				elementPos.y += EditorGUIUtility.standardVerticalSpacing;

				EditorGUI.PropertyField( elementPos, elementProperty );
			};

			reorderable.elementHeightCallback = ( int index ) =>
			{
				SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex( index );
				float height = EditorGUI.GetPropertyHeight( elementProperty );

				return height + EditorGUIUtility.singleLineHeight / 2f;
			};

			reorderable.DoList( position );
		}

		private ReorderablePropertyData GetReorderablePropertyData( SerializedProperty property )
		{
			if ( m_propertyPathToReorderableProperties.TryGetValue( property.propertyPath, out ReorderablePropertyData data ) )
			{
				return data;
			}

			data = new ReorderablePropertyData( property, k_itemsPropertyName );
			m_propertyPathToReorderableProperties.Add( property.propertyPath, data );

			return data;
		}
		
		protected void NormalizeWeights( SerializedProperty itemsProperty )
		{
			int weightSum = 0;
			for ( int idx = 0; idx < itemsProperty.arraySize; ++idx )
			{
				SerializedProperty elementProperty = itemsProperty.GetArrayElementAtIndex( idx );
				SerializedProperty elementWeightProperty = elementProperty.FindPropertyRelative( WeightedNodeDrawer.k_normalizedWeightPropertyName );

				weightSum += elementWeightProperty.intValue;
			}

			if ( weightSum <= 0 ) { return; }

			for ( int idx = 0; idx < itemsProperty.arraySize; ++idx )
			{
				SerializedProperty elementProperty = itemsProperty.GetArrayElementAtIndex( idx );
				SerializedProperty elementWeightProperty = elementProperty.FindPropertyRelative( WeightedNodeDrawer.k_normalizedWeightPropertyName );

				float influence = elementWeightProperty.intValue / (float)weightSum;
				int normalizedWeight = Mathf.RoundToInt( influence * WeightedList.k_maxWeight );

				elementWeightProperty.intValue = normalizedWeight;
			}
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			ReorderablePropertyData reorderableProperty = GetReorderablePropertyData( property );
			return reorderableProperty.Reorderable.GetHeight();
		}
	}

	public class ReorderablePropertyData
	{
		public ReorderableList Reorderable;
		public SerializedProperty Property;

		public ReorderablePropertyData( SerializedProperty property, string listPropertyName )
		{
			Property = property.FindPropertyRelative( listPropertyName );
			Reorderable = new ReorderableList( property.serializedObject, Property, true, true, true, true );
		}
	}

	[CustomPropertyDrawer( typeof( WeightedNode ), true )]
	public class WeightedNodeDrawer : PropertyDrawer
	{
		public const string k_normalizedWeightPropertyName = "m_normalizedWeight";
		public const string k_weightPropertyName = "Weight";
		public const string k_itemPropertyName = "Item";

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.DrawRect( position, new Color( 0, 0, 0, 0.2f ) );

			position.height = EditorGUIUtility.singleLineHeight;

			SerializedProperty weightProperty = property.FindPropertyRelative( k_normalizedWeightPropertyName );
			EditorGUI.PropertyField( position, weightProperty, new GUIContent( "Weight" ) );
			position.y += EditorGUI.GetPropertyHeight( weightProperty );

			position.y += EditorGUIUtility.standardVerticalSpacing;

			SerializedProperty itemProperty = property.FindPropertyRelative( k_itemPropertyName );
			if ( itemProperty != null )
			{
				EditorGUI.PropertyField( position, itemProperty, true );
			}
			else
			{
				EditorGUI.LabelField( position, "Non-serialized property in use" );
			}
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			int numProperties = 1; // "Weight" property ...

			SerializedProperty itemProperty = property.FindPropertyRelative( k_itemPropertyName );
			numProperties += itemProperty != null
				? itemProperty.CountInProperty()
				: 1; // "Non-serialized property in use" warning ...

			return numProperties * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
		}
	}
}