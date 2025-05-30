using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace tilemaps
{
    [CreateAssetMenu(menuName = "2D/Tiles/Color Rule Tile", fileName = "ColorRuleTile")]
    public class ColorRuleTile : RuleTile
    {
        public enum BaseColor
        {
            Red,
            Blue,
            Yellow
        }

        public List<BaseColor> colors = new();

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is not ColorRuleTile otherRule)
            {
                return base.RuleMatch(neighbor, other);
            }

            var match = otherRule.colors.Intersect(colors).Any();
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This: return match;
                case TilingRuleOutput.Neighbor.NotThis: return !match;
            }

            return base.RuleMatch(neighbor, other);
        }
    }
}