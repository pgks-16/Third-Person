using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start(){
        Time.timeScale =1f;
    }
    public void Button_Start(){
        SceneManager.LoadScene("Lv1");
    }

    public void Button_Quit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
   
}
