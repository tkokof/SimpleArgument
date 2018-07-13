// desc simple argument implementation
// support max 4 argument param now
// maintainer hugoyu

using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace SimpleArgument
{
	public class SimpleArgument
	{
		public void Add(string name, Action handler)
		{
			if (!m_argHandlers.ContainsKey(name))
			{
				m_argHandlers.Add(name, new Handler0(name, handler));
			}
			else
			{
				Debug.Print("[SimpleArgument]Add argument handler multi-times : " + name);
			}
		}
		
		public void Add<T1>(string name, Action<T1> handler)
		{
			if (!m_argHandlers.ContainsKey(name))
			{
				m_argHandlers.Add(name, new Handler1<T1>(name, handler));
			}
			else
			{
				Debug.Print("[SimpleArgument]Add argument handler multi-times : " + name);
			}
		}
		
		public void Add<T1, T2>(string name, Action<T1, T2> handler)
		{
			if (!m_argHandlers.ContainsKey(name))
			{
				m_argHandlers.Add(name, new Handler2<T1, T2>(name, handler));
			}
			else
			{
				Debug.Print("[SimpleArgument]Add argument handler multi-times : " + name);
			}
		}
		
		public void Add<T1, T2, T3>(string name, Action<T1, T2, T3> handler)
		{
			if (!m_argHandlers.ContainsKey(name))
			{
				m_argHandlers.Add(name, new Handler3<T1, T2, T3>(name, handler));
			}
			else
			{
				Debug.Print("[SimpleArgument]Add argument handler multi-times : " + name);
			}
		}
		
		public void Add<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> handler)
		{
			if (!m_argHandlers.ContainsKey(name))
			{
				m_argHandlers.Add(name, new Handler4<T1, T2, T3, T4>(name, handler));
			}
			else
			{
				Debug.Print("[SimpleArgument]Add argument handler multi-times : " + name);
			}
		}
		
		public void Remove(string name)
		{
			m_argHandlers.Remove(name);
		}
		
		public void Handle(string[] args, int parseStartIndex = 0)
		{
			if (args != null)
			{
				while (parseStartIndex >= 0 && parseStartIndex < args.Length)
				{
					var arg = args[parseStartIndex];
					BaseHandler handler;
					if (m_argHandlers.TryGetValue(arg, out handler))
					{
						var paramCount = handler.GetParamCount();
						handler.DoHandler(args, parseStartIndex + 1, parseStartIndex + paramCount);
						// move to next arg
						parseStartIndex += (paramCount + 1);
					}
					else
					{
						Debug.Print("[SimpleArgument]Error to handle argument : " + arg);
						// move to next arg
					    ++parseStartIndex;
					}
				}
			}
		}
		
		string[] Parse(string argsStr)
		{
			// reset arg buffer
			m_argBuffer.Clear();
			
			int startIndex = 0;
			while (!ReachEnd(argsStr, startIndex))
			{
				// skip space first
				SkipSpace(argsStr, ref startIndex);
				// parse param then
				var param = ParseParam(argsStr, ref startIndex);
				if (!string.IsNullOrEmpty(param))
				{
					m_argBuffer.Add(param);
				}
			}
			
			return m_argBuffer.Count > 0 ? m_argBuffer.ToArray() : null;
		}
		
		public void Handle(string argsStr)
		{
			Handle(Parse(argsStr));
		}
		
		bool IsSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\n' || c == '\r';
		}
		
		bool IsSep(char c)
		{
			return c == '\'' || c == '\"';
		}
		
		void SkipSpace(string argsStr, ref int startIndex)
		{
			if (argsStr != null)
			{
				Debug.Assert(startIndex >= 0, "[SimpleArgument]Invalid argument string index : " + argsStr + " with index " + startIndex);
				while (startIndex < argsStr.Length)
				{
					if (IsSpace(argsStr[startIndex]))
					{
						++startIndex;
					}
					else
					{
						break;
					}
				}
			}
		}
		
		bool ReachEnd(string argsStr, int checkIndex)
		{
			if (argsStr != null)
			{
				Debug.Assert(checkIndex >= 0, "[SimpleArgument]Invalid argument string index : " + argsStr + " with index " + checkIndex);
				return checkIndex >= argsStr.Length;
			}
			
			// default return true
			return true;
		}
		
		string ParseParam(string argsStr, ref int startIndex)
		{
			// reset str builder buffer
			m_strBuilder.Length = 0;
			bool inSep = false;
			
			while (!ReachEnd(argsStr, startIndex))
			{
				var curChar = argsStr[startIndex];
				if (inSep)
				{
				    if (IsSep(curChar))
					{
						// toggle inSep
						inSep = !inSep;
					}
					else
					{
					    m_strBuilder.Append(curChar);
					}
					// move to next char
					++startIndex;
				}
				else
				{
					if (IsSpace(curChar))
					{
						// param token end
						break;
					}
					else
					{
						if (IsSep(curChar))
						{
							// toggle inSep
							inSep = !inSep;
						}
						else
						{
						    m_strBuilder.Append(curChar);
						}
						// move to next char
						++startIndex;
					}
				}
			}
			
			return m_strBuilder.ToString();
		}
		
		class BaseHandler
		{
			protected BaseHandler(string name)
			{
				Debug.Assert(name != null, "[SimpleArgument]Invalid null argument name");
				m_name = name;
			}
			
			public virtual int GetParamCount()
			{
				return 0;
			}
			
			public virtual void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
			}
			
			protected bool ValidParam(string[] argParams, int startIndex, int endIndex, int requireCount)
			{
				if (argParams != null)
				{
					if (startIndex >= 0 && startIndex < argParams.Length && 
					    endIndex >= 0 && endIndex < argParams.Length)
					{
						return endIndex - startIndex + 1 >= requireCount;
					}
				}
				
				return false;
			}
			
			protected T ConvertParam<T>(string param)
			{
				return (T)Convert.ChangeType(param, typeof(T));
			}
			
			protected string m_name;
		}
		
		class Handler0 : BaseHandler
		{
			public Handler0(string name, Action handler) : base(name)
			{
				m_handler = handler;
			}
			
			public override void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
				if (m_handler != null)
				{
					m_handler();
				}
				else
				{
					Debug.Print("[SimpleArgument]Error to execute argument handler : " + m_name);
				}
			}
			
			Action m_handler;
		}
		
		class Handler1<T1> : BaseHandler
		{
			public Handler1(string name, Action<T1> handler) : base(name)
			{
				m_handler = handler;
			}
			
			public override int GetParamCount()
			{
				return 1;
			}
			
			public override void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
				if (ValidParam(argParams, startIndex, endIndex, 1))
				{
					m_handler(ConvertParam<T1>(argParams[startIndex]));
				}
				else
				{
					Debug.Print("[SimpleArgument]Error to execute argument handler : " + m_name);
				}
			}
			
			Action<T1> m_handler;
		}
		
		class Handler2<T1, T2> : BaseHandler
		{
			public Handler2(string name, Action<T1, T2> handler) : base(name)
			{
				m_handler = handler;
			}
			
			public override int GetParamCount()
			{
				return 2;
			}
			
			public override void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
				if (ValidParam(argParams, startIndex, endIndex, 2))
				{
					m_handler(ConvertParam<T1>(argParams[startIndex]), 
					          ConvertParam<T2>(argParams[startIndex + 1]));
				}
				else
				{
					Debug.Print("[SimpleArgument]Error to execute argument handler : " + m_name);
				}
			}
			
			Action<T1, T2> m_handler;
		}
		
		class Handler3<T1, T2, T3> : BaseHandler
		{
			public Handler3(string name, Action<T1, T2, T3> handler) : base(name)
			{
				m_handler = handler;
			}
			
			public override int GetParamCount()
			{
				return 3;
			}
			
			public override void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
				if (ValidParam(argParams, startIndex, endIndex, 3))
				{
					m_handler(ConvertParam<T1>(argParams[startIndex]), 
					          ConvertParam<T2>(argParams[startIndex + 1]), 
					          ConvertParam<T3>(argParams[startIndex + 2]));
				}
				else
				{
					Debug.Print("[SimpleArgument]Error to execute argument handler : " + m_name);
				}
			}
			
			Action<T1, T2, T3> m_handler;
		}
		
		class Handler4<T1, T2, T3, T4> : BaseHandler
		{
			public Handler4(string name, Action<T1, T2, T3, T4> handler) : base(name)
			{
				m_handler = handler;
			}
			
			public override int GetParamCount()
			{
				return 4;
			}
			
			public override void DoHandler(string[] argParams, int startIndex, int endIndex) 
			{
				if (ValidParam(argParams, startIndex, endIndex, 4))
				{
					m_handler(ConvertParam<T1>(argParams[startIndex]), 
					          ConvertParam<T2>(argParams[startIndex + 1]), 
					          ConvertParam<T3>(argParams[startIndex + 2]), 
					          ConvertParam<T4>(argParams[startIndex + 3]));
				}
				else
				{
					Debug.Print("[SimpleArgument]Error to execute argument handler : " + m_name);
				}
			}
			
			Action<T1, T2, T3, T4> m_handler;
		}
		
		Dictionary<string, BaseHandler> m_argHandlers = new Dictionary<string, BaseHandler>();
		StringBuilder m_strBuilder = new StringBuilder(128);
		List<string> m_argBuffer = new List<string>(64);
	}
}