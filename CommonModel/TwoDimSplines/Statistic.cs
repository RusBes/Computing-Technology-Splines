using System;
using System.Drawing;

namespace CommonModel.TwoDimSplines
{
    
    class Statistic
    {
        
        double E_R_max = 0;
        double E_G_max = 0;
        double E_B_max = 0;
        double E_max_grey = 0;

	    private Bitmap _bmpBefor;
	    private Bitmap _bmpAfter;

		public Statistic(Bitmap before, Bitmap after)
	    {
		    
	    }

		/// <summary>
		/// возвращает массив double средних значений интенсивности каждого цвета на изображении
		/// </summary>
		/// <param name="bit"></param>
		/// <returns></returns>
		public double[] P_Average(Bitmap bit)
        {
            double[] Result = new double[3];
            for(int i=0;i<bit.Height;i++)
            {
                for(int j=0;j<bit.Width;j++)
                {
                    Color PixerColor = bit.GetPixel(j,i);
                    double R = PixerColor.R;
                    double G = PixerColor.G;
                    double B = PixerColor.B;
                    Result[0] += R;
                    Result[1] += G;
                    Result[2] += B;

                }
            }
            Result[0] = Result[0] / (bit.Height * bit.Width);
            Result[1] = Result[1] / (bit.Height * bit.Width);
            Result[2] = Result[2] / (bit.Height * bit.Width);
            return Result;
        }

		/// <summary>
		/// возвращает гамма Е по каждой компоненте
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <returns></returns>
		public double[] Gamma_E(Bitmap bit_start, Bitmap bit_after)
        {
            double[] result = new double[3];
            double[] E_av = E_average(bit_start, bit_after);

            for (int i = 0; i < bit_start.Height; i++)
            {
                for (int j = 0; j < bit_start.Width; j++)
                {
                    Color PixerColor = bit_start.GetPixel(j, i);
                    double R = PixerColor.R;
                    double G = PixerColor.G;
                    double B = PixerColor.B;

                    Color PixerColor2 = bit_after.GetPixel(j, i);
                    double R2 = PixerColor2.R;
                    double G2 = PixerColor2.G;
                    double B2 = PixerColor2.B;

                    double ER = Modul(R - R2);
                    double EG = Modul(G - G2);
                    double EB = Modul(B - B2);
                    result[0] += (ER - E_av[0]) * (ER - E_av[0]);
                    result[1] += (EG - E_av[1]) * (EG - E_av[1]);
                    result[2] += (EG - E_av[2]) * (EB - E_av[2]);
                }
            }
            result[0] = Math.Sqrt(result[0] / (bit_start.Height * bit_start.Width - 1));
            result[1] = Math.Sqrt(result[1] / (bit_start.Height * bit_start.Width - 1));
            result[2] = Math.Sqrt(result[2] / (bit_start.Height * bit_start.Width - 1));

            return result;  
        }
		/// <summary>
		/// возвращает гамма Е по всем 3 компонентам
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <returns></returns>
		public double[] Delta_E(Bitmap bit_start, Bitmap bit_after)
        {
            double[] result = new double[3];

            for(int i=0;i<bit_start.Width;i++)
            {
                for(int j=0;j<bit_start.Height;j++)
                {
                    double[] Eij = new double[3];

                    Color PixerColor = bit_start.GetPixel(i, j);
                    double R = PixerColor.R;
                    double G = PixerColor.G;
                    double B = PixerColor.B;

                    Color PixerColor2 = bit_after.GetPixel(i, j);
                    double R2 = PixerColor2.R;
                    double G2 = PixerColor2.G;
                    double B2 = PixerColor2.B;
                    Eij[0] = Modul(R - R2);
                    Eij[1] = Modul(G - G2);
                    Eij[2] = Modul(B - B2);

                    result[0] += Eij[0] / R;
                    result[1] += Eij[1] / G;
                    result[2] += Eij[2] / B;

                }
            }



            result[0] = result[0] / (bit_start.Height * bit_start.Width);
            result[1] = result[1] / (bit_start.Height * bit_start.Width);
            result[2] = result[2] / (bit_start.Height * bit_start.Width);
            return result;
        }

