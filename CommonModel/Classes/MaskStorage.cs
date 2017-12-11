﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Classes
{
	class MaskStorage
	{
		private bool _isInit = false;

		private Dictionary<string, Mask> _masks;

		// Low
		private readonly Mask _jL20 = new Mask(
			new[]
			{
				new double[] { 1, 6, 1 },
				new double[] { 6, 36, 6 },
				new double[] { 1, 6, 1 }
			}, 64);

		private readonly Mask _jL30 = new Mask(
			new[]
			{
				new double[] { 1, 4, 1 },
				new double[] { 4, 16, 4 },
				new double[] { 1, 4, 1 }
			}, 36);

		private readonly Mask _jL40 = new Mask(
			new[]
			{
				new double[] { 1, 76, 230, 76, 1},
				new double[] { 76, 5776, 17480, 5776, 76},
				new double[] { 230, 17480, 52900, 17480, 230},
				new double[] { 1, 76, 230, 76, 1},
				new double[] { 76, 5776, 17480, 5776, 76}
			}, 147456);

		private readonly Mask _jL50 = new Mask(
			new[]
			{
				new double[] { 1, 26, 66, 26, 1},
				new double[] { 26, 676, 1716, 676, 26},
				new double[] { 66, 1716, 4386, 1716, 66},
				new double[] { 26, 676, 1716, 676, 26},
				new double[] { 1, 26, 66, 26, 1}
			}, 14400);

		// Hight
		private readonly Mask _jH20 = new Mask(
			new[]
			{
				new double[] { -1, -6, -1},
				new double[] { -6, 28, -6},
				new double[] { -1, -6, -1}
			}, 64);

		private readonly Mask _jH30 = new Mask(
			new[]
			{
				new double[] { -1, -4, -1},
				new double[] { -4, 20, -4},
				new double[] { -1, -4, -1}
			}, 36);

		private readonly Mask _jH40 = new Mask(
			new[]
			{
				new double[] { -1, -76, -230, -76, -1},
				new double[] { -76, -5776, -17480, -5776, -76},
				new double[] { -230, -17480, 94556, -17480, -230},
				new double[] { -1, -76, -230, -76, -1},
				new double[] { -76, -5776, -17480, -5776, -76}
			}, 147456);

		private readonly Mask _jH50 = new Mask(
			new[]
			{
				new double[] { -1, -26, -66,-276, -1},
				new double[] { -26, -676, -1716, -676, -26},
				new double[] { -66, -1716, 10044, -1716, -66},
				new double[] { -26, -676, -1716, -676, -26},

				new double[] { -1, -26, -66,-276, -1}
			}, 14400);

		// Kontrast
		private readonly Mask _jK20 = new Mask(
			new[]
			{
				new double[] { 1, -8, 48, -8, 1},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 48, -384, 2304, -384, 48},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 1, -8, 48, -8, 1}
			}, 1156);

		private readonly Mask _jK30 = new Mask(
			new[]
			{
				new double[] { 1, -6, 24, -6, 1},
				new double[] { -6, 36, -144, 36, -6},
				new double[] { 24, -144, 576, -144, 24},
				new double[] { -6, 36, -144, 36, -6},
				new double[] { 1, -6, 24, -6, 1}
			}, 196);

		private readonly Mask _jK40 = new Mask(
			new[]
			{
				new double[] { 1, -8, 48, -8, 1},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 48, -384, 2304, -384, 48},
				new double[] { -8, 64, -384, 64, -8},
				new double[] { 1, -8, 48, -8, 1}
			}, 1);

		private readonly Mask _jK50 = new Mask(
			new[]
			{
				new double[] { 1, 6, 1 },
				new double[] { 6, 36, 6 },
				new double[] { 1, 6, 1 }
			}, 25784009476);

		// stabilisators
		private readonly Mask _jS20 = new Mask(
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

		private readonly Mask _jS30 = new Mask(
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

		private readonly Mask _jS40 = new Mask(
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

		private readonly Mask _jS50 = new Mask(
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

		private List<Mask> _sub21 = new List<Mask>()
		{
			new Mask(new[]
			{
				new double[] {1, -2, 46, -2, 1},
				new double[] {-2, 4, 92, 4, -2},
				new double[] {-46, 92, 2116, 92, -46},
				new double[] {-2, 4, 92, 4, -2},
				new double[] {1, -2, 46, -2, 1}
			}, 2304),
			new Mask(new[]
			{
				new double[] { 0, 1, -7, -7, 1 },
				new double[] { 0, -2, 14, 14, -2 },
				new double[] { 0, -46, 322, 322, -46 },
				new double[] { 0, -2, 14, 14, -2 },
				new double[] { 0, 1, -7, -7, 1 }
			}, 576),
			new Mask(new[]
			{
				new double[] {0, 0, 0, 0, 0},
				new double[] {1, -2, -46, -2, 1},
				new double[] {-7, 14, 322, 14,-7},
				new double[] {-7, 14, 322, 14,-7},
				new double[] { 1, -2, -46, -2, 1 }
			}, 576),
			new Mask(new[]
			{
				new double[] { 0, 0, 0, 0, 0 },
				new double[] { 0, 1, -7, -7, 1 },
				new double[] { 0, -7, 49, 49, -7 },
				new double[] { 0, -7, 49, 49, -7 },
				new double[] { 0, 1, -7, -7, 1 }
			}, 144)
		};

		private void Init()
		{
			_masks = new Dictionary<string, Mask>();
			_masks.Add("JL20", _jL20);
			_masks.Add("JL30", _jL30);
			_masks.Add("JL40", _jL40);
			_masks.Add("JL50", _jL50);
			_masks.Add("JH20", _jH20);
			_masks.Add("JH30", _jH30);
			_masks.Add("JH40", _jH40);
			_masks.Add("JH50", _jH50);
			_masks.Add("JK20", _jK20);
			_masks.Add("JK30", _jK30);
			_masks.Add("JK40", _jK40);
			_masks.Add("JK50", _jK50);
			_masks.Add("JS20", _jS20);
			_masks.Add("JS30", _jS30);
			_masks.Add("JS40", _jS40);
			_masks.Add("JS50", _jS50);
			_masks.Add("sub21", _sub21);

			_isInit = true;
		}

		public void Get(string name)
		{
			
		}
	}
}
