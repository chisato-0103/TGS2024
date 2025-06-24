using UnityEngine;
using UnityEngine.UI;  // Imageを使用するために必要
using UnityEngine.SceneManagement;


public class StoryScript : MonoBehaviour
{
    Image image;
    public Sprite[] Story;
    private int i = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = Story[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("CountdownScene");
        }
        // 右矢印キーで次のスプライトに進む
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StoryChange();
        }

        // 左矢印キーで前のスプライトに戻る
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (i > 1)  // i=0の時は戻れないようにする
            {
                i -= 2;  // StoryChange()内でi++されるため2つ戻す
                StoryChange();
            }
        }
        
    }

    public void StoryChange()
    {
        if(i >= Story.Length || Story[i] == null)
        {
            SceneManager.LoadScene("CountdownScene");
        }
        else
        {
            image.sprite = Story[i];
            i++;
        }
    }
}