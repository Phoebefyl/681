/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DepAnalysis.cs  -  Finds, for each file in a specified collection, all other files from the collection on which they depend   //       
// ver 2.1                                                                                                                                                                                 //
// Language:    C#, Visual Studio 2017, .Net Framework 4.0                                                                                                 //
// Platform:    Lenovo 80KF, Win 10, SP 1                                                                                                                              //
// Application: Pr#3 , CSE681, Fall 2018                                                                                                                                //
// Author:      Yueliu Fan, Syracuse University, StudentID: 759015947                                                                                  //
//              (315) 455-0271, yfan33@syr.edu                                                                                                                          //
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * ==================
 * DepAnalysis class manages file information generated during
 * type-based code analysis, in its dependency analysis phase.
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
using StrongComponent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsGraph;
//using FileUtilities;

namespace DepAnalysis
{
    public class DepAnalysis
    {
        private List<IRule> Rules;

        public DepAnalysis()
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
        public static List<string> ProcessCommandline(string[] args)
        {
            List<string> files = new List<string>();
            if (args.Length < 1)
            {
                Console.Write("\n  Please enter path to analyze\n\n");
                return files;
            }
            string path = args[0];
            if (!Directory.Exists(path))
            {
                Console.Write("\n  invalid path \"{0}\"", Path.GetFullPath(path));
                return files;
            }
            path = Path.GetFullPath(path);
            string patt = "*.cs";
            files = find(path, patt);

            return files;
        }

        public static List<string> find(string searchpath, string patt)
        {
            Console.Write("\n {0}", searchpath);

            string[] fileArray = Directory.GetFiles(searchpath, patt);
            List<string> fileList = fileArray.ToList();

            string[] dirArray = Directory.GetDirectories(searchpath);
            foreach (string dir in dirArray)
            {
                if (dir == "." || dir == "..") continue;
                fileList.AddRange(find(dir, patt));
            }
            return fileList;
        }




        public static void ShowCommandLine(string[] args)
        {
            Console.Write("\n  Commandline args are:\n  ");
            foreach (string arg in args)
            {
                Console.Write("  {0}", arg);
            }
            Console.Write("\n  current directory: {0}", System.IO.Directory.GetCurrentDirectory());
            //Console.Write("\n");
        }

        public static void typeAnalysis(string[] args, List<string> files, Repository rep)
        {
            foreach (string file in files)
            {
                //Console.Write("\n  Processing file {0}", System.IO.Path.GetFileName(file));

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
               // Console.Write("\n");
                rep.stack.clear();
                rep.semi.close();

            }
        }

        public static void depAnalysis(string[] args, List<string> files, Repository rep)
        {
            foreach (string file in files)
            {

                if (!rep.semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", args[0]);
                    return;
                }

                BuildDepAnalyzer builder = new BuildDepAnalyzer(rep);
                DepAnalysis parser = builder.build();

                try
                {
                    while (rep.semi.get().Count > 0)
                        parser.parse(rep.semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }


               // Console.Write("\n");
            }
        }

        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Parser");
            Console.Write("\n ======================\n");

            ShowCommandLine(args);

            List<string> files = ProcessCommandline(args);

            Repository rep = new Repository();
            rep.semi = Factory.create();

            typeAnalysis(args, files, rep);

            Console.Write("\n  Type  Analysis");
            Console.Write("\n ----------------------------");

            TypeTable typeTable = rep.typeTable;
            Display.showTypeTable(typeTable);

            depAnalysis(args, files, rep);

            Display.showDependency(rep.depGraph);
            List<List<CsNode<string, string>>> strongcomponent = TarjanCycleDetect.DetectCycle(rep.depGraph);
            Display.showStrongcomponent(strongcomponent);

            Console.Write("\n\n");
            Console.Read();
        }
    }
}
