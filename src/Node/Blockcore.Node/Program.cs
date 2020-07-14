﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blockcore.Builder;
using Blockcore.Configuration;
using Blockcore.Utilities;

namespace Blockcore.Node
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                string chain = args
                   .DefaultIfEmpty("--chain=BTC")
                   .Where(arg => arg.StartsWith("--chain", ignoreCase: true, CultureInfo.InvariantCulture))
                   .Select(arg => arg.Replace("--chain=", string.Empty, ignoreCase: true, CultureInfo.InvariantCulture))
                   .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(chain))
                {
                    chain = "BTC";
                }

                string modeSelection = args
                   .DefaultIfEmpty("--mode=full")
                   .Where(arg => arg.StartsWith("--mode", ignoreCase: true, CultureInfo.InvariantCulture))
                   .Select(arg => arg.Replace("--mode=", string.Empty, ignoreCase: true, CultureInfo.InvariantCulture))
                   .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(modeSelection))
                {
                    modeSelection = "full";
                }

                Mode mode = (Mode)Enum.Parse(typeof(Mode), modeSelection, true);

                NodeSettings nodeSettings = NetworkSelector.Create(chain, args);
                IFullNodeBuilder nodeBuilder = NodeBuilder.Create(chain, nodeSettings, mode);

                IFullNode node = nodeBuilder.Build();

                if (node != null)
                    await node.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem initializing the node. Details: '{0}'", ex);
            }
        }
    }
}
