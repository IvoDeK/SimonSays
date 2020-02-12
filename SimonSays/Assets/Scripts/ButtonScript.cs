using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    MainScript ms = MainScript.instance;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(btn);
    }

    public void ApplySprite(Sprite sprite)
    {
        gameObject.GetComponent<Button>().GetComponent<Image>().sprite = sprite;
    }

    public void Disable()
    {
        gameObject.GetComponent<Button>().enabled = false;
    }

    public void Enable()
    {
        gameObject.GetComponent<Button>().enabled = true;
    }

    public void btn()
    {
        ms.CheckObject(gameObject);
    }
}