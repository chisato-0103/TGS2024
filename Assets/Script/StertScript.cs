using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StertScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //　スタートボタンを押したら"GameScene"というシーンに切り替わる
    public void StartButton()
    {
        //Debug.Log("Stert");
        SceneManager.LoadScene("StoryTutrial");
    }
    //  pauseボタン or RETRYボタンが押されると"TitleScene"というシーンに切り替わる
    public void StopButton()
    {
        //Debug.Log("Stop");
        SceneManager.LoadScene("TitleScene");
    }
}
