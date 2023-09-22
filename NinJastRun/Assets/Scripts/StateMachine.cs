using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    Dictionary<int, State> states;

    IEnumerator FiniteStateMachine()
    {
        yield return null;
    }

    void ChangeState()
    {

    }
}
