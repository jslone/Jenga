using UnityEngine;
using System.Collections;

public class AlignBlock : MonoBehaviour
{
    DragableBlock drag;
    LayerMask blocks;
    Quaternion rotBy90;
    Quaternion rotByNeg90;
    int touchingMe = 0;

    // Use this for initialization
    void Start()
    {
        drag = GetComponent<DragableBlock>();
        blocks = LayerMask.GetMask("Blocks");
        rotBy90 = Quaternion.Euler(0, 90, 0);
        rotByNeg90 = Quaternion.Euler(0, -90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (drag.heldBy != null && touchingMe == 0)
        {
            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.down, out info, float.PositiveInfinity, blocks))
            {
                DragableBlock block = info.collider.GetComponent<DragableBlock>();
                if (block != null)
                {
                    Quaternion rot1 = block.transform.rotation * rotBy90;
                    Quaternion rot2 = block.transform.rotation * rotByNeg90;
                    if(Mathf.Abs(Quaternion.Dot(transform.rotation, rot1)) > Mathf.Abs(Quaternion.Dot(transform.rotation, rot2)))
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot1, Time.deltaTime);
                    } else
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot2, Time.deltaTime);
                    }
                    
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        touchingMe++;
    }

    void OnCollisionExit(Collision col)
    {
        touchingMe--;
    }
}
