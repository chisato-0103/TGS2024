using UnityEngine;
using UnityEngine.UI;  // Imageを使用するために必要
using UnityEngine.SceneManagement;

public class TutrialScript : MonoBehaviour
{
    Image image;
    public Sprite[] Tutrial;
    private int i = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutrialChange()
    {
        
        if(Tutrial[i] == null)
        {
            SceneManager.LoadScene("CountdownScene");
        }
        else
        {
            image.sprite = Tutrial[i];
            i++;
        }
        
    }
}
