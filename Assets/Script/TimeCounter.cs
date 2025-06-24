using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    //参考にしたサイト
    //https://zenn.dev/fujimiya/articles/5775dd0824031d
    public AudioClip nokoriTime;
    public AudioClip tenSeconds;
    public AudioClip nineSeconds;
    public AudioClip eightSeconds;
    public AudioClip sevenSeconds;
    public AudioClip sixSeconds;
    public AudioClip fiveSeconds;
    public AudioClip fourSeconds;
    public AudioClip threeSeconds;
    public AudioClip twoSeconds;
    public AudioClip oneSeconds;
    bool ten = false;
    bool nine = false;
    bool eight = false;
    bool seven = false;
    bool six = false;
    bool five = false;
    bool four = false;
    bool three = false;
    bool two = false;
    bool one = false;
    public int countdownMinutes = 3;
    private float countdownSeconds;
    private bool isCalledOnce = false;
    private Text timeText;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  // AudioSourceのコンポーネントを取得する
        timeText = GetComponent<Text>();
        countdownSeconds = countdownMinutes * 60;
    }


    void Update()
    {
        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = span.ToString(@"mm\:ss");
        if((int)countdownSeconds == 30)
        {
            if (!isCalledOnce) 
            {
                isCalledOnce = true;
                audioSource.PlayOneShot(nokoriTime);
            }
        }
        else if((int)countdownSeconds <= 10)
        {
            timeText.color = Color.red;
        }
        //else if((int)countdownSeconds == 10)
        if((int)countdownSeconds == 10 && !ten)
        {
            ten = true;
            audioSource.PlayOneShot(tenSeconds);
        }   
        else if((int)countdownSeconds == 9 && !nine)
        {
            nine = true;
            audioSource.PlayOneShot(nineSeconds);
        }   
        else if((int)countdownSeconds == 8 && !eight)
        {
            eight = true;
            audioSource.PlayOneShot(eightSeconds);
        }      
        else if((int)countdownSeconds == 7 && !seven)
        {
            seven = true;
            audioSource.PlayOneShot(sevenSeconds);
        }         
        else if((int)countdownSeconds == 6 && !six  )
        {
            six = true;
            audioSource.PlayOneShot(sixSeconds);
        }   
        else if((int)countdownSeconds == 5 && !five)
        {
            five = true;
            audioSource.PlayOneShot(fiveSeconds);
        }   
        else if((int)countdownSeconds == 4 && !four)
        {
            four = true;
            audioSource.PlayOneShot(fourSeconds);
        }      
        else if((int)countdownSeconds == 3 && !three)
        {
            three = true;
            audioSource.PlayOneShot(threeSeconds);
        }        
        else if((int)countdownSeconds == 2 && !two)
        {
            two = true;
            audioSource.PlayOneShot(twoSeconds);
        }   
        else if((int)countdownSeconds == 1 && !one)
        {
            one = true;
            audioSource.PlayOneShot(oneSeconds);
        }
        if (countdownSeconds <= 0)
        {
            // 0秒以下になったときの処理
            countdownSeconds = 0; // 負の値にならないように0で固定
            SceneManager.LoadScene("gameover");
        }
    }
}