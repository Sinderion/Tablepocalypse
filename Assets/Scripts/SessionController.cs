using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.SceneManagement;
using TMPro;


public class SessionController: MonoBehaviour
{
    
    [HideInInspector] public static SessionController singleton = null;
    [HideInInspector] public UNetTransport connectionInfo = null;

    public TMP_InputField remoteAddressTMPInputField;
    public TMP_InputField remotePortTMPInputField;

    public GameObject loginUI;
    public GameObject HUDUI;
    public TMP_Text positionText;

    private void Start()
    {
        HUDUI.SetActive(false);
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
        connectionInfo = NetworkManager.Singleton.GetComponent<UNetTransport>();
        if (!connectionInfo)
        {
            Debug.Log("Connection Info empty!");
        }
   
    }



    public void ConnectBtn_OnClick()
    {
        int newPort = 0;

        if (!int.TryParse(remotePortTMPInputField.text, out newPort))
        {
            Debug.Log("Invalid port error.");
            return;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += ConnectionSuccessful;
        connectionInfo.ConnectAddress = remoteAddressTMPInputField.text;
        remoteAddressTMPInputField.enabled = false;
        remotePortTMPInputField.enabled = false;
        


        NetworkManager.Singleton.StartClient();
 
    }

    public void ConnectionFail()
    {

    }

    public void ConnectionSuccessful(ulong clientID)
    {

        loginUI.SetActive(false);
        HUDUI.SetActive(true);
        //NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        //SceneManager.LoadScene("CharacterSelection");
        
    }

    public void EnterGameBtn_OnClick()
    {
        loginUI.SetActive(false);
        HUDUI.SetActive(true);
        //NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void HostBtn_OnClick()
    {
        int newPort = 0;

        if (!int.TryParse(remotePortTMPInputField.text, out newPort))
        {
            Debug.Log("Invalid port error:" + newPort);
            return;
        }
        connectionInfo.ConnectPort = newPort;
        connectionInfo.ServerListenPort = newPort;

        //NetworkManager.ConnectionApprovedDelegate = 

        NetworkManager.Singleton.StartHost();
        loginUI.SetActive(false);
        HUDUI.SetActive(true);
        //NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);

    }

}
