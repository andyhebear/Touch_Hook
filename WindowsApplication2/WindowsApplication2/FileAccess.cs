using System.Runtime.InteropServices; 
using System.Text;
using System.Drawing;
public class IniFile
{
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
 
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    
    private string filepath;
    public IniFile(string filepath)
    {
        this.filepath = filepath;
    }
    
    public void WriteIni(string section,string key, object val)
    {
        string value = "";
        if (val is Color)
            value = ((Color)val).ToArgb().ToString("X");
        else
            value = val.ToString();
        WritePrivateProfileString(section, key, value, filepath);
    }
    
    public string ReadIni(string section,string key)
    {
        StringBuilder temp = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", temp, 255, filepath);
        return temp.ToString();
    }


    public T ReadIni<T>(string section, string key)
    {
        
        StringBuilder temp = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", temp, 255, filepath);
        if (string.IsNullOrEmpty(temp.ToString()))
            return default(T);
        return (T)LazyConvert.ToProperty(typeof(T), temp.ToString());
    }
    
}