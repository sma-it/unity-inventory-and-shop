using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    Button button;
    [SerializeField] Text text;
    [SerializeField] bool active;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Focus()
    {
        button = GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;
        text.color = Color.black;
    }

    public void UnFocus()
    {
        button = GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = Color.grey;
        button.colors = colors;
        text.color = Color.white;
    }
}
