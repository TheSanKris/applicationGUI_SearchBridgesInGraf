using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FiindBrigeInGraf
{

    internal class Graf
    {
        public void Init(List<MyPair> data)
        {
            m_mpGrafRebra = data;
            int number = 1;
            m_intStoryOfFathers.Add(number);

        }

        public Flags NextStep()
        {
            m_mpNoBridgeRebra.Clear();
            m_mpBridgeRebra.Clear();

            if (m_mpGrafRebra.Count() == 0)
                return Flags.END;

            int father = m_intStoryOfFathers.Last();
            MyPair m_mpRebro = FindRebro(father);
            m_mpThisRebro = m_mpRebro;

            m_mpGrafRebra.Remove(m_mpRebro);
            MyPair tmp = new MyPair();
            tmp.first = m_mpRebro.second;
            tmp.second = m_mpRebro.first;
            m_mpGrafRebra.Remove(tmp);

            m_mpStoryOfWay.Add(m_mpRebro);
            m_intStoryOfFathers.Add(m_mpRebro.second);

            m_flgFlag = GetFlag(m_mpRebro);

            //Console.WriteLine(m_mpRebro.first + " " + m_mpRebro.second); //UDOLI

            switch (m_flgFlag)
            {
                case Flags.NEXT:

                    return Flags.NEXT;

                case Flags.NO_BRIDGE:
                    DeleteData(m_mpRebro);
                    IsertStoryDataInGrafRebra();

                    return Flags.NO_BRIDGE;

                case Flags.IS_BRIDGE:
                    DeleteData(m_mpRebro);
                    IsertStoryDataInGrafRebra();

                    return Flags.IS_BRIDGE;

            }

            return Flags.END;


        }

        private MyPair FindRebro(int father)
        {
            MyPair result = m_mpGrafRebra.Find(parr => parr.first == father);
            //Нашёл | не нашёл
            if (result == null)
            {
                result = m_mpGrafRebra.Find(parr => parr.second == father);
                if (result != null)
                    ReverseRebro(result);
            }

            return result;

        }

        private void ReverseRebro(MyPair rebro)
        {
            m_mpGrafRebra.Remove(rebro);
            int frst = 0;
            frst = rebro.first;

            int scnd = 0;
            scnd = rebro.second;
            rebro.second = frst;
            rebro.first = scnd;
            m_mpGrafRebra.Add(rebro);

        }

        private Flags GetFlag(MyPair rebro)
        {
            int nextFather = m_intStoryOfFathers.Last();
            MyPair nextRebro = FindRebro(nextFather);

            if (IsNoBridge(rebro))
                return Flags.NO_BRIDGE;
            if (IsBridge(rebro))
                return Flags.IS_BRIDGE;
            if (nextRebro != null)
                return Flags.NEXT;

            return Flags.END;

        }

        private bool IsBridge(MyPair rebro)
        {
            int nextFather = m_intStoryOfFathers.Last();
            MyPair nextRebro = FindRebro(nextFather);

            if ((nextRebro == null) && (!HZ(rebro)))
                return true;

            return false;
        }

        /*
         * int nextFather = m_intStoryOfFathers.Last();
            MyPair nextRebro = FindRebro(nextFather);

            int count = 0;
            foreach (var da in m_intStoryOfFathers)
            {
                if (da == rebro.second)
                    count++;
            }

            if ((nextRebro == null) && (count < 2) && (!HZ(rebro)))
                return true;

            return false;
         */

        private bool HZ(MyPair rebro)
        {
            int frst = rebro.first;
            int scnd = rebro.second;
            int tmp1 = 0;
            int tmp2 = 0;
            tmp1 = m_intStoryOfFathers.Find(fthr => fthr == frst);
            tmp2 = m_intStoryOfFathers.Find(fthr => fthr == frst);
            if (tmp1 == frst)
            {
                tmp2 = m_intVershinsNoRebra.Find(fthr => fthr == frst);

            }
            else if (tmp2 == scnd)
            {
                tmp1 = m_intVershinsNoRebra.Find(fthr => fthr == frst);
                if (tmp1 == frst)
                    return true;
            }

            return false;
        } 

        private bool IsNoBridge(MyPair rebro)
        {
            int father = rebro.second;
            int counter = 0;

            foreach(var da in m_intStoryOfFathers)
            {
                if(da == father)
                    counter++; 
            }    

            if (counter > 1)
                return true;

            return false;

        }

        /*
         * int father = rebro.second;
            int SOVPADENIE = 0;
            SOVPADENIE = m_intStoryOfFathers.Find(fthr => fthr == father);

            if (SOVPADENIE != 0)
                return true;

            return false;
         */

        private void DeleteData(MyPair rebro)
        {
            if (m_flgFlag == Flags.IS_BRIDGE)
            {
                m_mpStoryOfWay.Remove(rebro);

                //Чистим вершины
                int last = m_intStoryOfFathers.Last();
                m_intStoryOfFathers.Reverse();
                m_intStoryOfFathers.Remove(last); //Просто, чтобы на первом же не остановилось
                foreach (var da in m_intStoryOfFathers.ToList())
                {
                    if (!IsFork(da))
                        m_intStoryOfFathers.Remove(da);

                    if (da == rebro.second)
                        break;
                }
                m_intStoryOfFathers.Reverse();

                m_mpBridgeRebra.Add(rebro); // ДОБАВЛЕНИЕ В СПИСОК - РЁБРА
            }
            else //Для NO_BRIDGE
            {
                //Чистим из итсории движения
                m_mpNoBridgeRebra.Add(m_mpStoryOfWay.Last()); // ДОБАВЛЕНИЕ В СПИСОК - НЕ РЁБРА
                m_mpStoryOfWay.Remove(m_mpStoryOfWay.Last()); //Просто, чтобы на первом же не остановилось
                m_mpStoryOfWay.Reverse();
                foreach (var da in m_mpStoryOfWay.ToList())
                {
                    if (da.second == rebro.second)
                        break;
                    m_mpStoryOfWay.Remove(da);

                    m_mpNoBridgeRebra.Add(da); // ДОБАВЛЕНИЕ В СПИСОК - НЕ РЁБРА
                }
                m_mpStoryOfWay.Reverse();

                //Чистим вершины
                int last = m_intStoryOfFathers.Last();
                m_intStoryOfFathers.Reverse();
                m_intStoryOfFathers.Remove(last); //Просто, чтобы на первом же не остановилось
                foreach (var da in m_intStoryOfFathers.ToList())
                {
                    if (!IsFork(da))
                        m_intStoryOfFathers.Remove(da);
                    m_intVershinsNoRebra.Add(da);

                    if (da == rebro.second)
                        break;
                }
                m_intStoryOfFathers.Reverse();

            }

        }
        private bool IsFork(int number)
        {
            int count = 0; //Колоичество рёбер от этого объекта
            foreach (var gr in m_mpGrafRebra)
            {
                if ((number == gr.first) || (number == gr.second))
                    count++;
            }
            if (count > 0)
                return true;

            return false;
        }

        private void IsertStoryDataInGrafRebra()
        {
            foreach(var da in m_mpStoryOfWay)
            {
                int counter = 0;
                foreach (var net in m_mpGrafRebra.ToList())
                {                    
                    if(da == net)
                        counter++;

                }
                if (counter == 0)
                    m_mpGrafRebra.Add(da);
            }
        }

        private List<MyPair> m_mpGrafRebra = new List<MyPair>();

        //Посредники в работе
        private Flags m_flgFlag;

        private List<MyPair> m_mpStoryOfWay = new List<MyPair>();
        private List<int> m_intStoryOfFathers = new List<int>();
        private List<int> m_intVershinsNoRebra = new List<int>();

        //Результат
        public List<MyPair> m_mpNoBridgeRebra = new List<MyPair>();
        public List<MyPair> m_mpBridgeRebra = new List<MyPair>();
        public MyPair m_mpThisRebro;




    }
}
