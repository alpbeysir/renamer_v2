using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static Filter[] filters = { new ReplaceFilter(), new TVSeriesFilter() };
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.Title = "İsimlendirici";

        #region Filter Selection
        Console.WriteLine("Filtre seçin");
        for (int i = 0; i < filters.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + filters[i].Name);
        }

        int selected;

        if (!int.TryParse(Console.ReadLine(), out selected)) 
            FilterSelectError();

        if (selected <= 0 || selected > filters.Length)
            FilterSelectError();

        Filter filter = filters[selected - 1];
        #endregion Filter Selection

        //Get files
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
        if (files.Length <= 0)
            Restart();

        //Remove blacklisted files
        GlobalFilter globalFilter = new GlobalFilter();
        files = globalFilter.Run(files);

        //Remove extensions
        string[] exts = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            exts[i] = Path.GetExtension(files[i]);
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }

        //Run filter
        Console.Clear();
        string[] result = filter.Run(files);
        Console.Clear();

        //Remove whitespace
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = result[i].Trim();
        }

        //Display changes
        for (int i = 0; i < files.Length; i++)
        {
            Console.WriteLine(files[i] + " --> " + result[i]);
        }

        //Confirmation
        Console.WriteLine();
        Console.WriteLine("Bu değişiklikleri onaylıyor musunuz? (E - Evet, H - Hayır)");
        if (Console.ReadLine() == "E")
        {
            //Rename
            for (int i = 0; i < result.Length; i++)
            {
                File.Move(files[i] + exts[i], result[i] + exts[i]);
            }
        }

        //Loop
        Restart();
    }

    static void FilterSelectError()
    {
        Console.WriteLine("Düzgün girin!");
        Console.ReadKey();
        Restart();
    }
    static void Restart()
    {
        Console.Clear();
        Main();
        Environment.Exit(0);
    }
}