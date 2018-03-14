using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private Character type;
    private bool typeIsSelected;

    public Text txtMatchName;

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
   
    }

    public void JoinGame()
    {
      
    }
    

}

