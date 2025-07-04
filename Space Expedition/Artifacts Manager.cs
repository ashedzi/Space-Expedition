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
                    string decodedName = Decode(encodedName);
                    artifacts[count++] = new Artifact(encodedName, decodedName, planet, discoveryDate, storageLocation, description);
                }
                else {
                    Console.WriteLine($"Line {i + 1} has an invalid format");
                }
            }
            InsertionSort(artifacts, count);
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
                    SaveToFile(artifacts, count);
                    Console.WriteLine("Goodbye explorer!");
                    isRunning = false;
                }
                else {
                    Console.WriteLine("Invalid choice. Try again");
                }
            }
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

        public static char DecodeChar(char letter, int level) {
            char[] originalArray = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            char[] mappedArray = { 'H', 'Z', 'A', 'U', 'Y', 'E', 'K', 'G', 'O', 'T', 'I', 'R', 'J', 'V', 'W', 'N', 'M', 'F', 'Q', 'S', 'D', 'B', 'X', 'L', 'C', 'P' };

            //base case: mirror when level is 0
            if(level == 0) {
                int index = letter - 'A';
                return (char)('Z' - index);
            }

            //Find index of letter in the original array
            int letterIndex = -1;
            for(int i =0; i < originalArray.Length; i++) {
                if (originalArray[i] == letter) {
                    letterIndex = i;
                    break;
                }
            }

            if(letterIndex == -1) {
                Console.WriteLine($"Invalid letter: {letter}");
                return '?';
            }

            //Map it once, then recurse
            char mappedLetter = mappedArray[letterIndex];
            return DecodeChar(mappedLetter, level - 1);

        }

        public static string Decode(string encodedName) {
            string decodedName = "";

            for(int i = 0; i < encodedName.Length - 1; i += 2) {
                char letter = encodedName[i];
                int level = encodedName[i + 1] - '0'; //convert char digit to int
                char decodedChar = DecodeChar(letter, level);
                decodedName += decodedChar;
            }

            return decodedName;
        }

        public static void InsertionSort(Artifact[] artifacts, int count) {
            for (int i = 1; i < count; i++) {
                Artifact tempValue = artifacts[i];
                int j = i - 1;

                while (j >= 0 && String.Compare(artifacts[j].DecodedName.ToLower(), tempValue.DecodedName.ToLower()) > 0) {
                    artifacts[j + 1] = artifacts[j];
                    j--;
                }
                artifacts[j + 1] = tempValue;
            }
        }


        public static void SaveToFile(Artifact[] artifacts, int count) {
            using (StreamWriter writer = new StreamWriter("expedition_summary.txt")) {
                for (int i = 0; i < count; i++) {
                    Artifact artifact = artifacts[i];
                    writer.WriteLine($"{artifact.EncodedName}|{artifact.Planet}|{artifact.DiscoveryDate}|{artifact.StorageLocation}|{artifact.Description}");
                }
            }
            Console.WriteLine("Inventory saved to expedition_summary.txt");
        }

    }
}
