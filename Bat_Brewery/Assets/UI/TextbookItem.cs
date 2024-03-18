using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextbookItem : MonoBehaviour
{
    private TextbookController tbController;
    public int nr;
    
    private void Start() {
        tbController = GetComponentInParent<TextbookController>(); 
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        tbController.ItemClicked(nr);
    }
}
