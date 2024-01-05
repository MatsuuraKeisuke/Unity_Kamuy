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
    private GameObject[] first_link = new GameObject[4];
    private GameObject[] second_link = new GameObject[4];
    private GameObject[] third_link = new GameObject[4]; 
    private ArticulationBody artBody= new ArticulationBody();
    private ArticulationBody[] first_link_Artic = new ArticulationBody[4];    
    private ArticulationBody[] second_link_Artic = new ArticulationBody[4];
    private ArticulationBody[] third_link_Artic = new ArticulationBody[4];
    private ArticulationDrive[] first_link_xDrive = new ArticulationDrive[4];    
    private ArticulationDrive[] second_link_xDrive = new ArticulationDrive[4];
    private ArticulationDrive[] third_link_xDrive = new ArticulationDrive[4];

    private float[] distance = {0,0,0,0,0,0,0,0,0,0};
    private Vector3 previousPositions = new Vector3(0, 0, 0); 
    // private float[] lim = {-10f,20f,-80f,40f,-50f,70f};
    private float[] lim = {-10f,60f,-80f,40f,-50f,70f};
    int sti=10000;
    int dam=100;
    int flim=1000;   
    bool bb=true;
    int w=1;
    float boxdistance = 8f;

    public override void Initialize(){ Debug.Log("Initialize");
        
        body = this.transform.Find("base_link").gameObject;
        first_link[0] = body.transform.Find("back_left_link_first").gameObject;
        first_link[1] = body.transform.Find("back_right_link_first").gameObject;
        first_link[2] = body.transform.Find("front_right_link_first").gameObject;
        first_link[3] = body.transform.Find("front_left_link_first").gameObject;
        second_link[0] = first_link[0].transform.Find("back_left_link_second").gameObject;
        second_link[1] = first_link[1].transform.Find("back_right_link_second").gameObject;
        second_link[2] = first_link[2].transform.Find("front_right_link_second").gameObject;
        second_link[3] = first_link[3].transform.Find("front_left_link_second").gameObject;
        third_link[0] = second_link[0].transform.Find("back_left_link_foot").gameObject;
        third_link[1] = second_link[1].transform.Find("back_right_link_foot").gameObject;
        third_link[2] = second_link[2].transform.Find("front_right_link_foot").gameObject;
        third_link[3] = second_link[3].transform.Find("front_left_link_foot").gameObject;

        artBody = body.GetComponent<ArticulationBody>();

        for(int i=0;i<4;i++){
            first_link_Artic[i] = first_link[i].GetComponent<ArticulationBody>();  
            second_link_Artic[i] = second_link[i].GetComponent<ArticulationBody>();
            third_link_Artic[i] = third_link[i].GetComponent<ArticulationBody>();
  
            first_link_xDrive[i] = first_link_Artic[i].xDrive; 
            second_link_xDrive[i] = second_link_Artic[i].xDrive;
            third_link_xDrive[i] = third_link_Artic[i].xDrive;

                if(i==0 || i==3){
                    first_link_xDrive[i].lowerLimit = -lim[1];
                    first_link_xDrive[i].upperLimit = -lim[0];
                }
                else{
                    first_link_xDrive[i].lowerLimit = lim[0];
                    first_link_xDrive[i].upperLimit = lim[1];
                }
                first_link_xDrive[i].stiffness = sti;  
                first_link_xDrive[i].damping = dam;       
                first_link_xDrive[i].forceLimit = flim;  

                second_link_xDrive[i].lowerLimit = lim[2];
                second_link_xDrive[i].upperLimit = lim[3];
                second_link_xDrive[i].stiffness = sti;  
                second_link_xDrive[i].damping = dam;       
                second_link_xDrive[i].forceLimit = flim;

                third_link_xDrive[i].lowerLimit = lim[4];
                third_link_xDrive[i].upperLimit = lim[5];
                third_link_xDrive[i].stiffness = sti;  
                third_link_xDrive[i].damping = dam;       
                third_link_xDrive[i].forceLimit = flim;  

            first_link_Artic[i].xDrive = first_link_xDrive[i];    
            second_link_Artic[i].xDrive = second_link_xDrive[i];
            third_link_Artic[i].xDrive = third_link_xDrive[i];    
        }
    }

    public override void OnEpisodeBegin(){ Debug.Log("episode begin");
   
        artBody.enabled = false;
        body.transform.localPosition = new Vector3(0, 0, 0);  
        body.transform.localRotation = Quaternion.Euler(0, 0, 0); 
        float scang = 0;
        first_link[0].transform.localRotation = Quaternion.Euler(0, 0, -scang);
        first_link[1].transform.localRotation = Quaternion.Euler(0, 0, scang);
        first_link[2].transform.localRotation = Quaternion.Euler(0, 0, scang);
        first_link[3].transform.localRotation = Quaternion.Euler(0, 0,-scang);

        for(int i=0;i<4;i++){
            second_link[i].transform.localRotation = Quaternion.Euler(40f, 0, 0);
            third_link[i].transform.localRotation = Quaternion.Euler(-75f,0,0); 
  
            first_link_xDrive[i] = first_link_Artic[i].xDrive; 
            second_link_xDrive[i] = second_link_Artic[i].xDrive;
            third_link_xDrive[i] = third_link_Artic[i].xDrive;

                if(i==0 || i==3){
                    first_link_xDrive[i].lowerLimit = -lim[1];
                    first_link_xDrive[i].upperLimit = -lim[0];
                }
                else{
                    first_link_xDrive[i].lowerLimit = lim[0];
                    first_link_xDrive[i].upperLimit = lim[1];
                }
                first_link_xDrive[i].stiffness = sti;  
                first_link_xDrive[i].damping = dam;       
                first_link_xDrive[i].forceLimit = flim;  
                first_link_xDrive[i].target =0;
                first_link_xDrive[i].targetVelocity = 0;

                second_link_xDrive[i].lowerLimit = lim[2];
                second_link_xDrive[i].upperLimit = lim[3];
                second_link_xDrive[i].stiffness = sti;  
                second_link_xDrive[i].damping = dam;       
                second_link_xDrive[i].forceLimit = flim;
                second_link_xDrive[i].target =0;        
                second_link_xDrive[i].targetVelocity = 0;

                third_link_xDrive[i].lowerLimit = lim[4];
                third_link_xDrive[i].upperLimit = lim[5];
                third_link_xDrive[i].stiffness = sti;  
                third_link_xDrive[i].damping = dam;       
                third_link_xDrive[i].forceLimit = flim;  
                third_link_xDrive[i].target =0;
                third_link_xDrive[i].targetVelocity = 0; 
////////////free/////////////////
            // first_link_xDrive[w].lowerLimit = -360;
            // first_link_xDrive[w].upperLimit = 360;
            // first_link_xDrive[w].stiffness = 0;  
            // first_link_xDrive[w].damping = 0;       
            // first_link_xDrive[w].forceLimit = 0;

            // second_link_xDrive[w].lowerLimit = -360;
            // second_link_xDrive[w].upperLimit = 360;
            // second_link_xDrive[w].stiffness = 0;  
            // second_link_xDrive[w].damping = 0;       
            // second_link_xDrive[w].forceLimit = 0; 

            // third_link_xDrive[w].lowerLimit = -360;
            // third_link_xDrive[w].upperLimit = 360;
            // third_link_xDrive[w].stiffness = 0;  
            // third_link_xDrive[w].damping = 0;   
            // third_link_xDrive[w].forceLimit = 0;   
//////////////////////////////////   
            first_link_Artic[i].xDrive = first_link_xDrive[i];    
            second_link_Artic[i].xDrive = second_link_xDrive[i];
            third_link_Artic[i].xDrive = third_link_xDrive[i];    
        }                 
        artBody.enabled = true; 
        bb=true; 
        previousPositions = new Vector3(0, 0, 0); 
        // target.localPosition = new Vector3(0,0,Random.Range(10f,20f));
        target.localPosition = new Vector3(0,0,boxdistance);
    }

    public override void CollectObservations(VectorSensor sensor){

        sensor.AddObservation(target.transform.forward-body.transform.forward);
        sensor.AddObservation(target.transform.up-body.transform.up);
        sensor.AddObservation(target.transform.right-body.transform.right);
        for(int i=0;i<4;i++){
            sensor.AddObservation(first_link[i].transform.localRotation.eulerAngles.z);         
            sensor.AddObservation(second_link[i].transform.localRotation.eulerAngles.x);
            sensor.AddObservation(third_link[i].transform.localRotation.eulerAngles.x);
        }
    }

    public override void OnActionReceived (ActionBuffers actionBuffers){
        float force = 2f;

        for(int i=0;i<4;i++){                    
            second_link_xDrive[i] = second_link_Artic[i].xDrive;
            third_link_xDrive[i] = third_link_Artic[i].xDrive;
            first_link_xDrive[i] = first_link_Artic[i].xDrive;

            second_link_xDrive[i].target += actionBuffers.ContinuousActions[i]*force; 
            third_link_xDrive[i].target  +=  actionBuffers.ContinuousActions[i+4]*force;   
            first_link_xDrive[i].target += actionBuffers.ContinuousActions[i+8];

            second_link_Artic[i].xDrive = second_link_xDrive[i];   
            third_link_Artic[i].xDrive = third_link_xDrive[i];
            first_link_Artic[i].xDrive = first_link_xDrive[i];    
        }


        // for(int i=0;i<2;i++){  
        //     distance[i] = Mathf.Abs(second_link[i].transform.localRotation.eulerAngles.x - second_link[i+2].transform.localRotation.eulerAngles.x);
        //     distance[i+2] = Mathf.Abs(third_link[i].transform.localRotation.eulerAngles.x - third_link[i+2].transform.localRotation.eulerAngles.x);       
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

        // distance[4] = Mathf.Abs(first_link[0].transform.localRotation.eulerAngles.z + first_link[1].transform.localRotation.eulerAngles.z);
        // distance[5] = Mathf.Abs(first_link[2].transform.localRotation.eulerAngles.z + first_link[3].transform.localRotation.eulerAngles.z); 
        // distance[6] = Mathf.Abs(first_link[0].transform.localRotation.eulerAngles.z + first_link[2].transform.localRotation.eulerAngles.z);
        // distance[7] = Mathf.Abs(first_link[1].transform.localRotation.eulerAngles.z + first_link[3].transform.localRotation.eulerAngles.z); 
        // distance[8] = Mathf.Abs(first_link[0].transform.localRotation.eulerAngles.z - first_link[3].transform.localRotation.eulerAngles.z);
        // distance[9] = Mathf.Abs(first_link[1].transform.localRotation.eulerAngles.z - first_link[2].transform.localRotation.eulerAngles.z);                     
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
        AddReward(-0.2f);
        AddReward(-1.0f*Mathf.Abs(body.transform.localPosition.x));  

        float distanceToTarget = Vector3.Distance(currentPosition, target.localPosition);
        float predistanceToTarget = Vector3.Distance(previousPositions, target.localPosition);
        float sinndou = Mathf.Abs(currentPosition.y - previousPositions.y);
        previousPositions = currentPosition;

        if (distanceToTarget < 1f){
            Debug.LogWarning("GOAL");
            EndEpisode();
        }
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
        // if(sinndou<0.0003){
        //     if(body.transform.localPosition.z>0.1f){
        //         Debug.LogWarning("sinndou");
        //         AddReward(0.5f);    
        //     }
        // }
        // if(Vector3.Dot(cubeForward, body.transform.forward)<0.995){
        //     Debug.LogWarning("yoko");
        //     AddReward(-0.1f);  
        // }        


////////////lock////////////////////
        if(body.transform.localPosition.z >Random.Range(1f,2f)){     
            if(bb){
                var lockfirst_link = first_link[w].transform.localRotation.eulerAngles.z;                
                var locksecond_link = second_link[w].transform.localRotation.eulerAngles.x;
                var lockthird_link = third_link[w].transform.localRotation.eulerAngles.x;

                artBody.enabled = false;
                    first_link[w].transform.localRotation = Quaternion.Euler(0, 0, lockfirst_link);
                    // second_link[w].transform.localRotation = Quaternion.Euler(locksecond_link, 0, 0);
                    // third_link[w].transform.localRotation = Quaternion.Euler(lockthird_link,0,0);  
                artBody.enabled = true;    

                // Debug.LogWarning("change");
                bb=false;

                first_link_xDrive[w].target =0;
                first_link_xDrive[w].targetVelocity = 0;
                first_link_xDrive[w].lowerLimit = -0;
                first_link_xDrive[w].upperLimit = 0;
                first_link_xDrive[w].stiffness = 0;  
                first_link_xDrive[w].damping = 0;       
                first_link_xDrive[w].forceLimit = 0;    
                first_link_Artic[w].xDrive = first_link_xDrive[w];   

                // second_link_xDrive[w].target =0;        
                // second_link_xDrive[w].targetVelocity = 0;
                // second_link_xDrive[w].lowerLimit = -0;
                // second_link_xDrive[w].upperLimit = 0;
                // second_link_xDrive[w].stiffness = 0;  
                // second_link_xDrive[w].damping = 0;       
                // second_link_xDrive[w].forceLimit = 0; 
                // second_link_Artic[w].xDrive = second_link_xDrive[w];

                // third_link_xDrive[w].target =0;
                // third_link_xDrive[w].targetVelocity = 0;    
                // third_link_xDrive[w].lowerLimit = -0;
                // third_link_xDrive[w].upperLimit = 0;
                // third_link_xDrive[w].stiffness = 0;  
                // third_link_xDrive[w].damping = 0;       
                // third_link_xDrive[w].forceLimit = 0;    
                // third_link_Artic[w].xDrive = third_link_xDrive[w];   
            }
        }
//////////////////////////////////////////////////// 

////////////free 途中から////////////////////////////
        // // if(body.transform.localPosition.z >Random.Range(2.5f,3.5f)){
        // if(body.transform.localPosition.z >Random.Range(1f,2f)){     
        //     if(bb){
        //         Debug.LogWarning("change");
        //         bb=false;                

        //         first_link_xDrive[w].lowerLimit = -360;
        //         first_link_xDrive[w].upperLimit = 360;
        //         first_link_xDrive[w].stiffness = 0;  
        //         first_link_xDrive[w].damping = 0;       
        //         first_link_xDrive[w].forceLimit = 0; 
        //         first_link_Artic[w].xDrive = first_link_xDrive[w];

        //         // second_link_xDrive[w].lowerLimit = -360;
        //         // second_link_xDrive[w].upperLimit = 360;
        //         // second_link_xDrive[w].stiffness = 0;  
        //         // second_link_xDrive[w].damping = 0;       
        //         // second_link_xDrive[w].forceLimit = 0; 
        //         // second_link_Artic[w].xDrive = second_link_xDrive[w];                

        //         // third_link_xDrive[w].lowerLimit = -360;
        //         // third_link_xDrive[w].upperLimit =360;
        //         // third_link_xDrive[w].stiffness = 0;  
        //         // third_link_xDrive[w].damping = 0;       
        //         // third_link_xDrive[w].forceLimit = 0;   
        //         // third_link_Artic[w].xDrive = third_link_xDrive[w];                    
        //     }
        // }
/////////////////////////////////////////////////////////////////        

        if(body.transform.localPosition.z<-0.1f){
            Debug.LogWarning("out");
            EndEpisode();
        }
        if(body.transform.localPosition.z>boxdistance){
            Debug.LogWarning("stop");
            EndEpisode();
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
