using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextbookItem : MonoBehaviour
{
    [SerializeField] TextbookController tbController;
    public int nr;
    
    private void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        tbController.ItemClicked(nr);
    }
}
