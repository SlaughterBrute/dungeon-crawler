using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private int numberOfInstansiatedNeighbouringChunks;
    [SerializeField] private Transform playerTransform;
    [SerializeField] public static Vector2 playerPosition;
    private MapGenerator mapGen;
    private int chunkSize;
    
    Dictionary<Vector2Int, Chunk> instansiatedChunksDict = new Dictionary<Vector2Int, Chunk>();

    private void Start()
    {
        playerPosition = playerTransform.position;

         mapGen = FindObjectOfType<MapGenerator>();
        if(mapGen == null)
        {
            Debug.LogError("ChunkManager need MapGenerator to function. MapGenerator not found");
            Application.Quit(-1);
        }
        else
        {
            chunkSize = mapGen.getChunkSize();
        }
       
    }

    private void Update()
    {
        playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
        UpdateVissibleChunks();
    }

    private void UpdateVissibleChunks()
    {
        //calculate chunk coordinates of current chunk
        Vector2Int currentChunkCoord = new Vector2Int(Mathf.RoundToInt(playerPosition.x / chunkSize), Mathf.RoundToInt(playerPosition.y / chunkSize));

        //Instanciate chunks if they haven't been instanciated already
        for (int chunkCoordOffsetX = -numberOfInstansiatedNeighbouringChunks; chunkCoordOffsetX <= numberOfInstansiatedNeighbouringChunks; chunkCoordOffsetX++)
        {
            for (int chunkCoordOffsetY = -numberOfInstansiatedNeighbouringChunks; chunkCoordOffsetY <= numberOfInstansiatedNeighbouringChunks; chunkCoordOffsetY++)
            {
                Vector2Int viewedChunkCoord = new Vector2Int(currentChunkCoord.x + chunkCoordOffsetX, currentChunkCoord.y + chunkCoordOffsetY);

                if (instansiatedChunksDict.ContainsKey(viewedChunkCoord))
                {
                    //if chunk weren't loaded in previous update, load chunk and it's items/chest, monsters etc
                    instansiatedChunksDict[viewedChunkCoord].UpdateChunk();
                }
                else
                {
                    //instansiate new chunk
                    instansiatedChunksDict.Add(viewedChunkCoord, new Chunk(viewedChunkCoord, this));
                }
                
            }
        }


    }

    public class Chunk
    {
        [SerializeField] private Vector2 coord;
        [SerializeField] private Vector2 position;
        [SerializeField] private int size;
        [SerializeField] private Bounds bound;
        [SerializeField] private Grid grid;
        //[SerializeField] private Tilemap tilemapWall;
        //[SerializeField] private Tilemap tilemapFloorShadow;
        //[SerializeField] private Tilemap tilemapFloor;
        [SerializeField] private Tilemap[] tilemaps = new Tilemap[3];//0: floor, 1: wall, 2: shadow
        private ChunkManager chunkManager;
        private int numberOfInstansiatedNeighbouringChunks;

        public Chunk(Vector2 coord, ChunkManager chunkManager)
        {
            this.chunkManager = chunkManager;
            this.size = chunkManager.chunkSize;
            this.coord = coord;
            this.position = new Vector2(coord.x * size, coord.y * size);
            this.bound = new Bounds(position, Vector2.one * size);
            this.numberOfInstansiatedNeighbouringChunks = chunkManager.numberOfInstansiatedNeighbouringChunks;

            this.grid = CreateGrid();
            this.tilemaps[0] = CreateTilemap("Floor", 0, grid);
            this.tilemaps[1] = CreateTilemap("Wall", 1, grid);
            this.tilemaps[2] = CreateTilemap("FloorShadow", 1, grid);

            //wall setup
            Rigidbody2D rb2d = this.tilemaps[1].transform.gameObject.AddComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Static;
            CompositeCollider2D cc2d = this.tilemaps[1].transform.gameObject.AddComponent<CompositeCollider2D>();
            cc2d.generationType = CompositeCollider2D.GenerationType.Manual;
            TilemapCollider2D tmc2d = this.tilemaps[1].transform.gameObject.AddComponent<TilemapCollider2D>();
            tmc2d.usedByComposite = true;

            PopulateTilemaps();

            grid.gameObject.SetActive(false);
        }

        private void PopulateTilemaps()
        {
            this.chunkManager.mapGen.PopulateTilemapWithTiles(tilemaps, coord);
        }

        public Tilemap CreateTilemap(string tilemapName, int sortingOrder, Grid gridParrent)
        {
            GameObject tileObject = new GameObject(tilemapName);
            Tilemap tm = tileObject.AddComponent<Tilemap>();
            TilemapRenderer tr = tileObject.AddComponent<TilemapRenderer>();
            tr.sortingLayerName = "Default";
            tr.sortingOrder = sortingOrder;

            tm.tileAnchor = new Vector3(0, 0, 0);
            tileObject.transform.SetParent(gridParrent.transform);
            tileObject.transform.localPosition = new Vector3(0, 0, 0);
            return tm;
        }
        public Grid CreateGrid()
        {
            GameObject gridObject = new GameObject("SideGrid");
            Grid gr = gridObject.AddComponent<Grid>();
            gr.transform.position = new Vector3(position.x, position.y, 0);

            return gr;
        }

        private IEnumerator updateChunkState()
        {
            while (true)
            {
                float distanceToChunk = Mathf.Sqrt(bound.SqrDistance(playerPosition));
                if (distanceToChunk > (size * numberOfInstansiatedNeighbouringChunks))
                {
                    //grid should be destroyed or set invisible
                    Debug.Log("Set unvissible");
                    grid.gameObject.SetActive(false);
                    yield break;
                }
                else
                {
                    grid.gameObject.SetActive(true);
                    Debug.Log("Everythging is guuchi");
                    yield return new WaitForSeconds(30);
                }
            }
        }

        public void UpdateChunk()
        {
            if (grid.isActiveAndEnabled == false)
            {
                chunkManager.StartCoroutine(updateChunkState());
            }
        }
    }
}
