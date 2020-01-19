/*
 * GSMdtTools: decodes and encodes Gyakuten Saiban script files
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mono.Options;

namespace GSMdtTools
{
    class Program
    {
        static void Main(string[] args)
        {
            bool shouldDecode = false;
            bool shouldEncode = false;
            bool shouldShowHelp = false;
            var options = new OptionSet
            {
                { "d|decode", "decode a file", d => shouldDecode = d != null },
                { "e|encode", "encode a file", e => shouldEncode = e != null },
                { "h|help", "show the help message", h => shouldShowHelp = h != null}
            };

            List<string> extra = null;
            try
            {
                extra = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine($"GsMdtTools: {e.Message}");
                Console.WriteLine("Try passing \"--help\" as an argument to learn about available options");
                return;
            }

            if (shouldShowHelp)
            {
                Console.WriteLine("GsMdtTools: decode or encode a decrypted Gyakuten Saiban MDT file");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string dataFilename = null;
            byte[] dataBytes = null;
            if (extra.Count > 0)
            {
                dataFilename = extra[0];
            }
            else
            {
                Console.WriteLine($"GsMdtTools: Input the path to a {(shouldEncode ? "JSON" : "MDT")} file");
                return;
            }

            // Read MDT or JSON file
            try
            {
                dataBytes = File.ReadAllBytes(dataFilename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"GsMdtTools: {e.Message}");
                Console.WriteLine("Does the given file exist?");
                return;
            }

            if (!(shouldDecode ^ shouldEncode))
            {
                // shouldDecode and shouldEncode are False
                string dataExtension = Path.GetExtension(dataFilename);
                shouldEncode = !(dataExtension == ".mdt" || dataExtension == ".dec");
            }

            // Decode to GSTokens, re-encode to target format
            if (!shouldEncode)
            {
                Decoders.MdtDecoder decoder = new Decoders.MdtDecoder(new MemoryStream(dataBytes));
                List<IGSToken> tokens = decoder.DecodeStream();

                // Encode GSTokens in JSON
                Encoders.JsonEncoder jsonEncoder = new Encoders.JsonEncoder(new FileStream(
                    $"{Path.GetFileNameWithoutExtension(dataFilename)}.json", FileMode.Create), tokens);
                jsonEncoder.EncodeTokens();
            }
            else
            {
                Decoders.JsonDecoder decoder = new Decoders.JsonDecoder(new MemoryStream(dataBytes));
                List<IGSToken> tokens = decoder.DecodeStream();

                // Encode GSTokens in MDT
                Encoders.MdtEncoder mdtEncoder = new Encoders.MdtEncoder(new FileStream(
                    $"{Path.GetFileNameWithoutExtension(dataFilename)}.mdt.new", FileMode.Create), tokens);
                mdtEncoder.EncodeTokens();
            }
        }
    }
}
