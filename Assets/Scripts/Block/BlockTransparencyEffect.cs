using UnityEngine;
using System.Collections;

public class BlockTransparencyEffect : MonoBehaviour
{

    public PlanarControls grabber;
    public Shader grabbedShader;


    // currently held piece
    // used by blocks to determine fade effect
    Rigidbody heldPiece = null;
    private Shader cachedShader = null;

    // Update is called once per frame
    void Update()
    {
        // check for grabbed piece
        if (heldPiece == null && grabber.heldPiece != null)
        {
            heldPiece = grabber.heldPiece;
            Fade();
        }
        // check for dropped piece
        if (heldPiece != null && grabber.heldPiece == null)
        {
            Clear();
            heldPiece = null;
        }
    }

    void Fade()
    {
        Renderer curRenderer = heldPiece.GetComponentInChildren<Renderer>();

        for (int i = 0; i < curRenderer.materials.Length; i++)
        {
            cachedShader = curRenderer.materials[i].shader;
            curRenderer.materials[i].shader = grabbedShader;
        }
    }

    // Reset piece shader
    void Clear()
    {
        Renderer curRenderer = heldPiece.GetComponentInChildren<Renderer>();
        for (int i = 0; i < curRenderer.materials.Length; i++)
        {
            curRenderer.materials[i].shader = cachedShader;
        }

    }



}
