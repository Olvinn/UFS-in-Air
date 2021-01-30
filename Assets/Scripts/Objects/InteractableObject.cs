using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool isBroken;
    // public Animator animator;

    /// <summary> 
    /// Функция для починки объектов
    /// </summary>
    public void FixObject() {
        if(isBroken) {
            isBroken = false;
            Debug.Log("Объект починен");
            // animator.SetBool("IsBroken", isBroken);
        }
    }

    /// <summary> 
    /// Функция задающая именения, которые будут приносить объекты после их починки
    /// </summary>
    virtual public void SetChanges() { }

}
