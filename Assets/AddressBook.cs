using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddressBook : MonoBehaviour
{
 
    public TMP_InputField addressBox;
    public TMP_InputField portBox;

    // Start is called before the first frame update
    void Start()
    {

        addressBox.text = "127.0.0.1";
        //addressBox.ForceMeshUpdate();
        portBox.text ="7777";
        //portBox.ForceMeshUpdate();
    }
    private void OnGUI()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
