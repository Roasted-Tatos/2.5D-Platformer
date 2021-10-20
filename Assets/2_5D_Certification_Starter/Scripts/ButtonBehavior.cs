using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioSource _backgroundMusic;

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
    }
}
