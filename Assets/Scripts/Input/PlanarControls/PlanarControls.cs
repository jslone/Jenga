using UnityEngine;
using System.Collections;

public enum MouseButton : int
{
    Left = 0,
    Right = 1,
    Middle = 2,
}

public class PlanarControls : Singleton<PlanarControls>
{
    public enum Spaces : int
    {
        World,
        Local,
        Camera,
    };

    LayerMask[] spaceMasks;
    public Spaces CurrentSpace = Spaces.World;
    public LayerMask CurrentSpaceMask { get { return spaceMasks[(int)CurrentSpace]; } }

    // Called before start
    void Awake()
    {
        Init(this);
        spaceMasks = new LayerMask[]
        {
            LayerMask.GetMask("Axes_World"),
            LayerMask.GetMask("Axes_Local"),
            LayerMask.GetMask("Axes_Camera"),
        };
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // cycle spaces
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // wrap around increment space
            CurrentSpace = (Spaces)(((int)CurrentSpace + 1) % spaceMasks.Length);
        }
    }
}
