using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct Cube{
    public Vector3 position;
    public Color color;
}


public class ComputeShaderTestv3 : MonoBehaviour{
    public ComputeShader computeShader; // manually provide the compute shader 
    public RenderTexture renderTexture;
    // Start is called before the first frame update

    public Mesh mesh;
    public Material material;
    public int count = 50;
    public int repititions = 1;

    private List<GameObject> objects;
    private Cube[] data;


    public void CreateCubes()
    {
        objects = new List<GameObject>();
        data = new Cube[count * count];
        for (int x=0 ; x<count; x++){
            for (int y=0; y<count; y++){
                CreateCube(x,y);
            }
        }
    }


    private void CreateCube(int x, int y){
        GameObject cube = new GameObject("Cube_"+x*count+y, typeof(MeshFilter), typeof(MeshRenderer));
        cube.GetComponent<MeshFilter>().mesh = mesh;
        cube.GetComponent<MeshRenderer>().material = new Material(material);
        cube.transform.position = new Vector3(x,y, Random.Range(-0.1f, 0.1f));
        Color color = Random.ColorHSV();
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", color);


        objects.Add(cube);

        Cube cubeData = new Cube();
        cubeData.position = cube.transform.position;
        cubeData.color = color;
        data[x*count + y] = cubeData;
    }
    

    public void OnRandomizedGPU(){
        int colorSize = sizeof(float) * 4;
        int vector3size = sizeof(float) * 3;
        int totalsize = colorSize * vector3size;
        ComputeBuffer cubesBuffer = new ComputeBuffer(data.Length, totalsize);
        cubesBuffer.SetData(data);

        computeShader.SetBuffer(0, "cubes", cubesBuffer);
        computeShader.SetFloat("resolution", data.Length);
        computeShader.Dispatch(0, data.Length / 10, 1, 1);

        for (int i=0; i<objects.Count; i++){
            GameObject obj = objects[i];
            Cube cube = data[i];
            obj.transform.position = cube.position;
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", cube.color);
        }
    }

    private void OnGUI(){
        if(objects == null){
            if(GUI.Button(new Rect(0,0,100,50), "Create")){
                CreateCubes();
            }
        }
        else{
            if (GUI.Button(new Rect(0,0,100,50),"Random GPU")){
                OnRandomizedGPU();
            } 

        }
    }

    //private void OnRenderImage(RenderTexture src, RenderTexture dest){
        /* Will render the texture directly on top of our camera viewport
        Args:
            RenderTexture src: 
            RenderTexture dest:
        */


/*         if (renderTexture == null){
            renderTexture = new RenderTexture(256, 256, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }

        //computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetFloat("Resolution", renderTexture.width);
        computeShader.Dispatch(0, renderTexture.width/8, renderTexture.height/8 , 1);

        Graphics.Blit(renderTexture, dest); */
    //}

    void Start(){

    }

    // Update is called once per frame
    void Update(){

    }
}
