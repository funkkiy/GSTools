using System.Collections.Generic;

namespace GSMdtTools.Decoders
{
    interface IDecoder
    {
        List<IGSToken> DecodeStream();
    }
}
