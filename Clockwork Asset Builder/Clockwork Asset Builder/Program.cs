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

        static void Main(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            Console.WriteLine("Type 'exit', 'stop' or 'quit' to stop the application.");

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

            Console.WriteLine("Running through assets.XML..");
            if (File.Exists(CLKWRK + "Data/assets.xml"))
            {
                XmlDocument assetXml = new XmlDocument();
                assetXml.Load(CLKWRK + "Data/assets.xml");

                XmlNodeList assets = assetXml.GetElementsByTagName("Asset");
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
                Console.WriteLine("Removing build-folder.");
                if(Directory.Exists(CLKWRK + "build/"))
                   Directory.Delete(CLKWRK + "build/", true);

                Console.WriteLine("Done.");
            }

            timer.Start();
        }
    }
}
