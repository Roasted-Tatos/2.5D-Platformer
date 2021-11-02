using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioSource _backgroundMusic;
    [SerializeField]
    private AudioClip _buttonSound;

    public float fadeTime = 1;
    public void FadeSound()
    {
        if (fadeTime == 0)
        {
            _backgroundMusic.volume = 0;
            return;
        }
        StartCoroutine(SoundFade());
    }

    IEnumerator SoundFade()
    {
        float t = fadeTime;
        while (t >0)
        {
            yield return null;
            t -= Time.deltaTime;
            _backgroundMusic.volume = t / fadeTime;
        }
        yield break;
    }
    public void PlayLevel()
    {
        SceneManager.LoadScene(1);
        AudioSource.PlayClipAtPoint(_buttonSound, Camera.main.transform.position);
    }
    public void PlayCredits()
    {
        SceneManager.LoadScene(3);
        AudioSource.PlayClipAtPoint(_buttonSound, Camera.main.transform.position);
    }
    public void MainMenuReturn()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(0);
        float loadProgress = loadingOperation.progress;

        if (loadingOperation.isDone)
        {
            Debug.Log("Loading is Finished");
        }
        AudioSource.PlayClipAtPoint(_buttonSound, Camera.main.transform.position);
    }
}
