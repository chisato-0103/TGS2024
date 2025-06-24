using UnityEngine;

public class God : MonoBehaviour
{
    public int result = 0;
    public float time = 1; 
    private float resetTime;
    public Sprite Default;
    public Sprite atari;
    public Sprite hazure;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer MainSpriteRenderer;
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //idou Idou = new idou();

        switch(result)
        {
            case 0:
                MainSpriteRenderer.sprite = Default;
                break;
            case 1:
                time -= Time.deltaTime;
                MainSpriteRenderer.sprite = atari;
                if(time <= 0.0f)
                {
                    result = 0;
                    time = resetTime;
                } 
                break;
            case 2:
                time -= Time.deltaTime;
                MainSpriteRenderer.sprite = hazure;
                if(time <= 0.0f)
                {
                    result = 0;
                    time = resetTime;
                } 
                break;
        }
    }
}
