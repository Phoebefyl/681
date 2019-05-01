///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DemoReqs.cs - Demonstrate Project #3 Requirements                                                   //
// ver 1.0                                                                                                                               //
// Language:    C#, Visual Studio 2017, .Net Framework 4.0                                               //
// Platform:    Lenovo 80KF, Win 10, SP 1                                                                            //
// Application: Pr#3 , CSE681, Fall 2018                                                                              //
// Author:      Yueliu Fan, Syracuse University, StudentID: 759015947                                //
//              (315) 455-0271, yfan33@syr.edu                                                                        //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
 * Package Operations:
 * -------------------
 * This package defines the following class:
 *   Executive:
 *   - uses DepAnalysis, TypeTable, TypeAnalysis, StrongComponent
 *   
 */
/* Required Files:
 *   Executive.cs
 *   StrongComponent.cs, DepAnalysis.cs, TypeAnalysis, TypeTable.cs
 *   IRulesAndActions.cs, RulesAndActions.cs, ScopeStack.cs, Elements.cs
 *   ITokenCollection.cs
 *   Display.cs
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 31 Oct 2018
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DepAnalysis
{
  using Lexer;

  class Executive
  {
    //----< process commandline to get file references >-----------------

    static List<string> ProcessCommandline(string[] args)
    {
      List<string> files = new List<string>();
      if (args.Length < 2)
      {
        Console.Write("\n  Please enter path and file(s) to analyze\n\n");
        return files;
      }
      string path = args[0];
      if (!Directory.Exists(path))
      {
        Console.Write("\n  invalid path \"{0}\"", System.IO.Path.GetFullPath(path));
        return files;
      }
      path = Path.GetFullPath(path);
      for (int i = 1; i < args.Length; ++i)
      {
        string filename = Path.GetFileName(args[i]);
        files.AddRange(Directory.GetFiles(path, filename));
      }
      return files;
    }

    bool testToker()
    {
      return false;
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
      Console.Write("\n  Demonstrating Project #3 Requirements");
      Console.Write("\n =======================================\n");

      TestHarness.Tester tester = new TestHarness.Tester();

      TestReq3 tr3 = new TestReq3();
      tester.add(tr3);

      TestReq4 tr4 = new TestReq4();
      tester.add(tr4);

      TestReq5 tr5 = new TestReq5();
      tester.add(tr5);

      TestReq6 tr6 = new TestReq6();
      tester.add(tr6);

      TestReq7 tr7 = new TestReq7();
      tester.add(tr7);

      TestReq8 tr8 = new TestReq8();
      tester.add(tr8);

      tester.execute();
      Console.Write("\n\n");
      Console.Read();
    }
  }
}
