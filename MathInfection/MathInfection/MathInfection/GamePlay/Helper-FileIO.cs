<<<<<<< HEAD
﻿using System.IO;
using System.Xml.Serialization;

namespace MathInfection
{
    public static class FileIO
    {
        static public void SerializeToXML(GameData gameData)
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = @"\GameData.xml";
            if(!File.Exists(path+fileName))
            {
                FileStream fs = File.Create(path + fileName);
                fs.Close();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            TextWriter textWriter = new StreamWriter(path + fileName);
            serializer.Serialize(textWriter, gameData);
            textWriter.Close();
        }

        static public GameData DeserializeFromXML()
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = @"\GameData.xml";
            if(!File.Exists(path+fileName))
            {
                return null;
            }
            FileInfo fileInfo = new FileInfo(path + fileName);
            if (fileInfo.Length == 0)
            {
                return null;
            }

            XmlSerializer deserializer =
                                 new XmlSerializer(typeof(GameData));
            TextReader textReader = new StreamReader(path + fileName);
            GameData gameData = 
                      (GameData)deserializer.Deserialize(textReader);
            textReader.Close();
            return gameData;
        }
    }
}
=======
﻿using System.IO;
using System.Xml.Serialization;

namespace MathInfection
{
    public static class FileIO
    {
        static public void SerializeToXML(GameData gameData)
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = @"\GameData.xml";
            if(!File.Exists(path+fileName))
            {
                FileStream fs = File.Create(path + fileName);
                fs.Close();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            TextWriter textWriter = new StreamWriter(path + fileName);
            serializer.Serialize(textWriter, gameData);
            textWriter.Close();
        }

        static public GameData DeserializeFromXML()
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = @"\GameData.xml";
            if(!File.Exists(path+fileName))
            {
                return null;
            }
            FileInfo fileInfo = new FileInfo(path + fileName);
            if (fileInfo.Length == 0)
            {
                return null;
            }

            XmlSerializer deserializer =
                                 new XmlSerializer(typeof(GameData));
            TextReader textReader = new StreamReader(path + fileName);
            GameData gameData = 
                      (GameData)deserializer.Deserialize(textReader);
            textReader.Close();
            return gameData;
        }
    }
}
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703
