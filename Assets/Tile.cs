using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoDriveSimulator
{
   public enum TileType
    {
        Normal,
        Obsticle,
        Destination,
        Initial,
    }


    public class Tile
    {
        public TileGrid Grid { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Vector2 Pos { get; private set; }

        public bool isStepped { get;  set; }

        public bool isMarked { get; set; }

       public string Name { get; private set; }
        public TileType TileType { get; set; }

        private GameObject tileObj;

        public Tile(TileGrid grid, int row, int col,TileType tileType)
        {
            Grid = grid;
            Row = row;
            Col = col;
            TileType = tileType;
            Pos = new Vector2(Row, Col);
            Name =$"Tile({Row},{Col})";
            isStepped= false;
            isMarked = false;

        }

        public void InitTile(Transform parent, GameObject prefab)
        {
            tileObj = GameObject.Instantiate(prefab);
            tileObj.name = $"Tile({Row},{Col})";
            tileObj.transform.parent = parent;
            tileObj.transform.position = new Vector3(Col, -Row, 0);
        }

        public void SetColor(Color color)
        {
            tileObj.GetComponent<SpriteRenderer>().color = color;
        }
        
    }
}

