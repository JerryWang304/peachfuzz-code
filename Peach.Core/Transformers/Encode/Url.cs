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
//   Mikhail Davidov (sirus@haxsys.net)

// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using Peach.Core.Dom;
using Peach.Core.IO;

namespace Peach.Core.Transformers.Encode
{
    [TransformerAttribute("UrlEncode", "Encode on output as a URL without pluses.")]
    class UrlEncode : Transformer
    {
        public UrlEncode(Dictionary<string,Variant>  args) : base(args)
		{
		}

        protected override BitStream internalEncode(BitStream data)
        {
            string dataString = System.Text.ASCIIEncoding.ASCII.GetString(data.Value);
            string ue = System.Web.HttpUtility.UrlPathEncode(dataString);

            return new BitStream(System.Text.ASCIIEncoding.ASCII.GetBytes(ue));
        }

        protected override BitStream internalDecode(BitStream data)
        {
            return new BitStream(System.Web.HttpUtility.UrlDecodeToBytes(data.Value));
        }
    }
     [TransformerAttribute("UrlEncodePlus", "Encode on output as a URL with spaces turned to pluses.")]
    class UrlEncodePlus : Transformer
    {
         public UrlEncodePlus(Dictionary<string,Variant> args)
             : base(args)
		{
		}

        protected override BitStream internalEncode(BitStream data)
        {
            return new BitStream(System.Web.HttpUtility.UrlEncodeToBytes(data.Value));
        }

        protected override BitStream internalDecode(BitStream data)
        {
            return new BitStream(System.Web.HttpUtility.UrlDecodeToBytes(data.Value));
        }
    }
}