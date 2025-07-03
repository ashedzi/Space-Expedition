using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                    AddArtifact(ref artifacts, ref count);
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
        public static void AddArtifact(ref Artifact[] artifacts, ref int count) {
            Console.WriteLine("Enter the name of the file you want to add (e.g nebula_noodle_net.txt):");
            Console.WriteLine("Enter name: ");
            string fileName = Console.ReadLine();

            string line = "";
            try {
                line = File.ReadAllText(fileName).Trim();
            }
            catch (Exception) {
                Console.WriteLine("Artifact file not found or could not be read.");
                return;
            }

            //Validate that it has exactly 5 parts 
            string[] parts = line.Split('|');
            if(parts.Length != 5) {
                Console.WriteLine("Invalid artifact file format.");
                return;
            }

            // 4. Assign each part to the corresponding variable
            string encodedName = parts[0].Trim();
            string planet = parts[1].Trim();
            string discoveryDate = parts[2].Trim();
            string storageLocation = parts[3].Trim();
            string description = parts[4].Trim();
            string decodedName = Decode(encodedName);


            int index = BinarySearch(artifacts, count, decodedName);
            if(index != -1) {
                Console.WriteLine("Artifact already exists in inventory");
                return;
            }

            // Expand array if it is full
            if (count >= artifacts.Length) {
                Artifact[] newArray = new Artifact[artifacts.Length * 2];
                for (int i = 0; i < artifacts.Length; i++) {
                    newArray[i] = artifacts[i];
                }
                artifacts = newArray;
            }

            Artifact newArtifact = new Artifact(encodedName, decodedName, planet, discoveryDate, storageLocation, description);
            //Find the position to insert in(sorted by decodedName)
            int insertIndex = count;
            for (int i = 0; i < count; i++) {
                if (newArtifact.DecodedName.ToLower().CompareTo(artifacts[i].DecodedName.ToLower()) < 0) {
                    insertIndex = i;
                    break;
                }
            }
            // Shift elements right to make space
            for (int j = count; j > insertIndex; j--) {
                artifacts[j] = artifacts[j - 1];
            }
            // Insert the new artwork
            artifacts[insertIndex] = newArtifact;
            count++;

            Console.WriteLine("Artifact added successfully.");
        }

        public static int BinarySearch(Artifact[] artifacts, int count, string decodedName) {
            string key = decodedName.ToLower();
            int lo = 0;
            int hi = count - 1;

            while (lo <= hi) {
                int mid = (lo + hi) / 2;
                string midArtifact = artifacts[mid].DecodedName.ToLower();
                if (midArtifact == key) {
                    return mid;
                }
                else if (String.Compare(midArtifact, key) > 0) {
                    hi = mid - 1;
                }
                else {
                    lo = mid + 1;
                }
            }
            return -1;
        }

    }
}
