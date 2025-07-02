using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Space_Expedition {
    internal class Artifacts_Manager {
        public static Artifact[] ReadFile(out int count) {
            // Read all lines from the text file into an array
            string[] artifactLines = File.ReadAllLines("galactic_vault.txt");
            Artifact[] artifacts = new Artifact[artifactLines.Length];
            count = 0;

            for (int i = 0; i < artifactLines.Length; i++) {
                string[] artifactParts = artifactLines[i].Split('|');
                if (artifactParts.Length == 5) {
                    string encodedName = artifactParts[0].Trim();
                    string planet = artifactParts[1].Trim();
                    string discoveryDate = artifactParts[2].Trim();
                    string storageLocation = artifactParts[3].Trim();
                    string description = artifactParts[4].Trim();
                    string decodedName = "";
                    artifacts[count++] = new Artifact(encodedName, decodedName, planet, discoveryDate, storageLocation, description);
                }
                else {
                    Console.WriteLine($"Line {i + 1} has an invalid format");
                }
            }
            Console.WriteLine("\n----Loaded Artifacts------");
            for (int i = 0; i < count; i++) {
                Console.WriteLine($"{artifacts[i].EncodedName} | {artifacts[i].DiscoveryDate}");
            }
            return artifacts;
        }

        public static void StartApp(Artifact[] artifacts, ref int count) {
            bool isRunning = true;
            while (isRunning) {
                Console.WriteLine("Hey there! What will you like to do today?");
                Console.WriteLine("Select an option fro 1-4");
                Console.WriteLine("1. View inventory");
                Console.WriteLine("2. Add artifact");
                Console.WriteLine("3. Save & Exit");
                string userChoice = Console.ReadLine();

                if (userChoice == "1") {
                    Console.WriteLine("--------Your current Artifacts-----");
                    if (count == 0) {
                        Console.WriteLine("There are no artifacts available");
                    }
                    else {
                        for (int i = 0; i < count; i++) {
                            Console.WriteLine($"{artifacts[i].EncodedName} | {artifacts[i].Planet}");
                        }
                    }
                }
                else if (userChoice == "2") {
                    //AddArtifact();
                }
                else if (userChoice == "3") {
                    Console.WriteLine("Goodbye explorer!");
                    isRunning = false;
                }
                else {
                    Console.WriteLine("Invalid choice. Try again");
                }
            }
        }
    }
}
