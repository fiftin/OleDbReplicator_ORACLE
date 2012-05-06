using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// Represents an DOS 866 character encoding of Unicode characters.
    /// </summary>
    public class Dos866Encoding : Encoding
    {
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }

        private static Dos866Encoding dos866 = new Dos866Encoding();
        public static Dos866Encoding Dos866
        {
            get
            {
                return dos866;
            }
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int k = byteIndex;
            for (int i = charIndex; i < charIndex + charCount; i++)
            {
                byte b;
                switch (chars[i])
                {
                    case 'А': b = 0x80; break;
                    case 'Б': b = 0x81; break;
                    case 'В': b = 0x82; break;
                    case 'Г': b = 0x83; break;
                    case 'Д': b = 0x84; break;
                    case 'Е': b = 0x85; break;
                    case 'Ж': b = 0x86; break;
                    case 'З': b = 0x87; break;
                    case 'И': b = 0x88; break;
                    case 'Й': b = 0x89; break;
                    case 'К': b = 0x8A; break;
                    case 'Л': b = 0x8B; break;
                    case 'М': b = 0x8C; break;
                    case 'Н': b = 0x8D; break;
                    case 'О': b = 0x8E; break;
                    case 'П': b = 0x8F; break;
                    case 'Р': b = 0x90; break;
                    case 'С': b = 0x91; break;
                    case 'Т': b = 0x92; break;
                    case 'У': b = 0x93; break;
                    case 'Ф': b = 0x94; break;
                    case 'Х': b = 0x95; break;
                    case 'Ц': b = 0x96; break;
                    case 'Ч': b = 0x97; break;
                    case 'Ш': b = 0x98; break;
                    case 'Щ': b = 0x99; break;
                    case 'Ъ': b = 0x9A; break;
                    case 'Ы': b = 0x9B; break;
                    case 'Ь': b = 0x9C; break;
                    case 'Э': b = 0x9D; break;
                    case 'Ю': b = 0x9E; break;
                    case 'Я': b = 0x9F; break;
                    case 'а': b = 0xA0; break;
                    case 'б': b = 0xA1; break;
                    case 'в': b = 0xA2; break;
                    case 'г': b = 0xA3; break;
                    case 'д': b = 0xA4; break;
                    case 'е': b = 0xA5; break;
                    case 'ж': b = 0xA6; break;
                    case 'з': b = 0xA7; break;
                    case 'и': b = 0xA8; break;
                    case 'й': b = 0xA9; break;
                    case 'к': b = 0xAA; break;
                    case 'л': b = 0xAB; break;
                    case 'м': b = 0xAC; break;
                    case 'н': b = 0xAD; break;
                    case 'о': b = 0xAE; break;
                    case 'п': b = 0xAF; break;
                    case 'р': b = 0xE0; break;
                    case 'с': b = 0xE1; break;
                    case 'т': b = 0xE2; break;
                    case 'у': b = 0xE3; break;
                    case 'ф': b = 0xE4; break;
                    case 'х': b = 0xE5; break;
                    case 'ц': b = 0xE6; break;
                    case 'ч': b = 0xE7; break;
                    case 'ш': b = 0xE8; break;
                    case 'щ': b = 0xE9; break;
                    case 'ъ': b = 0xEA; break;
                    case 'ы': b = 0xEB; break;
                    case 'ь': b = 0xEC; break;
                    case 'э': b = 0xED; break;
                    case 'ю': b = 0xEE; break;
                    case 'я': b = 0xEF; break;
                    case 'Ё': b = 0xF0; break;
                    case 'ё': b = 0xF1; break;
                    default:
                        b = Encoding.ASCII.GetBytes(new char[] { chars[i] })[0];
                        break;
                }
                bytes[k] = b;
                k++;
            }
            return k - byteIndex;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int k = charIndex;
            for (int i = byteIndex; i < byteIndex + byteCount; i++)
            {
                char c;
                switch (bytes[i])
                {
                    case 0x80: c = 'А'; break;
                    case 0x81: c = 'Б'; break;
                    case 0x82: c = 'В'; break;
                    case 0x83: c = 'Г'; break;
                    case 0x84: c = 'Д'; break;
                    case 0x85: c = 'Е'; break;
                    case 0x86: c = 'Ж'; break;
                    case 0x87: c = 'З'; break;
                    case 0x88: c = 'И'; break;
                    case 0x89: c = 'Й'; break;
                    case 0x8A: c = 'К'; break;
                    case 0x8B: c = 'Л'; break;
                    case 0x8C: c = 'М'; break;
                    case 0x8D: c = 'Н'; break;
                    case 0x8E: c = 'О'; break;
                    case 0x8F: c = 'П'; break;
                    case 0x90: c = 'Р'; break;
                    case 0x91: c = 'С'; break;
                    case 0x92: c = 'Т'; break;
                    case 0x93: c = 'У'; break;
                    case 0x94: c = 'Ф'; break;
                    case 0x95: c = 'Х'; break;
                    case 0x96: c = 'Ц'; break;
                    case 0x97: c = 'Ч'; break;
                    case 0x98: c = 'Ш'; break;
                    case 0x99: c = 'Щ'; break;
                    case 0x9A: c = 'Ъ'; break;
                    case 0x9B: c = 'Ы'; break;
                    case 0x9C: c = 'Ь'; break;
                    case 0x9D: c = 'Э'; break;
                    case 0x9E: c = 'Ю'; break;
                    case 0x9F: c = 'Я'; break;
                    case 0xA0: c = 'а'; break;
                    case 0xA1: c = 'б'; break;
                    case 0xA2: c = 'в'; break;
                    case 0xA3: c = 'г'; break;
                    case 0xA4: c = 'д'; break;
                    case 0xA5: c = 'е'; break;
                    case 0xA6: c = 'ж'; break;
                    case 0xA7: c = 'з'; break;
                    case 0xA8: c = 'и'; break;
                    case 0xA9: c = 'й'; break;
                    case 0xAA: c = 'к'; break;
                    case 0xAB: c = 'л'; break;
                    case 0xAC: c = 'м'; break;
                    case 0xAD: c = 'н'; break;
                    case 0xAE: c = 'о'; break;
                    case 0xAF: c = 'п'; break;
                    case 0xE0: c = 'р'; break;
                    case 0xE1: c = 'с'; break;
                    case 0xE2: c = 'т'; break;
                    case 0xE3: c = 'у'; break;
                    case 0xE4: c = 'ф'; break;
                    case 0xE5: c = 'х'; break;
                    case 0xE6: c = 'ц'; break;
                    case 0xE7: c = 'ч'; break;
                    case 0xE8: c = 'ш'; break;
                    case 0xE9: c = 'щ'; break;
                    case 0xEA: c = 'ъ'; break;
                    case 0xEB: c = 'ы'; break;
                    case 0xEC: c = 'ь'; break;
                    case 0xED: c = 'э'; break;
                    case 0xEE: c = 'ю'; break;
                    case 0xEF: c = 'я'; break;
                    case 0xF0: c = 'Ё'; break;
                    case 0xF1: c = 'ё'; break;
                    default:
                        c = Encoding.ASCII.GetChars(new byte[] { bytes[k] })[0];
                        break;
                }
                chars[k] = c;
                k++;
            }
            return k - charIndex;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }
}
