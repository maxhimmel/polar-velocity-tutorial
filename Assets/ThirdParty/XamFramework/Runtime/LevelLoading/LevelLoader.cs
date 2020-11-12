using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xam.Level
{
	public class LevelLoader : Utility.Patterns.SingletonMono<LevelLoader>
	{
		public void LoadScene( string sceneName, LoadSceneMode mode = LoadSceneMode.Single )
		{
			SceneManager.LoadScene( sceneName, mode );
		}

		public void LoadSceneAsync( string sceneName, LoadSceneMode mode = LoadSceneMode.Single, System.Action callback = null )
		{
			AsyncOperation loadOperation = SceneManager.LoadSceneAsync( sceneName, mode );

			StartCoroutine( WaitForAsync_Coroutine( loadOperation, callback ) );
		}

		private IEnumerator WaitForAsync_Coroutine( AsyncOperation operation, System.Action callback )
		{
			yield return operation;

			callback?.Invoke();
		}
	}
}