		/// <summary>
		/// по чб
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <param name="grey"></param>
		/// <returns></returns>
        public double Delta_E_grey(Bitmap bit_start, Bitmap bit_after)
        {
            double result = 0;

            for (int i = 0; i < bit_start.Width; i++)
            {
                for (int j = 0; j < bit_start.Height; j++)
                {
                    double Eij = 0;

                    Color PixerColor = bit_start.GetPixel(i, j);
                    double R = (PixerColor.R+ PixerColor.G + PixerColor.B) /3;

                    Color PixerColor2 = bit_after.GetPixel(i, j);
                    double R2 = (PixerColor2.R + PixerColor2.G + PixerColor2.B) / 3;
 
                    Eij = Modul(R - R2);


                    result += Eij / R;


                }
            }



            result = result / (bit_start.Height * bit_start.Width);
            return result;
        }

        //public double[] PSNR(double[] GE)
        //{
        //    double[] result = new double[3];

        //    for(int i=0;i<3;i++)
        //    {
        //        result[i] =  (10/(Math.Log(10))) * Math.Log((255 * 255) / (GE[i] * GE[i]));
        //    }

        //    return result;
        //}

        //public double PSNR(double GE)
        //{
        //    double result = 0;

        //        result = (10 / Math.Log(10)) * Math.Log((255 * 255) / (GE * GE));

        //    return result;
        //}

        public double GimmitheholyPSNR(Bitmap btmp11, Bitmap btmp22)
        {
            Bitmap btmp1 = new Bitmap(btmp11);
            Bitmap btmp2 = new Bitmap(btmp22);
            for (int i = 0; i < btmp1.Height; i++)
            {
                for (int j = 0; j < btmp1.Width; j++)
                {
                    Color color = btmp1.GetPixel(j, i);
                    btmp1.SetPixel(j, i, Color.FromArgb(255, (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B), (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B), (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B)));
                }
            }
            for (int i = 0; i < btmp2.Height; i++)
            {
                for (int j = 0; j < btmp2.Width; j++)
                {
                    Color color = btmp2.GetPixel(j, i);
                    btmp2.SetPixel(j, i, Color.FromArgb(255, (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B), (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B), (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B)));
                }
            }
            double mse = 0;
            for (int i = 0; i < btmp2.Height; i++)
                for (int j = 0; j < btmp2.Width; j++)
                    mse = mse + Math.Pow(btmp1.GetPixel(j, i).R - btmp2.GetPixel(j, i).R, 2);
            mse = mse / btmp2.Height / btmp2.Width;
            double psnr = 10 * Math.Log10(255 * 255 / mse);
            return psnr;
        }

		/// <summary>
		/// возвращает гамма Е по чернобелому
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public double Gamma_E(Bitmap bit_start, Bitmap bit_after, bool flag)
        {
            double result =0;
            double E_av = E_average(bit_start, bit_after,true);

            for (int i = 0; i < bit_start.Height; i++)
            {
                for (int j = 0; j < bit_start.Width; j++)
                {
                    Color PixerColor = bit_start.GetPixel(j, i);
                    double R = (PixerColor.R+ PixerColor.G + PixerColor.B) /3;

                    Color PixerColor2 = bit_after.GetPixel(j, i);
                    double R2 = (PixerColor2.R+ PixerColor2.G + PixerColor2.B) /3;


                    double ER = Modul(R - R2);

                    result += (ER - E_av) * (ER - E_av);

                }
            }
            result = Math.Sqrt(result / (bit_start.Height * bit_start.Width - 1));


            return result;
        }

		/// <summary>
		/// возвращает double средних значений интенсивности в чернобелом растре на изображении
		/// </summary>
		/// <param name="bit"></param>
		/// <param name="grey"></param>
		/// <returns></returns>
		public double P_Average(Bitmap bit,bool grey)
        {
            double Result=0;
            for (int i = 0; i < bit.Height; i++)
            {
                for (int j = 0; j < bit.Width; j++)
                {
                    Color PixerColor = bit.GetPixel(j, i);
                    double R = PixerColor.R;
                    double G = PixerColor.G;
                    double B = PixerColor.B;
                    Result += (R+G+B)/3;


                }
            }
            return Result / (bit.Height * bit.Width);
        }

