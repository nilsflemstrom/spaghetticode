using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetticode
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string dir = null;
                var extensions = new List<string>();

                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("Welcome dear coder! With this short command you can count the number of lines in source files.");
                    Console.WriteLine("Do you really want to know?");
                    Console.WriteLine();
                    Console.WriteLine("Syntax:");
                    Console.WriteLine("spagehtticode <dir> <extension1> <extension2> ... <extensionN>");
                    Console.WriteLine();
                    Console.WriteLine("Example:");
                    Console.WriteLine("spagehtticode . .cs .cshtml .js");
                    return;
                }

                //Get directory
                try
                {
                    dir = args[0];
                    if (!Directory.Exists(dir))
                    {
                        Console.WriteLine("ERROR: Directory not found: " + dir);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("ERROR: " + exc.Message);
                }

                //Get extensions
                for (var i = 1; i < args.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(args[i]))
                    {
                        var extension = args[i].Trim();
                        if (extension == ".") continue;
                        if (!extension.StartsWith(".")) extension = "." + extension;

                        extensions.Add(extension);
                    }
                }

                //To store result
                var rowCountsPerExtension = new SortedDictionary<string, int>();

                //Recursive count
                Count(dir, rowCountsPerExtension, extensions);

                Console.WriteLine();
                Console.WriteLine();

                var columnWitdth1 = 20;
                var columnWitdth2 = 15;
                var total = 0;
                Console.WriteLine("|" + "".PadRight(columnWitdth1, '=') + "|" + "".PadRight(columnWitdth2, '=') + "|");
                Console.WriteLine("| " + "Extension".PadRight(columnWitdth1-1, ' ') + "| " + "Rows".PadRight(columnWitdth2-1, ' ') + "|");
                Console.WriteLine("|" + "".PadRight(columnWitdth1, '=') + "|" + "".PadRight(columnWitdth2, '=') + "|");                                
                foreach (var extension in rowCountsPerExtension.Keys)
                {
                    total = total + rowCountsPerExtension[extension];
                    Console.WriteLine("| " + $"{extension}".PadRight(columnWitdth1-1, ' ') + "| " + $"{rowCountsPerExtension[extension]}".PadRight(columnWitdth2-1, ' ') + "|");
                }
                Console.WriteLine("|" + "".PadRight(columnWitdth1, '=') + "|" + "".PadRight(columnWitdth2, '=') + "|");
                Console.WriteLine("| " + $"Total".PadRight(columnWitdth1 - 1, ' ') + "| " + $"{total}".PadRight(columnWitdth2 - 1, ' ') + "|");
                Console.WriteLine("|" + "".PadRight(columnWitdth1, '=') + "|" + "".PadRight(columnWitdth2, '=') + "|");

            }
            catch(Exception exc)
            {
                Console.WriteLine("ERROR: " + exc.ToString());
            }




        }

        static void Count(string dir, SortedDictionary<string, int> rowCountsPerExtension, List<string> extensions)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    return;
                }
            }
            catch
            {
                if (!rowCountsPerExtension.ContainsKey("Too long dirpath")) rowCountsPerExtension["Too long dirpath"] = 1;
                else rowCountsPerExtension["Too long dirpath"]++;
                return;
            }

            try
            {
                foreach (var path in Directory.GetFiles(dir))
                {
                    try
                    {
                        foreach (var extension in extensions)
                        {
                            if (path.EndsWith(extension))
                            {
                                var lineCount = 0;
                                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                                {
                                    using (var sr = new StreamReader(fs))
                                    {
                                        while (sr.ReadLine() != null)
                                        {
                                            lineCount++;
                                        }
                                    }
                                }
                                Console.Write($"{path} ");
                                Console.WriteLine($"{lineCount}");
                                if (!rowCountsPerExtension.ContainsKey(extension)) rowCountsPerExtension[extension] = lineCount;
                                else rowCountsPerExtension[extension] = rowCountsPerExtension[extension] + lineCount;
                            }
                        }
                    }
                    catch 
                    {
                        if (!rowCountsPerExtension.ContainsKey("Too long filepath")) rowCountsPerExtension["Too long filepath"] = 1;
                        else rowCountsPerExtension["Too long filepath"]++;
                        //Ignore
                    }
                }
            }
            catch
            {
                //Ignore
            }


            foreach (var path in Directory.GetDirectories(dir))
            {
                try
                {
                    Count(path, rowCountsPerExtension, extensions);
                }
                catch
                {
                    if (!rowCountsPerExtension.ContainsKey("Too long dirpath")) rowCountsPerExtension["Too long dirpath"] = 1;
                    else  rowCountsPerExtension["Too long dirpath"]++;
                }
                
            }
        }




    }
}
