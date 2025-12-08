using UnityEngine;

namespace AugmeNDT
{
    public static class ColorSchemes
    {
        public static readonly Color[] Default = { Color.cyan, Color.white, Color.magenta };
        public static readonly Color[] Warm = { Color.red, Color.yellow, Color.magenta };
        public static readonly Color[] Cool = { Color.blue, Color.cyan, Color.white };
        public static readonly Color[] Purple = {
            new Color(0.6f, 0.4f, 0.8f),
            new Color(0.8f, 0.6f, 0.9f),
            new Color(0.4f, 0.2f, 0.6f)
        };
        public static readonly Color[] Green = {
            Color.green,
            new Color(0.5f, 0.8f, 0.5f),
            new Color(0.2f, 0.5f, 0.2f)
        };
    }
}
