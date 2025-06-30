using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Expedition {
    internal class Artifact {
        public string EncodedName { get; set; }
        public string DecodedName { get; set; }
        public string Planet { get; set; }
        public string DiscoveryDate { get; set; }
        public string StorageLocation { get; set; }
        public string Description { get; set; }

        public Artifact(string encodedName, string decodedName, string planet, string discoveryDate, string storageLocation, string description) {
            EncodedName = encodedName;
            DecodedName = decodedName;
            Planet = planet;
            DiscoveryDate = discoveryDate;
            StorageLocation = storageLocation;
            Description = description;
        }
    }
}
