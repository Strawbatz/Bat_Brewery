using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Sibling Rule Tile", menuName = "2D/Tiles/SiblingGroupTile")]
public class SiblingGroupTile : RuleTile<SiblingGroupTile.Neighbor> {
    
    public string siblingGroup;
    public string[] cousinGroups = new string[0];
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Sibing = 3;
        public const int Cousin = 4;
    }
    public override bool RuleMatch(int neighbor, TileBase tile) {
        SiblingGroupTile myTile = tile as SiblingGroupTile;
        switch (neighbor) {
            case Neighbor.Sibing: return myTile && myTile.siblingGroup.Equals(siblingGroup);
            case Neighbor.Cousin: return myTile && (cousinGroups.Contains(myTile.siblingGroup) || myTile.siblingGroup.Equals(siblingGroup));
        }
        return base.RuleMatch(neighbor, tile);
    }
}