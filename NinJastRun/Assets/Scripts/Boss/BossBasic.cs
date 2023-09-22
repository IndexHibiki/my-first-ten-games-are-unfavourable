using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBasic : MonoBehaviour
{
    protected Coroutine currentCorourine;

    [Header("State")]
    [SerializeField] protected StateData[] statesData;

    protected EnemyStateType currentStateType;

    protected virtual void HandleCurrentState()
    {
        if (currentCorourine != null)
        {
            StopCoroutine(currentCorourine);
            currentCorourine = null;
        }

        switch (currentStateType)
        {
            case EnemyStateType.Idle:
                // Handle idle logic
                currentCorourine = StartCoroutine(Idle());
                break;

            case EnemyStateType.Move:
                // Handle move logic
                currentCorourine = StartCoroutine(Move());
                break;

            case EnemyStateType.Attack:
                // Handle attack logic
                currentCorourine = StartCoroutine(Attack());
                break;

            case EnemyStateType.Skill:
                // Handle jump logic
                currentCorourine = StartCoroutine(Skill());
                break;
        }
    }

    protected virtual void SwitchToNextState() { }

    protected virtual void ChangeState(EnemyStateType newState)
    {
        currentStateType = newState;
    }

    protected virtual float GetStateDuration(EnemyStateType state)
    {
        foreach (var data in statesData)
        {
            if (data.stateType == state)
            {
                return data.duration;
            }
        }
        return 0f;
    }

    protected virtual IEnumerator Appear()
    {
        yield return null;
    }

    public virtual IEnumerator Leave()
    {
        yield return null;
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator Skill()
    {
        yield return null;
    }

    public virtual  IEnumerator Die()
    {
        yield return null;
    }
}
public enum EnemyStateType
{
    Idle,
    Move,
    Attack,
    Skill
}

[System.Serializable]
public class StateData
{
    public EnemyStateType stateType;
    public float duration;
}