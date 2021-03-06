﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;
using Random = UnityEngine.Random;

public class shipMovement : MonoBehaviour {
    //ship movement
    public Transform[] path;
    float speed = 0.3f;
    float rotationSpeed=0.1f;
    private Vector3 targetDirection;
    //carrier and presents
    public GameObject carrier;
    public GameObject presentTop;
    public GameObject presentBox;
    public Rigidbody rbCarrier;
    public Rigidbody rbBox;
    public Rigidbody rbTop;
    //UI
    public bool UIreadyToFade = false;
    /*//music
    public GameObject bgMusicObject;
    public float fadeOutDuration;
    music fadeOutMusic;
    AudioSource bgMusic;*/


    IEnumerator Followpath()
    {
        foreach (Transform waypoint in path)
        {
            yield return StartCoroutine(Move(waypoint.position, speed, waypoint));
        }
    }

    IEnumerator Move(Vector3 destination, float speed, Transform waypoint)
    {
        //Rotation variables
        targetDirection = Vector3.Normalize(destination - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        
        if (waypoint.tag == "lastWaypoint")
            speed = speed / 6;//landing speed at last waypoint
        
        //rotate and move ship
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            if (waypoint.tag != "lastWaypoint")
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-10, 180, 0), Time.deltaTime * rotationSpeed);
            //move the ship
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
        if (waypoint.tag == "dropCarrier")
        {
            carrier.transform.SetParent(null);
            
            presentBox.transform.SetParent(null);
            presentTop.transform.SetParent(null);
            presentTop.transform.parent = presentBox.transform;
            rbBox.isKinematic = false;
            
            //rbTop.isKinematic = false;
            //rbTop.useGravity = true;
            //rbBox.useGravity= true;
        }

        if (waypoint.tag == "lastWaypoint")
            UIreadyToFade = true;
    }
    private void Start()
    {
        rbCarrier= carrier.GetComponent<Rigidbody>();
        rbBox = presentBox.GetComponent<Rigidbody>();
        rbTop = presentBox.GetComponent<Rigidbody>();
        //bgMusic = bgMusicObject.GetComponent<AudioSource>();
        //fadeOutMusic= bgMusicObject.GetComponent<music>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Followpath());
            //StartCoroutine(fadeOutMusic.FadeOut(bgMusic, fadeOutDuration));       
        }
    }
}
