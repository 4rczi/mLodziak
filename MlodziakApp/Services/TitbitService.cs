using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class TitbitService : ITitbitService
    {
        private List<string> TitbitList;

        private void EnsureTitbitsLoaded()
        {
            if (TitbitList == null)
            {
                string jsonTitbitContent = LoadEmbeddedJson("MlodziakApp.Resources.Titbits.Titbits.json");
                TitbitList = JsonSerializer.Deserialize<List<string>>(jsonTitbitContent)
                    ?? throw new InvalidOperationException("Failed to load titbits from JSON.");
            }
        }

        private string LoadEmbeddedJson(string fileName)
        {
            var assembly = typeof(App).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(name => name.EndsWith(fileName));

            if (resourceName == null) throw new FileNotFoundException("Embedded resource not found");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public string GetRandomTitbit()
        {
            EnsureTitbitsLoaded();
            return TitbitList[new Random().Next(TitbitList.Count)];
        }
    }
}
