using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Xam.Editor
{
	public class StateAndFactoryPairCreationWizard : EditorWindow
	{
		private const string k_namespaceSaveKey = "StateAndFactory/Namespace";

		private string m_namespace = null;
		private string m_stateClassName = null;
		private string m_stateParentClassName = null;
		private string m_stateKeyTypeName = null;

		private bool m_isUnderPartialClass = true;
		private string m_partialClassName = null;

		private string m_keyName = null;
		private bool m_hasStateData = false;
		private string m_stateDataClassName = null;

		private bool m_initializedGui = false;
		private GUIStyle m_texFieldErrorStyle = null;

		[MenuItem( "Xam/State | Factory Wizard" )]
		public static void ShowWindow()
		{
			StateAndFactoryPairCreationWizard window = CreateInstance<StateAndFactoryPairCreationWizard>();

			window.titleContent = new GUIContent( "State And Factory Wizard" );
			window.Show();
		}

		private void OnEnable()
		{
			m_namespace = EditorPrefs.GetString( k_namespaceSaveKey );
		}

		private void OnGUI()
		{
			InitGui();

			bool isStateKeyTypeNameValid = !string.IsNullOrEmpty( m_stateKeyTypeName );
			bool isStateClassNameValid = !string.IsNullOrEmpty( m_stateClassName );
			bool isKeyNameValid = !string.IsNullOrEmpty( m_keyName );

			EditorGUILayout.Space();

			m_namespace = EditorGUILayout.TextField( "Namespace:", m_namespace );

			EditorGUILayout.Space();


			m_stateClassName = EditorGUILayout.TextField( "State Name:", m_stateClassName, GetTextFieldStyle( isStateClassNameValid ) );
			m_stateParentClassName = EditorGUILayout.TextField( "State Inheritance Name:", m_stateParentClassName );
			m_stateKeyTypeName = EditorGUILayout.TextField( "State Key Type Name:", m_stateKeyTypeName, GetTextFieldStyle( isStateKeyTypeNameValid ) );

			EditorGUILayout.Space();

			using ( var stateDataGroup = new EditorGUILayout.ToggleGroupScope( "Has State Data?", m_hasStateData ) )
			{
				m_hasStateData = stateDataGroup.enabled;
				m_stateDataClassName = EditorGUILayout.TextField( "State Data Name:", m_stateDataClassName );
			}

			EditorGUILayout.Space();

			bool canCreateScripts = isStateKeyTypeNameValid
				&& isStateClassNameValid
				&& isKeyNameValid;

			if ( !canCreateScripts )
			{
				GUI.enabled = false;
			}

			if ( GUILayout.Button( "Create" ) )
			{
				CreateScripts();
				SaveSettings();
			}

			GUI.enabled = true;

			EditorGUILayout.Space();

			EditorGUILayout.LabelField( "State Class:", EditorStyles.miniBoldLabel );
			
			using ( var partialClassGroup = new EditorGUILayout.ToggleGroupScope( "Under Partial Class?", m_isUnderPartialClass ) )
			{
				m_isUnderPartialClass = partialClassGroup.enabled;
				m_partialClassName = EditorGUILayout.TextField( "Partial Class Name:", m_partialClassName );
			}

			using ( new EditorGUILayout.VerticalScope() )
			{
				string classContent = GetStateClassContent();
				EditorGUILayout.LabelField( classContent, EditorStyles.textArea );
			}

			EditorGUILayout.Space();

			EditorGUILayout.LabelField( "Factory Class:", EditorStyles.miniBoldLabel );
			m_keyName = EditorGUILayout.TextField( "Key Name:", m_keyName, GetTextFieldStyle( isKeyNameValid ) );

			using ( new EditorGUILayout.VerticalScope() )
			{
				string classContent = GetFactoryClassContent();
				EditorGUILayout.LabelField( classContent, EditorStyles.textArea );
			}
		}

		private void InitGui()
		{
			if ( m_initializedGui ) { return; }
			m_initializedGui = true;

			m_texFieldErrorStyle = new GUIStyle( EditorStyles.textField );
			m_texFieldErrorStyle.normal.background = XamEditorUtilities.CreateTex( 2, 2, new Color( 1, 0, 0, 0.2f ) );
			m_texFieldErrorStyle.focused.background = XamEditorUtilities.CreateTex( 2, 2, new Color( 1, 0, 0, 0.2f ) );
		}

		private GUIStyle GetTextFieldStyle( bool isFieldValid )
		{
			return isFieldValid ? EditorStyles.textField : m_texFieldErrorStyle;
		}

		private string GetStateClassContent()
		{
			string content = GetClassUsings();
			content += "\r\n\r\n";

			if ( !string.IsNullOrEmpty( m_namespace ) )
			{
				content += $"namespace {m_namespace}\r\n";
				content += "{\r\n";
				{
					string parentClassName = string.IsNullOrEmpty( m_stateParentClassName )
						? $"DoNothingState<{m_stateKeyTypeName}>"
						: m_stateParentClassName;

					if ( m_isUnderPartialClass )
					{
						content += $"\tpublic partial class {m_partialClassName}\r\n";
						content += "\t{\r\n";
						{
							if ( m_hasStateData )
							{
								string stateDataClassName = string.IsNullOrEmpty( m_stateDataClassName )
									? $"{m_stateClassName}StateData"
									: $"{m_stateDataClassName}StateData";

								content += "\t\t[System.Serializable]\r\n";
								content += $"\t\tpublic class {stateDataClassName}\r\n";
								content += "\t\t{\r\n";
								content += "\t\t}\r\n\r\n";
							}

							content += $"\t\tpublic class {m_stateClassName}State : {parentClassName}\r\n";
							content += "\t\t{\r\n";
							{
								if ( m_hasStateData )
								{
									string stateDataClassName = string.IsNullOrEmpty( m_stateDataClassName )
										? $"{m_stateClassName}StateData"
										: $"{m_stateDataClassName}StateData";

									content += $"\t\t\tprivate {stateDataClassName} m_stateData = null;\r\n\r\n";

									content += $"\t\t\tpublic {m_stateClassName}State( {stateDataClassName} stateData )\r\n";
									content += "\t\t\t{\r\n";
									{
										content += "\t\t\t\tm_stateData = stateData;\r\n";
									}
									content += "\t\t\t}\r\n";
								}
							}
							content += "\t\t}\r\n";
						}
						content += "\t}\r\n";
					}
					else
					{
						if ( m_hasStateData )
						{
							string stateDataClassName = string.IsNullOrEmpty( m_stateDataClassName )
								? $"{m_stateClassName}StateData"
								: $"{m_stateDataClassName}StateData";

							content += "\t[System.Serializable]\r\n";
							content += $"\tpublic class {stateDataClassName}\r\n";
							content += "\t{\r\n";
							content += "\t}\r\n\r\n";
						}

						content += $"\tpublic class {m_stateClassName}State : {parentClassName}\r\n";
						content += "\t{\r\n";
						content += "\t}\r\n";
					}
				}
				content += "}";
			}

			return content;
		}

		private string GetFactoryClassContent()
		{
			string partialClassSpaceName = m_isUnderPartialClass
				? $"{m_partialClassName}."
				: null;

			string content = GetClassUsings();
			content += "\r\n\r\n";

			if ( !string.IsNullOrEmpty( m_namespace ) )
			{
				content += $"namespace {m_namespace}.Factory\r\n";
				content += "{\r\n";
				{
					string combinedStateKeyTypeName = $"{partialClassSpaceName}{m_stateKeyTypeName}";

					content += $"\tpublic class {m_stateClassName}StateFactory : MonoBehaviour, IFsmStateFactory<{combinedStateKeyTypeName}>\r\n";
					content += "\t{\r\n";
					{
						if ( m_hasStateData )
						{
							string stateDataClassName = string.IsNullOrEmpty( m_stateDataClassName )
								? $"{m_stateClassName}StateData"
								: $"{m_stateDataClassName}StateData";

							string combinedStateDataClassName = $"{partialClassSpaceName}{stateDataClassName}";

							content += $"\t\t[SerializeField] private {combinedStateDataClassName} m_stateData = new {combinedStateDataClassName}();\r\n\r\n";
						}

						content += $"\t\tpublic IFsmState<{combinedStateKeyTypeName}> CreateState()\r\n";
						content += "\t\t{\r\n";
						{
							string newClassConstructor = m_hasStateData
								? $"{m_stateClassName}State( m_stateData )"
								: $"{m_stateClassName}State()";
							content += $"\t\t\treturn new {partialClassSpaceName}{newClassConstructor};\r\n";
						}
						content += "\t\t}\r\n";

						content += "\r\n";

						content += $"\t\tpublic {combinedStateKeyTypeName} GetKey()\r\n";
						content += "\t\t{\r\n";
						{
							content += $"\t\t\treturn {combinedStateKeyTypeName}.{m_keyName};\r\n";
						}
						content += "\t\t}\r\n";
					}
					content += "\t}\r\n";
				}
				content += "}";
			}

			return content;
		}

		private string GetClassUsings()
		{
			return "using System.Collections;\r\n" +
				   "using System.Collections.Generic;\r\n" +
				   "using UnityEngine;\r\n" +
				   "using Xam.Utility.Fsm;";
		}

		private void CreateScripts()
		{
			string savePath = null;
			string assetsDirectory = Application.dataPath;

			savePath = EditorUtility.SaveFilePanel( "Save State Script", assetsDirectory, $"{m_stateClassName}State", "cs" );

			if ( string.IsNullOrEmpty( savePath ) )
			{
				Debug.Log( $"<color=purple>State and factory creation has been canceled.</color>" );
				return;
			}

			using ( StreamWriter writer = File.CreateText( savePath ) )
			{
				writer.Write( GetStateClassContent() );
			}
			Debug.Log( $"<color=brown>Create State ({savePath})!</color>" );

			savePath = EditorUtility.SaveFilePanel( "Save Factory Script", Path.GetDirectoryName( savePath ), $"{m_stateClassName}StateFactory", "cs" );
			using ( StreamWriter writer = File.CreateText( savePath ) )
			{
				writer.Write( GetFactoryClassContent() );
			}
			Debug.Log( $"<color=maroon>Create Factory ({savePath})!</color>" );

			AssetDatabase.Refresh();
		}

		private void SaveSettings()
		{
			EditorPrefs.SetString( k_namespaceSaveKey, m_namespace );
		}
	}
}