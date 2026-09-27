using FoldersSync.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace FoldersSync
{
    internal static class InputDataReader
    {
        private static string _sourceFolderPath;
        private static string _replicaFolderPath;
        private static double _syncIntervalMinute;
        private static string _logFileFolderPath;

        public static UserInputModel ReadInputParams() 
        {
           
            Console.WriteLine(Constants.EnterSrc);
            _sourceFolderPath = validateFolderInput();
            Console.WriteLine(Constants.EnterRpc);
            _replicaFolderPath = validateFolderInput();
            while (_replicaFolderPath.Equals(_sourceFolderPath))
            {
                Console.WriteLine(Constants.RpcCantBeSrc);
                _replicaFolderPath = validateFolderInput();
            }
            Console.WriteLine(Constants.EnterIntervalMin);
            _syncIntervalMinute = validateTimeInput();
            Console.WriteLine(Constants.EnterLog);
            _logFileFolderPath = validateFolderInput();
            while (_logFileFolderPath.Equals(_sourceFolderPath)||_logFileFolderPath.Equals(_replicaFolderPath))
            {
                Console.WriteLine(Constants.LogCantBeSrcRpc);
                _logFileFolderPath = validateFolderInput();
            }

            UserInputModel userInput = new UserInputModel();
            userInput.SourceFolderPath = _sourceFolderPath;
            userInput.ReplicaFolderPath = _replicaFolderPath;
            userInput.SyncIntervalMinute = _syncIntervalMinute;
            userInput.LogFileFolderPath = _logFileFolderPath;

            return userInput;

        }
        private static string validateFolderInput()
        {
            string toRead = cancelableReadLine(out bool isCancelled);
            exitOnEscapePress(isCancelled);

            while (!Directory.Exists(toRead))
            {
                Console.WriteLine(Constants.InputFolderNotValid);
                toRead = cancelableReadLine(out isCancelled);
                exitOnEscapePress(isCancelled);
            }
            return toRead; 
        }

        private static double validateTimeInput()
        {
            string toRead = cancelableReadLine(out bool isCancelled);
            exitOnEscapePress(isCancelled);

            double interval;
            while (!double.TryParse(toRead, out interval) || interval<=0)
            {
                Console.WriteLine(Constants.InputTimeNotValid);
                toRead = cancelableReadLine(out isCancelled);
                exitOnEscapePress(isCancelled);
            }
            return interval;
        }

        private static void exitOnEscapePress(bool isCancelled)
        {
            if (isCancelled)
            {
                Console.WriteLine(Constants.TerminateBeforeSyncStarted);
                Environment.Exit(0);
            }
        }

        private static string cancelableReadLine(out bool isCancelled)
        {
            var cancelKey = ConsoleKey.Escape;
            var builder = new StringBuilder(0, 256);
            var cki = Console.ReadKey(true);
            int index = 0;
            (int left, int top) startPosition;

            while (cki.Key != ConsoleKey.Enter && cki.Key != cancelKey)
            {
                if (cki.Key == ConsoleKey.LeftArrow)
                {
                    if (index < 1)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    leftArrow(ref index, cki);
                }
                else if (cki.Key == ConsoleKey.RightArrow)
                {
                    if (index >= builder.Length)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    rightArrow(ref index, cki, builder);
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (index < 1)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    backSpace(ref index, cki, builder);
                }
                else if (cki.Key == ConsoleKey.Delete)
                {
                    if (index >= builder.Length)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    ddelete(ref index, cki, builder);
                }
                else if (cki.Key == ConsoleKey.Tab)
                {
                    cki = Console.ReadKey(true);
                    continue;
                }
                else
                {
                    if (cki.KeyChar == '\0')
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    ddefault(ref index, cki, builder);
                }

                cki = Console.ReadKey(true);
            }

            if (cki.Key == cancelKey)
            {
                startPosition = getStartPosition(index);
                erasePrint(builder, startPosition);

                isCancelled = true;
                return string.Empty;
            }

            isCancelled = false;

            startPosition = getStartPosition(index);
            var endPosition = getEndPosition(startPosition.left, builder.Length);
            var left = 0;
            var top = startPosition.top + endPosition.top;

            Console.SetCursorPosition(left, top);

            var value = builder.ToString();
            return value;
        }

        private static void leftArrow(ref int index, ConsoleKeyInfo cki)
        {
            var previousIndex = index;
            index--;

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                index = 0;

                var startPosition = getStartPosition(previousIndex);
                Console.SetCursorPosition(startPosition.left, startPosition.top);

                return;
            }

            if (Console.CursorLeft > 0)
                Console.CursorLeft--;
            else
            {
                Console.CursorTop--;
                Console.CursorLeft = Console.BufferWidth - 1;
            }
        }

        private static void rightArrow(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index++;

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                index = builder.Length;

                var startPosition = getStartPosition(previousIndex);
                var endPosition = getEndPosition(startPosition.left, builder.Length);
                var top = startPosition.top + endPosition.top;
                var left = endPosition.left;

                Console.SetCursorPosition(left, top);

                return;
            }

            if (Console.CursorLeft < Console.BufferWidth - 1)
                Console.CursorLeft++;
            else
            {
                Console.CursorTop++;
                Console.CursorLeft = 0;
            }
        }

        private static void backSpace(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index--;

            var startPosition = getStartPosition(previousIndex);
            erasePrint(builder, startPosition);

            builder.Remove(index, 1);
            Console.Write(builder.ToString());

            goBackToCurrentPosition(index, startPosition);
        }

        private static void ddelete(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var startPosition = getStartPosition(index);
            erasePrint(builder, startPosition);

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                builder.Remove(index, builder.Length - index);
                Console.Write(builder.ToString());

                goBackToCurrentPosition(index, startPosition);
                return;
            }

            builder.Remove(index, 1);
            Console.Write(builder.ToString());

            goBackToCurrentPosition(index, startPosition);
        }

        private static void ddefault(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index++;

            builder.Insert(previousIndex, cki.KeyChar);

            var startPosition = getStartPosition(previousIndex);
            Console.SetCursorPosition(startPosition.left, startPosition.top);
            Console.Write(builder.ToString());

            goBackToCurrentPosition(index, startPosition);
        }

        private static (int left, int top) getStartPosition(int previousIndex)
        {
            int top;
            int left;

            if (previousIndex <= Console.CursorLeft)
            {
                top = Console.CursorTop;
                left = Console.CursorLeft - previousIndex;
            }
            else
            {
                var decrementValue = previousIndex - Console.CursorLeft;
                var rowsFromStart = decrementValue / Console.BufferWidth;
                top = Console.CursorTop - rowsFromStart;
                left = decrementValue - rowsFromStart * Console.BufferWidth;

                if (left != 0)
                {
                    top--;
                    left = Console.BufferWidth - left;
                }
            }

            return (left, top);
        }

        private static void goBackToCurrentPosition(int index, (int left, int top) startPosition)
        {
            var rowsToGo = (index + startPosition.left) / Console.BufferWidth;
            var rowIndex = index - rowsToGo * Console.BufferWidth;

            var left = startPosition.left + rowIndex;
            var top = startPosition.top + rowsToGo;

            Console.SetCursorPosition(left, top);
        }

        private static (int left, int top) getEndPosition(int startColumn, int builderLength)
        {
            var cursorTop = (builderLength + startColumn) / Console.BufferWidth;
            var cursorLeft = startColumn + (builderLength - cursorTop * Console.BufferWidth);

            return (cursorLeft, cursorTop);
        }

        private static void erasePrint(StringBuilder builder, (int left, int top) startPosition)
        {
            Console.SetCursorPosition(startPosition.left, startPosition.top);
            Console.Write(new string(Enumerable.Range(0, builder.Length).Select(o => ' ').ToArray()));

            Console.SetCursorPosition(startPosition.left, startPosition.top);
        }

    }
}
