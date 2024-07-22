using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObiSolver))]
public class SolverEventHandler : MonoBehaviour
{
    private ObiSolver solver;

    void Awake()
    {
        solver = GetComponent<ObiSolver>();
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();

        // just iterate over all contacts in the current frame:
        foreach (Oni.Contact contact in e.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance < 0.01f && contact.bodyB >= 0f)
            {
                ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                    // do something with the collider.
                    var component1 = col.GetComponent<TreeHitChecker>();

                    if (component1 != null)
                    {
                        component1.OnMyDestroy();
                    }

                    var component2 = col.GetComponent<RockController>();

                    if (component2 != null)
                    {
                        component2.OnPush();
                    }

                    var component3 = col.GetComponent<LeverHitCheck>();

                    if(component3 != null)
                    {
                        component3.OnChange();
                    }
                }
            }
        }
    }
}
