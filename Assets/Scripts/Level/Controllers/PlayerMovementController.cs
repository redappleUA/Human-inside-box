using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[DisallowMultipleComponent]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Camera _camera;
    [SerializeField, Range(0.1f, 5f)] float _moveSpeed, _rotateSpeed, _jumpHeight;
    [SerializeField, Range(0f , - 10f)] float _gravityMultiplier;
    public bool IsGrounded { get; private set; } = true;

    const float GRAVITY = -9.81f;
    const string JUMP_TRIGGER = "Jump";
    const string SPEED = "Speed";

    private CharacterController _controller;
    private Movement _input;
    private Vector3 _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = new();
    }

    private void Update()
    {
        IsGrounded = _controller.isGrounded;
        //if (IsGrounded && _velocity.y < 0f)
        //{
        //    _velocity.y = 0f;
        //}

        //Movement
        if (_input.Player.Move.IsPressed())
        {
            var moveVector = GetMovementVector();
            _controller.Move(moveVector * Time.deltaTime * _moveSpeed);
            _animator.SetFloat(SPEED, _moveSpeed);

            //Rotation towards move vector
            var lookRotation = Quaternion.LookRotation(moveVector.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _animator.SetFloat(SPEED, 0f);
        }

        //Jump
        if (CheckJump())
        {
            //_velocity.y = Mathf.Sqrt(_jumpHeight * GRAVITY * _gravityMultiplier);
            _animator.SetTrigger(JUMP_TRIGGER);
        }
        //_velocity.y += GRAVITY * Time.deltaTime;
        //_controller.Move(_velocity * Time.deltaTime);
    }

    private Vector3 GetMovementVector()
    {
        Vector2 moveVector = _input.Player.Move.ReadValue<Vector2>();
        Vector3 move = new(moveVector.x, 0f, moveVector.y);
        move = move.x * _camera.transform.right + move.z * _camera.transform.forward;
        return move;
    }
    private bool CheckJump()
    {
        return _input.Player.Jump.triggered && IsGrounded;
    }

    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();
}
