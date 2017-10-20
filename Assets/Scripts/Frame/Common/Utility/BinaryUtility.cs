﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BinaryUtility : GameBase
{
	protected static Encoding ENCODING_GB2312;
	public void init()
	{
		ENCODING_GB2312 = Encoding.GetEncoding("gb2312");
	}
	// 计算 16进制的c中1的个数
	public static int crc_check(byte c)
	{
		int count = 0;
		int bitCount = sizeof(char) * 8;
		for (int i = 0; i < bitCount; ++i)
		{
			if ((c & (0x01 << i)) > 0)
			{
				++count;
			}
		}
		return count;
	}
	public static bool readBool(byte[] buffer, ref int curIndex)
	{
		if (buffer.Length < 1)
		{
			return false;
		}
		bool value = (0xff & buffer[curIndex++]) != 0;
		return value;
	}
	public static byte readByte(byte[] buffer, ref int curIndex)
	{
		if (buffer.Length < 1)
		{
			return 0;
		}
		byte byte0 = (byte)(0xff & buffer[curIndex++]);
		return byte0;
	}
	public static short readShort(byte[] buffer, ref int curIndex, bool inverse = false)
	{
		if (buffer.Length < 2)
		{
			return 0;
		}
		int byte0 = (int)(0xff & buffer[curIndex++]);
		int byte1 = (int)(0xff & buffer[curIndex++]);
		if(inverse)
		{
			short finalValue = (short)((byte1 << (8 * 0)) | (byte0 << (8 * 1)));
			return finalValue;
		}
		else
		{
			short finalValue = (short)((byte1 << (8 * 1)) | (byte0 << (8 * 0)));
			return finalValue;
		}
	}
	public static int readInt(byte[] buffer, ref int curIndex, bool inverse = false)
	{
		if (buffer.Length < 4)
		{
			return 0;
		}
		int byte0 = (int)(0xff & buffer[curIndex++]);
		int byte1 = (int)(0xff & buffer[curIndex++]);
		int byte2 = (int)(0xff & buffer[curIndex++]);
		int byte3 = (int)(0xff & buffer[curIndex++]);
		if (inverse)
		{
			int finalInt = (int)((byte3 << (8 * 0)) | (byte2 << (8 * 1)) | (byte1 << (8 * 2)) | (byte0 << (8 * 3)));
			return finalInt;
		}
		else
		{
			int finalInt = (int)((byte3 << (8 * 3)) | (byte2 << (8 * 2)) | (byte1 << (8 * 1)) | (byte0 << (8 * 0)));
			return finalInt;
		}
	}
	public static float readFloat(byte[] buffer, ref int curIndex, bool inverse = false)
	{
		if (buffer.Length < 4)
		{
			return 0.0f;
		}
		byte[] floatBuffer = new byte[4];
		if(inverse)
		{
			floatBuffer[3] = buffer[curIndex++];
			floatBuffer[2] = buffer[curIndex++];
			floatBuffer[1] = buffer[curIndex++];
			floatBuffer[0] = buffer[curIndex++];
		}
		else
		{
			floatBuffer[0] = buffer[curIndex++];
			floatBuffer[1] = buffer[curIndex++];
			floatBuffer[2] = buffer[curIndex++];
			floatBuffer[3] = buffer[curIndex++];
		}
		return bytesToFloat(floatBuffer);
	}
	public static void readBools(byte[] buffer, ref int index, bool[] destBuffer)
	{
		int shortCount = destBuffer.Length;
		for (int i = 0; i < shortCount; ++i)
		{
			destBuffer[i] = readBool(buffer, ref index);
		}
	}
	public static bool readBytes(byte[] buffer, ref int index, byte[] destBuffer, int bufferSize = -1, int destBufferSize = -1, int readSize = -1)
	{
		if (bufferSize == -1)
		{
			bufferSize = buffer.Length;
		}
		if (destBufferSize == -1)
		{
			destBufferSize = destBuffer.Length;
		}
		if (readSize == -1)
		{
			readSize = destBuffer.Length;
		}
		if (destBufferSize < readSize || readSize + index > bufferSize)
		{
			return false;
		}
		memcpy(destBuffer, buffer, 0, index, readSize);
		index += readSize;
		return true;
	}
	public static void readShorts(byte[] buffer, ref int index, short[] destBuffer)
	{
		int shortCount = destBuffer.Length;
		for(int i = 0; i < shortCount; ++i)
		{
			destBuffer[i] = readShort(buffer, ref index);
		}
	}
	public static void readInts(byte[] buffer, ref int index, int[] destBuffer)
	{
		int shortCount = destBuffer.Length;
		for (int i = 0; i < shortCount; ++i)
		{
			destBuffer[i] = readInt(buffer, ref index);
		}
	}
	public static void readFloats(byte[] buffer, ref int index, float[] destBuffer)
	{
		int shortCount = destBuffer.Length;
		for (int i = 0; i < shortCount; ++i)
		{
			destBuffer[i] = readFloat(buffer, ref index);
		}
	}
	public static bool writeBool(byte[] buffer, ref int index, bool value)
	{
		if (buffer.Length < 1)
		{
			return false;
		}
		buffer[index++] = (byte)(value ? 1 : 0);
		return true;
	}
	public static bool writeByte(byte[] buffer, ref int index, byte value)
	{
		if (buffer.Length < 1)
		{
			return false;
		}
		buffer[index++] = value;
		return true;
	}
	public static bool writeShort(byte[] buffer, ref int index, short value)
	{
		if (buffer.Length < 2)
		{
			return false;
		}
		buffer[index++] = (byte)((0x00ff & value) >> 0);
		buffer[index++] = (byte)((0xff00 & value) >> 8);
		return true;
	}
	public static bool writeInt(byte[] buffer, ref int index, int value)
	{
		if (buffer.Length < 4)
		{
			return false;
		}
		buffer[index++] = (byte)((0x000000ff & value) >> 0);
		buffer[index++] = (byte)((0x0000ff00 & value) >> 8);
		buffer[index++] = (byte)((0x00ff0000 & value) >> 16);
		buffer[index++] = (byte)((0xff000000 & value) >> 24);
		return true;
	}

	public static bool writeFloat(byte[] buffer, ref int index, float value)
	{
		if (buffer.Length < 4)
		{
			return false;
		}
		byte[] valueByte = toBytes(value);
		for (int i = 0; i < 4; ++i)
		{
			buffer[index++] = valueByte[i];
		}
		return true;
	}
	public static bool writeBools(byte[] buffer, ref int index, bool[] sourceBuffer)
	{
		bool ret = true;
		int floatCount = sourceBuffer.Length;
		for (int i = 0; i < floatCount; ++i)
		{
			ret = ret && writeBool(buffer, ref index, sourceBuffer[i]);
		}
		return ret;
	}
	public static bool writeBytes(byte[] buffer, ref int index, byte[] sourceBuffer, int bufferSize = -1, int sourceBufferSize = -1, int writeSize = -1)
	{
		if (bufferSize == -1)
		{
			bufferSize = buffer.Length;
		}
		if (sourceBufferSize == -1)
		{
			sourceBufferSize = sourceBuffer.Length;
		}
		if (writeSize == -1)
		{
			writeSize = sourceBuffer.Length;
		}
		if (writeSize > sourceBufferSize || writeSize + index > bufferSize)
		{
			return false;
		}
		memcpy(buffer, sourceBuffer, index, 0, writeSize);
		index += writeSize;
		return true;
	}
	public static bool writeShorts(byte[] buffer, ref int index, short[] sourceBuffer)
	{
		bool ret = true;
		int floatCount = sourceBuffer.Length;
		for (int i = 0; i < floatCount; ++i)
		{
			ret = ret && writeShort(buffer, ref index, sourceBuffer[i]);
		}
		return ret;
	}
	public static bool writeInts(byte[] buffer, ref int index, int[] sourceBuffer)
	{
		bool ret = true;
		int floatCount = sourceBuffer.Length;
		for (int i = 0; i < floatCount; ++i)
		{
			ret = ret && writeInt(buffer, ref index, sourceBuffer[i]);
		}
		return ret;
	}
	public static bool writeFloats(byte[] buffer, ref int index, float[] sourceBuffer)
	{
		bool ret = true;
		int floatCount = sourceBuffer.Length;
		for(int i = 0; i < floatCount; ++i)
		{
			ret = ret && writeFloat(buffer, ref index, sourceBuffer[i]);
		}
		return ret;
	}
	public static string bytesToHEXString(byte[] byteList, bool addSpace = true, bool upperOrLower = true)
	{
		string byteString = "";
		int byteCount = byteList.Length;
		for (int i = 0; i < byteCount; ++i)
		{
			if (addSpace)
			{
				byteString += byteToHEXString(byteList[i], upperOrLower) + " ";
			}
			else
			{
				byteString += byteToHEXString(byteList[i], upperOrLower);
			}
		}
		if (addSpace)
		{
			byteString = byteString.Substring(0, byteString.Length - 1);
		}
		return byteString;
	}
	public static string byteToHEXString(byte value, bool upperOrLower = true)
	{
		string hexString = "";
		char[] hexChar = null;
		if (upperOrLower)
		{
			hexChar = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
		}
		else
		{
			hexChar = new char[] { 'a', 'b', 'c', 'd', 'e', 'f' };
		}
		int high = value / 16;
		int low = value % 16;
		if (high < 10)
		{
			hexString += (char)('0' + high);
		}
		else
		{
			hexString += hexChar[high - 10];
		}
		if (low < 10)
		{
			hexString += (char)('0' + low);
		}
		else
		{
			hexString += hexChar[low - 10];
		}
		return hexString;
	}
	public static void memcpy<T>(T[] dest, T[] src, int destOffset, int srcOffset, int count)
	{
		for (int i = 0; i < count; ++i)
		{
			dest[destOffset + i] = src[srcOffset + i];
		}
	}
	public static void memmove(short[] data, int start0, int start1, int count)
	{
		if (start1 > start0 && (start0 + count > start1))
		{
			// 如果源地址与目标地址有重叠,并且源地址在前面,则从后面往前拷贝字节
			for (int i = 0; i < count; ++i)
			{
				data[count - i - 1 + start0] = data[count - i - 1 + start1];
			}
		}
		else
		{
			for (int i = 0; i < count; ++i)
			{
				data[i + start0] = data[i + start1];
			}
		}
	}
	public static void memset<T>(T[] p, T value, int length = -1)
	{
		if(length == -1)
		{
			length = p.Length;
		}
		for (int i = 0; i < length; ++i)
		{
			p[i] = value;
		}
	}
	public static byte[] toBytes(byte value)
	{
		return BitConverter.GetBytes(value);
	}
	public static byte[] toBytes(short value)
	{
		return BitConverter.GetBytes(value);
	}
	public static byte[] toBytes(int value)
	{
		return BitConverter.GetBytes(value);
	}
	public static byte[] toBytes(float value)
	{
		return BitConverter.GetBytes(value);
	}
	public static byte bytesToByte(byte[] array)
	{
		return array[0];
	}
	public static short bytesToShort(byte[] array)
	{
		return BitConverter.ToInt16(array, 0);
	}
	public static int bytesToInt(byte[] array)
	{
		return BitConverter.ToInt32(array, 0);
	}
	public static float bytesToFloat(byte[] array)
	{
		return BitConverter.ToSingle(array, 0);
	}
	public static byte[] stringToBytes(string str)
	{
		return stringToBytes(str, ENCODING_GB2312);
	}
	public static byte[] stringToBytes(string str, Encoding encoding)
	{
		return encoding.GetBytes(str);
	}
	public static string bytesToString(byte[] bytes)
	{
		return bytesToString(bytes, Encoding.Default);
	}
	public static string bytesToString(byte[] bytes, Encoding encoding)
	{
		return removeLastZero(encoding.GetString(bytes));
	}
	public static string convertStringFormat(string str, Encoding source, Encoding target)
	{
		return bytesToString(stringToBytes(str, source), target);
	}
	public static string UTF8ToUnicode(string str)
	{
		return convertStringFormat(str, Encoding.UTF8, Encoding.Unicode);
	}
	public static string UTF8ToGB2312(string str)
	{
		return convertStringFormat(str, Encoding.UTF8, ENCODING_GB2312);
	}
	public static string UnicodeToUTF8(string str)
	{
		return convertStringFormat(str, Encoding.Unicode, Encoding.UTF8);
	}
	public static string UnicodeToGB2312(string str)
	{
		return convertStringFormat(str, Encoding.Unicode, ENCODING_GB2312);
	}
	public static string GB2312ToUTF8(string str)
	{
		return convertStringFormat(str, ENCODING_GB2312, Encoding.UTF8);
	}
	public static string GB2312ToUnicode(string str)
	{
		return convertStringFormat(str, ENCODING_GB2312, Encoding.Unicode);
	}
	// 字节数组转换为字符串时,末尾可能会带有数字0,此时在字符串比较时会出现错误,所以需要移除字符串末尾的0
	public static string removeLastZero(string str)
	{
		int strLen = str.Length;
		int newLen = strLen;
		for(int i = 0; i < strLen; ++i)
		{
			if(str[i] == 0)
			{
				newLen = i;
				break;
			}
		}
		str = str.Substring(0, newLen);
		return str;
	}
}