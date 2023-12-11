using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using System.Net.Sockets;

public class Kam : Agent
{
    private float TargetWalkingSpeed =0.9f;

    public Transform target;
    private GameObject body;
    private GameObject[] scapulas = new GameObject[4];
    private GameObject[] thighs = new GameObject[4];
    private GameObject[] legs = new GameObject[4]; 
    private ArticulationBody artBody= new ArticulationBody();
    private ArticulationBody[] scapulas_Artic = new ArticulationBody[4];    
    private ArticulationBody[] thighs_Artic = new ArticulationBody[4];
    private ArticulationBody[] legs_Artic = new ArticulationBody[4];
    private ArticulationDrive[] scapulas_xDrive = new ArticulationDrive[4];    
    private ArticulationDrive[] thighs_xDrive = new ArticulationDrive[4];
    private ArticulationDrive[] legs_xDrive = new ArticulationDrive[4];

    private float[] distance = {0,0,0,0,0,0,0,0,0,0};
    private Vector3 previousPositions = new Vector3(0, 0, 0); 
    // private float[] lim = {-10f,20f,-80f,40f,-50f,70f};
    private float[] lim = {-10f,60f,-80f,40f,-50f,70f};
    int sti=10000;
    int dam=100;
    int flim=1000;   
    bool bb=true;
    int w=2;

    public override void Initialize(){ Debug.Log("Initialize");
        
        body = this.transform.Find("base_link").gameObject;
        scapulas[0] = body.transform.Find("back_left_link_first").gameObject;
        scapulas[1] = body.transform.Find("back_right_link_first").gameObject;
        scapulas[2] = body.transform.Find("front_right_link_first").gameObject;
        scapulas[3] = body.transform.Find("front_left_link_first").gameObject;
        thighs[0] = scapulas[0].transform.Find("back_left_link_second").gameObject;
        thighs[1] = scapulas[1].transform.Find("back_right_link_second").gameObject;
        thighs[2] = scapulas[2].transform.Find("front_right_link_second").gameObject;
        thighs[3] = scapulas[3].transform.Find("front_left_link_second").gameObject;
        legs[0] = thighs[0].transform.Find("back_left_link_foot").gameObject;
        legs[1] = thighs[1].transform.Find("back_right_link_foot").gameObject;
        legs[2] = thighs[2].transform.Find("front_right_link_foot").gameObject;
        legs[3] = thighs[3].transform.Find("front_left_link_foot").gameObject;

        artBody = body.GetComponent<ArticulationBody>();

        for(int i=0;i<4;i++){
            scapulas_Artic[i] = scapulas[i].GetComponent<ArticulationBody>();  
            scapulas_xDrive[i] = scapulas_Artic[i].xDrive;   
            scapulas_xDrive[i].stiffness = sti;  
            scapulas_xDrive[i].damping = dam;       
            scapulas_xDrive[i].forceLimit = flim;  
            if(i==0 || i==3){
                scapulas_xDrive[i].lowerLimit = -lim[1];
                scapulas_xDrive[i].upperLimit = -lim[0];
            }
            else{
                scapulas_xDrive[i].lowerLimit = lim[0];
                scapulas_xDrive[i].upperLimit = lim[1];
            }
            scapulas_Artic[i].xDrive = scapulas_xDrive[i];    
        }
        for(int i=0;i<4;i++){
            thighs_Artic[i] = thighs[i].GetComponent<ArticulationBody>();
            legs_Artic[i] = legs[i].GetComponent<ArticulationBody>();
  
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            legs_xDrive[i] = legs_Artic[i].xDrive;

            thighs_xDrive[i].lowerLimit = lim[2];
            thighs_xDrive[i].upperLimit = lim[3];
            thighs_xDrive[i].stiffness = sti;  
            thighs_xDrive[i].damping = dam;       
            thighs_xDrive[i].forceLimit = flim;
            legs_xDrive[i].lowerLimit = lim[4];
            legs_xDrive[i].upperLimit = lim[5];
            legs_xDrive[i].stiffness = sti;  
            legs_xDrive[i].damping = dam;       
            legs_xDrive[i].forceLimit = flim;  

            thighs_Artic[i].xDrive = thighs_xDrive[i];
            legs_Artic[i].xDrive = legs_xDrive[i];    
        }
    }

