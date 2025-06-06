using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosmoConsole
{
    public static class Buffer
    {
        public static string[] cmds;
        public static int _bufferSize;
        public static int place = 0;

        /// <summary>
        /// Loads the buffer with the specified size.
        /// </summary>
        /// <param name="bufferSize">Buffer size.</param>
        /// <exception cref="ArgumentException">Thrown if bufferSize is not specified/lower than 1/null.</exception>
        public static void Initialize(int bufferSize)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentException("Buffer size must be greater than 0", nameof(bufferSize));
            }
            else
            {
                _bufferSize = bufferSize;
                for (int i = 1; i <= _bufferSize; i++)
                {
                    cmds[i] = "";
                }
            }
        }

        /// <summary>
        /// Adds a command to the buffer.
        /// </summary>
        /// <param name="command">The command.</param>
        public static void Add(string command)
        {
            for (int i = 1; i <= _bufferSize; i++)
            {
                if (cmds[i] == string.Empty)
                {
                    cmds[i] = command;
                    return;
                }
            }
        }

        /// <summary>
        /// Reads a command from the buffer based on the specified way.
        /// </summary>
        /// <param name="way">0 to read one command down, 1 to read one command up</param>
        /// <returns>The command saved in buffer.</returns>
        /// <exception cref="ArgumentException">Thrown if `way` is not 0 or 1.</exception>
        public static string Read(int way)
        {
            string returning = string.Empty;
            if (way < 0 || way > 1)
            {
                throw new ArgumentException("The value must me 1 or 2.", nameof(way));
            }
            if (way == 0)
            {
                if (place < _bufferSize - 1)
                {
                    place++;
                }
                returning = cmds[place];
            }
            else if (way == 1)
            {
                if (place > 0)
                {
                    place--;
                }
                returning = cmds[place];
            }
            return returning;
        }
    }
}
