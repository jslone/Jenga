using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum State {
        SelectBlock,  // testing blocks, or choosing a block to remove
        MoveBlock,  // currently grabbing and pulling a block
        GameOver,     // all is lost
    };

    private State currentState = State.SelectBlock;

    private static GameState gameState;
    public static State CurrentState {
        get {
            return gameState.currentState;
        }
    }

    // Handle state change logic
    public static void ChangeState(State newState) {
        Debug.Log("Changing state: " + newState.ToString());
        gameState.currentState = newState;
    }

    // Use this for initialization
    void Start() {
        gameState = this;
    }

    // Update is called once per frame
    void Update () {
    }

}
