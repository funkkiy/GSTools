using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GSMdtTools.Decoders
{
    public class MdtDecoder : IDecoder
    {
        private readonly Stream mdtStream;

        private ushort messageCount = 0;
        private ushort invalidOffsets = 0;

        GSOperation[] gsOps = {
            new GSOperation("CodeProc_00", 0),
            new GSOperation("CodeProc_01", 0),
            new GSOperation("CodeProc_02", 0),
            new GSOperation("CodeProc_03", 1),
            new GSOperation("CodeProc_04", 1),
            new GSOperation("CodeProc_05", 2),
            new GSOperation("CodeProc_06", 2),
            new GSOperation("CodeProc_02 (2)", 0),
            new GSOperation("CodeProc_08", 2),
            new GSOperation("CodeProc_09", 3),
            new GSOperation("CodeProc_02 (3)", 1),
            new GSOperation("CodeProc_0B", 1),
            new GSOperation("CodeProc_0C", 1),
            new GSOperation("CodeProc_0D", 0),
            new GSOperation("CodeProc_0E", 1),
            new GSOperation("CodeProc_0F", 2),
            new GSOperation("CodeProc_10", 1),
            new GSOperation("CodeProc_11", 0),
            new GSOperation("CodeProc_12", 3),
            new GSOperation("CodeProc_13", 1),
            new GSOperation("CodeProc_14", 0),
            new GSOperation("CodeProc_15", 0),
            new GSOperation("CodeProc_16", 0),
            new GSOperation("CodeProc_17", 1),
            new GSOperation("CodeProc_18", 1),
            new GSOperation("CodeProc_19", 2),
            new GSOperation("CodeProc_1A", 4),
            new GSOperation("CodeProc_1B", 1),
            new GSOperation("CodeProc_1C", 1),
            new GSOperation("CodeProc_1D", 1),
            new GSOperation("CodeProc_1E", 3),
            new GSOperation("CodeProc_1F", 0),
            new GSOperation("CodeProc_20", 1),
            new GSOperation("CodeProc_21", 0),
            new GSOperation("CodeProc_22", 2),
            new GSOperation("CodeProc_23", 2),
            new GSOperation("CodeProc_24", 0),
            new GSOperation("CodeProc_25", 1),
            new GSOperation("CodeProc_26", 1),
            new GSOperation("CodeProc_27", 2),
            new GSOperation("CodeProc_28", 1),
            new GSOperation("CodeProc_29", 1),
            new GSOperation("CodeProc_2A", 3),
            new GSOperation("CodeProc_2B", 0),
            new GSOperation("CodeProc_2C", 1),
            new GSOperation("CodeProc_02 (4)", 0),
            new GSOperation("CodeProc_2E", 0),
            new GSOperation("CodeProc_2F", 2),
            new GSOperation("CodeProc_30", 1),
            new GSOperation("CodeProc_31", 2),
            new GSOperation("CodeProc_32", 2),
            new GSOperation("CodeProc_33", 5),
            new GSOperation("CodeProc_34", 1),
            new GSOperation("CodeProc_35", 2),
            new GSOperation("CodeProc_36", 1),
            new GSOperation("CodeProc_37", 2),
            new GSOperation("CodeProc_38", 1),
            new GSOperation("CodeProc_39", 1),
            new GSOperation("CodeProc_3A", 3),
            new GSOperation("CodeProc_3B", 2),
            new GSOperation("CodeProc_3C", 1),
            new GSOperation("CodeProc_3D", 1),
            new GSOperation("CodeProc_3E", 1),
            new GSOperation("CodeProc_3F", 0),
            new GSOperation("CodeProc_40", 0),
            new GSOperation("CodeProc_41", 0),
            new GSOperation("CodeProc_42", 1),
            new GSOperation("CodeProc_43", 1),
            new GSOperation("CodeProc_44", 1),
            new GSOperation("CodeProc_15", 0),
            new GSOperation("CodeProc_46", 1),
            new GSOperation("CodeProc_47", 2),
            new GSOperation("CodeProc_48", 2),
            new GSOperation("CodeProc_49", 0),
            new GSOperation("CodeProc_4A", 1),
            new GSOperation("CodeProc_4B", 1),
            new GSOperation("CodeProc_4C", 0),
            new GSOperation("CodeProc_4D", 2),
            new GSOperation("CodeProc_4E", 1),
            new GSOperation("CodeProc_4F", 7),
            new GSOperation("CodeProc_50", 1),
            new GSOperation("CodeProc_51", 2),
            new GSOperation("CodeProc_52", 1),
            new GSOperation("CodeProc_53", 0),
            new GSOperation("CodeProc_54", 2),
            new GSOperation("CodeProc_55", 1),
            new GSOperation("CodeProc_56", 2),
            new GSOperation("CodeProc_57", 1),
            new GSOperation("CodeProc_58", 0),
            new GSOperation("CodeProc_59", 1),
            new GSOperation("CodeProc_5A", 1),
            new GSOperation("CodeProc_5B", 2),
            new GSOperation("CodeProc_5C", 3),
            new GSOperation("CodeProc_5D", 0),
            new GSOperation("CodeProc_5E", 0),
            new GSOperation("CodeProc_5F", 3),
            new GSOperation("CodeProc_60", 4),
            new GSOperation("CodeProc_61", 3),
            new GSOperation("CodeProc_62", 0),
            new GSOperation("CodeProc_63", 0),
            new GSOperation("CodeProc_64", 1),
            new GSOperation("CodeProc_65", 2),
            new GSOperation("CodeProc_66", 3),
            new GSOperation("CodeProc_67", 0),
            new GSOperation("CodeProc_68", 0),
            new GSOperation("CodeProc_69", 4),
            new GSOperation("CodeProc_6A", 1),
            new GSOperation("CodeProc_6B", 3),
            new GSOperation("CodeProc_6C", 0),
            new GSOperation("CodeProc_6D", 1),
            new GSOperation("CodeProc_6E", 1),
            new GSOperation("CodeProc_6F", 1),
            new GSOperation("CodeProc_70", 3),
            new GSOperation("CodeProc_71", 3),
            new GSOperation("CodeProc_DUMMY", 0),
            new GSOperation("CodeProc_DUMMY", 0),
            new GSOperation("CodeProc_74", 2),
            new GSOperation("CodeProc_75", 4),
            new GSOperation("CodeProc_76", 2),
            new GSOperation("CodeProc_77", 2),
            new GSOperation("CodeProc_36", 1),
            new GSOperation("CodeProc_15", 0),
            new GSOperation("CodeProc_7A", 1),
            new GSOperation("CodeProc_7B", 2),
            new GSOperation("CodeProc_7C", 0),
            new GSOperation("CodeProc_7D", 1),
            new GSOperation("CodeProc_7E", 1),
            new GSOperation("CodeProc_7F", 1),
        };

        public MdtDecoder(Stream inputStream)
        {
            if (inputStream.CanRead)
            {
                mdtStream = inputStream;
            }
            else
            {
                throw new Exception("MDT Stream is not readable");
            }
        }

        public ushort GetMessageCount()
        {
            if (messageCount == default(ushort))
            {
                using (var reader = new BinaryReader(mdtStream, Encoding.UTF8, true))
                {
                    messageCount = reader.ReadUInt16();

                    long previousPos = mdtStream.Position;

                    // Skip dummy
                    _ = reader.ReadUInt16();

                    // Check if offsets are valid :-)
                    for (int i = 0; i < messageCount; i++)
                    {
                        uint offset = reader.ReadUInt32();
                        if (offset > mdtStream.Length)
                        {
                            Console.WriteLine($"Pruned invalid offset #{invalidOffsets}: {offset} > {mdtStream.Length}");
                            messageCount--;
                            invalidOffsets++;
                        }
                    }

                    // Go back to original position
                    mdtStream.Seek(previousPos, SeekOrigin.Begin);
                }
            }

            return messageCount;
        }

        public List<IGSToken> DecodeStream()
        {
            List<IGSToken> tokens = new List<IGSToken>();

            using (var reader = new BinaryReader(mdtStream))
            {
                ushort count = GetMessageCount();
                Console.WriteLine(count);

                // Skip dummy sized 2 bytes
                _ = reader.ReadUInt16();

                // Obtain offset and length for messages
                uint[] messageOffsets = new uint[count];
                for (ushort i = 0; i < count; i++)
                {
                    messageOffsets[i] = reader.ReadUInt32();
                }

                // Skip invalid offsets
                for (ushort i = 0; i < invalidOffsets; i++)
                {
                    _ = reader.ReadUInt32();
                }

                uint[] messageLengths = new uint[count];
                for (ushort i = 0; i < count - 1; i++)
                {
                    messageLengths[i] = (messageOffsets[i + 1] - messageOffsets[i]) / sizeof(ushort);
                }
                messageLengths[count - 1] = (uint)(mdtStream.Length - messageOffsets[count - 1]) / sizeof(ushort);

                tokens.Add(new GSMessageCountToken(count));

                /*
                 * A character in the MDT script file is represented by 2 bytes, UTF-16LE encoding
                 * An operation and its arguments are represented by 2 bytes
                 */
                for (ushort i = 0; i < count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    int opTimes = 0;
                    for (uint j = 0; j < messageLengths[i] - opTimes; j++)
                    {
                        if (reader.BaseStream.Length - reader.BaseStream.Position < sizeof(ushort))
                        {
                            // Can't read anymore!
                            return tokens;
                        }

                        ushort opOrCharacter = reader.ReadUInt16();
                        if (opOrCharacter >= 128)
                        {
                            // Character, append to String token
                            opOrCharacter -= 128;
                            sb.Append(char.ConvertFromUtf32(opOrCharacter).Normalize(NormalizationForm.FormKC));
                        }
                        else
                        {
                            // Operation
                            if (sb.Length > 0)
                            {
                                tokens.Add(new GSStringToken(sb.ToString()));
                                sb.Clear();
                            }

                            ushort argCount = gsOps[opOrCharacter].ArgCount;
                            ushort[] args = new ushort[argCount];
                            for (ushort x = 0; x < argCount; x++)
                            {
                                args[x] = reader.ReadUInt16();
                            }
                            opTimes += argCount;
                            tokens.Add(new GSOperationToken(gsOps[opOrCharacter].Name, opOrCharacter, args));
                        }
                    }

                    if (sb.Length > 0)
                    {
                        tokens.Add(new GSStringToken(sb.ToString()));
                        sb.Clear();
                    }

                    // Add message ending token
                    // TODO: the EndMessage decoding is flimsy, not 1:1
                    tokens.Add(new GSEndMessageToken());
                }
            }

            return tokens;
        }
    }
}
