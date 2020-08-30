/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class CustomBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    private bool renderedOnce = false;

    // Polar Bear movement variables
    private Vector3 pos;
    private Vector3 minPos = new Vector3(-10, 0, -10);
    private Vector3 maxPos = new Vector3(10, 0, 10);
    private Vector3 tarPos;
    private float speed = 5.0f;


    private string[] bearInfo = {
        "Eddy",
        "Henry",
        "Inukshuk",
        "Ganuk"
    };

    // Use this for initialization
    void Start()
    {
        // Add RemoteTransformations script to object and set its entry
        this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        // Qurey additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
            var name = value;
            Debug.Log(value);

            entry.getAdditionalData().TryGetValue("age", out value);
            var age = value;

            entry.getAdditionalData().TryGetValue("birth", out value);
            var birth = value;

            entry.getAdditionalData().TryGetValue("birthplace", out value);
            var birthplace = value;

            entry.getAdditionalData().TryGetValue("arrival", out value);
            var arrival = value;

            {
                //add text
                GameObject text = new GameObject();
                TextMesh t = text.AddComponent<TextMesh>();
                t.text = name + "\r\n" + age + "\r\n" + birth + "\r\n" + birthplace + "\r\n" + arrival;
                t.fontSize = 50;
                t.color = new Vector4(0, 0, 0, 1);
                text.name = "Text" + name;
                text.transform.localScale = 0.1f * Vector3.one;
                
                //set text location
                text.transform.position = this.gameObject.transform.position + new Vector3(7f * System.Array.IndexOf(bearInfo, name) - 13, 0f * System.Array.IndexOf(bearInfo, name) + 5.5f, 0);
            }

        }

        //Set initial target position
        tarPos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), 
                        Random.Range(minPos.z, maxPos.z));

        //Flip polar bear in z-axis
        if(this.gameObject.name.Contains("Polar Bear")) {
                 this.transform.localScale = new Vector3(0.5f, 0.5f, -0.5f);
         }
    }

    // Update is called once per frame
    void Update()
    {
        if (entry.getAdditionalData() != null && !renderedOnce) {

            if(this.gameObject.name.Contains("Polar Bear")) {
                float step = speed * Time.deltaTime;
                this.gameObject.transform.rotation =  Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation (tarPos - transform.position), 
                    5f *Time.deltaTime);
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, tarPos, step);

                if (Vector3.Distance(transform.position, tarPos) < 0.001f){
                    tarPos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), 
                        Random.Range(minPos.z, maxPos.z));
                }

            } else{
                renderedOnce = true;
            }

        }
    }
}
