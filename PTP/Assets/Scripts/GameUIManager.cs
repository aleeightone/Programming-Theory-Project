using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(1000)]

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    
    public int healthValueUI;
    public int scoreValueUI;
    public GameObject scoreDisplayBox;
    public TMP_Text scoreText;
    public GameObject canvas;
    public GameObject healthBar;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        canvas = gameObject;
        healthBar = canvas.transform.Find("Health Bar").gameObject;
        heart1 = healthBar.transform.Find("Health1").gameObject;
        heart2 = healthBar.transform.Find("Health2").gameObject;
        heart3 = healthBar.transform.Find("Health3").gameObject;
        scoreDisplayBox = canvas.transform.Find("ScoreUI").gameObject;
        scoreText = scoreDisplayBox.GetComponent<TMP_Text>();
    }

    public void SetScoreUI(int score)
    {
        
        
        scoreText.text = score.ToString("D7");
    }

    public void setHealthUI(int hp)
    {
        if (hp == 3) { heart1.SetActive(true); heart2.SetActive(true); heart3.SetActive(true); }
        else if (hp == 2) { heart1.SetActive(true); heart2.SetActive(true); heart3.SetActive(false); }
        else if (hp == 1) { heart1.SetActive(true); heart2.SetActive(false); heart3.SetActive(false); }
        else if (hp == 0) { heart1.SetActive(false); heart2.SetActive(false); heart3.SetActive(false); }
        else Debug.Log("HP out of bounds!");
    }

}