    public override void OnEpisodeBegin(){ Debug.Log("episode begin");
   
        artBody.enabled = false;
        body.transform.localPosition = new Vector3(0, 0, 0);  
        body.transform.localRotation = Quaternion.Euler(0, 0, 0); 
        float scang = 0;
        scapulas[0].transform.localRotation = Quaternion.Euler(0, 0, -scang);
        scapulas[1].transform.localRotation = Quaternion.Euler(0, 0, scang);
        scapulas[2].transform.localRotation = Quaternion.Euler(0, 0, scang);
        scapulas[3].transform.localRotation = Quaternion.Euler(0, 0,-scang);

        for(int i=0;i<4;i++){
            thighs[i].transform.localRotation = Quaternion.Euler(40f, 0, 0);
            legs[i].transform.localRotation = Quaternion.Euler(-75f,0,0); 

            scapulas_xDrive[i].target =0;
            scapulas_xDrive[i].targetVelocity = 0;
            thighs_xDrive[i].target =0;        
            thighs_xDrive[i].targetVelocity = 0;
            legs_xDrive[i].target =0;
            legs_xDrive[i].targetVelocity = 0;

            thighs_xDrive[w].lowerLimit = -360;
            thighs_xDrive[w].upperLimit = 360;
            thighs_xDrive[w].stiffness = 0;  
            thighs_xDrive[w].damping = 0;       
            thighs_xDrive[w].forceLimit = 0; 

            // legs_xDrive[w].lowerLimit = -360;
            // legs_xDrive[w].upperLimit = 360;
            // legs_xDrive[w].stiffness = 0;  
            // legs_xDrive[w].damping = 0;       
            
            // thighs_xDrive[w].lowerLimit = -0;
            // thighs_xDrive[w].upperLimit = 0;
            // thighs_xDrive[w].stiffness = 0;  
            // thighs_xDrive[w].damping = 0;       
            // thighs_xDrive[w].forceLimit = 0; 

            // legs_xDrive[w].lowerLimit = -0;
            // legs_xDrive[w].upperLimit = 0;
            // legs_xDrive[w].stiffness = 0;  
            // legs_xDrive[w].damping = 0;       
            // legs_xDrive[w].forceLimit = 0;    
            
            // thighs_xDrive[w].lowerLimit = lim[2];
            // thighs_xDrive[w].upperLimit = lim[3];
            // thighs_xDrive[w].stiffness = sti;  
            // thighs_xDrive[w].damping = dam;       
            // thighs_xDrive[w].forceLimit = flim;

            // legs_xDrive[w].lowerLimit = lim[4];
            // legs_xDrive[w].upperLimit = lim[5];
            // legs_xDrive[w].stiffness = sti;  
            // legs_xDrive[w].damping = dam;       
            // legs_xDrive[w].forceLimit = flim;  
            // bb=true;                 
    
            scapulas_Artic[i].xDrive = scapulas_xDrive[i];    
            thighs_Artic[i].xDrive = thighs_xDrive[i];
            legs_Artic[i].xDrive = legs_xDrive[i];   

        }
        artBody.enabled = true; 

        previousPositions = new Vector3(0, 0, 0); 
        // target.localPosition = new Vector3(0,0,Random.Range(10f,20f));
        target.localPosition = new Vector3(0,0,7f);
    }

    public override void CollectObservations(VectorSensor sensor){

        sensor.AddObservation(target.transform.forward-body.transform.forward);
        sensor.AddObservation(target.transform.up-body.transform.up);
        sensor.AddObservation(target.transform.right-body.transform.right);
        for(int i=0;i<4;i++){
            sensor.AddObservation(scapulas[i].transform.localRotation.eulerAngles.z);         
            sensor.AddObservation(thighs[i].transform.localRotation.eulerAngles.x);
            sensor.AddObservation(legs[i].transform.localRotation.eulerAngles.x);
        }
    }

