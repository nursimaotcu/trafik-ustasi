using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;

    void Start()
    {
        mainMenuPanel.SetActive(true);
    }

    public void OyunuBaslat()
    {
        mainMenuPanel.SetActive(false);
    }
}