using System;
using System.Collections.Generic;
using CommonModel.OneDimSplines;

namespace CommonModel.Classes
{
	static class FilterStorage
	{
		private static Dictionary<string, IMaskFilter> _masks;

		private static Dictionary<string, Vector> _vectors;

		// Low
		private static readonly ImageMask Jl20 = new ImageMask(
			new[]
			{
				new double[] { 1, 6, 1 },
				new double[] { 6, 36, 6 },
				new double[] { 1, 6, 1 }
			}, 64);

		private static readonly ImageMask Jl30 = new ImageMask(
			new[]
			{
				new double[] { 1, 4, 1 },
				new double[] { 4, 16, 4 },
				new double[] { 1, 4, 1 }
			}, 36);

		private static readonly ImageMask Jl40 = new ImageMask(
			new[]
			{
				new double[] { 1, 76, 230, 76, 1},
				new double[] { 76, 5776, 17480, 5776, 76},
				new double[] { 230, 17480, 52900, 17480, 230},
				new double[] { 1, 76, 230, 76, 1},
				new double[] { 76, 5776, 17480, 5776, 76}
			}, 147456);

		private static readonly ImageMask Jl50 = new ImageMask(
			new[]
			{
				new double[] { 1, 26, 66, 26, 1},
				new double[] { 26, 676, 1716, 676, 26},
				new double[] { 66, 1716, 4386, 1716, 66},
				new double[] { 26, 676, 1716, 676, 26},
				new double[] { 1, 26, 66, 26, 1}
			}, 14400);

		// Hight
		private static readonly ImageMask Jh20 = new ImageMask(
			new[]
			{
				new double[] { -1, -6, -1},
				new double[] { -6, 28, -6},
				new double[] { -1, -6, -1}
			}, 64);

		private static readonly ImageMask Jh30 = new ImageMask(
			new[]
			{
				new double[] { -1, -4, -1},
				new double[] { -4, 20, -4},
				new double[] { -1, -4, -1}
			}, 36);

		private static readonly ImageMask Jh40 = new ImageMask(
			new[]
			{
				new double[] { -1, -76, -230, -76, -1},
				new double[] { -76, -5776, -17480, -5776, -76},
				new double[] { -230, -17480, 94556, -17480, -230},
				new double[] { -1, -76, -230, -76, -1},
				new double[] { -76, -5776, -17480, -5776, -76}
			}, 147456);

		private static readonly ImageMask Jh50 = new ImageMask(
			new[]
			{
				new double[] { -1, -26, -66,-276, -1},
				new double[] { -26, -676, -1716, -676, -26},
				new double[] { -66, -1716, 10044, -1716, -66},
				new double[] { -26, -676, -1716, -676, -26},

				new double[] { -1, -26, -66,-276, -1}
			}, 14400);

