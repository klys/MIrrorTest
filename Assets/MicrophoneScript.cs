using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;



public struct noiseChunk {
    public float part;
    public bool end;
}


[RequireComponent (typeof (AudioSource))]
public class MicrophoneScript : NetworkBehaviour
{

    public readonly SyncList<noiseChunk> othersVoice = new SyncList<noiseChunk>();

    private bool micConnected = false;  
  
    //The maximum and minimum available recording frequencies  
    private int minFreq;  
    private int maxFreq;  
  
    
    //[SyncVar(hook = nameof(OnStatusFloatChanged))]
    //public string statusAudio;


    public AudioSource netAudioSource;

    public PlayerScript playerScript;

    //A handle to the attached AudioSource  
    public AudioSource goAudioSource;  

    // void OnStatusFloatChanged(string _Old, string _New) {
    //     Debug.Log("OnStatusFloatChanged executed!!!");
    //     string[] subs = statusAudio.Split(",");
    //     float[] rebuilded = new float[subs.Length];
    //     for(var i = 0; i < subs.Length; i++) {
    //         rebuilded[i] = float.Parse(subs[i],CultureInfo.InvariantCulture.NumberFormat); 
    //     }
        
    //     AudioSource NoisePart = GetComponent<AudioSource>();
    //     AudioClip AC_SecondClip = AudioClipCreateEmpty ("Second Clip",rebuilded.Length);

    //     AC_SecondClip.SetData (rebuilded, 0);
                    
    //     //Play it
    //     NoisePart.clip = AC_SecondClip;
    //     NoisePart.Play ();
    // }
  
