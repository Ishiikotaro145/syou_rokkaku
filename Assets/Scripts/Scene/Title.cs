using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Title : MonoBehaviour
{
    private AudioSource _audioSource;
    public Text pTapStartText;
    float textAlpha = 1.0f;

    bool isAlphaUp = false;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlphaUp == false)
        {
            textAlpha -= 0.01f;
            if (textAlpha <= 0.5f)
            {
                isAlphaUp = true;
            }
        }
        else
        {
            textAlpha += 0.01f;
            if (textAlpha >= 1.0f)
            {
                isAlphaUp = false;
            }
        }

        pTapStartText.color = new Vector4(1.0f, 1.0f, 1.0f, textAlpha);


        ChackTap();
    }


    void ChackTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _audioSource.Play();
            StartCoroutine("LoadScene");
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(.5f);
        Application.LoadLevel("StageSelect");
    }
}