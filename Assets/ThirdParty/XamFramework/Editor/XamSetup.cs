using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Xam.Editor
{
	public class XamSetup : EditorWindow
	{
		[MenuItem( "Xam/Setup/Create GameManager Prefab" )]
		private static void CreateGameManagerPrefab()
		{
			string savePath = GetNewFileSavePath<GameManager>();
			if ( string.IsNullOrEmpty( savePath ) ) { return; }

			string prefabName = System.IO.Path.GetFileNameWithoutExtension( savePath );

			GameManager gameManager = XamFactory.CreateGameManager( prefabName );
			GameObject prefabObject = gameManager.gameObject;

			SaveAndCleanup( ref prefabObject, savePath );
		}

		[MenuItem( "Xam/Setup/Create LevelInitializer Prefab" )]
		private static void CreateLevelInitializePrefab()
		{
			string savePath = GetNewFileSavePath<Initialization.LevelInitializer>();
			if ( string.IsNullOrEmpty( savePath ) ) { return; }

			string prefabName = System.IO.Path.GetFileNameWithoutExtension( savePath );

			Initialization.LevelInitializer levelInitializer = XamFactory.CreateLevelInitializer( prefabName );
			GameObject prefabObject = levelInitializer.gameObject;

			SaveAndCleanup( ref prefabObject, savePath );
		}

		private static string GetNewFileSavePath<T>() where T : Component
		{
			string typeName = typeof( T ).Name;
			return EditorUtility.SaveFilePanelInProject( $"Save {typeName} Prefab", typeName, "prefab", $"Create a new {typeName} prefab." );
		}

		private static bool SaveAndCleanup( ref GameObject prefab, string savePath )
		{
			GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset( prefab, savePath, out bool success );
			Selection.activeGameObject = newPrefab;

			DestroyImmediate( prefab );

			return success;
		}
	}
}