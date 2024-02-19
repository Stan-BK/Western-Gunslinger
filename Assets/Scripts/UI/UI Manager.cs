using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Gaming;
    public GameObject Menu;

    public void ShowGamingUI()
    {
        Menu.SetActive(false);
        Gaming.SetActive(true);
    }

    public void ShowMenuUI()
    {
        Menu.SetActive(true);
        Gaming.SetActive(false);
    }
}
