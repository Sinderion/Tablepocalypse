using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.SceneManagement;


public class SessionController: MonoBehaviour
{
    public static SessionController singleton;

    private void Awake()
    {
        if(SessionController.singleton)
        {
            Destroy(this);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("singleton created!");
        }
        
   
    }


    public void ConnectBtn_OnClick()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void EnterGameBtn_OnClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
