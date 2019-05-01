///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ReqsTests.cs - Test classes for Project3                                                                          //       
// ver 2.1                                                                                                                               //
// Language:    C#, Visual Studio 2017, .Net Framework 4.0                                               //
// Platform:    Lenovo 80KF, Win 10, SP 1                                                                            //
// Application: Pr#3 , CSE681, Fall 2018                                                                              //
// Author:      Yueliu Fan, Syracuse University, StudentID: 759015947                                //
//              (315) 455-0271, yfan33@syr.edu                                                                        //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lexer;
using CsGraph;
using StrongComponent;

namespace DepAnalysis
{
  ///////////////////////////////////////////////////////////////////
  // ReqDisplay class
  // - display methods for Requirements testing

  class ReqDisplay
  {
    public static void title(string tle)
    {
      Console.Write("\n  {0}", tle);
      Console.Write("\n {0}", new string('-', tle.Length + 2));
    }
    public static void message(string msg)
    {
      Console.Write("\n  {0}\n", msg);
    }
    public static void showSet(HashSet<string> set, string msg = "")
    {
      if (msg.Length > 0)
        Console.Write("\n  {0}\n  ", msg);
      else
        Console.Write("\n  Set:\n  ");
      foreach (var tok in set)
      {
        Console.Write("\"{0}\" ", tok);
      }
      Console.Write("\n");
    }

    public static void showList(List<string> lst, string msg = "")
    {
      if (msg.Length > 0)
        Console.Write("\n  {0}\n  ", msg);
      else
        Console.Write("\n  List:\n  ");
      int count = 0;
      foreach (var tok in lst)
      {
        Console.Write("\"{0}\" ", tok);
        if(++count == 10)
        {
          count = 0;
          Console.Write("\n  ");
        }
      }
      Console.Write("\n");
    }
  }
 


  ///////////////////////////////////////////////////////////////////
  // TestReq3 class

  class TestReq3 : ITest
  {
    public string name { get; set; } = "Test Req3";
    public string path { get; set; } = "../../";
    public bool result { get; set; } = false;
    void onFile(string filename)
    {
      Console.Write("\n    {0}", filename);
      result = true;
    }
    void onDir(string dirname)
    {
      Console.Write("\n  {0}", dirname);
    }
        public bool doTest()
    {
      ReqDisplay.title("Requirement #3");
      ReqDisplay.message("C# packages: Toker, SemiExp, TypeTable, TypeAnalysis, DepAnalysis, Graph, StrongComponent");
      FileUtilities.Navigate nav = new FileUtilities.Navigate();
            nav.Add("*.cs");
    //nav.newDir += new FileUtilities.Navigate.newDirHandler(onDir);
    nav.newFile += new FileUtilities.Navigate.newFileHandler(onFile);
    path = "../../../Toker";
      nav.go(path, false);
      path = "../../../SemiExp";
      nav.go(path, false);
      path = "../../../TypeTable";
      nav.go(path, false);
      path = "../../../TypeAnalysis";
      nav.go(path, false);
      path = "../../../DepAnalysis";
      nav.go(path, false);
      path = "../../../Graph";
      nav.go(path, false);
      path = "../../../StrongComponent";
      nav.go(path, false);
      return result;
    }
  }
  ///////////////////////////////////////////////////////////////////
  // TestReq4 class

  class TestReq4 : ITest
  {
    public string name { get; set; } = "Test Req4";
    public string path { get; set; } = "../../";
    public bool result { get; set; } = false;
        void onFile(string filename)
        {
            Console.Write("\n    {0}", filename);
            result = true;
        }
        void onDir(string dirname)
        {
            Console.Write("\n  {0}", dirname);
        }
    public bool doTest()
    {
     ReqDisplay.title("Requirement #4");
     ReqDisplay.message("All C# files to evaluate the dependencies");
     FileUtilities.Navigate nav = new FileUtilities.Navigate();
     nav.Add("*.cs");
     nav.newDir += new FileUtilities.Navigate.newDirHandler(onDir);
     nav.newFile += new FileUtilities.Navigate.newFileHandler(onFile);
     path = "../../TestCase"; nav.go(path, false);
     return result;
    }
  }
  ///////////////////////////////////////////////////////////////////
  // TestReq5 class

