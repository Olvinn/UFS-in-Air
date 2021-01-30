using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool isBroken;
    // public Animator animator;
    public Sprite normalState;
    public Sprite brokenState;

    /// <summary> 
    /// Функция для починки объектов
    /// </summary>
    public void FixObject() {
        if(isBroken) {
            isBroken = false;
            Debug.Log("Объект починен");
            this.GetComponent<SpriteRenderer>().sprite = normalState;
            // animator.SetBool("IsBroken", isBroken);
        }
    }

    /// <summary> 
    /// Функция для поломки объектов
    /// </summary>
    public void BrokeObject() {
        if(!isBroken) {
            isBroken = true;
            Debug.Log("Объект сломан");
            this.GetComponent<SpriteRenderer>().sprite = brokenState;
        }
    }

    /// <summary> 
    /// Функция задающая именения, которые будут приносить объекты после их починки/поломки
    /// </summary>
    virtual public void SetChanges() { }

}
