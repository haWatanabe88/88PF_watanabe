using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromManualToTitle : MonoBehaviour
{
    [SerializeField] AudioSource audiosrc;

    private void Start()
    {
        audiosrc = GetComponent<AudioSource>();
    }

    public void Onclick()
    {
        StartCoroutine(LoadNextScene("Title"));
    }

    IEnumerator LoadNextScene(string str)
    {
        audiosrc.PlayOneShot(audiosrc.clip);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(str);
    }
}
