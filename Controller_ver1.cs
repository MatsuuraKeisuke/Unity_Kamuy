using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller_ver1 : MonoBehaviour
{
    private GameObject body;
    private GameObject[] scapulas = new GameObject[4];
    private GameObject[] thighs = new GameObject[4];
    private GameObject[] legs = new GameObject[4]; 
    private ArticulationBody[] scapulas_Artic = new ArticulationBody[4];    
    private ArticulationBody[] thighs_Artic = new ArticulationBody[4];
    private ArticulationBody[] legs_Artic = new ArticulationBody[4];
    private ArticulationDrive[] scapulas_xDrive = new ArticulationDrive[4];    
    private ArticulationDrive[] thighs_xDrive = new ArticulationDrive[4];
    private ArticulationDrive[] legs_xDrive = new ArticulationDrive[4];
    private ArticulationBody artBody= new ArticulationBody();
    private float[] lim = {-10f,20f,-80f,40f,-50f,70f};

    void Start(){

        float delayInSeconds = 1.0f; 
        Invoke("InitializeArticulationBodies", delayInSeconds);
    }
    void InitializeArticulationBodies(){

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
            legs_xDrive[i].lowerLimit = lim[4];
            legs_xDrive[i].upperLimit = lim[5];

            thighs_Artic[i].xDrive = thighs_xDrive[i];
            legs_Artic[i].xDrive = legs_xDrive[i];    
        }

        artBody.enabled = false;
        body.transform.localPosition = new Vector3(0, 0, 0);  
        body.transform.localRotation = Quaternion.Euler(0, 0, 0); 
        float scang = -5;
        scapulas[0].transform.localRotation = Quaternion.Euler(0, 0, -scang);
        scapulas[1].transform.localRotation = Quaternion.Euler(0, 0, scang);
        scapulas[2].transform.localRotation = Quaternion.Euler(0, 0, scang);
        scapulas[3].transform.localRotation = Quaternion.Euler(0, 0,-scang);
        for(int i=0;i<4;i++){
            thighs[i].transform.localRotation = Quaternion.Euler(40f, 0, 0);
            legs[i].transform.localRotation = Quaternion.Euler(-75f,0,0); 
        }
        artBody.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    //back left       
        if (Input.GetKey(KeyCode.A))
        {
            for(int i=0;i<4;i++){   
                if(i==0 || i==3){
                    scapulas_xDrive[i] = scapulas_Artic[i].xDrive;    
                    scapulas_xDrive[i].target -= 1f;
                    scapulas_Artic[i].xDrive = scapulas_xDrive[i];   
                }
                else{
                    scapulas_xDrive[i] = scapulas_Artic[i].xDrive;                        
                    scapulas_xDrive[i].target += 1f;
                    scapulas_Artic[i].xDrive = scapulas_xDrive[i];    
                }
            }
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            for(int i=0;i<4;i++){   
                if(i==0 || i==3){
                    scapulas_xDrive[i] = scapulas_Artic[i].xDrive;
                    scapulas_xDrive[i].target += 1f;
                    scapulas_Artic[i].xDrive = scapulas_xDrive[i];   
                }
                else{
                    scapulas_xDrive[i] = scapulas_Artic[i].xDrive;
                    scapulas_xDrive[i].target -= 1f;
                    scapulas_Artic[i].xDrive = scapulas_xDrive[i];    
                }
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            int i=0; 
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            thighs_xDrive[i+2] = thighs_Artic[i+2].xDrive;           
            thighs_xDrive[i].target -= 1f;     
            thighs_xDrive[i+2].target -= 1f;       
            thighs_Artic[i].xDrive = thighs_xDrive[i];   
            thighs_Artic[i+2].xDrive = thighs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.C))
        {
            int i=0;  
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            thighs_xDrive[i+2] = thighs_Artic[i+2].xDrive;      
            thighs_xDrive[i].target += 1f;     
            thighs_xDrive[i+2].target += 1f;       
            thighs_Artic[i].xDrive = thighs_xDrive[i];   
            thighs_Artic[i+2].xDrive = thighs_xDrive[i+2]; 
        }
        else if (Input.GetKey(KeyCode.F))
        {
            int i=0; 
            legs_xDrive[i] = legs_Artic[i].xDrive;
            legs_xDrive[i+2] = legs_Artic[i+2].xDrive;
            legs_xDrive[i].target  -= 1f; 
            legs_xDrive[i+2].target  -= 1f;       
            legs_Artic[i].xDrive = legs_xDrive[i];
            legs_Artic[i+2].xDrive = legs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.V))
        {
            int i=0;  
            legs_xDrive[i] = legs_Artic[i].xDrive;
            legs_xDrive[i+2] = legs_Artic[i+2].xDrive;                    
            legs_xDrive[i].target  += 1f; 
            legs_xDrive[i+2].target  += 1f;       
            legs_Artic[i].xDrive = legs_xDrive[i];
            legs_Artic[i+2].xDrive = legs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.G))
        {
            int i=1; 
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            thighs_xDrive[i+2] = thighs_Artic[i+2].xDrive;                
            thighs_xDrive[i].target -= 1f;     
            thighs_xDrive[i+2].target -= 1f;       
            thighs_Artic[i].xDrive = thighs_xDrive[i];   
            thighs_Artic[i+2].xDrive = thighs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.B))
        {
            int i=1;  
            thighs_xDrive[i] = thighs_Artic[i].xDrive;
            thighs_xDrive[i+2] = thighs_Artic[i+2].xDrive;                        
            thighs_xDrive[i].target += 1f;     
            thighs_xDrive[i+2].target += 1f;       
            thighs_Artic[i].xDrive = thighs_xDrive[i];   
            thighs_Artic[i+2].xDrive = thighs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.H))
        {
            int i=1; 
            legs_xDrive[i] = legs_Artic[i].xDrive;
            legs_xDrive[i+2] = legs_Artic[i+2].xDrive;            
            legs_xDrive[i].target  -= 1f; 
            legs_xDrive[i+2].target  -= 1f;       
            legs_Artic[i].xDrive = legs_xDrive[i];
            legs_Artic[i+2].xDrive = legs_xDrive[i+2];
        }
        else if (Input.GetKey(KeyCode.N))
        {
            int i=1;    
            legs_xDrive[i] = legs_Artic[i].xDrive;
            legs_xDrive[i+2] = legs_Artic[i+2].xDrive;                      
            legs_xDrive[i].target  += 1f; 
            legs_xDrive[i+2].target  += 1f;       
            legs_Artic[i].xDrive = legs_xDrive[i];
            legs_Artic[i+2].xDrive = legs_xDrive[i+2];
        }

    }
}
