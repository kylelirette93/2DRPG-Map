using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void MapGeneratorButton()
    {
        SceneManager.LoadScene("MapGenerator");
    }

    public void MapPainterButton()
    {
        SceneManager.LoadScene("MapPainter");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
