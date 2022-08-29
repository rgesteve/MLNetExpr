// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#if false
using Microsoft.ML.Runtime;
#endif

namespace ExpressionLib
{
    internal sealed class CharCursor
    {
        private readonly char[] _buffer;

        // The base index for the beginning of the buffer.
        private int _ichBase;

        // Position within the buffer.
        private int _ichNext;
        private int _ichLim;
        private bool _fNoMore;

        public bool Eof { get; private set; }

        public int IchCur => _ichBase + _ichNext - 1;

        public char ChCur { get; private set; }

#if false
        public CharCursor(string text)
            : this(Contracts.CheckRef(text, nameof(text)).ToCharArray(), text.Length)
        {
        }
#endif

        public CharCursor(string text, int ichMin, int ichLim)
            : this(text.ToCharArray(ichMin, ichLim - ichMin), ichLim - ichMin)
        {
        }

        private CharCursor(char[] buffer, int ichLimInit)
        {
	#if false
            Contracts.AssertValue(buffer);
            Contracts.Assert(0 <= ichLimInit && ichLimInit <= buffer.Length);
	    #endif

            _buffer = buffer;
            _ichBase = 0;
            _ichNext = 0;
            _ichLim = ichLimInit;
            _fNoMore = false;
            Eof = false;
            ChNext();
        }

        // Fetch the next character into _chCur and return it.
        public char ChNext()
        {
            if (Eof)
                return ChCur;

            if (_ichNext < _ichLim || EnsureMore())
            {
	    #if false
                Contracts.Assert(_ichNext < _ichLim);
		#endif
                return ChCur = _buffer[_ichNext++];
            }

#if false
            Contracts.Assert(_fNoMore);
	    #endif
            Eof = true;
            _ichNext++; // This is so the final IchCur is reported correctly.
            return ChCur = '\x00';
        }

        public char ChPeek(int dich)
        {
            // If someone is peeking at ich, they should have peeked everything up to ich.
	    #if false
            Contracts.Assert(0 < dich && dich <= _ichLim - _ichNext + 1);
	    #endif

            int ich = dich + _ichNext - 1;
            if (ich < _ichLim)
                return _buffer[ich];
            if (EnsureMore())
            {
                ich = dich + _ichNext - 1;
		#if false
                Contracts.Assert(ich < _ichLim);
		#endif
                return _buffer[ich];
            }

#if false
            Contracts.Assert(_fNoMore);
#endif
            return '\x00';
        }

        private bool EnsureMore()
        {
            if (_fNoMore)
                return false;

            if (_ichNext > 0)
            {
                int ichDst = 0;
                int ichSrc = _ichNext;
                while (ichSrc < _ichLim)
                    _buffer[ichDst++] = _buffer[ichSrc++];
                _ichBase += _ichNext;
                _ichNext = 0;
                _ichLim = ichDst;
            }

            int ichLim = _ichLim;

#if false
            Contracts.Assert(ichLim == _ichLim);
#endif
            _fNoMore = true;
            return false;
        }

    }
}
