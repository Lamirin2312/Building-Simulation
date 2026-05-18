using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    private float _verticalVelocity;

    private void Update()
    {
        Vector3 move = transform.right * _input.Move.x + transform.forward * _input.Move.y;
        _controller.Move(move * _speed * Time.deltaTime);

        // Thêm trọng lực cơ bản
        if (!_controller.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        _controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }
}