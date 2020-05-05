// Simple Sun Shader // Copyright 2016 Kybernetik //

using UnityEngine;

namespace Kybernetik
{
#if EXECUTION_ORDER_ATTRIBUTE
    [ExecutionOrder(ExecutionOrderAttribute.Last)]
#endif
    public sealed class FaceCamera : MonoBehaviour
    {
        /************************************************************************************************************************/

        private void LateUpdate()
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        /************************************************************************************************************************/
    }
}