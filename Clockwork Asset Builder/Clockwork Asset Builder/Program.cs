using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Timers;

namespace Clockwork_Asset_Builder
{
    class Program
    {
        public static string CLKWRK = Environment.ExpandEnvironmentVariables("%CLKWRK%") + '\\';
        static Timer timer = new Timer(3000);

        static List<string> files = new List<string>();

        static XmlDocument assetXml = new XmlDocument();
        static XmlNodeList assets;

        static void Main(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            Console.WriteLine("Type 'exit', 'stop' or 'quit' to stop the application.");
            if (File.Exists(CLKWRK + "Data/assets.xml"))
            {
                assetXml.Load(CLKWRK + "Data/assets.xml");
                assets = assetXml.GetElementsByTagName("Asset");
            }
            else
            {
                return;
            }

            while (true)
            {
                string s = Console.ReadLine();
                if (s == "quit" || s == "exit" || s == "stop")
                    return;
                else if (s == "pause" || s == "pauze")
                    timer.Stop();
                else if (s == "start" || s == "continue")
                    timer.Start();
            }
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Console.WriteLine("\n\nStarting sequence at "+DateTime.Now.TimeOfDay+".\n========================================");

            Console.WriteLine("Looking for new files..");
            files.Clear();
            SearchDirectory(CLKWRK + "Effects");
            AddFiles("Effect");
            SearchDirectory(CLKWRK + "Models");
            AddFiles("Model");
            SearchDirectory(CLKWRK + "Textures");
            AddFiles("Texture");

            assetXml.Save(CLKWRK + "Data/assets.xml");
            Console.WriteLine("Running through assets.XML..");
            assets = assetXml.GetElementsByTagName("Asset");
            foreach (XmlNode asset in assets)
            {
                if (File.GetLastWriteTime(CLKWRK + asset.ChildNodes[0].InnerText) > Convert.ToDateTime(asset.ChildNodes[1].InnerText).AddSeconds(3))
                {
                    asset.ChildNodes[1].InnerText = File.GetLastWriteTime(CLKWRK + asset.ChildNodes[0].InnerText).ToString();
                }
                if (File.GetLastWriteTime(CLKWRK + asset.ChildNodes[0].InnerText) > Convert.ToDateTime(asset.ChildNodes[2].InnerText))
                {
                    string name = asset.ChildNodes[0].InnerText;
                    string[] nameParts = name.Split('.');
                    nameParts[nameParts.Length - 1] = "";
                    name = String.Join("", nameParts);

                    ContentBuilder.Singleton.Add(CLKWRK + asset.ChildNodes[0].InnerText, name, null, asset.ChildNodes[3].InnerText + "Processor");
                    Console.WriteLine(ContentBuilder.Singleton.Build());

                    asset.ChildNodes[2].InnerText = DateTime.Now.ToString();
                }
                ContentBuilder.Singleton.CopyContents(ContentBuilder.Singleton.buildDirectory + "bin/content/");
                assetXml.Save(CLKWRK + "Data/assets.xml");
            }
            Console.WriteLine("Removing temp-folder.");
            if(Directory.Exists(CLKWRK + "build/"))
                Directory.Delete(CLKWRK + "build/", true);

            Console.WriteLine("Done.");

            Console.WriteLine("========================================\nEnd of sequence at " + DateTime.Now.TimeOfDay + ".");
            timer.Start();
        }

        private static void AddFiles(string type)
        {
            if (files.Count < 1)
            {
                Console.WriteLine("No new assets of type \"" + type + "\" found.");
            }
            else
            {
                XmlNode rootNode = assetXml.DocumentElement;
                foreach (string file in files)
                {
                    Console.WriteLine("Adding " + file + " to XML");
                    XmlElement newNode = assetXml.CreateElement("Asset");
                    XmlElement fileName = assetXml.CreateElement("Location");
                    fileName.InnerText = file;

                    XmlElement lastUpdated = assetXml.CreateElement("LastUpdated");
                    lastUpdated.InnerText = File.GetLastWriteTime(CLKWRK + file).ToString();

                    XmlElement lastBuild = assetXml.CreateElement("LastBuild");
                    lastBuild.InnerText = "01-01-2000 00:00:00";

                    XmlElement assetType = assetXml.CreateElement("Type");
                    assetType.InnerText = type;

                    rootNode.AppendChild(newNode);
                    newNode.AppendChild(fileName);
                    newNode.AppendChild(lastUpdated);
                    newNode.AppendChild(lastBuild);
                    newNode.AppendChild(assetType);
                }
            }
        }

        private static void SearchDirectory(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                string fileName = file.Replace(CLKWRK,"");
                fileName = fileName.Replace('\\', '/');

                bool alreadyExists = false;
                foreach (XmlNode asset in assets)
                {
                    if (asset.ChildNodes[0].InnerText == fileName)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    files.Add(fileName);
                }
            }
            foreach (string dir in Directory.GetDirectories(directory))
            {
                SearchDirectory(dir);
            }
        }
    }
}
