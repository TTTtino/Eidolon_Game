using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject m_droppedItem;
    public Transform m_dropPosition;
    public int m_itemsToDrop;

    public void DropItem()
    {

        Vector3 dropPosition = transform.position;
        if (m_dropPosition != null) dropPosition = m_dropPosition.position;

        for (int i = 0; i < m_itemsToDrop; i++)
        {
            GameObject dropI = Instantiate(m_droppedItem, dropPosition, Quaternion.identity);
            Rigidbody2D rb = dropI.GetComponent<Rigidbody2D>();
            if (rb != null) rb.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
        }

    }
}
