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



    private bool micConnected = false;  
  
    //The maximum and minimum available recording frequencies  
    private int minFreq;  
    private int maxFreq;  
  
    
   


    public AudioSource netAudioSource;

    public PlayerScript playerScript;

    //A handle to the attached AudioSource  
    public AudioSource goAudioSource;  

   
  
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
            
            //assignAuthorityObj.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient); 
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
                    goAudioSource.Play(); //Playback the recorded audio  
                    
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



}
