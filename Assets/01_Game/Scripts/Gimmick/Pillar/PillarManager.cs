using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Obi;

[RequireComponent(typeof(ObiSolver))]
public class PillarManager : MonoBehaviour
{

    [SerializeField] private ObiSolver _solver;

    [SerializeField] private Wrappable[] _wrappables;
    [SerializeField] private UnityEvent onFinish = new UnityEvent();

    [SerializeField] private GameObject _clearApply;

    private void Awake()
    {
        if (!_solver) _solver = GetComponent<ObiSolver>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _solver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable()
    {
        _solver.OnCollision -= Solver_OnCollision;
    }

    private void Update()
    {
        bool allWrapped = true;

        // Test our win condition: all pegs must be wrapped.
        foreach (var wrappable in _wrappables)
        {
            if (!wrappable.IsWrapped())
            {
                allWrapped = false;
                break;
            }
        }

        if (allWrapped)
            onFinish.Invoke();
    }

    private void Solver_OnCollision(ObiSolver s, ObiSolver.ObiCollisionEventArgs e)
    {
        // reset to unwrapped state:
        foreach (var wrappable in _wrappables)
            wrappable.Reset();

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            // look for actual contacts only:
            if (contact.distance < 0.025f)
            {
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                    var wrappable = col.GetComponent<Wrappable>();
                    if (wrappable != null)
                    {
                        wrappable.SetWrapped();
                    }

                }
            }
        }
    }

    public void OnClear()
    {
        _clearApply.SetActive(true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PillarManager))]
public class PillarManagerCustom:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PillarManager pillarManager = target as PillarManager;

        if (GUILayout.Button("ThisGimmickClear!!"))
        {
            if (Application.isPlaying)
            {
                pillarManager.OnClear();
            }
        }
    }
}
#endif

