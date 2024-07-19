using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Fluid
{
    [AddComponentMenu("FusionWater/Fluid")]
    public class Fluid : MonoBehaviour
    {
        public float density = 1;

        public float drag = 1;

        public float angularDrag = 1f;

        [SerializeField, Tooltip("�������̃G�t�F�N�g")] private GameObject _effect;

        public Collider coll { get; private set; }

        private void Start()
        {
            coll = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out BaseFluidInteractor fluidInteractor))
            {
                fluidInteractor.EnterFluid(this);
            }
            // �Փˈʒu���擾����
            Vector3 hitPos = other.ClosestPointOnBounds(other.transform.position);
            Instantiate(_effect, hitPos, Quaternion.identity);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BaseFluidInteractor fluidInteractor))
            {
                fluidInteractor.ExitFluid(this);
            }
        }
    }
}