  class TestReq5 : ITest
  {
    public string name { get; set; } = "Test Req5";
    public string fileSpec { get; set; } = "../../../DemoExecutive";
    public bool result { get; set; } = true;
    public bool doTest()
    {
      ReqDisplay.title("Requirement #5");
      ReqDisplay.message("Identification of all the user-defined types");

       string[] args = new string[] { "../../TestCase" };
       DepAnalysis .ShowCommandLine(args);

        List<string> files = DepAnalysis.ProcessCommandline(args);

        Repository rep = new Repository();
        rep.semi = Factory.create();

        DepAnalysis.typeAnalysis(args, files, rep);

        Console.Write("\n  Type  Analysis");
        Console.Write("\n ----------------------------");

        TypeTable typeTable = rep.typeTable;
        Display.showTypeTable(typeTable);
        return result;
    }
  }

///////////////////////////////////////////////////////////////////
// TestReq6 class

class TestReq6 : ITest
{
    public string name { get; set; } = "Test Req6";
    public string fileSpec { get; set; } = "../../../DemoExecutive";
    public bool result { get; set; } = true;
    public bool doTest()
    {
        ReqDisplay.title("Requirement #6");
        ReqDisplay.message("Find all strong components");

        string[] args = new string[] { "../../TestCase" };
        DepAnalysis.ShowCommandLine(args);
        Repository rep = new Repository();
        rep.semi = Factory.create();

            List<string> files = DepAnalysis.ProcessCommandline(args);
            DepAnalysis.typeAnalysis(args, files, rep);
            DepAnalysis.depAnalysis(args, files, rep);
            Display.showDependency(rep.depGraph);
            List<List<CsNode<string, string>>> strongcomponent = TarjanCycleDetect.DetectCycle(rep.depGraph);
            Display.showStrongcomponent(strongcomponent);
            return result;
    }

}
    ///////////////////////////////////////////////////////////////////
    // TestReq7 class

    class TestReq7 : ITest
    {
        public string name { get; set; } = "Test Req7";
        public string path { get; set; } = "../../";
        public bool result { get; set; } = false;
        public bool doTest()
        {
            ReqDisplay.title("Requirement #7");
            ReqDisplay.message("Display the results in a well formated area of the output");
            ReqDisplay.message("Reference to Requirement #8");
            return result;
        }
    }


    ///////////////////////////////////////////////////////////////////
    // TestReq8 class

    class TestReq8 : ITest
    {
        public string name { get; set; } = "Test Req8";
        public string path { get; set; } = "../../";
        public bool result { get; set; } = false;
        public bool doTest()
        {
            ReqDisplay.title("Requirement #8");
            ReqDisplay.message("Test all the special cases for two packages");
            string[] args = new string[] { "../../TestCase" };
            DepAnalysis.ShowCommandLine(args);

            List<string> files = DepAnalysis.ProcessCommandline(args);

            Repository rep = new Repository();
            rep.semi = Factory.create();
            Console.Write("\n  Type  Analysis");
            Console.Write("\n ----------------------------");

            DepAnalysis.typeAnalysis(args, files, rep);
           
            TypeTable typeTable = rep.typeTable;
            Display.showTypeTable(typeTable);

            DepAnalysis.depAnalysis(args, files, rep);

            Display.showDependency(rep.depGraph);
            List<List<CsNode<string, string>>> strongcomponent = TarjanCycleDetect.DetectCycle(rep.depGraph);
            Display.showStrongcomponent(strongcomponent);
            return result;
        }
    }
}
