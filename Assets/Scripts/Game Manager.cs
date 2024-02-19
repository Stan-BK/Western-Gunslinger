using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Timeline;
    private TimelinePreferences timeline;

    public UIManager UIManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        Timeline.SetActive(true);
    }

    public void GameLoaded()
    {
        UIManager.ShowGamingUI();
    }
}
