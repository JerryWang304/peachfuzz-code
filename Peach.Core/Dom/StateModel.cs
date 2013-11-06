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
//   Michael Eddington (mike@dejavusecurity.com)

// $Id$

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Linq;

using Peach.Core;
using Peach.Core.IO;

using NLog;

namespace Peach.Core.Dom
{
	public delegate void StateModelStartingEventHandler(StateModel model);
	public delegate void StateModelFinishedEventHandler(StateModel model);

	[Serializable]
	public class StateModel : INamed
	{
		static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public string _name = null;
		public object parent;
		protected State _initialState = null;

		protected Dictionary<string, BitwiseStream> _dataActions = new Dictionary<string, BitwiseStream>();

		public IEnumerable<KeyValuePair<string, BitwiseStream>> dataActions
		{
			get
			{
				return _dataActions.AsEnumerable();
			}
		}

		/// <summary>
		/// All states in state model.
		/// </summary>
		public Dictionary<string, State> states = new Dictionary<string, State>();

		public string name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// The initial state to run when state machine executes.
		/// </summary>
		public State initialState
		{
			get
			{
				return _initialState;
			}

			set
			{
				_initialState = value;
			}
		}

		public void SaveData(ActionData data, string suffix)
		{
			var args = new[]
			{
				(_dataActions.Count + 1).ToString(),
				data.action.parent.name,
				data.action.name,
				data.name,
				"bin"
			};

			var key = string.Join(".", args.Where(s => s != null));
			var value = data.dataModel.Value;

			_dataActions.Add(key, value);
		}

		/// <summary>
		/// StateModel is starting to execute.
		/// </summary>
		public static event StateModelStartingEventHandler Starting;
		/// <summary>
		/// StateModel has finished executing.
		/// </summary>
		public static event StateModelFinishedEventHandler Finished;

		protected virtual void OnStarting()
		{
			if (Starting != null)
				Starting(this);
		}

		protected virtual void OnFinished()
		{
			if (Finished != null)
				Finished(this);
		}

		/// <summary>
		/// Start running the State Machine
		/// </summary>
		/// <remarks>
		/// This will start the initial State.
		/// </remarks>
		/// <param name="context"></param>
		public void Run(RunContext context)
		{
			try
			{
				OnStarting();

				foreach (Publisher publisher in context.test.publishers.Values)
				{
					publisher.Iteration = context.test.strategy.Iteration;
					publisher.IsControlIteration = context.controlIteration;
				}

				_dataActions.Clear();

				// Update all data model to clones of origionalDataModel
				// before we start down the state path.
				foreach (State state in states.Values)
				{
					state.UpdateToOriginalDataModel();
				}

				State currentState = _initialState;

				while (true)
				{
					try
					{
						currentState.Run(context);
						break;
					}
					catch (ActionChangeStateException ase)
					{
						var newState = context.test.strategy.MutateChangingState(ase.changeToState);
						
						if(newState == ase.changeToState)
							logger.Debug("Run(): Changing to state \"" + newState.name + "\".");
						else
							logger.Debug("Run(): Changing state mutated.  Switching to \"" + newState.name + 
								"\" instead of \""+ase.changeToState+"\".");
						
						currentState.OnChanging(newState);
						currentState = newState;
					}
				}
			}
			catch (ActionException)
			{
				// Exit state model!
			}
			finally
			{
				foreach (Publisher publisher in context.test.publishers.Values)
					publisher.close();

				OnFinished();
			}
		}
	}
}

// END
