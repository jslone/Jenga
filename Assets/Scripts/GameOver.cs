using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    public ScoreKeeper scoreKeeper;
    public Text countdown;

    Stopwatch timer = new Stopwatch(); // defined in ScoreKeeper
    const float DISCHARGE = 5.0f;


    HashSet<Rigidbody> onTable = new HashSet<Rigidbody>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.CurrentState == GameState.State.GameOver) {
            return;
        }

        timer.Update();
        if ((!timer.IsRunning) && onTable.Count > 4) {
            timer.Start();
        } else if (timer.IsRunning && onTable.Count < 4) {
            timer.Stop();
        }

        if (timer.IsRunning && (timer.getElapsedSeconds() > DISCHARGE))
        {
            // now ya done son
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in blocks)
            {
                float scoreF = (scoreKeeper.score / 1000f) * 40.0f;
                float force = 10.0f + scoreF;
                block.GetComponent<Rigidbody>().AddExplosionForce(force, CameraControls.Instance.Focus, force);
                block.GetComponent<Rigidbody>().AddExplosionForce(2 * force, Vector3.zero, force);
                this.enabled = false;
            }

            GameState.ChangeState(GameState.State.GameOver);
        }

        // countdown
        if (timer.IsRunning) {
            countdown.enabled = true;
            countdown.text = (((int)DISCHARGE) - (int)timer.getElapsedSeconds()).ToString();
        } else {
            countdown.enabled = false;
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            onTable.Add(col.rigidbody);

            if (GameState.CurrentState == GameState.State.MoveBlock && DragableBlock.held.Count == 0) {
                GameState.ChangeState(GameState.State.SelectBlock);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            onTable.Remove(col.rigidbody);
        }
    }
}
