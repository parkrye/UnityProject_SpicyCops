using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace jung
{
    public class PlayerMover : MonoBehaviour
    {

        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private float roateSpeed;

        private Rigidbody rigid;
        private Vector2 inputDir;


        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Right(inputDir.x);
            Foward(inputDir.y);
        }

        private void Foward(float input)
        {
            rigid.AddForce(transform.forward * input * moveSpeed * Time.deltaTime);

            if (rigid.velocity.sqrMagnitude > maxSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * maxSpeed;
            }
        }

        private void Right(float input)
        {
            rigid.AddForce(transform.right * input * moveSpeed * Time.deltaTime);

            if (rigid.velocity.sqrMagnitude > maxSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * maxSpeed;
            }
        }

        private void OnMove(InputValue value)
        {
            inputDir = value.Get<Vector2>();
        }


    }

}
