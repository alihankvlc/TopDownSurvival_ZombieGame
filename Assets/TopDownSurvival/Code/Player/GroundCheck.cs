using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeadNation
{
    using UnityEngine;

    [System.Serializable]
    public class GroundCheck
    {
        private float m_GroundRadius = 0.12f;
        private float m_GroundOffset = 0.08f;
        private LayerMask m_GroundLayerMask = LayerMask.GetMask("Everything");

        private Transform m_PlayerTransformPosition;
        public GroundCheck(Transform playerTransform, float offset, float radius, LayerMask layerMask)
        {
            m_PlayerTransformPosition = playerTransform;
            m_GroundRadius = radius;
            m_GroundOffset = offset;
            m_GroundLayerMask = layerMask;
        }

        private bool m_IsGrounded;
        private Vector3 m_VerticalVelocity;

        private float m_Gravity = -20f;

        public float SetGravity
        {
            get => m_Gravity;
            set => m_Gravity = value;
        }
        public Vector3 Gravity()
        {
            if (IsGrounded() && m_VerticalVelocity.y < 0.0f)
            {
                m_VerticalVelocity.y = -2f;
            }
            m_VerticalVelocity.y += Time.deltaTime * m_Gravity;
            return m_VerticalVelocity;
        }
        public bool IsGrounded()
        {
            Vector3 spherePosition = m_PlayerTransformPosition.position + Vector3.up * m_GroundOffset;
            m_IsGrounded = Physics.CheckSphere(spherePosition, m_GroundRadius, m_GroundLayerMask, QueryTriggerInteraction.Ignore);

            return m_IsGrounded;
        }
    }
}
