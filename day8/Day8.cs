﻿using common;
using System.Text;

namespace day8
{
    public class Day8 : IAdventOfCodeDay
    {
        public Day8(IEnumerable<string> input)
        {
            Data = input.Select(x => x.Select(CharDigitToInt).ToArray()).ToArray();
        }

        public int[][] Data { get; set; }

        public string GetAnswerForPart1()
        {
            var visibility = GetEmptyVisibilityMatrix();
            MarkExtemitiesVisible(visibility);
            MarkVisibleFromWest(visibility);
            MarkVisibleFromEast(visibility);
            MarkVisibleFromNorth(visibility);
            MarkVisibleFromSouth(visibility);

            return CountVisible(visibility).ToString();
        }

        private static int CountVisible(Visibility[][] visibility)
        {
            var count = 0;
            for (var r = 0; r < visibility.Length; r++)
            {
                for (var c = 0; c < visibility[r].Length; c++)
                {
                    if (visibility[r][c] == Visibility.Visible)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a visibility matrix for the size of the Data.
        /// </summary>
        /// <returns></returns>
        public Visibility[][] GetEmptyVisibilityMatrix()
        {
            var result = new Visibility[Data.Length][];
            for (var i = 0; i < Data.Length; i++)
            {
                result[i] = new Visibility[Data[i].Length];
            }

            return result;
        }

        public static string FormatVisibility(Visibility[][] visibility)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < visibility.Length; i++)
            {
                sb.Append(string.Join("", visibility[i].Select(x => ((int)x).ToString())));
                if (i + 1 < visibility.Length)
                {
                    sb.Append('\n');
                }
            }
            return sb.ToString();
        }

        public void MarkVisibleFromWest(Visibility[][] visibility)
        {
            for (var rowIndex = 1; rowIndex < Data.Length - 1; rowIndex++)
            {
                var rowData = Data[rowIndex];
                var highest = rowData[0];
                for (var columnIndex = 1; columnIndex < rowData.Length - 1; columnIndex++)
                {
                    if (rowData[columnIndex] > highest)
                    {
                        visibility[rowIndex][columnIndex] = Visibility.Visible;
                        highest = rowData[columnIndex];
                        if (highest == MAX_HEIGHT)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void MarkVisibleFromEast(Visibility[][] visibility)
        {
            for (var rowIndex = 1; rowIndex < Data.Length - 1; rowIndex++)
            {
                var rowData = Data[rowIndex];
                var highest = rowData[rowData.Length - 1];
                for (var columnIndex = rowData.Length - 2; columnIndex > 0; columnIndex--)
                {
                    if (rowData[columnIndex] > highest)
                    {
                        visibility[rowIndex][columnIndex] = Visibility.Visible;
                        highest = rowData[columnIndex];
                        if (highest == MAX_HEIGHT)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void MarkVisibleFromNorth(Visibility[][] visibility)
        {
            for (var columnIndex = 1; columnIndex < Data[0].Length - 1; columnIndex++)
            {
                var highest = Data[0][columnIndex];
                for (var rowIndex = 1; rowIndex < Data.Length - 1; rowIndex++)
                {
                    if (Data[rowIndex][columnIndex] > highest)
                    {
                        visibility[rowIndex][columnIndex] = Visibility.Visible;
                        highest = Data[rowIndex][columnIndex];
                        if (highest == MAX_HEIGHT)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void MarkVisibleFromSouth(Visibility[][] visibility)
        {
            for (var columnIndex = 1; columnIndex < Data[0].Length - 1; columnIndex++)
            {
                var highest = Data[Data.Length - 1][columnIndex];
                for (var rowIndex = Data.Length - 2; rowIndex > 0; rowIndex--)
                {
                    if (Data[rowIndex][columnIndex] > highest)
                    {
                        visibility[rowIndex][columnIndex] = Visibility.Visible;
                        highest = Data[rowIndex][columnIndex];
                        if (highest == MAX_HEIGHT)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void MarkExtemitiesVisible(Visibility[][] matrix)
        {
            for(var i = 0; i < matrix[0].Length; i++)
            {
                matrix[0][i] = Visibility.Visible;
            }

            for (var i = 0; i < matrix[matrix.Length - 1].Length; i++)
            {
                matrix[matrix.Length - 1][i] = Visibility.Visible;
            }

            for (var i = 1; i < matrix.Length - 1; i++)
            {
                matrix[i][0] = Visibility.Visible;
                matrix[i][matrix[i].Length - 1] = Visibility.Visible;
            }
        }

        /// <summary>
        /// Converts a character digit '0' through '9' to an int.  Does not
        /// perform any validation.
        /// </summary>
        public static int CharDigitToInt(char c) => c - '0';

        public enum Visibility
        {
            NotSure,
            Visible,
            Invisible
        }

        const int MAX_HEIGHT = 9;
    }
}