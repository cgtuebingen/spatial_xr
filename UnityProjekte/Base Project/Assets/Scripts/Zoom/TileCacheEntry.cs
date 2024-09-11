using System;
using UnityEngine;
//this is an entry into the Tile Cache
public class TileCacheEntry
{
    public int x;
    public int y;
    public int z;
    public Color[] value;

    public TileCacheEntry(int x, int y, int z, Color[] value)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.value = value;
    }
    //is this a good hash function? Does it matter?
    public int hashCode(int tableSize)
    {
        int prime1 = 73856093;
        int prime2 = 19349663;
        int prime3 = 83492791;
        return Math.Abs((x * prime1 + y * prime2 + z * prime3) % tableSize);
    }
}
