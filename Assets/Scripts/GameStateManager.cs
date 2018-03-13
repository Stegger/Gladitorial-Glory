using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private Character type;
    private bool typeIsSelected;

    private void Start()
    {
        typeIsSelected = false;
    }

    public void SetCharacterTypeSelection(Character type)
    {
        this.type = type;
        typeIsSelected = true;
    }

    public void StartGame()
    {
        NetworkManager nm = GetComponent<NetworkManager>();
        nm.networkAddress = "127.0.0.1";
        nm.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager nm = GetComponent<NetworkManager>();
        nm.networkAddress = "127.0.0.1";
        nm.StartClient();
    }
    

}

