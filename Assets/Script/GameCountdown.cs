using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameCountdown : MonoBehaviour
{
    //private bool isCalledOnce_5 = false;
    private bool isCalledOnce_4 = false;
    private bool isCalledOnce_3 = false;
    private bool isCalledOnce_2 = false;
    private bool isCalledOnce_1 = false;

    public float time;
    public Sprite yo_i;
    public Sprite san;
    public Sprite ni;
    public Sprite iti;
    public AudioClip Ad_yo_i;
    public AudioClip count_3;
    public AudioClip count_2;
    public AudioClip count_1;

    AudioSource audioSource;
    SpriteRenderer MainSpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Application.targetFrameRate = 60;
        time = 5;
        audioSource = GetComponent<AudioSource>();  // AudioSourceのコンポーネントを取得する
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if((int)time == 4.0f)
        {
            if (!isCalledOnce_4) 
            {
                MainSpriteRenderer.sprite = yo_i;
                isCalledOnce_4 = true;
                audioSource.PlayOneShot(Ad_yo_i);
            }
        }
        else if((int)time == 3.0f)
        {
            if (!isCalledOnce_3) 
            {
                MainSpriteRenderer.sprite = san;
                isCalledOnce_3 = true;
                audioSource.PlayOneShot(count_3);
            }
        }
        else if((int)time == 2.0f)
        {
            if (!isCalledOnce_2) 
            {
                MainSpriteRenderer.sprite = ni;
                isCalledOnce_2 = true;
                audioSource.PlayOneShot(count_2);
            }
        }
        else if((int)time == 1.0f)
        {
            if (!isCalledOnce_1) 
            {
                MainSpriteRenderer.sprite = iti;
                isCalledOnce_1 = true;
                audioSource.PlayOneShot(count_1);
            }
        }
        else if((int)time == 0.0f)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
