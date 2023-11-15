using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI contextText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNameText(string text)
    {
        nameText.text = text;
    }

    public void setContextText(string text)
    {
        contextText.text = text;
    }
}
