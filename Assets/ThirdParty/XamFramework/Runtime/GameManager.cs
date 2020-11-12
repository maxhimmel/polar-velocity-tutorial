using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xam
{
	public class GameManager : Utility.Patterns.SingletonMono<GameManager>
	{
		[SerializeField] private string m_mainMenuSceneName = "MainMenu";
		[SerializeField] private float m_levelLoadFadeTransition = 1;

		private Coroutine m_loadingLevelRoutine = null;

		public void LoadMainMenu()
		{
			m_loadingLevelRoutine = StartCoroutine( LoadLevel_Coroutine( m_mainMenuSceneName ) );
		}

		public void ReloadActiveLevel()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string activeScenePath = activeScene.path;

			if ( string.IsNullOrEmpty( activeScenePath ) )
			{
				// Show the end menu!
				// ...

				Debug.LogError( $"Fatal error! How did the scene become in-active?", this );
				return;
			}

			m_loadingLevelRoutine = StartCoroutine( LoadLevel_Coroutine( activeScenePath ) );
		}

		/// <summary>
		/// Loads the scene at the active scene's buildIndex + 1.
		/// </summary>
		public void LoadNextLevel()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string nextScenePath = SceneUtility.GetScenePathByBuildIndex( activeScene.buildIndex + 1 );

			if ( string.IsNullOrEmpty( nextScenePath ) )
			{
				Debug.Log( $"<color=purple>Back to main menu! You beat the game!</color>", this );
				nextScenePath = m_mainMenuSceneName;
			}

			m_loadingLevelRoutine = StartCoroutine( LoadLevel_Coroutine( nextScenePath ) );
		}

		private IEnumerator LoadLevel_Coroutine( string levelName )
		{
			// Begin fading out ...
			Initialization.TransitionController.Instance.Close( m_levelLoadFadeTransition );
			if ( m_levelLoadFadeTransition > 0 )
			{
				float timer = 0;
				while ( timer < 1 )
				{
					timer += Time.unscaledDeltaTime / m_levelLoadFadeTransition;
					yield return null;
				}
			}

			// Finished fade-out!
			// ...

			// Proceed to next level!
			Level.LevelLoader.Instance.LoadScene( levelName );
		}
	}
}