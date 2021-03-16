using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
	public GameObject LoadingScreen;
	public Slider slider;
	public Text text;

	public void ButtonStart(int sceneIndex)
	{
		StartCoroutine(LoadScene(sceneIndex));
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	public IEnumerator LoadScene (int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		LoadingScreen.SetActive(true);


		while (!operation.isDone)
		{
			float Progress = Mathf.Clamp01(operation.progress / .9f);

			slider.value = Progress;
			text.text = Progress * 100f + "%";

			yield return null;
		}
	}
}
