using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameSession : MonoBehaviour
{
    public void EnterGameBtn_OnClick()
    {
        SessionController.singleton.EnterGameBtn_OnClick();
    }
}
