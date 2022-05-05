using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactor that can be used to activate nearby interactable
public class Key : MonoBehaviour, IInteractor
{
    [SerializeField] GameObject m_opens;
    public void Use()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 2.0f);
        foreach (Collider2D hitCol in nearbyColliders)
        {
            Debug.Log(hitCol.name);
            if (hitCol.gameObject == m_opens)
            {
                IInteractable i = hitCol.GetComponent<IInteractable>();
                Debug.Log(i);
                if (i != null)
                {
                    i.Interact(this.gameObject);

                    Destroy(gameObject);
                }
            }

        }


    }
}
