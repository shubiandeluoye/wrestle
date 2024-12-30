using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class WallReference
    {
        public string wallId;
        public GameObject wallObject;
        public bool isBounceEnabled = true;
    }

    [Header("Wall References")]
    public List<WallReference> reboundWalls = new List<WallReference>();
    public List<WallReference> missileWalls = new List<WallReference>();
    
    [Header("Floor References")]
    public GameObject leftFloor;
    public GameObject rightFloor;
    public GameObject aisleFloor;

    private Dictionary<string, WallReference> wallLookup = new Dictionary<string, WallReference>();

    private void Awake()
    {
        // Initialize wall lookup dictionary
        foreach (var wall in reboundWalls)
        {
            wallLookup[wall.wallId] = wall;
        }
        foreach (var wall in missileWalls)
        {
            wallLookup[wall.wallId] = wall;
        }

        // Set up wall tags
        SetupWallTags();
    }

    private void SetupWallTags()
    {
        foreach (var wall in reboundWalls)
        {
            if (wall.wallObject != null)
            {
                wall.wallObject.tag = "ReboundWall";
            }
        }

        foreach (var wall in missileWalls)
        {
            if (wall.wallObject != null)
            {
                wall.wallObject.tag = "MissileWall";
            }
        }

        if (leftFloor) leftFloor.tag = "Floor";
        if (rightFloor) rightFloor.tag = "Floor";
        if (aisleFloor) aisleFloor.tag = "Floor";
    }

    public bool IsWallBounceEnabled(string wallId)
    {
        if (wallLookup.TryGetValue(wallId, out WallReference wall))
        {
            return wall.isBounceEnabled;
        }
        return false;
    }

    public void SetWallBounceEnabled(string wallId, bool enabled)
    {
        if (wallLookup.TryGetValue(wallId, out WallReference wall))
        {
            wall.isBounceEnabled = enabled;
        }
    }

    public bool IsReboundWall(GameObject wallObject)
    {
        return wallObject.CompareTag("ReboundWall");
    }

    public bool IsMissileWall(GameObject wallObject)
    {
        return wallObject.CompareTag("MissileWall");
    }

    public bool IsFloor(GameObject floorObject)
    {
        return floorObject.CompareTag("Floor");
    }

    // Helper method to get wall reference by GameObject
    public WallReference GetWallReference(GameObject wallObject)
    {
        foreach (var wall in wallLookup.Values)
        {
            if (wall.wallObject == wallObject)
            {
                return wall;
            }
        }
        return null;
    }

    // Method to validate wall setup
    public void ValidateWallSetup()
    {
        foreach (var wall in reboundWalls)
        {
            if (wall.wallObject == null)
            {
                Debug.LogWarning($"Rebound wall {wall.wallId} is not assigned!");
            }
        }

        foreach (var wall in missileWalls)
        {
            if (wall.wallObject == null)
            {
                Debug.LogWarning($"Missile wall {wall.wallId} is not assigned!");
            }
        }

        if (leftFloor == null) Debug.LogWarning("Left floor is not assigned!");
        if (rightFloor == null) Debug.LogWarning("Right floor is not assigned!");
        if (aisleFloor == null) Debug.LogWarning("Aisle floor is not assigned!");
    }
}
