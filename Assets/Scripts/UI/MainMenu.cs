using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject creditsMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Play()
    {
        SceneManager.LoadScene(1);
    }


    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        
    }

    public void Back()
    {
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
