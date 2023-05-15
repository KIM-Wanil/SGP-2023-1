using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MenuPanel;
    AudioSource audioSource;
    public AudioClip audioClick;
    public GameObject infoCanvas;
    void Start()
    {
        //this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Menu_button()
    {
        Time.timeScale = 0; //게임 일시정지
        MenuPanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        MenuPanel.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }
    public void LoadTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadFailStage()
    {
        SceneManager.LoadScene("Fail_Stage");
    }
    public void ClearStage()
    {
        SceneManager.LoadScene("Clear_Stage");
    }
    public void Info_button()
    {
        infoCanvas.SetActive(true);
    }
    public void Back_button()
    {
        infoCanvas.SetActive(false);
    }
    //public void click_sound()
    //{
    //    audioSource.clip = audioClick;
    //    audioSource.Play();
    //}
}
