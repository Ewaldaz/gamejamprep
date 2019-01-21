using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Core
{
    class MapGenerator : MonoBehaviour
    {
        public static int GenerateMap(GameObject[] tiles, int mapWidth, int mapHeight, int rectangleCount, float tileSize, GameObject[] coins, AudioSource coinCollectorSound)
        {
            var temp = new List<MapTile>();
            int collectibles = 0;

            // init map
            for (int r = 0; r < rectangleCount; r++)
            {
                int rectWidth = Random.Range(7, mapWidth / 2 / rectangleCount);
                int rectHeight = Random.Range(7, mapHeight / 2 / rectangleCount);
                int offsetX = r == 0 ? 0 : Random.Range(-mapWidth / 2, mapWidth / 2);
                int offsetZ = r == 0 ? 0 : Random.Range(-mapHeight / 2, mapHeight / 2);

                for (int i = 0; i < rectWidth; i++)
                {
                    for (int j = 0; j < rectHeight; j++)
                    {
                        int x = i - rectWidth / 2 + offsetX;
                        int z = j - rectHeight / 2 + offsetZ;

                        Instantiate(GenerateTile(tiles), new Vector3(x, 0, z), Quaternion.identity);

                        bool occupied = GenerateRocks();
                        temp.Add(new MapTile() { x = x, z = z, occupied = occupied, center = i == rectWidth / 2 && j == rectHeight / 2, hasTerrain = true });

                        collectibles += GenerateCollectible(x, z, coins, coinCollectorSound);
                    }
                }
            }

            // corridors
            var centers = temp.Where(x => x.center).ToArray();
            var fromX = 0;
            var fromZ = 0;
            var toX = 0;
            var toZ = 0;
            for (int i = 0; i < centers.Length; i++)
            {
                fromX = centers[i].x;
                fromZ = centers[i].z;
                toX = centers[i + 1 == centers.Length ? 0 : i + 1].x;
                toZ = centers[i + 1 == centers.Length ? 0 : i + 1].z;

                GenerateCoridor(ref temp, fromX, fromZ, toX, toZ, tiles);
            }

            return collectibles;
        }

        private static int GenerateCollectible(int x, int z, GameObject[] coins, AudioSource coinCollectorSound)
        {
            if (Random.Range(0, 100) > 95 && x != 0 && z != 0)
            {
                var coin = coins[Random.Range(0, coins.Length)];
                var go = Instantiate(coin, new Vector3(x, 0, z), Quaternion.identity);
                go.GetComponent<CoinCollector>().CoinCollectedSound = coinCollectorSound;
                return 1;
            }

            return 0;
        }

        private static void GenerateCoridor(ref List<MapTile> map, int fromX, int fromZ, int toX, int toZ, GameObject[] tiles)
        {
            bool down = fromX < toX;
            bool right = fromZ < toZ;

            for (int x = 1; x <= Math.Abs(fromX - toX)+1; x++)
            {
                int newX = fromX + (down ? x : -x);
                AddTileIfEmpty(ref map, newX, fromZ-1, tiles);
                AddTileIfEmpty(ref map, newX, fromZ, tiles);
                AddTileIfEmpty(ref map, newX, fromZ+1, tiles);
            }

            for (int z = 1; z <= Math.Abs(fromZ - toZ); z++)
            {
                int newZ = fromZ + (right ? z : -z);
                AddTileIfEmpty(ref map, toX-1, newZ, tiles);
                AddTileIfEmpty(ref map, toX, newZ, tiles);
                AddTileIfEmpty(ref map, toX+1, newZ, tiles);
            }
        }

        private static void AddTileIfEmpty(ref List<MapTile> map, int x, int z, GameObject[] tiles)
        {
            var tile = map.Where(t => t.x == x && t.z == z).FirstOrDefault();
            if (tile == null || !tile.hasTerrain)
            {
                Instantiate(GenerateTile(tiles), new Vector3(x, 0, z), Quaternion.identity);

                bool occupied = GenerateRocks();
                map.Add(new MapTile() { x = x, z = z, occupied = occupied, center = false, hasTerrain = true });
            }
        }

        private static GameObject GenerateTile(GameObject[] tiles)
        {
            return tiles[Random.Range(0, tiles.Length)];
        }

        private static bool GenerateRocks()
        {
            return Random.Range(0, 1) == 1;
        }
    }
}