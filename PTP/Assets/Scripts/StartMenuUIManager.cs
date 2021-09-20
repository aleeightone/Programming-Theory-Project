using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour
{

    public void StartGame()
    {
        DataController.Instance.SetPlayerHealth(3);
        DataController.Instance.SetScore(0);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
