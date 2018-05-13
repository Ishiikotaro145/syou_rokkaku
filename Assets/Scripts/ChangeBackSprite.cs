using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackSprite : MonoBehaviour
{
    public List<GameObject> pBackSpritePrefab;
    public List<AudioClip> bgm;
    int selectStage = 0;

    // Use this for initialization
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        selectStage = PlayerPrefs.GetInt("StageSelect");
        SpriteRenderer sprite = null;
        if (selectStage >= 0 && selectStage < 3)
        {
            sprite = pBackSpritePrefab[0].GetComponent<SpriteRenderer>();
            audioSource.clip = bgm[0];
            audioSource.Play();
        }
        else if (selectStage >= 3 && selectStage < 6)
        {
            sprite = pBackSpritePrefab[1].GetComponent<SpriteRenderer>();
            audioSource.clip = bgm[1];
            audioSource.Play();
        }
        else if (selectStage >= 6 && selectStage < 9)
        {
            sprite = pBackSpritePrefab[2].GetComponent<SpriteRenderer>();
            audioSource.clip = bgm[2];
            audioSource.Play();
        }

        var color = sprite.color;
        color.a = 1.0f;
        sprite.color = color;
    }


    // Update is called once per frame
    void Update()
    {
    }
}