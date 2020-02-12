using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static MainScript instance { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<GameObject> buttonsList = new List<GameObject>();
    private List<GameObject> buttonOrder = new List<GameObject>();
    private GameObject finalScreen;
    private int rounds = 1;

    private int round
    {
        get { return rounds; }
        set
        {
            rounds = value;
            currentRounds.text = "Round: " + rounds;
        }
    }
    
    private int counter = 0;

    public Sprite[] buttonImages;
    public Sprite[] newButtonImages;
    public Text currentRounds;
    public GameObject Failed;
    public GameObject button;
    public Transform mainUI;

    System.Random rnd = new System.Random();

    void Start()
    {
        SpawnButtons(new Vector3(0, 180, 0), new Vector2(260, 100));
        SpawnButtons(new Vector3(-180, 0, 0), new Vector2(100, 260));
        SpawnButtons(new Vector3(0, -180, 0), new Vector2(260, 100));
        SpawnButtons(new Vector3(180, 0, 0), new Vector2(100, 260));
        SpawnButtons(new Vector3(0, 0, 0), new Vector2(225, 225));
        FinalScreen();

        for (int i = 0; i < buttonsList.Count; i++)
        {
            buttonsList[i].GetComponent<ButtonScript>().ApplySprite(buttonImages[i]);
        }

        AddObject();
    }

    private void DisableButtons()
    {
        for (int i = 0; i < buttonsList.Count; i++)
        {
            buttonsList[i].GetComponent<ButtonScript>().Disable();
        }
    }

    private void EnableButtons()
    {
        for (int i = 0; i < buttonsList.Count; i++)
        {
            buttonsList[i].GetComponent<ButtonScript>().Enable();
        }
    }
    
    void AddObject()
    {
        DisableButtons();
        GameObject rndBtn = buttonsList[rnd.Next(buttonsList.Count)];
        buttonOrder.Add(rndBtn);
        ShowOrder();
    }

    public void CheckObject(GameObject obj)
    {
        if (counter == (buttonOrder.Count -1) && obj == buttonOrder[counter]) { counter = 0; round++; AddObject();}
        else if (buttonOrder[counter] && obj == buttonOrder[counter]) counter++;
        else Restart();
    }

    private async void Restart()
    {
        counter = 0;
        round = 1;
        buttonOrder.Clear();
        DisableButtons();
        finalScreen.SetActive(true);
        await Task.Delay(5000);
        AddObject();
        finalScreen.SetActive(false);
    }

    private async void ShowOrder()
    {
        for (int i = 0; i < buttonOrder.Count; i++)
        {
            await Task.Delay(500);
            buttonOrder[i].GetComponent<ButtonScript>().ApplySprite(newButtonImages[buttonsList.IndexOf(buttonOrder[i])]);
            await Task.Delay(1000);
            buttonOrder[i].GetComponent<ButtonScript>().ApplySprite(buttonImages[buttonsList.IndexOf(buttonOrder[i])]);
        }
        EnableButtons();
    }

    private void SpawnButtons(Vector3 pos, Vector2 newSize)
    {
        GameObject _button = Instantiate(button, pos, Quaternion.identity);
        _button.GetComponent<RectTransform>().sizeDelta = newSize;
        buttonsList.Add(_button);
        _button.transform.SetParent(mainUI, false);
    }

    private void FinalScreen()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        finalScreen = Instantiate(Failed, pos, Quaternion.identity);
        finalScreen.SetActive(false);
        finalScreen.transform.SetParent(mainUI, false);      
    }
}