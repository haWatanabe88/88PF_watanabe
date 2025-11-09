using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleObjManager : MonoBehaviour
{
    [SerializeField] GameObject Hero_l;
    [SerializeField] GameObject Hero_r;
    [SerializeField] GameObject pressEnter;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject manualBtn;
    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip titleMoveSE;
    [SerializeField] AudioClip titledicisionSE;

    bool isLeft;
    bool isRight;

    void Start()
    {
        Hero_r.SetActive(false);
        audioSrc = GetComponent<AudioSource>();
        isLeft = true;
        isRight = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSrc.PlayOneShot(titleMoveSE, 0.7f);
            Hero_l.SetActive(true);
            Hero_r.SetActive(false);
            isLeft = true;
            isRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSrc.PlayOneShot(titleMoveSE, 0.7f);
            Hero_r.SetActive(true);
            Hero_l.SetActive(false);
            isLeft = false;
            isRight = true;
        }

        if(isLeft)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSrc.PlayOneShot(titledicisionSE);
                StartCoroutine(LoadNextScene("main"));
            }
        }
        else if(isRight)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                audioSrc.PlayOneShot(titledicisionSE);
                StartCoroutine(LoadNextScene("manual"));
            }
        }
    }

    IEnumerator LoadNextScene(string str)
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(str);
    }
}
