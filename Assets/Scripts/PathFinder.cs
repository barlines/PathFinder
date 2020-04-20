using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder
{
    public static TilePath DiscoverPath(Tilemap map, Vector3Int start, Vector3Int end)
    {
        //you will return this path to the user.  It should be the shortest path between
        //the start and end vertices 
        TilePath discoveredPath = new TilePath();

        //TileFactory is how you get information on tiles that exist at a particular vector's
        //coordinates
        TileFactory tileFactory = TileFactory.GetInstance();

        //This is the priority queue of paths that you will use in your implementation of
        //Dijkstra's algorithm
        PriortyQueue<TilePath> pathQueue = new PriortyQueue<TilePath>();

        //You can slightly speed up your algorithm by remembering previously visited tiles.
        //This isn't strictly necessary.
        Dictionary<Vector3Int, int> discoveredTiles = new Dictionary<Vector3Int, int>();

        //quick sanity check
        if(map == null || start == null || end == null)
        {
            return discoveredPath;
        }

        //This is how you get tile information for a particular map location
        //This gets the Unity tile, which contains a coordinate (.Position)
        var startingMapLocation = map.GetTile(start);

        //And this converts the Unity tile into an object model that tracks the
        //cost to visit the tile.
        var startingTile = tileFactory.GetTile(startingMapLocation.name);
        startingTile.Position = start;

        //Any discovered path must start at the origin!
        discoveredPath.AddTileToPath(startingTile);

        //This adds the starting tile to the PQ and we start off from there...
        pathQueue.Enqueue(discoveredPath);
        bool found = false;
        while(found == false && pathQueue.IsEmpty() == false)
        {
            
            //For each tile in discoveredPath
            foreach (Tile u in discoveredPath)
            {
                //all adjacent Tiles to the current tile U in discoverpath
                var adjacentTilePositionDown = map.GetTile(u.Position - Vector3Int.down);
                Tile adjacentTileDown = tileFactory.GetTile(adjacentTilePositionDown.name);

                var adjacentTilePositionUp = map.GetTile(u.Position - Vector3Int.up);
                Tile adjacentTileUp = tileFactory.GetTile(adjacentTilePositionUp.name);

                var adjacentTilePositionRight = map.GetTile(u.Position - Vector3Int.right);
                Tile adjacentTileRight = tileFactory.GetTile(adjacentTilePositionRight.name);

                var adjacentTilePositionLeft = map.GetTile(u.Position - Vector3Int.left);
                Tile adjacentTileLeft = tileFactory.GetTile(adjacentTilePositionLeft.name);

                //AddedTile will be the tile added to the discoverPath
                Tile addedTile = null;

                //Determining best Tile to add to path 
                if (u.Weight > adjacentTileDown.Weight)
                {
                    addedTile = adjacentTileDown;
                }
                if (u.Weight > adjacentTileUp.Weight + startingTile.Weight)
                {
                    addedTile = adjacentTileDown;
                }
                if (u.Weight > adjacentTileRight.Weight + startingTile.Weight)
                {
                    addedTile = adjacentTileRight;
                }
                if (u.Weight > adjacentTileLeft.Weight + startingTile.Weight)
                {
                    addedTile = adjacentTileLeft;
                }

                //Adding new tile to the path
                discoveredPath.AddTileToPath(addedTile);
                if(u.Position == end)
                {
                    found = true;
                }
            }
        }
        return discoveredPath;
    }
}
