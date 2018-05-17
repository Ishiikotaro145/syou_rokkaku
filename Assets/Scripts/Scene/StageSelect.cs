using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public GameObject Panel;
    private Image _image;

    // Use this for initialization
    void Start()
    {
        _image = Panel.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ButtonTap(int _Stage)
    {
       

        //フェード処理 
        PlayerPrefs.SetInt("StageSelect", _Stage);


        StartCoroutine("LoadStage");
    }

    IEnumerator LoadStage()
    {
        for (int i = 0; i < 10; i++)
        {
            _image.color = new Color(1, 1, 1, _image.color.a + .1f);
            yield return new WaitForSeconds(.05f);
        }

        //フェードが完全に終わったら次のシーンへ
        Application.LoadLevel("Main");
    }
}