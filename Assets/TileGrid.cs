using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoDriveSimulator
{
    public class TileGrid : MonoBehaviour
    {
        public int Rows = 10;
        public int Cols = 10;
        public GameObject tilePrefab;
        public Color obsticleColor = new Color(0, 0, 0);
        public Color initialColor = new Color(1, 1,1);
        public Color destinationColor = new Color(1f, 0.2f, 0.2f);
        public Vector2[] Obsticles;
        public Vector2 initialPosition;
        public Vector2 destination;


        private Dictionary<string, Tile> tileDic=new Dictionary<string, Tile>();

       
        void Start()
        {
           
            DrawGrid();
            AeraSet();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 画格子
        /// </summary>
        private void DrawGrid()
        {
            
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    
                    Tile tile = new Tile(this, row, col, TileType.Normal);
                    tile.InitTile(transform, tilePrefab);
                    tileDic.Add(tile.Name, tile);
                }
            }

        }


        /// <summary>
        /// 设置贴块类型和颜色等属性
        /// </summary>
        private void AeraSet()
        {
            foreach (var item in Obsticles)
            {
                string tileName = $"Tile({item.x},{item.y})";
                print(tileName);
                tileDic[tileName].TileType = TileType.Obsticle;
            }

            string initialName = $"Tile({initialPosition.x},{initialPosition.y})";
            tileDic[initialName].TileType = TileType.Initial;

            string desName = $"Tile({destination.x},{destination.y})";
            tileDic[desName].TileType = TileType.Destination;

            foreach (var item in tileDic)
            {
                Tile tile = item.Value;
                switch (tile.TileType)
                {
                    case TileType.Normal:
                        tile.SetColor(new Color(0.15f, 0.26f, 0.61f));
                        break;
                    case TileType.Obsticle:
                        tile.SetColor(Color.black);
                        tile.isClosed = true;
                        break;
                    case TileType.Initial:
                        tile.SetColor(initialColor);
                        break;
                    case TileType.Destination:
                        tile.SetColor(destinationColor);
                        break;

                }
            }
        }
    }
}

