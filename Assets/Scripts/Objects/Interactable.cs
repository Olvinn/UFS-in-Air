using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
   
   public bool isInRange;
   public KeyCode interactKey;
   public UnityEvent interactAction;

    void Start()
    {
        
    }

    void Update()
    {
        if(isInRange) {
            if(Input.GetKeyDown(interactKey)) {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Human")) {
            isInRange = true;
            Debug.Log("Человек может заняться починкой");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Human")) {
            isInRange = false;
            Debug.Log("Человек далеко от объекта");
        }
    }
}
