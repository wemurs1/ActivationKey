﻿/***************************************************************

•   File: Base16Encoding.cs

•   Description.

    Base16Encoding is designed to work with hexadecimal data and
    implements methods for encoding and decoding data.

***************************************************************/

using static System.InternalTools;

namespace System.Text
{
    internal sealed class Base16Encoding : InternalBaseEncoding
    {
        public override string EncodingName => "base-16";

        public Base16Encoding()
          : base(0)
        {
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            Validate(chars, charIndex, charCount, bytes, byteIndex);
            charCount = GetCharCount(chars, charIndex, charCount, bytes, byteIndex);
            int startByteIndex = byteIndex;
            int endCharIndex = charIndex + charCount;
            while (charIndex < endCharIndex)
            {
                byte value = (byte)((GetValue(chars[charIndex++]) << 4) + GetValue(chars[charIndex++]));
                bytes[byteIndex++] = value;
            }

            return byteIndex - startByteIndex;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            Validate(bytes, byteIndex, byteCount, chars, charIndex);
            byteCount = GetByteCount(bytes, byteIndex, byteCount, chars, charIndex);
            int startCharIndex = charIndex;
            int endByteIndex = byteIndex + byteCount;
            while (byteIndex < endByteIndex)
            {
                byte value = bytes[byteIndex++];
                chars[charIndex++] = GeDigit(value / 16);
                chars[charIndex++] = GeDigit(value % 16);
            }
            return charIndex - startCharIndex;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount / 2;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount * 2;
        }

        private static int GetValue(char digit)
        {
            if (digit > 0x2F && digit < 0x3A)
                return digit - 48;
            if (digit > 0x40 && digit < 0x47)
                return digit - 55;
            if (digit > 0x60 && digit < 0x67)
                return digit - 87;
            throw new ArgumentOutOfRangeException(nameof(digit), digit, GetResourceString("Format_BadBase"));
        }

        private static char GeDigit(int value)
        {
            return value >= 0xA ? (char)(value + 0x37) : (char)(value + 0x30);
        }

        public override object Clone()
        {
            return new Base16Encoding();
        }

   }
}