using System;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Drawing;

public class LazyConvert
{
    static public int StringToDecInt(string str)
    {
        int value = 0;
        char[] cStr = str.ToCharArray();
        for (int i = 0; i < cStr.Length; i++)
        {
            int number = Convert.ToInt32(cStr[cStr.Length - 1 - i]);
            value += number * (int)Math.Pow(10, i);
        }

        return value;
    }

    static public int HexStringToDecInt(string hex)
    {
        int value = 0;
        char[] cHex = hex.ToCharArray();
        for (int i = 0; i < cHex.Length; i++)
        {
            switch (cHex[cHex.Length - 1 - i])
            {
                case '0':
                    {
                        /* do nothing */
                    }
                    break;
                case '1':
                    {
                        value += 1 * (int)Math.Pow(16, i);
                    }
                    break;
                case '2':
                    {
                        value += 2 * (int)Math.Pow(16, i);
                    }
                    break;
                case '3':
                    {
                        value += 3 * (int)Math.Pow(16, i);
                    }
                    break;
                case '4':
                    {
                        value += 4 * (int)Math.Pow(16, i);
                    }
                    break;
                case '5':
                    {
                        value += 5 * (int)Math.Pow(16, i);
                    }
                    break;
                case '6':
                    {
                        value += 6 * (int)Math.Pow(16, i);
                    }
                    break;
                case '7':
                    {
                        value += 7 * (int)Math.Pow(16, i);
                    }
                    break;
                case '8':
                    {
                        value += 8 * (int)Math.Pow(16, i);
                    }
                    break;
                case '9':
                    {
                        value += 9 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'A':
                case 'a':
                    {
                        value += 10 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'B':
                case 'b':
                    {
                        value += 11 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'C':
                case 'c':
                    {
                        value += 12 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'D':
                case 'd':
                    {
                        value += 13 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'E':
                case 'e':
                    {
                        value += 14 * (int)Math.Pow(16, i);
                    }
                    break;
                case 'F':
                case 'f':
                    {
                        value += 15 * (int)Math.Pow(16, i);
                    }
                    break;
                default:
                    return -1;
            }
        }
        return value;
    }

    static public string DecIntToHexString(int dec)
    {
        string hexString = "";
        int remainder = 0;
        int times = 0;

        while (true)
        {
            times++;
            remainder = dec % 16;
            dec = dec / 16;
            if (remainder <= 9)
            {
                hexString = remainder.ToString() + hexString;
            }
            else if (remainder == 10)
            {
                hexString = "A" + hexString;
            }
            else if (remainder == 11)
            {
                hexString = "B" + hexString;
            }
            else if (remainder == 12)
            {
                hexString = "C" + hexString;
            }
            else if (remainder == 13)
            {
                hexString = "D" + hexString;
            }
            else if (remainder == 14)
            {
                hexString = "E" + hexString;
            }
            else if (remainder == 15)
            {
                hexString = "F" + hexString;
            }

            if (dec == 0 && times >= 2)
                break;
        }

        return hexString;
    }


    static public string FileNameAddSuffix(string filename, string suffix)
    {
        int pos = filename.LastIndexOf(".");
        if (pos > 0)
            filename = filename.Substring(0, pos);
        return filename + suffix;
    }

    static public void RevertEscape(ref string value)
    {
        if (value == null || value == "")
            return;
        value = value.Replace("&lt;", "<");
        value = value.Replace("&gt;", ">");
        value = value.Replace("&quot;", "\"");
        value = value.Replace("&apos;", "\'");
        value = value.Replace("&amp;", "&");
    }

    /*********************************************************************************************/

    static public int ToInt(string value)
    {
        int tmp = 0;
        try
        {
            tmp = Convert.ToInt32(value);
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message + " value: " + value);
        }
        return tmp;
    }

    static public int ToInt(bool value)
    {
        int tmp = 0;
        if (value)
            tmp = 1;

        return tmp;
    }

    static public Color ToColor(string value)
    {
        return Color.FromArgb(int.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier));
    }

    /// <summary>
    /// 傳入用逗號分割的字串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    static public int[] ToIntArray(string value)
    {
        return ToIntArray(value, ',');
    }

    static public int[] ToIntArray(string value, char separator)
    {
        int[] tmp = null;
        if (value == null || value == "")
            return tmp;
        string[] v = value.Split(separator);
        tmp = new int[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            tmp[i] = int.Parse(v[i]);
        }
    
        return tmp;
    }

    static public float ToFloat(string value)
    {
        float tmp = 0;
        tmp = Convert.ToSingle(value);
        return tmp;
    }

    static public float[] ToFloatArray(string value)
    {
        return ToFloatArray(value, ',');
    }

    static public float[] ToFloatArray(string value, char separator)
    {
        float[] tmp = null;
        if (value == null || value == "")
            return tmp;
        string[] v = value.Split(separator);
        tmp = new float[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            tmp[i] = float.Parse(v[i]);
        }
        
        return tmp;
    }

    static public bool ToBool(int value)
    {
        bool tmp = false;
        tmp = Convert.ToBoolean(value);
        
        return tmp;
    }

    static public bool ToBool(string value)
    {
        bool tmp = false;
        if (value == "1" || value == "y" || value == "true" || value == "True" || value == "TRUE")
        {
            tmp = true;
        }
        else if (value == "0" || value == "n" || value == "false" || value == "False" || value == "FALSE")
        {
            tmp = false;
        }
        else
        {
            tmp = Convert.ToBoolean(value);
        }
        
        return tmp;
    }

    static public DateTime ToDateTime(string value)
    {
        DateTime tmp = DateTime.MinValue;
        tmp = Convert.ToDateTime(value);
        return tmp;
    }

    //static public string ToDateTimeString(DateTime value)
    //{
    //    string time = value.Year + "-" + value.Month.ToString("00") + "-" + value.Day.ToString("00") + " " + value.Hour.ToString("00") + ":" + value.Minute.ToString("00") + ":" + value.Second.ToString("00");
    //    return time;
    //}

    static public void SetDateTimeStringFormat(string shortPattern, string longPattern)
    {
        CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.DateTimeFormat.ShortDatePattern = shortPattern; // ex: "yyyy-MM-dd";
        culture.DateTimeFormat.LongTimePattern = longPattern; // ex: "HH:mm:ss.fffffff";
        Thread.CurrentThread.CurrentCulture = culture;
    }

    /// <summary>
    /// 將秒換算成 時::分::秒
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static void ToDateTime(int second, out int h, out int m, out int s)
    {
        if (second < 0)
            second = 0;

        h = (int)(second / 3600.0f);
        int hs = h * 3600;
        m = (int)((second - hs) / 60.0f);
        int ms = m * 60;
        s = second - hs - ms;
    }

    static public object ToEnum(Type enumType, string value)
    {
        object tmp = null;
        
        tmp = Enum.Parse(enumType, value);
        
        return tmp;
    }

    static public Enum[] ToEnumArray(Type enumType, string value)
    {
        return ToEnumArray(enumType, value, ',');
    }

    static public Enum[] ToEnumArray(Type enumType, string value, char separator)
    {
        Enum[] tmp = null;
        
        if (value == null || value == "")
            return tmp;
        string[] v = value.Split(separator);
        tmp = new Enum[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            int intVal = 0;
            if (int.TryParse(v[i], out intVal))
                tmp[i] = Enum.ToObject(enumType, intVal) as Enum;
            else
                tmp[i] = Enum.Parse(enumType, v[i]) as Enum;
        }
    
        return tmp;
    }

    static public long ToLong(string value)
    {
        long tmp = 0;
        
        tmp = long.Parse(value);
        return tmp;
    }

    static public object ToProperty(Type type, string value)
    {
        if (type == typeof(int))
            return LazyConvert.ToInt(value);
        if (type == typeof(float))
            return LazyConvert.ToFloat(value);
        if (type == typeof(bool))
            return LazyConvert.ToBool(value);
        if (type == typeof(DateTime))
            return LazyConvert.ToDateTime(value);
        if (type == typeof(string))
            return value;
        if (type.IsEnum)
            return LazyConvert.ToEnum(type, value);
        if (type == typeof(long))
            return LazyConvert.ToLong(value);
        if (type == typeof(Color))
            return LazyConvert.ToColor(value);
        if (type.IsArray)
        {
            if (type.GetElementType() == typeof(int))
            {
                return ToIntArray(value);
            }
            if (type.GetElementType() == typeof(float))
            {
                return ToFloatArray(value);
            }
            if (type.GetElementType() == typeof(string))
            {
                return value.Split(',');
            }
        }
        if (type == typeof(Char))
        {
            if (value != null && value.Length > 0)
                return value.ToCharArray()[0];
            else
                return null;
        }

        return Activator.CreateInstance(type);
    }

    /// <summary>
    /// 是否為基本類型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static public bool IsPrimitive(Type type)
    {
        if (type == typeof(string) || type.IsEnum)
            return true;
        return type.IsPrimitive;
    }

    public class Description : Attribute
    {
        private string mText;
        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }
        public Description(string text)
        {
            Text = text;
        }
    }

    public static string GetDescription(Enum en)
    {
        Type type = en.GetType();
        MemberInfo[] memInfo = type.GetMember(en.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description), false);
            if (attrs != null && attrs.Length > 0)
                return ((Description)attrs[0]).Text;
        }
        return en.ToString();
    }

    /// <summary>
    /// 將字串加密成MD5
    /// </summary>
    /// <param name="strToEncrypt">要加密的資料</param>
    /// <returns>MD5 String</returns>
    static public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    static public string StringFormat(string format, params object[] args)
    {
        string temp = "";

        temp = string.Format(format, args);
        return temp;
    }
}
