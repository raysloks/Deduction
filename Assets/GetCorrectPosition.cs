﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCorrectPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.transform.position = player.transform.position;
    }
}