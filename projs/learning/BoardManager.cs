using System.Collections;
// system.collections.generic is added such that we are able to use
// lists . . .
using System.Collections.Generic;
// "using System;" such that we will be able to use the serializable attribute
// It allows us to modify variables and how they appear in the inspector and the
// editor, and to show and hide them using fold out. 
using System;
using UnityEngine;

// We do it this way, because it is called Random in both the 
// system and unityEngine namespaces, which could cause conflicts. 
using Random = UnityEngine.Random;


public class BoardManager: MonoBehaviour {
    // this adds a serializable public class called count. 
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    public int columns = 8;
    public int rows = 8;

    // What this does is to create an object called wallCount
    // which in turn accesses the class Count . . . where we 
    // enter the method name with given parameters. . .
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    
    
    // only one exit, because there is only one exit per level. 
    public GameObject exit;


    // we will choose the contents of these arrays between the prefabs, from the inspector!
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;


    // will be used such that the hierarchy will be clean, as we are spawning many game objects. 
    // gonna child them all to boardholder. 
    private Transform BoardHolder;

    // we gotta study this object declaration. Tracking all of the possible positions on the gameboard, whether
    // an object has been spawned in that position or not. 
    private List<Vector3> gridPositions = new List<Vector3>();


    // is a function that will return void
    void InitializeList()
    {
        // starts by clearing teh gridPositions, by calling the method Clear associated with
        // list<Vector3> . . . 
        gridPositions.Clear();

        // nested for loop such that we will fill the gridpositions with random objects.
        // The reason we start with 1 to columns or rows -1 is such that a border of empty floor tiles is 
        // left. 
        for (int x=1; x<columns-1; x++)
        {
            for(int y=1; y<rows-1; y++)
            {
                // adding a new vector object to the gridposition. 
                // we are creating a list of possible positions to place walls, enemies and items. 
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

    }


    // sets up the floors and the outerwalls
    void BoardSetup()
    {
        // adds a gameObject called Board to the BoardHolder hierarchy? . . .
        // alternatively, it is making the BoardHolder a gameObject "Board"'s transform . . .
        BoardHolder = new GameObject("Board").transform;


        // reason from -1 to +1 is such that we create an edge 
        for(int x = -1; x<columns+1; x++)
        {
            for(int y = -1; y<rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x ==-1 || x == columns || y==-1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    // we crete an gameobject instance, by calling instantiate which accepts the prefab we chose
                    // i.e. "toInstantiate" in addition to the position of the given prefab, which will be our 
                    // position in the loop, which is x,y 
                    // Quaternion.identity means it is instatiated with no rotation.
                    // as GameObject, means we are casting the instantiation as a GameObject. 

                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(BoardHolder);
            }
        }

    }

    Vector3 RandomPosition()
    {
        // note that gridpositions is of type List<Vector3>
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        // such that we will not spawn to objects into the same position. 
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }


    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // how many of given object we are spawning
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i=0; i<objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    

    // note that this is the single public function of this class, as it will be caleld by the game manager 
    // when the board is being setup
    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        // min max is the same here, cuz no random range
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        // the exit item will always be at the same position.
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