    //Use this for initialization  
    void Start()   
    {  
        //Check if there is at least one microphone connected  
        if(Microphone.devices.Length <= 0)  
        {  
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");  
        }  
        else //At least one microphone is present  
        {  
            //Set 'micConnected' to true  
            micConnected = true;  
  
            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);  
  
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if(minFreq == 0 && maxFreq == 0)  
            {  
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;  
            }  
  
            //Get the attached AudioSource component  
            goAudioSource = this.GetComponent<AudioSource>();
            playerScript = GameObject.FindObjectOfType<PlayerScript>();  
        }  
    }  

    // public static float[] GetClipData(AudioClip _clip)
	// {
	// 	//Get data
	// 	float[] floatData = new float[_clip.samples * _clip.channels];
	// 	_clip.GetData(floatData,0);			
		
    //     return floatData;
	// 	//convert to byte array
	// 	// byte[] byteData = new byte[floatData.Length * 4];
	// 	// Buffer.BlockCopy(floatData, 0, byteData, 0, byteData.Length);
		
	// 	// return(byteData);
	// }	

    // public static byte[] Compress(byte[] bytes)
    //     {
    //         using (var memoryStream = new MemoryStream())
    //         {
    //             using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
    //             {
    //                 gzipStream.Write(bytes, 0, bytes.Length);
    //             }
    //             return memoryStream.ToArray();
    //         }
    //     }

    // public static byte[] Decompress(byte[] bytes)
    //     {
    //         using (var memoryStream = new MemoryStream(bytes))
    //         {

    //             using (var outputStream = new MemoryStream())
    //             {
    //                 using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
    //                 {
    //                     decompressStream.CopyTo(outputStream);
    //                 }
    //                 return outputStream.ToArray();
    //             }
    //         }
    //     }

     public static AudioClip AudioClipCreateEmpty(string ClipName, int Length) {
        AudioClip AudioClipToReturn = AudioClip.Create (ClipName, Length, 1, 44100,false);
        return AudioClipToReturn;
    }
  
    void OnGUI()   
    {  
        //If there is a microphone  
        if(micConnected)  
        {  
            //If the audio from any microphone isn't being captured  
            if(!Microphone.IsRecording(null))  
            {  
                //Case the 'Record' button gets pressed  
                if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Record"))  
                {  
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                    goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);  
                }  
            }  
            else //Recording is in progress  
            {  
                //Case the 'Stop and Play' button gets pressed  
                if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Stop and Play!"))  
                {  
                    Microphone.End(null); //Stop the audio recording  
                    //goAudioSource.Play(); //Playback the recorded audio  
                    // byte[] result = GetClipData(goAudioSource.clip);
                    //Debug.Log("Audio Data "+GetClipData(goAudioSource.clip).Length);
                    if (playerScript == null) playerScript = GameObject.FindObjectOfType<PlayerScript>();
                    playerScript.CmdSendPlayerVoice();
                    
                    //////
                    /// what is suppoused to be on the above line
                    // float[] beforeSend = new float[goAudioSource.clip.samples];
                    // goAudioSource.clip.GetData(beforeSend,0);
                    // foreach(var el in beforeSend) {
                    //     othersVoice.Add(new noiseChunk {
                    //         part = el,
                    //         end = false
                    //     });
                    // }
                    // othersVoice.Add(new noiseChunk {
                    //         part = 0.0f,
                    //         end = true
                    //     });
                    //// NEWER VERSION END WITHOUT STRINGS ///
                    //statusAudio = string.Join(",",beforeSend);
                    //Debug.Log("statusAudio"+ statusAudio.Length);
                    /// end of above process
                    //////

                    // AudioSource NoisePart = GetComponent<AudioSource>();

                    // float[] samples = new float[goAudioSource.clip.samples];
                    // AudioClip AC_SecondClip = AudioClipCreateEmpty ("Second Clip",goAudioSource.clip.samples);
                    
                    // //Use GetData and SetData
                    // goAudioSource.clip.GetData(samples,0);
                    // AC_SecondClip.SetData (samples, 0);
                    
                    // //Play it
                    // NoisePart.clip = AC_SecondClip;
                    // NoisePart.Play ();
                }  
  
                GUI.Label(new Rect(Screen.width/2-100, Screen.height/2+25, 200, 50), "Recording in progress...");  
            }  
        }  
        else // No microphone  
        {  
            //Print a red "Microphone not connected!" message at the center of the screen  
            GUI.contentColor = Color.red;  
            GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Microphone not connected!");  
        }  
  
    }

    // void Update() {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         print("space key was pressed");
    //     }
    // } 

    public void ButtonSendVoice() {
        if (playerScript != null)  
                playerScript.CmdSendPlayerVoice();
    } 

    public override void OnStartClient()
    {
        othersVoice.Callback += OnVoiceUpdated;
        
        // Process initial SyncList payload
        for (int index = 0; index < othersVoice.Count; index++)
            OnVoiceUpdated(SyncList<noiseChunk>.Operation.OP_ADD, index, new noiseChunk(), othersVoice[index]);
    }

    void OnVoiceUpdated(SyncList<noiseChunk>.Operation op, int index, noiseChunk oldItem, noiseChunk newItem)
    {
        switch (op)
        {
            case SyncList<noiseChunk>.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item
                if (newItem.end == true) {
                    Debug.Log("reached the end of the chunk!!!");
                    
                    float[] rebuilded = new float[othersVoice.Count];
                    for(var i = 0; i < othersVoice.Count; i++) {
                        if (othersVoice[i].end == false) rebuilded[i] = othersVoice[i].part;
                    }
                    
                    AudioSource NoisePart = GetComponent<AudioSource>();
                    AudioClip AC_SecondClip = AudioClipCreateEmpty ("Second Clip",rebuilded.Length);

                    AC_SecondClip.SetData (rebuilded, 0);
                                
                    //Play it
                    NoisePart.clip = AC_SecondClip;
                    NoisePart.Play ();
                    othersVoice.Clear();
                }
                break;
            case SyncList<noiseChunk>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncList<noiseChunk>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                break;
            case SyncList<noiseChunk>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                break;
            case SyncList<noiseChunk>.Operation.OP_CLEAR:
                // list got cleared
                break;
        }
    }
}
