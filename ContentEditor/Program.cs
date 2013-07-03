using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;


using ContentLibrary;

namespace ContentEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EnemyInfo> enemies = new List<EnemyInfo>();

            EnemyInfo enemy = new EnemyInfo("BirdMan", 5, 90, 180, 2.0f);

            enemies.Add(enemy);

            Serialize(enemies, "level2.xml");
            
        }

        static void Serialize(List<EnemyInfo> entity, string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                IntermediateSerializer.Serialize<List<EnemyInfo>>(writer, entity, null);
            }
        }

        static List<EnemyInfo> Deserialize(string fileName)
        {
            List<EnemyInfo> data = null;

            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    data = IntermediateSerializer.Deserialize<List<EnemyInfo>>(reader, null);
                }
            }

            return data;
        }
    }
}
