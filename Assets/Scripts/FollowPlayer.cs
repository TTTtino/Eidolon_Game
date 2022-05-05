using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// moves towards player at followSpeed
public class FollowPlayer : MonoBehaviour
{
    private GameObject m_player;
    [SerializeField] private float followSpeed;
    [SerializeField] private bool followX, followY;
    // Start is called before the first frame update
    void Start()
    {
        m_player = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        if (followX)
        {
            newPos.x = m_player.transform.position.x;
        }
        if (followY)
        {
            newPos.y = m_player.transform.position.y;
        }
        transform.position = Vector3.MoveTowards(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
