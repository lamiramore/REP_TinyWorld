using UnityEngine;

public class Interactable : MonoBehaviour
{

    public event System.Action onInteracted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
//virtual allows subclass to make variations/expansions of the function
    public virtual void Interact()
    {
        Debug.Log("Interacted with" + name);
            if(onInteracted != null)
                onInteracted.Invoke(); //event auslosen / start / invoke 
    }
}
