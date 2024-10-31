using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inheritable interaction interface. Interactable objects will inherit from IInteractable and perform unique functions within Interact().
interface IInteractable
{
    public void Interact();
}

public class InteractManager : MonoBehaviour
{
    //This Transform references the object in scene from which the interactions will be performed. It is best filled with the camera for simplicity and accuracy.
    public Transform InteractSource;
    public float InteractRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractSource.position, InteractSource.forward);
            if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                //If object hit by raycast has interactable component, run the interact function on it.
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
