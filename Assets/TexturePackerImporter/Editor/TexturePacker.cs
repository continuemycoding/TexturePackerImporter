using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TexturePackerImporter
{
    public class Sprite
    {
        [XmlAttribute("n")]
        public string name;

        [XmlAttribute]
        public int x;
        [XmlAttribute]
        public int y;
        [XmlAttribute("w")]
        public int width;
        [XmlAttribute("h")]
        public int height;

        //pX => x pos of the pivot point (relative to sprite width)
        //pY => y pos of the pivot point (relative to sprite height)

        [XmlAttribute("oX")]
        public int xOffset;
        [XmlAttribute("oY")]
        public int yOffset;

        [XmlAttribute("oW")]
        public int originalWidth;
        [XmlAttribute("oH")]
        public int originalHeight;

        [XmlAttribute("r")]
        public string rotated;

        public override string ToString()
        {
            return name;
        }
    }

    public class TextureAtlas
    {
        [XmlAttribute]
        public string imagePath;
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int height;

        [XmlElement("sprite")]
        public List<Sprite> sprites;
    }
}
