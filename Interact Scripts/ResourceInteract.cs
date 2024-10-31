using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInteract : MonoBehaviour, IInteractable
{
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    //This class should control what happens when a resource is interacted with, in general.
    public void Interact()
    {
        Debug.Log("Resource Interact");

        //Triggers the mesh deform for interacting with resources, in this resource's animator component.
        anim.SetTrigger("SquashStretch");
    }
}
