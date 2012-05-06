using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// 
    /// </summary>
    public class UnknownEncoding : System.Text.Encoding
    {
        private static UnknownEncoding unknown = new UnknownEncoding();
        public static UnknownEncoding Unknown
        {
            get
            {
                return unknown;
            }
        }

        private static char DecodeChar(char c)
        {
            switch (c)
            {
                case '': return 'Ï';
                case '…': return 'Å';
                case '‘': return 'Ñ';
                case '’': return 'Ò';
                case '': return 'Ğ';
                case 'Ÿ': return 'ß';
                case 'Š': return 'Ê';
                case '': return 'Î';
                case '‚': return 'Â';
                case '€': return 'À';
                case ' ': return ' ';
                case 'œ': return 'Ü';
                case '': return 'Í';
                case 'ˆ': return 'È';
                case '„': return 'Ä';
                case '®': return 'î';
                case '¬': return 'ì';
                case '¥': return 'å';
                case '­': return 'í';
                case 'é': return 'ù';
                case '¨': return 'è';
                case 'ª': return 'ê';
                case '¢': return 'â';
                case 'à': return 'ğ';
                case '¤': return 'ä';
                case '¦': return 'æ';
                case '§': return 'ç';
                case 'á': return 'ñ';
                case '£': return 'ã';
                case '-': return '-';
                case ' ': return 'à';
                case '.': return '.';
                case 'Œ': return 'Ì';
                case 'â': return 'ò';
                case '': return 'Á';
                case '‹': return 'Ë';
                case 'í': return 'ı';
                case 'ï': return 'ÿ';
                case '‡': return 'Ç';
                case '«': return 'ë';
                case 'ë': return 'û';
                case '©': return 'é';
                case '“': return 'Ó';
                case '•': return 'Õ';
                case '‰': return 'É';
                case '—': return '×';
                case '”': return 'Ô';
                case '˜': return 'Ø';
                case '†': return 'Æ';
                case 'ã': return 'ó';
                case '¡': return 'á';
                case 'ƒ': return 'Ã';
                case 'è': return 'ø';
                case '': return 'Ş';
                case '–': return 'Ö';
                case '"': return '"';
                case '5': return '5';
                case '0': return '0';
                case '›': return 'Û';
                case 'ì': return 'ü';
                case 'æ': return 'ö';
                case 'ç': return '÷';
                case '': return 'İ';
                case '¯': return 'ï';
                //
                case 'ñ': return '¸';
                case 'å': return 'õ';
                case 'ä': return 'ô';
                case 'î': return 'ş';
                default:
                    return c;
            }
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return (count - index) * 2;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return (count - index) / 2;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int result = Unicode.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = DecodeChar(chars[i]);
            }
            return result;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount * 2;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount / 2;
        }
    }
}
