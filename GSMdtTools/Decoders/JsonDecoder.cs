using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GSMdtTools.Decoders
{
    class JsonDecoder : IDecoder
    {
        private readonly Stream jsonStream;

        public JsonDecoder(Stream inputStream)
        {
            if (inputStream.CanRead)
            {
                jsonStream = inputStream;
            }
            else
            {
                throw new Exception("JSON Stream is not readable");
            }
        }

        public List<IGSToken> DecodeStream()
        {
            byte[] streamBytes = new byte[jsonStream.Length];

            int bytesRead;
            do
            {
                bytesRead = jsonStream.Read(streamBytes, 0, (int)jsonStream.Length);
            } while (bytesRead > 0);

            string streamString = Encoding.UTF8.GetString(streamBytes);

            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            List<IGSToken> tokens = JsonConvert.DeserializeObject<List<IGSToken>>(streamString, settings);

            return tokens;
        }
    }
}