    public override void OnActionReceived (ActionBuffers actionBuffers){
        float force = 2f;

        for(int i=0;i<4;i++){                    
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            legs_xDrive[i] = legs_Artic[i].xDrive;
            scapulas_xDrive[i] = scapulas_Artic[i].xDrive;

            thighs_xDrive[i].target += actionBuffers.ContinuousActions[i]*force; 
            legs_xDrive[i].target  +=  actionBuffers.ContinuousActions[i+4]*force;   
            scapulas_xDrive[i].target += actionBuffers.ContinuousActions[i+8];

            thighs_Artic[i].xDrive = thighs_xDrive[i];   
            legs_Artic[i].xDrive = legs_xDrive[i];
            scapulas_Artic[i].xDrive = scapulas_xDrive[i];    
        }


        // for(int i=0;i<2;i++){  
        //     distance[i] = Mathf.Abs(thighs[i].transform.localRotation.eulerAngles.x - thighs[i+2].transform.localRotation.eulerAngles.x);
        //     distance[i+2] = Mathf.Abs(legs[i].transform.localRotation.eulerAngles.x - legs[i+2].transform.localRotation.eulerAngles.x);       
        // }
        // for(int i=0;i<4;i++){ 
        //     AddReward(CalculateReward(distance[i]));
        // }
        // float CalculateReward(float distance){
        //     if (distance > 8f){
        //         return 0f;
        //     }
        //     else {
        //         return 0.03f - (distance*0.0002f);
        //     }
        // }

        // distance[4] = Mathf.Abs(scapulas[0].transform.localRotation.eulerAngles.z + scapulas[1].transform.localRotation.eulerAngles.z);
        // distance[5] = Mathf.Abs(scapulas[2].transform.localRotation.eulerAngles.z + scapulas[3].transform.localRotation.eulerAngles.z); 
        // distance[6] = Mathf.Abs(scapulas[0].transform.localRotation.eulerAngles.z + scapulas[2].transform.localRotation.eulerAngles.z);
        // distance[7] = Mathf.Abs(scapulas[1].transform.localRotation.eulerAngles.z + scapulas[3].transform.localRotation.eulerAngles.z); 
        // distance[8] = Mathf.Abs(scapulas[0].transform.localRotation.eulerAngles.z - scapulas[3].transform.localRotation.eulerAngles.z);
        // distance[9] = Mathf.Abs(scapulas[1].transform.localRotation.eulerAngles.z - scapulas[2].transform.localRotation.eulerAngles.z);                     
        // for(int i=4;i<10;i++){ 
        //     AddReward(CalculateReward2(distance[i]));
        // }
        // float CalculateReward2(float distance){
        //     if (distance > 4f){
        //         return 0f;
        //     }
        //     else {
        //         return 0.01f - (distance*0.0002f);
        //     }
        // }


        Vector3 currentPosition = body.transform.localPosition;
        Vector3 bodyVelocity = (currentPosition - previousPositions) / Time.deltaTime;

        var cubeForward = target.transform.forward;
        var cubegravity = target.transform.up;
        var cubeholizon = target.transform.right;     
        var lookAtTargetReward = (Vector3.Dot(cubeForward, body.transform.forward) + 1) * .5F;
        var gravityReward = (Vector3.Dot(cubegravity, body.transform.up) + 1) * .5F;       
        var holizonReward = (Vector3.Dot(cubeholizon, body.transform.right) + 1) * .5F;   
        var matchSpeedReward = GetMatchingVelocityReward(cubeForward * TargetWalkingSpeed, bodyVelocity);
            float GetMatchingVelocityReward(Vector3 velocityGoal, Vector3 actualVelocity){
                var velDeltaMagnitude = Mathf.Clamp(Vector3.Distance(actualVelocity, velocityGoal), 0, TargetWalkingSpeed);
                return Mathf.Pow(1 - Mathf.Pow(velDeltaMagnitude / TargetWalkingSpeed, 2), 2);
            }
        AddReward(matchSpeedReward*lookAtTargetReward);
        // AddReward(gravityReward*holizonReward);
        AddReward(-0.05f);

        float distanceToTarget = Vector3.Distance(currentPosition, target.localPosition);
        float predistanceToTarget = Vector3.Distance(previousPositions, target.localPosition);
        float sinndou = Mathf.Abs(currentPosition.y - previousPositions.y);
        previousPositions = currentPosition;

        if (distanceToTarget < 1f){
            Debug.LogWarning("GOAL");
            EndEpisode();
        }

        // if(sinndou<0.0003){
        //     if(body.transform.localPosition.z>0.1f){
        //         Debug.LogWarning("sinndou");
        //         AddReward(0.5f);    
        //     }
        // }


        if (distanceToTarget - predistanceToTarget > 0.5f){
            Debug.LogWarning("back");
            EndEpisode();
        }
        else if (distanceToTarget - predistanceToTarget >= 0f){
            if(body.transform.localPosition.z>0.1f){
                // Debug.LogWarning("littleback");
                AddReward(-0.05f);    
            }
        }


        if(body.transform.localPosition.z<-0.1f){
            Debug.LogWarning("out");
            EndEpisode();
        }

        if(body.transform.localPosition.z>7f){
            Debug.LogWarning("stop");
            EndEpisode();
        }


        // if(body.transform.localPosition.z >Random.Range(1f,3f)){     

        //     if(bb){
        //         var lockthings = thighs[w].transform.localRotation.eulerAngles.x;
        //         var locklegs = legs[w].transform.localRotation.eulerAngles.x;
        //         var lockscapulas = scapulas[w].transform.localRotation.eulerAngles.z;

        //         artBody.enabled = false;
        //             // scapulas[w].transform.localRotation = Quaternion.Euler(0, 0, lockscapulas);
        //             thighs[w].transform.localRotation = Quaternion.Euler(lockthings, 0, 0);
        //             // legs[w].transform.localRotation = Quaternion.Euler(locklegs,0,0);  
        //         artBody.enabled = true;    

        //         Debug.LogWarning("change");
        //         bb=false;

        //         // scapulas_xDrive[w].target =0;
        //         // scapulas_xDrive[w].targetVelocity = 0;
        //         // scapulas_xDrive[w].lowerLimit = -0;
        //         // scapulas_xDrive[w].upperLimit = 0;
        //         // scapulas_xDrive[w].stiffness = 0;  
        //         // scapulas_xDrive[w].damping = 0;       
        //         // scapulas_xDrive[w].forceLimit = 0;    
        //         // scapulas_Artic[w].xDrive = scapulas_xDrive[w];   

        //         thighs_xDrive[w].target =0;        
        //         thighs_xDrive[w].targetVelocity = 0;
        //         thighs_xDrive[w].lowerLimit = -0;
        //         thighs_xDrive[w].upperLimit = 0;
        //         thighs_xDrive[w].stiffness = 0;  
        //         thighs_xDrive[w].damping = 0;       
        //         thighs_xDrive[w].forceLimit = 0; 
        //         thighs_Artic[w].xDrive = thighs_xDrive[w];

        //         legs_xDrive[w].target =0;
        //         legs_xDrive[w].targetVelocity = 0;    
        //         legs_xDrive[w].lowerLimit = -0;
        //         legs_xDrive[w].upperLimit = 0;
        //         legs_xDrive[w].stiffness = 0;  
        //         legs_xDrive[w].damping = 0;       
        //         legs_xDrive[w].forceLimit = 0;    
        //         legs_Artic[w].xDrive = legs_xDrive[w];   

        //     }
        // }


        // if(body.transform.localPosition.z >Random.Range(2.5f,3.5f)){
        // if(body.transform.localPosition.z >Random.Range(1f,2f)){     
        //     if(bb){
        //         Debug.LogWarning("change");
        //         thighs_xDrive[w].lowerLimit = -360;
        //         thighs_xDrive[w].upperLimit = 360;
        //         thighs_xDrive[w].stiffness = 0;  
        //         thighs_xDrive[w].damping = 0;       
        //         thighs_xDrive[w].forceLimit = 0; 

        //         // legs_xDrive[w].lowerLimit = -360;
        //         // legs_xDrive[w].upperLimit =360;
        //         // legs_xDrive[w].stiffness = 0;  
        //         // legs_xDrive[w].damping = 0;       
        //         // legs_xDrive[w].forceLimit = 0;   

        //         // legs_xDrive[w].lowerLimit = lim[4];
        //         // legs_xDrive[w].upperLimit = lim[5];
        //         // legs_xDrive[w].stiffness = sti;  
        //         // legs_xDrive[w].damping = dam;       
        //         // legs_xDrive[w].forceLimit = flim;  

        //         thighs_Artic[w].xDrive = thighs_xDrive[w];
        //         legs_Artic[w].xDrive = legs_xDrive[w];                    

        //         bb=false;
        //     }
        // }

        // if(Vector3.Dot(cubeForward, body.transform.forward)<0.995){
        //     Debug.LogWarning("yoko");
        //     AddReward(-0.1f);  
        // }

        if(Mathf.Abs(body.transform.localPosition.x)>0.1f){
            // Debug.LogWarning("yokozure");  
            AddReward(-0.05f*body.transform.localPosition.x);  
        }


        if(body.transform.localPosition.y <-0.25){
            Debug.LogWarning("ylowreset");
            EndEpisode();   
        }
        float rotz=body.transform.localRotation.eulerAngles.z;
        if((rotz>50f) && (rotz<310f)){
            Debug.LogWarning("nanamezreset");
            EndEpisode();
        }
        float rotx=body.transform.localRotation.eulerAngles.x;
        if((rotx>50f) && (rotx<310f)){
            Debug.LogWarning("nanamexreset");
            EndEpisode();
        }
    }
    
    public void Update(){ 

    }
}
