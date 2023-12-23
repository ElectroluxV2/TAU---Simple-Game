using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private CharacterController _controller;

    public float speed;

    private void Awake() => Trigger.E_GameEnd.AddListener(() => GameLoop = false);

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private bool GameLoop = true;
    private Vector2 _input;
    
    // Update is called once per frame
    void Update()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
    }

    private Vector3 _move;
    
    private void FixedUpdate()
    {
        if (!GameLoop) return;
        _move = transform.forward * _input.y + transform.right * _input.x;
        
        _controller.Move(_move * (speed * Time.deltaTime));
    }
}
