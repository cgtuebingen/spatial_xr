using System;
using System.Collections.Generic;
using UnityEngine;
//this is a simple tile cache so we don't always request something we already got  
public class TileCache
{
    private int TABLE_SIZE = 100;
    private List<TileCacheEntry>[] table;

    public TileCache()
    {
        table = new List<TileCacheEntry>[TABLE_SIZE];
        for (int i = 0; i < TABLE_SIZE; i++)
        {
            table[i] = new List<TileCacheEntry>();
        }
    }
    //try to find the coordinates in the cache
    public Color[] Get(int x, int y, int z)
    {
        List<TileCacheEntry> row = table[hashCode(x, y, z)];
        foreach(TileCacheEntry entry in row)
        {
            if (entry.x == x && entry.y == y && entry.z == z)
            {
                //Debug.Log("hit");
                return entry.value;
            }
        }
        //Debug.Log("miss");
        return null;
    }
    //add something to the cache
    public void Insert(int x, int y, int z, Color[] tile)
    {
        TileCacheEntry entry = new TileCacheEntry(x, y, z, tile);
        table[entry.hashCode(TABLE_SIZE)].Add(entry);
    }
    
    private int hashCode(int x,int y, int z)
    {
        int prime1 = 73856093;
        int prime2 = 19349663;
        int prime3 = 83492791;
        return Math.Abs((x * prime1 + y * prime2 + z * prime3) % TABLE_SIZE);
    }
}
