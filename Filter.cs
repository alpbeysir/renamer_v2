using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

abstract class Filter
{
    public abstract string Name { get; }
    public abstract string[] Run(string[] names);

}
class GlobalFilter : Filter
{
    public override string Name => "GlobalFilter";

    readonly string[] extensionBlacklist = { ".exe", ".config", ".pdb" };

    public override string[] Run(string[] names)
    {
        List<string> temp = new List<string>(names);
        for (int i = 0; i < names.Length; i++)
        {
            string ext = Path.GetExtension(names[i]);
            for (int n = 0; n < extensionBlacklist.Length; n++)
            {
                if (ext == extensionBlacklist[n])
                {
                    temp.Remove(names[i]);
                }
            }
        }
        return temp.ToArray();
    }
}
class ReplaceFilter : Filter
{
    public override string Name => "Değiştirme Filtresi";

    public override string[] Run(string[] names)
    {
        Console.WriteLine("Değiştirmek istediğiniz yazıyı girin, ardından Enter'a basın");
        string target = Console.ReadLine();
        Console.Write("Girdiğiniz şey ne ile değiştirilsin? Girin ve Enter'a basın\n(Hiçbir şey girmezseniz yukarıda girdiğinizi siler)\n");
        string source = Console.ReadLine();

        string[] result = new string[names.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = names[i].Replace(target, source);
        }
        return result;
    }
}

class TVSeriesFilter : Filter
{
    public override string Name => "Dizi Filtresi";

    public override string[] Run(string[] names)
    {
        Console.WriteLine("Sezon numarası girin");
        string season = Console.ReadLine();
        Console.WriteLine("Kalıp girin (Örnek kalıp: \"Fringe {0}. Sezon {1}. Bölüm\")");
        string kalıp = Console.ReadLine();

        string[] result = new string[names.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = string.Format(kalıp, season, (i + 1));
        }
        return result;
    }
}