﻿
//
// Copyright (c) Michael Eddington
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in	
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

// Authors:
//   Michael Eddington (mike@phed.org)

// $Id$

using System;
using System.Collections.Generic;
using System.Text;

namespace Peach.Core
{
	/// <summary>
	/// Unrecoverable error.  Causes Peach to exit with an error
	/// message, but no stack trace.
	/// </summary>
	public class PeachException : ApplicationException
	{
		public PeachException(string message)
			: base(message)
		{
		}
	}

	/// <summary>
	/// Thrown to cause the Peach Engine to re-run
	/// the same test iteration.
	/// </summary>
	public class RedoIterationException : ApplicationException
	{
	}

	/// <summary>
	/// Thrown to stop current iteration and move to next.
	/// </summary>
	public class SoftException : ApplicationException
	{
	}

	/// <summary>
	/// Similar to SoftException but used by state model
	/// path code.
	/// </summary>
	public class PathException : ApplicationException
	{
	}
}

// end