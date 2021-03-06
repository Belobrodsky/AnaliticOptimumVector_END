﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnaliticOptimumVector
{
    public partial class Form1 : Form
    {

        List<List<double>> vecA;
        List<List<double>> vecB;
        
        List<List<double>> vecRaz;
        List<List<double>> vecChas;
        
        public Form1()
        {
            InitializeComponent();

            vecA = new List<List<double>>();
            vecB = new List<List<double>>();

            vecRaz = new List<List<double>>();
            vecChas = new List<List<double>>();

        }


        public void Normalize(List<Double> src)
        {
            //double sr = src.Average();
            ////для 0 среднего
            //for (int i = 0; i < src.Count; i++)
            //{
            //    src[i] = src[i] - sr;
            //}


            double A = new double();
            A = 0.0;
            //для нахождения энергии начального
            for (int i = 0; i < src.Count; i++)
            {
                A = A + src[i] * src[i];
            }


            //для нормализации каждого
            for (int i = 0; i < src.Count; i++)
            {
                src[i] = src[i] / ((double)Math.Sqrt(A));
            }


            //return src;
        }



        private double PrintNorma(List<double> src)
        {
            //для контрольной проверки
            double A = 0;
            for (int i = 0; i < src.Count; i++)
            {
                A = A + src[i] * src[i];
            }

            return Math.Sqrt(A);
        }



        /// <summary>
        /// Открыть вектора А
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void открытьAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cc = 0;
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {

                bool f = false;

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

               

                foreach (var FileName in openFileDialog1.FileNames)
                {

                    StreamReader sr = new StreamReader(FileName);
                    string line = "123";

                    List<double> vecA_single = new List<double>();

                    int c = 0;
                    
                    while (line != null)
                    {
                        line = sr.ReadLine();

                        if (line != null)
                        {
                            //   MessageBox.Show(vecA.Count.ToString());
                            vecA_single.Add(Double.Parse(line));
                           
                            if (!f)
                            {
                                c++;
                                comboBox1.Items.Add(c);
                                comboBox2.Items.Add(c);
                            }
                       
                        }
                    }

                    f = true;


                    if ((checkBox5.Checked) && (PrintNorma(vecA_single)!=0))
                    {
                        Normalize(vecA_single); //так как по ссылке то так просто

                        cc++;
                    }
                    

                    vecA.Add(vecA_single);

                    chart1.Series[0].Points.AddXY(vecA_single[0], vecA_single[1]);



                   // MessageBox.Show(vecA_single[0].ToString()+" "+ vecA_single[1].ToString());
                    
                   sr.Close();

                } //foreach

                
                } //if Open

            MessageBox.Show("Добавлено" + vecA.Count.ToString()  + " векторов типа А из них отнормировано было " + cc.ToString());
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            //  openFileDialog1.

        }

        private void открытьBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cc = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                foreach (var FileName in openFileDialog1.FileNames)
                {

                    StreamReader sr = new StreamReader(FileName);
                    string line = "123";

                    List<double> vecB_single = new List<double>();

                    while (line != null)
                    {
                        line = sr.ReadLine();

                        if (line != null)
                        {
                            //   MessageBox.Show(vecA.Count.ToString());
                            vecB_single.Add(Double.Parse(line));
                        }
                    }


                    if ((checkBox5.Checked) && (PrintNorma(vecB_single) != 0))
                    {
                        Normalize(vecB_single); //так как по ссылке то так просто
                        cc++;
                    }
                   // 

                    vecB.Add(vecB_single);

                    chart1.Series[1].Points.AddXY(vecB_single[0], vecB_single[1]);

                    sr.Close();

                } //foreach


            } //if Open

            MessageBox.Show("Добавлено" + vecB.Count.ToString() + " векторов типа B из них отнормированно было " + cc.ToString());
        }


        //private double skalyar(List<double> a, List<double> b)
        //{
        
        //    double sum = 0.0;
        //    for (int i = 0; i < a.Count; i++)
        //    {
        //        sum += a[i] * b[i];
        //    }

        //    return sum;
        //}

        //private double kriterii(List<double> a, List<double> b, List<double> c)
        //{
        //    return Math.Abs(skalyar(a,c)-skalyar(b,c));
        //}



        /// <summary>
        /// Реализуем формулу Киселева
        /// </summary>
        /// <param name="F1"></param>
        /// <param name="F2"></param>
        /// <returns></returns>
        private List<double> FindOptimumRazniza(List<double> F1, List<double> F2)
        { 

        List<double> res = new List<double>();

        for (int i = 0; i < F1.Count; i++)
        {

            res.Add(F2[i]-F1[i]);
        }

        double temp = PrintNorma(res);

        for (int i = 0; i < F1.Count; i++)
        {
            res[i] = res[i] / temp;
        }
         return res;
        }


        private List<double> FindOptimumChastnoe(List<double> F1, List<double> F2)
        {
            List<double> res = new List<double>();


            //     F3_kiselev.Add(vecA[i]-F1F2*vecB[i]/F2F2);

           // res.Add(F1[])

            double F2F2 = PrintNorma(F2)*PrintNorma(F2); //ну судя по формуле скалярного произведения и норму вектора так оно и есть

            //найдем F1F2
            double F1F2 = 0.0;


            for (int i = 0; i < F1.Count; i++)
            {
                F1F2 += F1[i] * F2[i];
            }


            //теперь когда все посчитано реализуем формулу Киселева


            //F3_kiselev.Add(vecA[i]-F1F2*vecB[i]/F2F2);

            for (int i = 0; i < F1.Count; i++)
            {
                res.Add(F1[i]-F1F2*F2[i]/F2F2);
            }
            

            return res; //Ну все посчитано
        }




   









        //Переберем всевозможные варианты векторов А и Б и найдем для каждого из них оптимальный вектор разницы и частного
        private void найтиСToolStripMenuItem_Click(object sender, EventArgs e)
        {

            





            //пройдемся по всем векторам A
            foreach (var A_single in vecA)
            {
                
                //для каждого конкретно A пробежимся по всем векторам B

                foreach (var B_single in vecB)
                {
                    
                    //И сформируем массив из оптимальных векторов для разницы
                    vecRaz.Add(FindOptimumRazniza(A_single,B_single));
                 //   chart1.Series[2].Points.AddXY(vecRaz[vecRaz.Count-1][0], vecRaz[vecRaz.Count - 1][1]);



                    vecChas.Add(FindOptimumChastnoe(A_single,B_single));
                   // chart1.Series[3].Points.AddXY(vecChas[vecChas.Count - 1][0], vecChas[vecChas.Count - 1][1]);

                }

            }

            // comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 1;

            OnChangeComboboxes(this, null);

            // if (saveFileDialog1.ShowDialog()==DialogResult.OK)
            // {

            StreamWriter sw = new StreamWriter("OPTIMUM RAZ.csv");

            //по длине сигнала
                for (int i = 0; i < vecRaz[0].Count; i++)
                {

                    //по кол-во сигналов
                    for (int j = 0; j < vecRaz.Count; j++)
			             {			 
                        sw.Write("{0};",vecRaz[j][i]);
			             }
                    sw.WriteLine();
                }
             sw.Close();


           StreamWriter sw2 = new StreamWriter("OPTIMUM CHASNOE.csv");
            //по длине сигнала
                for (int i = 0; i < vecRaz[0].Count; i++)
                {
                    //по кол-во сигналов
                    for (int j = 0; j < vecRaz.Count; j++)
			             {			 
                        sw2.Write("{0};",vecChas[j][i]);
			             }                
                    sw2.WriteLine();
                }
             sw2.Close();


            StreamWriter sw3 = new StreamWriter("sos_1_vhod_NORM_" +checkBox5.Checked + "_.csv");
            //по длине сигнала
            for (int i = 0; i < vecA[0].Count; i++)
            {
                //по кол-во сигналов
                for (int j = 0; j < vecA.Count; j++)
                {
                    sw3.Write("{0};", vecA[j][i]);
                }
                sw3.WriteLine();
            }
            sw3.Close();


            StreamWriter sw4 = new StreamWriter("sos_2_vhod_NORM_" + checkBox5.Checked + "_.csv");
            //по длине сигнала
            for (int i = 0; i < vecB[0].Count; i++)
            {
                //по кол-во сигналов
                for (int j = 0; j < vecB.Count; j++)
                {
                    sw4.Write("{0};", vecB[j][i]);
                }
                sw4.WriteLine();
            }
            sw4.Close();



            //     }
            //






            //    //начнем считать свертку как в основной программе
            //    double sum = new float();
            //    double xx = new float();
            //    double hh = new float();


            //for (int k = 0; k < vecA.Count; k++) //пробежимся по длине всего сигнала
            //{
            //    sum = 0;
            //    for (int l = 0; l < vecB.Count; l++) //по длине всего фильтра
            //    {
            //        if (k + l < vecA.Count)
            //        {
            //            xx = vecA[k + l];
            //            hh = vecB[l];
            //        }
            //        else { xx = 0; hh = 0; }
            //        sum = sum + xx * hh;
            //    }
            //    vecC.Add(sum);
            //} //for k  



            //double F1F2 = 0.0;
            //double F1F1 = 0.0;
            //double F2F2 = 0.0;


            //if (vecA.Count==vecB.Count)
            //{
            //    for (int i = 0; i < vecA.Count; i++)
            //    {
            //        //F1F2 += vecA[i] * vecB[i];
            //        //F1F1 += vecA[i] * vecA[i];
            //        //F2F2 += vecB[i] * vecB[i]; 
            //    }
            //}


            //найдем B1 и B2 согласно формудам Борзунову

            //double B1 = 1 / Math.Sqrt(F1F1-2*F1F2+F2F2);
            //double B2 = -1 / Math.Sqrt(F1F1 - 2 * F1F2 + F2F2);

            ////найдем A1,A2

            //double A1= (-B1*F1F2 + Math.Sqrt((B1*B1)*((F1F2*F1F2)-(F1F1*F2F2))+F1F1))/F1F1;

            //double A2 = (-B1 * F1F2 - Math.Sqrt((B1 * B1) * ((F1F2 * F1F2) - (F1F1 * F2F2)) + F1F1)) / F1F1;

            //double A3 = (-B2 * F1F2 + Math.Sqrt((B2 * B2) * ((F1F2 * F1F2) - (F1F1 * F2F2)) + F1F1)) / F1F1;

            //double A4 = (-B2 * F1F2 - Math.Sqrt((B2 * B2) * ((F1F2 * F1F2) - (F1F1 * F2F2)) + F1F1)) / F1F1;

            //Таким образом, у нас имееться 8 комбинаций и лишь одна из них максималльная

            //Найдем ее путем перебора

            //List<Double>[] F3 = new List<double>[8];


            //List<Double> F3_kiselev = new List<double>(); //Поместим сюда лучший по формуле киселева для деления


            //for (int i = 0; i < F3.Length; i++)
            //{
            //    F3[i] = new List<double>();
            //}




            //C=A-(A,B)B/|B|^{2}

            //сформируем 8 разнообразных векторов- кандидатов
            //for (int i = 0; i < vecA.Count; i++)
            //{
            //    //F3[0].Add(A1 * vecA[i] + B1 * vecB[i]);
            //    //F3[1].Add(A2 * vecA[i] + B1 * vecB[i]);
            //    //F3[2].Add(A3* vecA[i] + B1 * vecB[i]);
            //    //F3[3].Add(A4 * vecA[i] + B1 * vecB[i]);

            //    //F3[4].Add(A1 * vecA[i] + B2 * vecB[i]);
            //    //F3[5].Add(A2 * vecA[i] + B2 * vecB[i]);
            //    //F3[6].Add(A3 * vecA[i] + B2 * vecB[i]);
            //    //F3[7].Add(A4 * vecA[i] + B2 * vecB[i]);  


            //    //F3_kiselev.Add(vecA[i]-F1F2*vecB[i]/F2F2);

            //}


            ////Итак вектора сформированы

            ////найдем максимум самый лучший из них

            //int maxIndex =-1; //предположим максимум самый первый вектор
            //double Max = -1;//kriterii(vecA,vecB,F3[0]);

            //for (int i = 0; i < 8; i++)
            //{
            //    if ((kriterii(vecA,vecB,F3[i])>Max) && (Math.Abs(PrintNorma(F3[i])-1)<0.01))
            //    { 
            //        Max = kriterii(vecA, vecB, F3[i]);
            //        maxIndex = i;
            //    }
            //}





            //            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //            {

            //                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName + ".csv");

            //                sw.WriteLine("signal F1;signal F2; OPTIMUM F3 (F1F3-F2F3=MAX) ; |F1F3-F2F3|= ;a=;b=;NORMA F1;NORMA F2;NORMA F3; alpha^2+2alpha*beta*(f1,f2) +beta^2 = 1 ???;|F1F3/F2F3|= MAX ;");

            //                int length;
            //                length = Math.Max(vecA.Count, vecB.Count);
            //                length = Math.Max(length, F3[maxIndex].Count);


            //                for (int i = 0; i < length; i++)
            //                {


            //                    //сигнал F1
            //                    if (i<vecA.Count)
            //                    {
            //                        sw.Write(vecA[i]+";");
            //                    }
            //                    else sw.Write(";");


            //                    //сигнал F2
            //                    if (i < vecB.Count)
            //                    {
            //                        sw.Write(vecB[i] + ";");
            //                    }
            //                    else sw.Write(";");

            //                    //оптимум F3
            //                    if (i < F3[maxIndex].Count)
            //                    {
            //                        sw.Write(F3[maxIndex][i] + ";");
            //                    }
            //                    else sw.Write(";");


            //                  //  sw.WriteLine("signal F1;signal F2; OPTIMUM F3; |F1F3-F2F3|=;a=;b=");


            //                    //тут выведем |F1F3-F2F3|=
            //                    if (i == 0)
            //                    {
            //                       // sw.Write(kriterii(vecA, vecB, F3[maxIndex]) + ";");
            //                    }
            //                    else sw.Write(";");



            //                    //тут выведем a и b оптимального фильтра
            //                    if (i == 0)
            //                    {
            //                        switch (maxIndex)
            //                        {
            //                //F3[0].Add(A1 * vecA[i] + B1 * vecB[i]);
            //                //F3[1].Add(A2 * vecA[i] + B1 * vecB[i]);
            //                //F3[2].Add(A3* vecA[i] + B1 * vecB[i]);
            //                //F3[3].Add(A4 * vecA[i] + B1 * vecB[i]);

            //                //F3[4].Add(A1 * vecA[i] + B2 * vecB[i]);
            //                //F3[5].Add(A2 * vecA[i] + B2 * vecB[i]);
            //                //F3[6].Add(A3 * vecA[i] + B2 * vecB[i]);
            //                //F3[7].Add(A4 * vecA[i] + B2 * vecB[i]);


            //                            case 0:
            //                                 sw.Write("{0};{1};",A1,B1);
            //                                break;
            //                            case 1:
            //                                sw.Write("{0};{1};", A2, B1);
            //                                break;
            //                            case 2:
            //                                sw.Write("{0};{1};", A3, B1);
            //                                break;
            //                            case 3:
            //                                sw.Write("{0};{1};", A4, B1);
            //                                break;
            //                            case 4:
            //                                sw.Write("{0};{1};", A1, B2);
            //                                break;
            //                            case 5:
            //                                sw.Write("{0};{1};", A2, B2);
            //                                break;
            //                            case 6:
            //                                sw.Write("{0};{1};", A3, B2);
            //                                break;
            //                            case 7:
            //                                sw.Write("{0};{1};", A4, B2);
            //                                break;

            //                            default:
            //                                break;
            //                        }
            //                    }
            //                    else sw.Write(";;");

            //                    //тут выведем нормы
            //                    if (i == 0)
            //                    {
            //                     //   sw.Write("{0};{1};{2};", PrintNorma(vecA), PrintNorma(vecB), PrintNorma(F3[maxIndex]));
            //                    }
            //                    else sw.Write(";;;");


            //                    //Выведем дополнительныю проверку из Борзуновского письма
            //                    if (i == 0)
            //                    {
            //                        switch (maxIndex)
            //                        {
            //                            //F3[0].Add(A1 * vecA[i] + B1 * vecB[i]);
            //                            //F3[1].Add(A2 * vecA[i] + B1 * vecB[i]);
            //                            //F3[2].Add(A3* vecA[i] + B1 * vecB[i]);
            //                            //F3[3].Add(A4 * vecA[i] + B1 * vecB[i]);

            //                            //F3[4].Add(A1 * vecA[i] + B2 * vecB[i]);
            //                            //F3[5].Add(A2 * vecA[i] + B2 * vecB[i]);
            //                            //F3[6].Add(A3 * vecA[i] + B2 * vecB[i]);
            //                            //F3[7].Add(A4 * vecA[i] + B2 * vecB[i]);

            ////alpha^2+2alpha*beta*(f1,f2) +beta^2
            //                            case 0:
            //                                sw.Write("{0};", Math.Sqrt(A1*A1+2*A1*B1*F1F2+B1*B1));
            //                                break;
            //                            case 1:
            //                                sw.Write("{0};", Math.Sqrt(A2 * A2 + 2 * A2 * B1 * F1F2 + B1 * B1));
            //                                break;
            //                            case 2:
            //                                sw.Write("{0};", Math.Sqrt(A3 * A3 + 2 * A3 * B1 * F1F2 + B1 * B1));
            //                                break;
            //                            case 3:
            //                                sw.Write("{0};", Math.Sqrt(A4 * A4 + 2 * A4 * B1 * F1F2 + B1 * B1));
            //                                break;
            //                            case 4:
            //                                sw.Write("{0};", Math.Sqrt(A1 * A1 + 2 * A1 * B2 * F1F2 + B2 * B2));
            //                                break;
            //                            case 5:
            //                                sw.Write("{0};", Math.Sqrt(A2 * A2 + 2 * A2 * B2 * F1F2 + B2 * B2));
            //                                break;
            //                            case 6:
            //                                sw.Write("{0};", Math.Sqrt(A3 * A3 + 2 * A3 * B2 * F1F2 + B2 * B2));
            //                                break;
            //                            case 7:
            //                                sw.Write("{0};", Math.Sqrt(A4 * A4 + 2 * A4 * B2 * F1F2 + B2 * B2));
            //                                break;

            //                            default:
            //                                break;
            //                        }
            //                    }
            //                    else sw.Write(";");


            //                    if (i < F3_kiselev.Count)
            //                    {
            //                        sw.Write("{0};", F3_kiselev[i]);
            //                    }
            //                    else sw.Write(";");



            //                    sw.WriteLine();
            //                }
            //                   sw.Close();


            //            }

        }

        private void OnChangeComboboxes(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();


            int X = (int)comboBox1.SelectedIndex;
            int Y = (int)comboBox2.SelectedIndex;


            if (X<0)
            {
                X = 0;
            }


            if (Y<0)
            {
                Y = 1;
            }


            
            

         //  MessageBox.Show(X.ToString() + " " + Y.ToString());

            foreach (var item in vecA)
            {
                chart1.Series[0].Points.AddXY(item[X], item[Y]);

            }

            foreach (var item in vecB)
            {
                chart1.Series[1].Points.AddXY(item[X], item[Y]);
            }

            foreach (var item in vecRaz)
            {
                chart1.Series[2].Points.AddXY(item[X], item[Y]);
            }

            foreach (var item in vecChas)
            {
                chart1.Series[3].Points.AddXY(item[X], item[Y]);
            }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[0].Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[1].Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[2].Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[3].Enabled = checkBox4.Checked;
        }
    }
}
