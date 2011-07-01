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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Peach.Core.Dom;
using NLog;

namespace Peach.Core.Agent
{
	/// <summary>
	/// Abstract base class for all Agent servers.
	/// </summary>
	public abstract class AgentClient
	{
		public object parent;

        /// <summary>
        /// Does AgentServer instance support specified protocol?  For example, if
        /// the user supplies an agent URL of "http://....", than "http" is the protocol.
        /// </summary>
        /// <param name="protocol">Protocol to check</param>
        /// <returns>True if protocol is supported, else false.</returns>
        public abstract bool SupportedProtocol(string protocol);

        /// <summary>
        /// Connect to agent
        /// </summary>
        /// <param name="name">Name of agent</param>
        /// <param name="url">Agent URL</param>
        /// <param name="password">Agent Password</param>
        public abstract void AgentConnect(string name, string url, string password);
        /// <summary>
        /// Disconnect from agent
        /// </summary>
        public abstract void AgentDisconnect();

		/// <summary>
		/// Start a specific monitor
		/// </summary>
		/// <param name="name">Name for monitor instance</param>
		/// <param name="cls">Class of monitor to start</param>
		/// <param name="args">Arguments</param>
		public abstract void StartMonitor(string name, string cls, Dictionary<string, Variant> args);
		/// <summary>
		/// Stop a specific monitor by name
		/// </summary>
		/// <param name="name">Name of monitor instance</param>
		public abstract void StopMonitor(string name);
		/// <summary>
		/// Stop all monitors currently running
		/// </summary>
		public abstract void StopAllMonitors();

        /// <summary>
        /// Starting a fuzzing session.  A session includes a number of test iterations.
        /// </summary>
		public abstract void SessionStarting();
        /// <summary>
        /// Finished a fuzzing session.
        /// </summary>
		public abstract void SessionFinished();

        /// <summary>
        /// Starting a new iteration
        /// </summary>
        /// <param name="iterationCount">Iteration count</param>
        /// <param name="isReproduction">Are we re-running an iteration</param>
		public abstract void IterationStarting(int iterationCount, bool isReproduction);
        /// <summary>
        /// Iteration has completed.
        /// </summary>
        /// <returns>Returns true to indicate iteration should be re-run, else false.</returns>
		public abstract bool IterationFinished();

        /// <summary>
        /// Was a fault detected during current iteration?
        /// </summary>
        /// <returns>True if a fault was detected, else false.</returns>
		public abstract bool DetectedFault();
		public abstract Hashtable GetMonitorData();
        /// <summary>
        /// Can the fuzzing session continue, or must we stop?
        /// </summary>
        /// <returns>True if session must stop, else false.</returns>
		public abstract bool MustStop();

		/// <summary>
		/// Send a message to all monitors.
		/// </summary>
		/// <param name="name">Message Name</param>
		/// <param name="data">Message data</param>
		/// <returns>Returns data as Variant or null.</returns>
		public abstract Variant Message(string name, Variant data);
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class AgentAttribute : Attribute
	{
		public string protocol;
		public AgentAttribute(string protocol)
		{
			this.protocol = protocol;
		}
	}


}