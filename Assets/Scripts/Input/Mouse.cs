using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Mouse : Singleton<Mouse>
{
    RectTransform trans;

    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        trans = GetComponent<RectTransform>();
        trans.position = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        trans.localPosition += delta;
    }

    public Vector2 Position { 
        get { return trans.position; }
        set { trans.position = value; }
    }
}
