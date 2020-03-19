﻿using System.Collections;
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
        public Color pathColor = new Color(0.5f, 0.5f, 0.5f);
        public Color steppedColor = new Color(0.6f, 0.6f, 0.6f);
        public Vector2[] Obsticles;
        public Vector2 initialPosition;
        public Vector2 destination;

        private static int tileNum;


        public static Dictionary<string, Tile> tileDic=new Dictionary<string, Tile>();
        private static Color _pathColor;
        public static Color _steppedColor;

       
        void Start()
        {
            InitialData();
            tileNum = Rows * Cols;
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
            
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
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
                        //tile.isStepped= true;
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

        public static void SetTilePathColor(Vector2 vector)
        {
            string key = $"Tile({vector.x},{vector.y})";
            Tile tile = tileDic[key];
            tile.SetColor(_pathColor);

        }

        private void InitialData()
        {
            _pathColor = pathColor;
            _steppedColor = steppedColor;
        }

        public static void SetTileClosed(int row,int col)
        {
            if (tileDic.Count == tileNum)
            {
                string key = $"Tile({row},{col})";
                tileDic[key].isStepped = true;
                tileDic[key].SetColor(Color.gray);
            }
        }


    }
}

