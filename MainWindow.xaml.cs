using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;

namespace ChartsWPF
{
	public partial class MainWindow : Window
	{
		const int N = 1000000;
		const int NumIntervals = 30;

		private enum ChartMode
		{
			Mode1,
			Mode2,
			Mode3
		}

		private ChartMode currentMode = ChartMode.Mode1; // За замовчуванням вибираємо Режим 1

		public dynamic SeriesCollection { get; set; }
		public string[] Labels { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			UpdateChart();
		}

		private void Mode1_Click(object sender, RoutedEventArgs e)
		{
			currentMode = ChartMode.Mode1;
			UpdateChart();
		}

		private void Mode2_Click(object sender, RoutedEventArgs e)
		{
			currentMode = ChartMode.Mode2;
			UpdateChart();
		}

		private void Mode3_Click(object sender, RoutedEventArgs e)
		{
			currentMode = ChartMode.Mode3;
			UpdateChart();
		}

		private void UpdateChart()
		{
			double step = 1.0 / NumIntervals;
			int[] counts = new int[NumIntervals];

			for (int i = 0; i < N; i++)
			{
				double rand = 0;

				switch (currentMode)
				{
					case ChartMode.Mode1:
						rand = GetUniformRandom(0, 1);
						break;
					case ChartMode.Mode2:
						rand = GetExponentialRandom(6);
						break;
					case ChartMode.Mode3:
						rand = GetNormalRandom(0.5, 0.15);
						break;
					default:
						rand = GetUniformRandom(0, 1);
						break;
				}

				int index = (int)(rand / step);

				if (index < NumIntervals)
				{
					counts[index]++;
				}
			}

			int maxCount = counts.Max();

			cartesianChart.AxisY[0].MinValue = 0;
			cartesianChart.AxisY[0].MaxValue = maxCount;

			// Створення графіка
			SeriesCollection = new SeriesCollection
			{
				new ColumnSeries
				{
					Title = "Розподіл",
					Values = new ChartValues<int>(counts)
				}
			};

			Labels = Enumerable.Range(0, NumIntervals).Select(i => i.ToString()).ToArray();

			DataContext = null;
			DataContext = this;
		}

		private double GetUniformRandom(double a, double b)
		{
			Random random = new Random();
			return a + (b - a) * random.NextDouble();
		}

		private double GetExponentialRandom(double lambda)
		{
			Random random = new Random();
			return -Math.Log(1.0 - random.NextDouble()) / lambda;
		}

		private double GetNormalRandom(double mean, double stdDev)
		{
			double u1 = 1.0 - new Random().NextDouble();
			double u2 = 1.0 - new Random().NextDouble();

			double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
								   Math.Cos(2.0 * Math.PI * u2);

			return Math.Max(0, Math.Min(1, mean + stdDev * randStdNormal));
		}
	}
}