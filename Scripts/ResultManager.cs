using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
