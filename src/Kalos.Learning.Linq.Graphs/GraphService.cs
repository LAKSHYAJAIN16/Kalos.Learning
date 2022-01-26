using System;
using System.Linq;
using System.Collections.Generic;

namespace Kalos.Learning.Linq.Graphs
{
    public class GraphService
    {
        public const int NumberOfNonDataColumns = 2;
        
        public static string PlotLineGraph(double[] data, Options options = null)
        {
            //Check for Options
            options = options ?? new Options();

            //Convert to a List just cuz
            List<double> seriesList = data.ToList();

            //Min and Max
            double min = data.Min();
            double max = data.Max();

            //Get the Range
            double range = Math.Abs(max - min);

            //Get Ration
            double ratio = ((options.Height) ?? range) / range;

            //More Updated Min Max
            double min2 = Math.Round(min * ratio, MidpointRounding.AwayFromZero);
            double max2 = Math.Round(max * ratio, MidpointRounding.AwayFromZero);
            double rows = Math.Abs(max2 - min2);

            //Get Column Index
            int columnIndexOfFirstDataPoint = options.AxisLabelRightMargin + NumberOfNonDataColumns;

            //Get Width
            int width = seriesList.Count + columnIndexOfFirstDataPoint;

            //Get a 2d array so that we can fill it
            string[][] resultArray = CreateAndFill2dArray(rows, width, options.Fill.ToString());

            //Get Y-Axis Labels
            IReadOnlyList<AxisLabel> yAxisLabels = GetYAxisLabels(max, range, rows, options);

            //Apply Axis Labels
            ApplyYAxisLabels(resultArray, yAxisLabels, columnIndexOfFirstDataPoint);

            //Loop through and do math
            for (var x = 0; x < seriesList.Count - 1; x++)
            {
                var rowIndex0 = Math.Round(seriesList[x] * ratio, MidpointRounding.AwayFromZero) - min2;
                var rowIndex1 = Math.Round(seriesList[x + 1] * ratio, MidpointRounding.AwayFromZero) - min2;

                if (x == 0)
                {
                    resultArray[(int)(rows - rowIndex0)][columnIndexOfFirstDataPoint - 1] = "┼";
                }

                if (rowIndex0 == rowIndex1)
                {
                    resultArray[(int)(rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = "─";
                }
                else
                {
                    resultArray[(int)(rows - rowIndex1)][x + columnIndexOfFirstDataPoint] = (rowIndex0 > rowIndex1) ? "╰" : "╭";
                    resultArray[(int)(rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = (rowIndex0 > rowIndex1) ? "╮" : "╯";
                    var from = Math.Min(rowIndex0, rowIndex1);
                    var to = Math.Max(rowIndex0, rowIndex1);
                    for (var y = from + 1; y < to; y++)
                    {
                        resultArray[(int)(rows - y)][x + columnIndexOfFirstDataPoint] = "│";
                    }
                }
            }

            return ToString(resultArray);
        }
        private static void DisplayRenderInfo(string callingMethod, string subName, double renderTimeMs)
        {
            Console.WriteLine($"{callingMethod}() {subName}");
            Console.WriteLine($"Rendered in {renderTimeMs} ms");
        }

        public static string[][] CreateAndFill2dArray(double rows, int width, string fill)
        {
            var array = new string[((int)rows + 1)][];
            for (var i = 0; i <= rows; i++){
                array[i] = new string[width];
                for (var j = 0; j < width; j++){
                    array[i][j] = fill;
                }
            }

            return array;
        }

        public static IReadOnlyList<AxisLabel> GetYAxisLabels(double max, double range, double rows, Options options)
        {
            var yAxisTicks = GetYAxisTicks(max, range, rows);
            var labels = yAxisTicks.Select(tick => new AxisLabel(tick, options.AxisLabelFormat)).ToList();

            var maxLabelLength = labels.Max(label => label.Label.Length) + options.AxisLabelLeftMargin;
            foreach (var label in labels)
            {
                label.LeftPad = maxLabelLength;
            }

            return labels;
        }

        public static IReadOnlyList<double> GetYAxisTicks(double max, double range, double rows)
        {
            var numberOfTicks = rows + 1;
            var yTicks = new List<double>();
            for (var i = 0; i < numberOfTicks; i++)
            {
                yTicks.Add(max - i * range / rows);
            }

            return yTicks;
        }

        public static void ApplyYAxisLabels(string[][] resultArray, IReadOnlyList<AxisLabel> yAxisLabels, int columnIndexOfFirstDataPoint)
        {
            for (var i = 0; i < yAxisLabels.Count; i++)
            {
                resultArray[i][0] = yAxisLabels[i].Label;
                resultArray[i][columnIndexOfFirstDataPoint - 1] = (Math.Abs(yAxisLabels[i].Value) < 0.001) ? "┼" : "┤";
            }
        }

        public static string ToString(string[][] resultArray)
        {
            var rowStrings = resultArray.Select(row => String.Join("", row));
            return String.Join(Environment.NewLine, rowStrings);
        }
    }

    public class Options
    {
        int _axisLabelLeftMargin = 1;
        int _axisLabelRightMargin = 1;


        /// <summary>
        /// The margin between the axis label and the left of the output.
        /// </summary>
        public int AxisLabelLeftMargin
        {
            get => _axisLabelLeftMargin;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Margin must be >= 0");
                }
                _axisLabelLeftMargin = value;
            }
        }

        /// <summary>
        /// The margin between the axis label and the axis.
        /// </summary>
        public int AxisLabelRightMargin
        {
            get => _axisLabelRightMargin;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Margin must be >= 0");
                }
                _axisLabelRightMargin = value;
            }
        }

        /// <summary>
        /// Roughly the number of lines to scale the output to.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// The background fill.
        /// </summary>
        public char Fill { get; set; } = ' ';

        /// <summary>
        /// The axis label format.
        /// </summary>
        public string AxisLabelFormat { get; set; } = "0.00";
    }

    public class AxisLabel
    {
        readonly string _format;

        public AxisLabel(double value, string format)
        {
            _format = format;
            Value = value;
        }

        public double Value { get; }
        public int LeftPad { get; set; } = 0;
        public string Label => Value.ToString(_format).PadLeft(LeftPad);
    }
}
