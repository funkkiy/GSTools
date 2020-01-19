using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GSMdtTools.Encoders
{
    class JsonEncoder : IEncoder
    {
        private readonly List<IGSToken> tokens;
        private Stream outputStream;

        public JsonEncoder(Stream outputStream, List<IGSToken> tokens)
        {
            if (outputStream.CanWrite)
            {
                this.outputStream = outputStream;
            }
            else
            {
                throw new Exception("JSON Stream is not writable");
            }

            this.tokens = tokens;
        }

        public void EncodeTokens()
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;

            string json = JsonConvert.SerializeObject(tokens, Formatting.Indented, settings);
            outputStream.Write(Encoding.UTF8.GetBytes(json));
        }
    }
}
