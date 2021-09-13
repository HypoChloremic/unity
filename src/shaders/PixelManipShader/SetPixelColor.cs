using System;
using UnityEngine;
using System.Collections;




public class SetPixelColor : MonoBehaviour
{
    //Create a custom struct and apply [Serializable] attribute to it
    [Serializable]
    public struct Dimensions
    {
        public int height;
        public int width;
    };

    public struct position{
        public int x;
        public int y;
    };

    //Make the private field of our Dimensions struct visible in the Inspector
    //by applying [SerializeField] attribute to it
    [SerializeField]
    private Dimensions dim;

    public ComputeShader computeShader;
    public RenderTexture renderTexture;



    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.Dispatch(0,1,1,1);
        Graphics.Blit(renderTexture, dest);
    }

    private void OnGUI(){

        if(GUI.Button(new Rect(0,0,100,50), "Create")){
            computeShader.SetInt("width", dim.width);
            computeShader.SetInt("height", dim.height);
            OnRenderImage(renderTexture, renderTexture);
        }
    }

    void Start()
    {
        if (renderTexture == null){
            renderTexture = new RenderTexture(256, 256, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();               
        }

        // Initializing the position struct
        position[] pos = new position[1]; 
        ComputeBuffer positionBuffer = new ComputeBuffer(1, 2*sizeof(int));
        int kernelHandle = computeShader.FindKernel("CSMain");
        positionBuffer.SetData(pos);
        computeShader.SetBuffer(kernelHandle, "PositionBuffer", positionBuffer);
        positionBuffer.Release();

    }

    void Update(){
        computeShader.SetFloat("DeltaTime", Time.deltaTime);
        computeShader.Dispatch(0, 1, 1, 1);
    }
}