using UnityEngine;

public abstract class StateMashine : MonoBehaviour
{
    protected State currentState;

    public void SetState(State state)
    {
        currentState = state;
        currentState.Start();
    }

    protected virtual void Update()
    {
        currentState?.Update();
    }
}