		// Kontrast
		private static readonly ImageMask Jk20 = new ImageMask(
			new[]
			{
				new double[] { 1, -8, 48, -8, 1},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 48, -384, 2304, -384, 48},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 1, -8, 48, -8, 1}
			}, 1156);

		private static readonly ImageMask Jk30 = new ImageMask(
			new[]
			{
				new double[] { 1, -6, 24, -6, 1},
				new double[] { -6, 36, -144, 36, -6},
				new double[] { 24, -144, 576, -144, 24},
				new double[] { -6, 36, -144, 36, -6},
				new double[] { 1, -6, 24, -6, 1}
			}, 196);

		private static readonly ImageMask Jk40 = new ImageMask(
			new[]
			{
				new double[] { 1, -8, 48, -8, 1},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 48, -384, 2304, -384, 48},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 1, -8, 48, -8, 1}
			}, 1);

		private static readonly ImageMask Jk50 = new ImageMask(
			new[]
			{
				new double[] { 1, 6, 1 },
				new double[] { 6, 36, 6 },
				new double[] { 1, 6, 1 }
			}, 25784009476);

		// stabilisators
		private static readonly ImageMask Js20 = new ImageMask(
			new[]
			{
				new[] {3.75457E-09,8.93587E-07,5.40282E-06,-7.38748E-05, 5.40282E-06, 8.93587E-07, 3.75457E-09 },
				new[] {8.93587E-07,0.000212674,0.001285871,-0.011758221,0.001285871,0.000212674,8.93587E-07 },
				new[] {5.40282E-06,0.001285871, 0.007774658,-0.106305883,0.007774658,0.001285871,5.40282E-06},
				new[] {-7.38748E-05,-0.01758221,-0.106305883,1.45356119,-0.106305883,-0.01758221,-7.38748E-05 },
				new[] {5.40282E-06,0.001285871, 0.007774658,-0.106305883,0.007774658,0.001285871,5.40282E-06 },
				new[] {8.93587E-07,0.000212674,0.001285871,-0.011758221,0.001285871,0.000212674,8.93587E-07  },
				new[] {3.75457E-09,8.93587E-07,5.40282E-06,-7.38748E-05, 5.40282E-06, 8.93587E-07, 3.75457E-09 }
			}, 1);

		private static readonly ImageMask Js30 = new ImageMask(
			new[]
			{
				new[] {1.24562E-08,2.96456E-06,1.159314E-05,-0.000149424,1.159314E-05,2.96456E-06,1.24562E-08 },
				new[] {2.96456E-06,0.000705566,0.003791678,-0.035562919,0.003791678,0.000705566,2.96456E-06 },
				new[] {1.59314E-05,0.003791678,0.020376288,-0.191113331, 0.020376288,0.003791678,1.59314E-05},
				new[] {-0.000149424,-0.035562919,-0.191113391,1.792490633,-0.191113391,-0.035562919, -0.000149424},
				new[] {1.59314E-05,0.003791678,0.020376288,-0.191113331, 0.020376288,0.003791678,1.59314E-05 },
				new[] {2.96456E-06,0.000705566,0.003791678,-0.035562919,0.003791678,0.000705566,2.96456E-06  },
				new[] { 1.24562E-08,2.96456E-06,1.159314E-05,-0.000149424,1.159314E-05,2.96456E-06,1.24562E-08}
			}, 1);

		private static readonly ImageMask Js40 = new ImageMask(
			new[]
			{
				new[] {1.6236E-10,4.1165E-08,9.20847E-07,2.14132E-06,-1.8949E-05,2.14132E-06,9.20847E-07,4.1165E-08 ,1.6236E-10},
				new[] {4.1165E-08,1.0437E-05,0.000233473,0.000542915,-0.004804375,0.000542915,0.000233473,1.0437E-05,4.1165E-08 },
				new[] {9.20847E-07,0.000233473,0.000522272,0.012144825,-0.107472272,0.012144825,0.000522272,0.000233473,9.20847E-07 },
				new[] {2.14132E-06,0.000542915,0.012144825,0.028241375,-0.249914233,0.028241375,0.012144825,0.000542915,2.14132E-06 },
				new[] {-1.8949E-05,-0.004804375,-0.107472272,-0.249914233,2.211546853,-0.249914233,-0.107472272,-0.004804375,-1.8949E-05 },
				new[] {2.14132E-06,0.000542915,0.012144825,0.028241375,-0.249914233,0.028241375,0.012144825,0.000542915,2.14132E-06  },
				new[] {9.20847E-07,0.000233473,0.000522272,0.012144825,-0.107472272,0.012144825,0.000522272,0.000233473,9.20847E-07 },
				new[] {4.1165E-08,1.0437E-05,0.000233473,0.000542915,-0.004804375,0.000542915,0.000233473,1.0437E-05,4.1165E-08 },
				new[] {1.6236E-10,4.1165E-08,9.20847E-07,2.14132E-06,-1.8949E-05,2.14132E-06,9.20847E-07,4.1165E-08 ,1.6236E-10 }
			}, 1);

		private static readonly ImageMask Js50 = new ImageMask(
			new[]
			{
				new[] {5.26177E-10,1.32248E-07,3.7676E-06,4.92362E-06,-3.85865E-05,4.92362E-06,3.7676E-06,1.32248E-07,5.26177E-10},
				new[] {1.32248E-07,3.32391E-05,0.000695605,0.001237494,-0.009698272,0.001237494,0.000695605,3.32391E-05, 1.32248E-07},
				new[] {3.7676E-06,0.000695605,0.0145571357,0.025897446,-0.202958992,0.025897446,0.0145571357,0.000695605,3.7676E-06},
				new[] {4.92362E-06,0.001237494,0.025897446,0.046072025,-0.361067725,0.046072025, 0.025897446,0.001237494,4.92362E-06 },
				new[] {-3.85865E-05,-0.009698272,-0.202958992,-0.361067725,2.829697678,-0.361067725,-0.202958992,-0.009698272,-3.85865E-05 },
				new[] {4.92362E-06,0.001237494,0.025897446,0.046072025,-0.361067725,0.046072025, 0.025897446,0.001237494,4.92362E-06  },
				new[] {3.7676E-06,0.000695605,0.0145571357,0.025897446,-0.202958992,0.025897446,0.0145571357,0.000695605,3.7676E-06},
				new[] {1.32248E-07,3.32391E-05,0.000695605,0.001237494,-0.009698272,0.001237494,0.000695605,3.32391E-05, 1.32248E-07 },
				new[] {5.26177E-10,1.32248E-07,3.7676E-06,4.92362E-06,-3.85865E-05,4.92362E-06,3.7676E-06,1.32248E-07,5.26177E-10 }
			}, 1);

		private static readonly ComposedMask Sub21 = new ComposedMask
		{
			A = new ImageMask(new[]
			{
				new double[] {1, -2, -46, -2, 1},
				new double[] {-2, 4, 92, 4, -2},
				new double[] {-46, 92, 2116, 92, -46},
				new double[] {-2, 4, 92, 4, -2},
				new double[] {1, -2, -46, -2, 1}
			}, 2304),
			C = new ImageMask(new[]
			{
				new double[] { 0, 1, -7, -7, 1 },
				new double[] { 0, -2, 14, 14, -2 },
				new double[] { 0, -46, 322, 322, -46 },
				new double[] { 0, -2, 14, 14, -2 },
				new double[] { 0, 1, -7, -7, 1 }
			}, 576),
			B = new ImageMask(new[]
			{
				new double[] {0, 0, 0, 0, 0},
				new double[] {1, -2, -46, -2, 1},
				new double[] {-7, 14, 322, 14,-7},
				new double[] {-7, 14, 322, 14,-7},
				new double[] { 1, -2, -46, -2, 1 }
			}, 576),
			D = new ImageMask(new[]
			{
				new double[] { 0, 0, 0, 0, 0 },
				new double[] { 0, 1, -7, -7, 1 },
				new double[] { 0, -7, 49, 49, -7 },
				new double[] { 0, -7, 49, 49, -7 },
				new double[] { 0, 1, -7, -7, 1 }
			}, 144)
		};

		private static readonly HaarMask Haar = new HaarMask()
		{
			A = new ImageMask(new[]
			{
				new double[] { 1, 1 },
				new double[] { 1, 1 }
			}, 4),
			B = new ImageMask(new[]
			{
				new double[] { 1, 1 },
				new double[] { -1, -1 }
			}, 4),
			C = new ImageMask(new[]
			{
				new double[] { 1, -1 },
				new double[] { 1, -1 }
			}, 4),
			D = new ImageMask(new[]
			{
				new double[] { 1, -1 },
				new double[] { -1, 1 }
			}, 4)
		};
		private static readonly Vector Vl20;
		private static readonly Vector Vl30;
		private static readonly Vector Vl40;
		private static readonly Vector Vl50;
		private static readonly Vector Vh20;
		private static readonly Vector Vh30;
		private static readonly Vector Vh40;
		private static readonly Vector Vh50;
		private static readonly Vector Vk20;
		private static readonly Vector Vk30;
		private static readonly Vector Vk40;
		private static readonly Vector Vk50;
		private static readonly Vector Vs20;
		private static readonly Vector Vs30;
		private static readonly Vector Vs40;
		private static readonly Vector Vs50;

		private static void InitMasks()
		{
			_masks = new Dictionary<string, IMaskFilter>
			{
				{"JL20", Jl20},
				{"JL30", Jl30},
				{"JL40", Jl40},
				{"JL50", Jl50},
				{"JH20", Jh20},
				{"JH30", Jh30},
				{"JH40", Jh40},
				{"JH50", Jh50},
				{"JK20", Jk20},
				{"JK30", Jk30},
				{"JK40", Jk40},
				{"JK50", Jk50},
				{"JS20", Js20},
				{"JS30", Js30},
				{"JS40", Js40},
				{"JS50", Js50},
				{"Subdiv21", Sub21},
				{"Haar", Haar}
			};
		}

		private static void InitVectors()
		{
			_vectors = new Dictionary<string, Vector>
			{
				{"VL20", Vl20},
				{"VL30", Vl30},
				{"VL40", Vl40},
				{"VL50", Vl50},
				{"VH20", Vh20},
				{"VH30", Vh30},
				{"VH40", Vh40},
				{"VH50", Vh50},
				{"VK20", Vk20},
				{"VK30", Vk30},
				{"VK40", Vk40},
				{"VK50", Vk50},
				{"VS20", Vs20},
				{"VS30", Vs30},
				{"VS40", Vs40},
				{"VS50", Vs50}
			};
		}

		private static ImageMask _calcMaskFromVectors(double[] column, double[] row, double denominator = 1)
		{
			if (row.Length != column.Length)
			{
				throw new Exception("Для розразунку матриці потрібна однакова довжина векторів");
			}

			var len = column.Length;
			var matrix = new double[len][];
			for (int i = 0; i < len; i++)
			{
				matrix[i] = new double[len];
				for (int j = 0; j < len; j++)
				{
					matrix[i][j] = column[j] * row[i] / denominator;
				}
			}

			return new ImageMask(matrix);
		}

		public static IMaskFilter GetMask(string name)
		{
			if (_masks == null)
			{
				InitMasks();
			}

			if (_masks == null || !_masks.ContainsKey(name))
			{
				throw new ArgumentException($"Маски \"{name}\" немає в списку");
			}

			return _masks[name];
		}

		public static Vector GetVector(string name)
		{
			if (_vectors == null)
			{
				InitMasks();
			}

			if (_vectors == null || !_vectors.ContainsKey(name))
			{
				throw new ArgumentException($"Фільтру \"{name}\" немає в списку");
			}

			return _vectors[name];
		}
		
	}
}
