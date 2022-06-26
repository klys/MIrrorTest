using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Mirror;



public class PlayerScript : NetworkBehaviour
{

    public readonly SyncList<noiseChunk> othersVoice = new SyncList<noiseChunk>();

    


    

    
    //Import the following.
            [DllImport("user32.dll", EntryPoint = "SetWindowText")]
            public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
            [DllImport("user32.dll", EntryPoint = "FindWindow")]
            public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

    private SceneScript sceneScript;
    

    public TextMesh playerNameText;

        public GameObject floatingInfo;

        private Material playerMaterialClone;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;

        void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = playerName;
        }

        void OnColorChanged(Color _Old, Color _New)
        {
            playerNameText.color = _New;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            GetComponent<Renderer>().material = playerMaterialClone;
        }

    
        

    void Awake()
    {
        //allow all players to run this
        sceneScript = GameObject.FindObjectOfType<SceneScript>();
        //microphone = GameObject.FindObjectOfType<MicrophoneScript>();//GameObject.Find("MicrophoneScript").GetComponent<MicrophoneScript>();//GameObject.FindObjectOfType<MicrophoneScript>();//GameObject.Find("MicrophoneController").GetComponent<MicrophoneController>();//GameObject.FindWithTag("MicrophoneObject");//GameObject.Find("MicrophoneController").GetComponent<MicrophoneController>();//GameObject.FindObjectOfType<MicrophoneController>();
    }

    [Command]
    public void CmdSendPlayerMessage()
    {
        if (sceneScript) 
            sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
    }

    

    // [Command(requiresAuthority = false)]
    // public void CmdSendPlayerVoice()
    // {
    //     Debug.Log("CmdSendPlayerVoice execution.");
        
        
        
        
    //          //float[] beforeSend = VoiceData();
    //                 var count = 0;
    //                 foreach(var el in beforeSend) {
    //                     if (count < 10) {
    //                         count++;
    //                         Debug.Log("Value:"+el);
    //                     }
    //                     othersVoice.Add(new noiseChunk {
    //                         part = el,
    //                         end = false
    //                     });
    //                 }
    //                 othersVoice.Add(new noiseChunk {
    //                         part = 0.0f,
    //                         end = true
    //                     });
                    
            
        
    //         // microphone.statusAudio = microphone.goAudioSource.clip;
    // }

    
    public override void OnStartLocalPlayer()
        {
            sceneScript.playerScript = this;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);

            floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
            floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            string name = "Player" + Random.Range(100, 999);
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdSetupPlayer(name, color);

            if (isLocalPlayer) {
            //Get the window handle.
                var windowPtr = FindWindow(null, "MirrorTest");
            //Set the title text using the window handle.
            if (isServer) {
                SetWindowText(windowPtr, "Server:"+name);
            } else {
                SetWindowText(windowPtr, "Client:"+name);
            }
                
            }
        }

        [Command]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            // player info sent to server, then server updates sync vars which handles it on all clients
            playerName = _name;
            playerColor = _col;
            sceneScript.statusText = $"{playerName} joined.";
        }

        void Update()
        {
            if (!isLocalPlayer)
            {
                // make non-local players run this
                floatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }

    
        
}