		/// <summary>
		/// возвращает массив double Гамм по интевсивностях каждого цвета
		/// </summary>
		/// <param name="bit"></param>
		/// <returns></returns>
		public double[] Disp_p(Bitmap bit)
		{
			double[] Result = new double[3];
			double[] p_av = P_Average(bit);
			for (int i = 0; i < bit.Height; i++)
			{
				for (int j = 0; j < bit.Width; j++)
				{
					Color PixerColor = bit.GetPixel(j, i);
					double R = PixerColor.R;
					double G = PixerColor.G;
					double B = PixerColor.B;

					Result[0] += (R - p_av[0]) * (R - p_av[0]);
					Result[1] += (G - p_av[1]) * (G - p_av[1]);
					Result[2] += (B - p_av[2]) * (B - p_av[2]);

				}
			}
			Result[0] = Result[0] / ((bit.Height * bit.Width) - 1);
			Result[1] = Result[1] / ((bit.Height * bit.Width) - 1);
			Result[2] = Result[2] / ((bit.Height * bit.Width) - 1);
			return Result;
		}

		/// <summary>
		/// возвращает  double Гамм по интевсивности в чб
		/// </summary>
		/// <param name="bit"></param>
		/// <param name="grey"></param>
		/// <returns></returns>
		public double Disp_p(Bitmap bit,bool grey)
        {
            double Result = 0;
            double p_av = P_Average(bit,true);
            for (int i = 0; i < bit.Height; i++)
            {
                for (int j = 0; j < bit.Width; j++)
                {
                    Color PixerColor = bit.GetPixel(j, i);
                    double R = (PixerColor.R+ PixerColor.G + PixerColor.B) /3;

                    Result += (R - p_av) * (R - p_av);

                }
            }
            Result = Result / ((bit.Height * bit.Width) - 1);

            return Result;
        }

        //тупо модуль
        public double Modul(double a)
        {
            if (a < 0)
                return a * (-1);
            else
                return a;
        }


		/// <summary>
		/// Среднее Е по всем компонентам цвета
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <returns></returns>
		public double[] E_average(Bitmap bit_start, Bitmap bit_after)
        {
            double[] result = new double[3];

            for (int i = 0; i < bit_start.Height; i++)
            {
                for (int j = 0; j < bit_start.Width; j++)
                {
                    Color PixerColor = bit_start.GetPixel(j, i);
                    double R = PixerColor.R;
                    double G = PixerColor.G;
                    double B = PixerColor.B;

                    Color PixerColor2 = bit_after.GetPixel(j, i);
                    double R2 = PixerColor2.R;
                    double G2 = PixerColor2.G;
                    double B2 = PixerColor2.B;

                    result[0] += Modul(R-R2);
                    if (Modul(R - R2) > E_R_max) E_R_max = Modul(R - R2);
                    result[1] += Modul(G - G2);
                    if (Modul(G - G2) > E_G_max) E_G_max = Modul(G - G2);
                    result[2] += Modul(B - B2);
                    if (Modul(B - B2) > E_B_max) E_B_max = Modul(B - B2);
                }
            }
                    return result;
        }

		/// <summary>
		/// Среднее Е по чб
		/// </summary>
		/// <param name="bit_start"></param>
		/// <param name="bit_after"></param>
		/// <param name="grey"></param>
		/// <returns></returns>
		public double E_average(Bitmap bit_start, Bitmap bit_after, bool grey)
        {
            double result =0;

            for (int i = 0; i < bit_start.Height; i++)
            {
                for (int j = 0; j < bit_start.Width; j++)
                {
                    Color PixerColor = bit_start.GetPixel(j, i);
                    double R = (PixerColor.R+ PixerColor.G + PixerColor.B) /3;

                    Color PixerColor2 = bit_after.GetPixel(j, i);
                    double R2 = (PixerColor2.R+ PixerColor2.G + PixerColor2.B) /3;

                    double mod = Modul(R - R2);
                    result += mod;
                    if (mod > E_max_grey) E_max_grey = mod;

                }
            }
            return result;
        }
    }
}
