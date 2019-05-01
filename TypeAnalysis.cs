////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// TypeAnalysis.cs  -  Finds all the types defined in each of a collection of C# source files. //       
// ver 2.1                                                                                                                                //
// Language:    C#, Visual Studio 2017, .Net Framework 4.0                                                //
// Platform:    Lenovo 80KF, Win 10, SP 1                                                                             //
// Application: Pr#3 , CSE681, Fall 2018                                                                               //
// Author:      Yueliu Fan, Syracuse University, StudentID: 759015947                                 //
//              (315) 455-0271, yfan33@syr.edu                                                                         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * ==================
 * TypeAnalysis Finds all the types defined in each of a collection
 * of C# source files. It does this by building rules to detect type 
 * definitions - classes, structs, enums, delegate , or interface
 *
 */
/*
 * Build Process
 * =============
 * Required Files:
 *   TypeAnalysis.cs
 * 
 * Compiler Command:
 *   devenv Project2.sln /ReBuild
 *   
 * Run Command:
 *   cd .\DemoExecutive\bin\Debug\
 *   .\DemoExecutive.exe
 * 
 * Maintenance History
 * ===================
 * ver 1.0 : 27 October 18
 *   - Add TypeAnalysis Class
 * 
 */


using Lexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepAnalysis
{
    public class TypeAnalysis
    {
        private List<IRule> Rules;

        public TypeAnalysis()
        {
            Rules = new List<IRule>();
        }
        public void add(IRule rule)
        {
            Rules.Add(rule);
        }
        public void parse(Lexer.ITokenCollection semi)
        {
            // Note: rule returns true to tell parser to stop
            //       processing the current semiExp

            Display.displaySemiString(semi.ToString());

            foreach (IRule rule in Rules)
            {
                if (rule.test(semi))
                    break;
            }
        }
    

        //----< process commandline to get file references >-----------------

        static List<string> ProcessCommandline(string[] args)
        {
            List<string> files = new List<string>();
            if (args.Length == 0)
            {
                Console.Write("\n  Please enter file(s) to analyze\n\n");
                return files;
            }
            string path = args[0];
            path = Path.GetFullPath(path);
            for (int i = 1; i < args.Length; ++i)
            {
                string filename = Path.GetFileName(args[i]);
                files.AddRange(Directory.GetFiles(path, filename));
            }
            return files;
        }

        static void ShowCommandLine(string[] args)
        {
            Console.Write("\n  Commandline args are:\n  ");
            foreach (string arg in args)
            {
                Console.Write("  {0}", arg);
            }
            Console.Write("\n  current directory: {0}", System.IO.Directory.GetCurrentDirectory());
            Console.Write("\n");
        }

      

        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Parser");
            Console.Write("\n ======================\n");

            ShowCommandLine(args);

            List<string> files = ProcessCommandline(args);

            //Repository rep = Repository.getInstance();
            Repository rep = new Repository();
            rep.semi = Factory.create();

            foreach (string file in files)
            {
                Console.Write("\n  Processing file {0}\n", System.IO.Path.GetFileName(file));

                if (!rep.semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", args[0]);
                    return;
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(rep);
                TypeAnalysis parser = builder.build();

                try
                {
                    while (rep.semi.get().Count > 0)
                        parser.parse(rep.semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }

                Console.Write("\n");

                rep.stack.clear();
                rep.semi.close();
            }

            Console.Write("\n  Type  Analysis");
            Console.Write("\n ----------------------------");

            TypeTable typeTable = rep.typeTable;
            Display.showTypeTable(typeTable);

            Console.Write("\n\n");
            Console.Read();
        }
    }
}
