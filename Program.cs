﻿/*
 * Created by SharpDevelop.
 * User: hugoyu
 * Date: 2018/7/13
 * Time: 14:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SimpleArgument
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Program
	{
		static void OnArg0()
		{
			Console.WriteLine("Arg0 handler");
		}
		
		static void OnArg1(int p1)
		{
			Console.WriteLine("Arg1 handler {0}", p1);
		}
		
		static void OnArg2(int p1, string p2)
		{
			Console.WriteLine("Arg2 handler {0} {1}", p1, p2);
		}
		
		static void OnArg3(float p1, float p2, float p3)
		{
			Console.WriteLine("Arg3 handler {0} {1} {2}", p1, p2, p3);
		}
		
		static void OnArg4(string p1, int p2, float p3, double p4)
		{
			Console.WriteLine("Arg4 handler {0} {1} {2} {3}", p1, p2, p3, p4);
		}
		
		public static void Main(string[] args)
		{
			/*
			// #1
			if (args.Length > 0)
			{
				if (args[0] == "-a0")
				{
					OnArg0();
				}
				else if (args[0] == "-a1")
				{
					OnArg1(Convert.ToInt32(args[1]));
				}
				
				// other situations here ...
			}
			
			Console.WriteLine("Press any key to continue . . . ");
			Console.ReadKey(true);
			
			// #2
			int index = 0;
			while (index < args.Length)
			{
				if (args[index] == "-a0")
				{
					OnArg0();
					++index;
				}
				else if (args[index] == "-a1")
				{
					OnArg1(Convert.ToInt32(args[index + 1]));
					index += 2;
				}
				else
				{
					++index;
				}
				
				// other situations here ...
			}
			
			Console.WriteLine("Press any key to continue . . . ");
			Console.ReadKey(true);
			*/
			
			SimpleArgument sa = new SimpleArgument();
			sa.Add("-a0", OnArg0);
			sa.Add<int>("-a1", OnArg1);
			sa.Add<int, string>("-a2", OnArg2);
			sa.Add<float, float, float>("-a3", OnArg3);
			sa.Add<string, int, float, double>("-a4", OnArg4);
			
			try
			{
			    sa.Handle(args);
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.Message);
			}
			
			try
			{
			    sa.Handle("-a0 -a1 123 -a2 345 test2\"test2\"test2 -a3 13.6 14.7 177 -a4 test1 222 333 444");
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.Message);
			}
			
			Console.WriteLine("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}