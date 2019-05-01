///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Display.cs  -  Manage Display properties                                                                          //       
// ver 2.1                                                                                                                               //
// Language:    C#, Visual Studio 2017, .Net Framework 4.0                                               //
// Platform:    Lenovo 80KF, Win 10, SP 1                                                                            //
// Application: Pr#3 , CSE681, Fall 2018                                                                              //
// Author:      Yueliu Fan, Syracuse University, StudentID: 759015947                                //
//              (315) 455-0271, yfan33@syr.edu                                                                        //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * ==================
 * Display manages static public properties used to control what is displayed and
 * provides static helper functions to send information to MainWindow and Console.
 * Display the form of TypeTable, Class Dependencies, or StrongComponent
 * 
 *
 * Build Process
 * =============
 * Required Files:
 *   FileMgr.cs
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
 * ver 1.1 : 28 Oct 2018
 * - TypeTable from display and adjust form
 * ver 1.0 : 29 Oct 2018
 * - Display class dependencies and StrongComponent
 * 
 */
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using CsGraph;

namespace DepAnalysis
{
  ///////////////////////////////////////////////////////////////////
  // StringExt static class
  // - extension method to truncate strings

  public static class StringExt
  {
    public static string Truncate(this string value, int maxLength)
    {
      if (string.IsNullOrEmpty(value)) return value;
      return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
  }

  static public class Display
  {
    static Display()
    {
      showFiles = true;
      showDirectories = true;
      showActions = false;
      showRules = false;
      useFooter = false;
      useConsole = false;
      goSlow = false;
      width = 33;
    }
    static public bool showFiles { get; set; }
    static public bool showDirectories { get; set; }
    static public bool showActions { get; set; }
    static public bool showRules { get; set; }
    static public bool showSemi { get; set; }
    static public bool useFooter { get; set; }
    static public bool useConsole { get; set; }
    static public bool goSlow { get; set; }
    static public int width { get; set; }

    ////----< display results of Code Analysis >-----------------------

    //static public void showMetricsTable(List<Elem> table)
    //{
    //  Console.Write(
    //      "\n  {0,10}  {1,25}  {2,5}  {3,5}  {4,5}  {5,5}",
    //      "category", "name", "bLine", "eLine", "size", "cmplx"
    //  );
    //  Console.Write(
    //      "\n  {0,10}  {1,25}  {2,5}  {3,5}  {4,5}  {5,5}",
    //      "--------", "----", "-----", "-----", "----", "-----"
    //  );
    //  foreach (Elem e in table)
    //  {
    //    /////////////////////////////////////////////////////////
    //    // Uncomment to leave a space before each defined type
    //    // if (e.type == "class" || e.type == "struct")
    //    //   Console.Write("\n");

    //    Console.Write(
    //      "\n  {0,10}  {1,25}  {2,5}  {3,5}  {4,5}  {5,5}",
    //      e.type, e.name, e.beginLine, e.endLine,
    //      e.endLine - e.beginLine + 1, e.endScopeCount - e.beginScopeCount + 1
    //    );
    //  }
    //}

        //----< display the TypeTable on Console >--------------------

        static public void showTypeTable(TypeTable typetable)
        {
            Console.Write(
                "\n  {0,10}  {1,25}  {2,35}  {3,10}",
                "category", "name", "namespace", "filename"
            );
            Console.Write(
                "\n  {0,10}  {1,25}  {2,35}  {3,10}",
                "--------", "----", "-----", "-----"
            );
            foreach (KeyValuePair<string, List<TypeItem>> e in typetable.table)
            {
                foreach (TypeItem typeItem in e.Value )
                {
                    StringBuilder nsStringBuilder = new StringBuilder();
                    nsStringBuilder.Append("[");
                    if (typeItem.namesp.Count != 0)
                        nsStringBuilder.Append(typeItem.namesp[0]);

                    for (int i = 1; i < typeItem.namesp.Count; i++)
                    {
                        nsStringBuilder.Append(":").Append(typeItem.namesp[i]);
                    }

                    nsStringBuilder.Append("]");

                    Console.Write(
                      "\n  {0,10}  {1,25}  {2,35}  {3,10}",
                      typeItem.category, e.Key, nsStringBuilder, Path.GetFileName(typeItem.file)
                    );
                }
            }
        }

        //----< display the Dependency between classes on Console >--------------------

        static public void showDependency(CsGraph<string,string> graph)
        {
            Console.WriteLine();
            foreach (var node in graph.adjList)
            {
                if (node.children.Count != 0)
                {
                    Console.Write("\n" + Path.GetFileName(node.nodeValue) + " depends on ");
                    foreach (var nodechild in node.children)
                    {
                        Console.Write(" " + Path.GetFileName(nodechild.targetNode.nodeValue));
                    }
                }else
                    Console.Write("\n" + Path.GetFileName(node.nodeValue) + " doesn't depend on any file.");
            }
        }

        //----< display Strongcomponents between packages on Console >--------------------

        static public void showStrongcomponent(List<List<CsNode<string, string>>> strongcomponent)
        {
            Console.WriteLine();
            for (int i=0;i<strongcomponent.Count;i++)
            {
                Console.Write("\n" + "StrongComponent" + i.ToString() + " ");
                foreach(var node in strongcomponent[i])
                {
                    Console.Write(" "+ Path.GetFileName(node.nodeValue));
                }
            }
        }

        //----< display a semiexpression on Console >--------------------

        static public void displaySemiString(string semi)
    {
      if (showSemi && useConsole)
      {
        Console.Write("\n");
        System.Text.StringBuilder sb = new StringBuilder();
        for (int i = 0; i < semi.Length; ++i)
          if (!semi[i].Equals('\n'))
            sb.Append(semi[i]);
        Console.Write("\n  {0}", sb.ToString());
      }
    }
    //----< display, possibly truncated, string >--------------------

    static public void displayString(Action<string> act, string str)
    {
      if (goSlow) Thread.Sleep(200);  //  here only to support visualization
      if (act != null && useFooter)
        act.Invoke(str.Truncate(width));
      if (useConsole)
        Console.Write("\n  {0}", str);
    }
    //----< display string, possibly overriding client pref >--------

    static public void displayString(string str, bool force=false)
    {
      if (useConsole || force)
        Console.Write("\n  {0}", str);
    }
    //----< display rules messages >---------------------------------

    static public void displayRules(Action<string> act, string msg)
    {
      if (showRules)
      {
        displayString(act, msg);
      }
    }
    //----< display actions messages >-------------------------------

    static public void displayActions(Action<string> act, string msg)
    {
      if (showActions)
      {
        displayString(act, msg);
      }
    }
    //----< display filename >---------------------------------------

    static public void displayFiles(Action<string> act, string file)
    {
      if (showFiles)
      {
        displayString(act, file);
      }
    }
    //----< display directory >--------------------------------------

    static public void displayDirectory(Action<string> act, string file)
    {
      if (showDirectories)
      {
        displayString(act, file);
      }
    }

#if(TEST_DISPLAY)
    static void Main(string[] args)
    {
      Console.Write("\n  Tested by use in Parser\n\n");
    }
#endif
  }
}
