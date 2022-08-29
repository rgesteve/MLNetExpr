// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if false
using Microsoft.ML.Internal.Utilities;
using Microsoft.ML.Runtime;
#endif

namespace ExpressionLib
{
    internal sealed class Error
    {
        public readonly Token Token;
        public readonly string Message;
        // Args may be null.
        public readonly object[] Args;

        public Error(Token tok, string msg)
        {
	#if false
            Contracts.AssertValue(tok);
            Contracts.AssertNonEmpty(msg);
	    #endif
            Token = tok;
            Message = msg;
            Args = null;
        }

        public Error(Token tok, string msg, params object[] args)
        {
	#if false
            Contracts.AssertValue(tok);
            Contracts.AssertNonEmpty(msg);
            Contracts.AssertValue(args);
	    #endif
            Token = tok;
            Message = msg;
            Args = args;
        }

        public string GetMessage()
        {
            var msg = Message;
            if (Utils.Size(Args) > 0)
                msg = string.Format(msg, Args);
            if (Token != null)
                msg = string.Format("at '{0}': {1}", Token, msg);
            return msg;
        }
    }

}
