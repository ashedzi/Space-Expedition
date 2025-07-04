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
                string[] artifactParts = artifactLines[i].Split(',', 5);

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
            return artifacts;
        }

        public static void StartApp(Artifact[] artifacts, ref int count) {
            bool isRunning = true;
            while (isRunning) {
                Console.WriteLine("\nHey there! What will you like to do today?");
                Console.WriteLine("Select an option from 1-3");
                Console.WriteLine("1. View inventory");
                Console.WriteLine("2. Add artifact");
                Console.WriteLine("3. Save & Exit");
                string userChoice = Console.ReadLine();

                switch (userChoice) {
                    case "1":
                        ViewInventory(artifacts, count);
                        break;
                    case "2":
                        artifacts = AddArtifact(artifacts, ref count);
                        break;
                    case "3":
                        SaveToFile(artifacts, count);
                        Console.WriteLine("Goodbye, space explorer!");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
            }
        }

        public static void ViewInventory(Artifact[] artifacts, int count) {
            Console.WriteLine("\n--- CURRENT INVENTORY ----");
            if (count == 0) {
                Console.WriteLine("No artifacts in inventory.");
            }
            else {
                Console.WriteLine($"Total artifacts: {count}");
                for (int i = 0; i < count; i++) {
                    Console.WriteLine($"{artifacts[i].DecodedName} | {artifacts[i].DiscoveryDate} | {artifacts[i].Planet} | {artifacts[i].StorageLocation} | {artifacts[i].Description}");
                    Console.WriteLine();
                }
            }
        }
        public static Artifact[] AddArtifact(Artifact[]artifacts, ref int count) {
            Console.WriteLine("Enter the name of the file you want to add. Name should match the exact way it appears in your files (e.g Nebula Noodle Net.txt): ");

            string fileName = Console.ReadLine().Trim();

            // Check if file exists
            if (!File.Exists(fileName)) {
                Console.WriteLine("File not found. Please check the name and try again.");
                return artifacts;
            }

            string line = "";
            try {
                line = File.ReadAllText(fileName).Trim();
            }
            catch(Exception) {
                Console.WriteLine("Artifact file not found or could not be read.");
                return artifacts;
            }

            string[] parts = line.Split(',', 5);
            if(parts.Length != 5) {
                Console.WriteLine("Invalid artifact file format.");
                return artifacts;
            }

            string encodedName = parts[0].Trim();
            string planet = parts[1].Trim();
            string discoveryDate = parts[2].Trim();
            string storageLocation = parts[3].Trim();
            string description = parts[4].Trim();
            Console.WriteLine($"Encoded input from added file: '{encodedName}'");
            string decodedName = Decode(encodedName);
            Console.WriteLine($"Decoded name: '{decodedName}'");

            int index = BinarySearch(artifacts, count, decodedName);
            if(index != -1) {
                Console.WriteLine("Artifact already exists in inventory.");
                return artifacts;
            }

            // Resize array if full
            if (count >= artifacts.Length) {
                Artifact[] newArray = new Artifact[artifacts.Length * 2];
                for (int i = 0; i < artifacts.Length; i++) {
                    newArray[i] = artifacts[i];
                }
                artifacts = newArray;
            }

            Artifact newArtifact = new Artifact(encodedName, decodedName, planet, discoveryDate, storageLocation, description);

            int insertIndex = count;
            for(int i = 0; i < count; i++) {
                if (newArtifact.DecodedName.ToLower().CompareTo(artifacts[i].DecodedName.ToLower())< 0) {
                    insertIndex = i;
                    break;
                }
            }

            for(int j = count; j > insertIndex; j--) {
                artifacts[j] = artifacts[j - 1];
            }

            artifacts[insertIndex] = newArtifact;
            count++;
            Console.WriteLine($"Artifact '{decodedName}' added successfully.");
            //Console.WriteLine("Artifact added successfully.");
            return artifacts;
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

            letter = char.ToUpper(letter);
            //Console.WriteLine($"Decoding: {letter}{level}");

            if (level == 1) {
                if(letter < 'A' || letter > 'Z') {
                    Console.WriteLine($"Invalid letter for mirroring: {letter}");
                    return '?';
                }
                return (char)('Z' - (letter - 'A'));
            }
            // Recursive case: find letter in original array and map it
            int letterIndex = Array.IndexOf(originalArray, letter);
            if (letterIndex == -1) {
                Console.WriteLine($"Invalid letter: {letter}");
                return '?';
            }
            char nextLetter = mappedArray[letterIndex];
            //Console.WriteLine($"  Level {level}->{level - 1}: {letter} maps to {nextLetter}");
            return DecodeChar(nextLetter, level - 1);
        }

        public static string Decode(string encodedName) {
            string decodedName = "";

            // Step 1: Split by space to get words
            string[] words = encodedName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words) {
                string decodedWord = "";
                string[] encodedChars = word.Split('|', StringSplitOptions.RemoveEmptyEntries);

                foreach(string encodedChar in encodedChars) {
                    if(encodedChar.Length < 2) {
                        Console.WriteLine($"Invalid encoded character: {encodedChar}");
                        continue;
                    }

                    char letter = encodedChar[0];
                    string levelStr = encodedChar.Substring(1);

                    if(!char.IsLetter(letter) || !int.TryParse(levelStr, out int level)) {
                        Console.WriteLine($"Invalid encoded character format: {encodedChar}");
                        continue;
                    }
                    char decodedChar = DecodeChar(letter, level);
                    decodedWord += decodedChar;
                }

               if(!string.IsNullOrEmpty(decodedWord)) {
                    decodedName += decodedWord + " ";
                }
            }
            return decodedName.Trim();
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
