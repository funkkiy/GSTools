using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GSMdtTools.Encoders
{
    class MdtEncoder : IEncoder
    {
        private readonly List<IGSToken> tokens;
        private Stream outputStream;

        string ToFullWidth(string text)
        {
            Dictionary<string, string> widthConversion = new Dictionary<string, string>
            {
                {
                    "1",
                    "１"
                },
                {
                    "2",
                    "２"
                },
                {
                    "3",
                    "３"
                },
                {
                    "4",
                    "４"
                },
                {
                    "5",
                    "５"
                },
                {
                    "6",
                    "６"
                },
                {
                    "7",
                    "７"
                },
                {
                    "8",
                    "８"
                },
                {
                    "9",
                    "９"
                },
                {
                    "0",
                    "０"
                },
                {
                    "A",
                    "Ａ"
                },
                {
                    "B",
                    "Ｂ"
                },
                {
                    "C",
                    "Ｃ"
                },
                {
                    "D",
                    "Ｄ"
                },
                {
                    "E",
                    "Ｅ"
                },
                {
                    "F",
                    "Ｆ"
                },
                {
                    "G",
                    "Ｇ"
                },
                {
                    "H",
                    "Ｈ"
                },
                {
                    "I",
                    "Ｉ"
                },
                {
                    "J",
                    "Ｊ"
                },
                {
                    "K",
                    "Ｋ"
                },
                {
                    "L",
                    "Ｌ"
                },
                {
                    "M",
                    "Ｍ"
                },
                {
                    "N",
                    "Ｎ"
                },
                {
                    "O",
                    "Ｏ"
                },
                {
                    "P",
                    "Ｐ"
                },
                {
                    "Q",
                    "Ｑ"
                },
                {
                    "R",
                    "Ｒ"
                },
                {
                    "S",
                    "Ｓ"
                },
                {
                    "T",
                    "Ｔ"
                },
                {
                    "U",
                    "Ｕ"
                },
                {
                    "V",
                    "Ｖ"
                },
                {
                    "W",
                    "Ｗ"
                },
                {
                    "X",
                    "Ｘ"
                },
                {
                    "Y",
                    "Ｙ"
                },
                {
                    "Z",
                    "Ｚ"
                },
                {
                    "a",
                    "ａ"
                },
                {
                    "b",
                    "ｂ"
                },
                {
                    "c",
                    "ｃ"
                },
                {
                    "d",
                    "ｄ"
                },
                {
                    "e",
                    "ｅ"
                },
                {
                    "f",
                    "ｆ"
                },
                {
                    "g",
                    "ｇ"
                },
                {
                    "h",
                    "ｈ"
                },
                {
                    "i",
                    "ｉ"
                },
                {
                    "j",
                    "ｊ"
                },
                {
                    "k",
                    "ｋ"
                },
                {
                    "l",
                    "ｌ"
                },
                {
                    "m",
                    "ｍ"
                },
                {
                    "n",
                    "ｎ"
                },
                {
                    "o",
                    "ｏ"
                },
                {
                    "p",
                    "ｐ"
                },
                {
                    "q",
                    "ｑ"
                },
                {
                    "r",
                    "ｒ"
                },
                {
                    "s",
                    "ｓ"
                },
                {
                    "t",
                    "ｔ"
                },
                {
                    "u",
                    "ｕ"
                },
                {
                    "v",
                    "ｖ"
                },
                {
                    "w",
                    "ｗ"
                },
                {
                    "x",
                    "ｘ"
                },
                {
                    "y",
                    "ｙ"
                },
                {
                    "z",
                    "ｚ"
                },
                {
                    " ",
                    "\u3000"
                },
                {
                    ".",
                    "．"
                },
                {
                    ",",
                    "，"
                },
                {
                    "'",
                    "＇"
                },
                {
                    "!",
                    "！"
                },
                {
                    "(",
                    "（"
                },
                {
                    ")",
                    "）"
                },
                /*
                {
                    "-",
                    "－"
                },
                */
                {
                    "/",
                    "／"
                },
                {
                    "?",
                    "？"
                },
                {
                    "_",
                    "∠"
                },
                {
                    "[",
                    "［"
                },
                {
                    "]",
                    "］"
                },
                /*
                {
                    "\"",
                    "“"
                },
                {
                    "\"",
                    "”"
                },
                */
                {
                    "\"",
                    "＂"
                },
                {
                    "-",
                    "―"
                },
                /*
                {
                    "'",
                    "‘"
                },
                {
                    "'",
                    "’"
                },
                */
                {
                    ":",
                    "："
                },
                {
                    "*",
                    "＊"
                },
                {
                    ";",
                    "；"
                },
                {
                    "$",
                    "＄"
                },
                {
                    "©",
                    "Ы"
                },
                {
                    "è",
                    "∋"
                },
                {
                    "é",
                    "∈"
                },
                {
                    "á",
                    "∀"
                },
                {
                    "à",
                    "∧"
                },
                {
                    "ç",
                    "⊆"
                },
                {
                    "Ç",
                    "⊂"
                },
                {
                    "û",
                    "Ц"
                },
                {
                    "î",
                    "↑"
                },
                {
                    "â",
                    "α"
                },
                {
                    "ñ",
                    "л"
                },
                {
                    "ï",
                    "↓"
                },
                {
                    "ê",
                    "ε"
                },
                {
                    "&",
                    "＆"
                }
            };

            StringBuilder sb = new StringBuilder();
            foreach (string ch in text.ToCharArray().Select(c => c.ToString()).ToArray())
            {
                sb.Append(widthConversion.ContainsKey(ch) ? widthConversion[ch] : ch);
            }

            return sb.ToString();
        }

        public MdtEncoder(Stream outputStream, List<IGSToken> tokens)
        {
            if (outputStream.CanWrite)
            {
                this.outputStream = outputStream;
            }
            else
            {
                throw new Exception("MDT Stream is not writable");
            }

            this.tokens = tokens;
        }

        public void EncodeTokens()
        {
            using (var writer = new BinaryWriter(outputStream))
            {
                ushort messageCount = ((GSMessageCountToken)(tokens[0])).MessageCount;
                writer.Write(messageCount);

                // Write dummy sized 2 bytes
                writer.Write((ushort)0x00);

                // Write File Message Offset
                uint fileMessageOffset = (uint)(writer.BaseStream.Position + (messageCount * 4));
                writer.Write(fileMessageOffset);

                // Write message offsets
                uint lastOffset = fileMessageOffset;

                int tokenOffsetIndex = 0;
                for (int i = 0; i < messageCount - 1; i++)
                {
                    bool foundMessageEnd = false;
                    uint messageSize = 0;
                    for (int j = tokenOffsetIndex; j < tokens.Count; j++)
                    {
                        if (!foundMessageEnd)
                        {
                            switch (tokens[j])
                            {
                                case GSStringToken stringToken:
                                    messageSize += (uint)stringToken.Content.Length * sizeof(ushort);
                                    break;
                                case GSOperationToken operationToken:
                                    messageSize += (uint)(1 + operationToken.Args.Length) * sizeof(ushort);
                                    break;
                                case GSEndMessageToken endToken:
                                    foundMessageEnd = true;

                                    // Write message offset
                                    lastOffset += messageSize;
                                    writer.Write(lastOffset);
                                    break;
                                default:
                                    break;
                            }

                            // Sync offset index for next message loop with index on GSEndMessageToken
                            tokenOffsetIndex++;
                        }
                    }
                }

                /*
                 * A character in the MDT script file is represented by 2 bytes, UTF-16LE encoding
                 * An operation and its arguments are represented by 2 bytes
                 */
                foreach (IGSToken token in tokens)
                {
                    switch (token)
                    {
                        case GSStringToken stringToken:
                            string fullContent = ToFullWidth(stringToken.Content);
                            foreach (char ch in fullContent)
                            {
                                ushort encodedChar = (ushort)(ch + 128);
                                writer.Write(encodedChar);
                            }
                            break;
                        case GSOperationToken operationToken:
                            writer.Write(operationToken.Opcode);
                            foreach (ushort arg in operationToken.Args)
                            {
                                writer.Write(arg);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
