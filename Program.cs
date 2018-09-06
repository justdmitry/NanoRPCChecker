namespace NanoRPCChecker
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var peersText = await File.ReadAllTextAsync("peers.json");
            var peers = JsonConvert.DeserializeObject<PeersFile>(peersText);

            Console.WriteLine($"{peers.Peers.Count} peers are read");

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(1);

            var content = new StringContent(@"{ ""action"": ""version"" }");

            foreach (var item in peers.Peers.Keys)
            {
                var delim = item.LastIndexOf(':');
                var adr = item.Substring(0, delim).TrimStart('[').TrimEnd(']').Replace("::ffff:", string.Empty);
                var port = int.Parse(item.Substring(delim + 1)) + 1;

                try
                {
                    using (var resp = await client.PostAsync($"http://{adr}:{port}", content))
                    {
                        if (resp.IsSuccessStatusCode)
                        {
                            Console.Write("                                                                   ");
                            Console.CursorLeft = 0;
                            Console.WriteLine($"{adr}:{port}: {await resp.Content.ReadAsStringAsync()}");
                        }
                        else
                        {
                            Console.Write($"{adr}:{port}: {resp.StatusCode}");
                            Console.CursorLeft = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"{adr}:{port}: {ex.Message}");
                    Console.CursorLeft = 0;
                }
            }
        }

        private class PeersFile
        {
            public Dictionary<string, string> Peers { get; set; }
        }
    }
}