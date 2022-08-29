// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
#if false
using Microsoft.ML.Internal.Utilities;
using Microsoft.ML.Runtime;
#endif

namespace ExpressionLib
{
    /// <summary>
    /// Maps from normalized string to keyword token kind. A lexer must be provided with one of these.
    /// </summary>
    internal partial class KeyWordTable
    {
        public struct KeyWordKind
        {
            public readonly TokKind Kind;
            public readonly bool IsContextKeyWord;

            public KeyWordKind(TokKind kind, bool isContextKeyWord)
            {
                Kind = kind;
                IsContextKeyWord = isContextKeyWord;
            }
        }

        private readonly NormStr.Pool _pool;
        private readonly Dictionary<NormStr, KeyWordKind> _mpnstrtidWord;
        private readonly Dictionary<NormStr, TokKind> _mpnstrtidPunc;

        public KeyWordTable(NormStr.Pool pool)
        {
	#if false
            Contracts.AssertValue(pool);
	    #endif

            _pool = pool;
            _mpnstrtidWord = new Dictionary<NormStr, KeyWordKind>();
            _mpnstrtidPunc = new Dictionary<NormStr, TokKind>();
        }


        public void AddKeyWord(string str, TokKind tid)
        {
	    #if false
            Contracts.AssertNonEmpty(str);
	    #endif
            _mpnstrtidWord.Add(_pool.Add(str), new KeyWordKind(tid, false));
        }

        public bool TryAddPunctuator(string str, TokKind tid)
        {
	    #if false
            Contracts.AssertNonEmpty(str);
	    #endif

            // Note: this assumes that once a prefix is found, that all shorter
            // prefixes are mapped to something (TokKind.None to indicate that
            // it is only a prefix and not itself a token).

            TokKind tidCur;
            NormStr nstr = _pool.Add(str);
            if (_mpnstrtidPunc.TryGetValue(_pool.Add(str), out tidCur))
            {
                if (tidCur == tid)
                    return true;
                if (tidCur != TokKind.None)
                    return false;
            }
            else
            {
                // Map all prefixes (that aren't already mapped) to TokKind.None.
                for (int cch = str.Length; --cch > 0;)
                {
                    NormStr nstrTmp = _pool.Add(str.Substring(0, cch));
                    TokKind tidTmp;
                    if (_mpnstrtidPunc.TryGetValue(_pool.Add(nstrTmp.Value), out tidTmp))
                        break;
                    _mpnstrtidPunc.Add(nstrTmp, TokKind.None);
                }
            }
            _mpnstrtidPunc[nstr] = tid;
            return true;
        }

        public void AddPunctuator(string str, TokKind tid)
        {
	#if false
            Contracts.AssertNonEmpty(str);
	    #endif
            if (!TryAddPunctuator(str, tid)) {
	    #if false
                Contracts.Assert(false, "duplicate punctuator!");
		#endif
		}
        }

        public bool IsKeyWord(NormStr nstr, out KeyWordKind kind)
        {
	#if false
            Contracts.Assert(!nstr.Value.IsEmpty);
	    #endif
            return _mpnstrtidWord.TryGetValue(nstr, out kind);
        }

        public bool IsPunctuator(NormStr nstr, out TokKind tid)
        {
	#if false
            Contracts.Assert(!nstr.Value.IsEmpty);
	    #endif
            return _mpnstrtidPunc.TryGetValue(nstr, out tid);
        }

        public IEnumerable<KeyValuePair<NormStr, TokKind>> Punctuators
        {
            get { return _mpnstrtidPunc; }
        }

        public IEnumerable<KeyValuePair<NormStr, KeyWordKind>> KeyWords
        {
            get { return _mpnstrtidWord; }
        }
    }

}
