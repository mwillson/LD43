using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CasualGameMenu : MonoBehaviour
{

    public Transform selectedGameType;

    public CustomGameParams customParams;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight(Text text)
    {
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        foreach(Text t in text.GetComponentsInChildren<Text>()){
            t.color = new Color(t.color.r, t.color.g, t.color.b, 1);

        }
    }

    public void DeHighlight(Text text)
    {

        text.color = new Color(text.color.r, text.color.g, text.color.b, .5f);
        foreach (Text t in text.GetComponentsInChildren<Text>()){
            t.color = new Color(t.color.r, t.color.g, t.color.b, .5f);
        }
    }

    public void SetSelectedType(Transform tf)
    {
        selectedGameType = tf;
    }

    public void StartGame()
    {
        if (selectedGameType == null) return;

        string gameType = selectedGameType.name;
        if(gameType == "Standard")
        {
            customParams.active = false;
        }
        else if (gameType == "CustomMode")
        {
            customParams.active = true;
        }
        SceneManager.LoadScene("casual");

    }
}
