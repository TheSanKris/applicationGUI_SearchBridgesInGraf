using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FiindBrigeInGraf
{

    internal class Graf
    {
        public void Init(List<MyPair> data)
        {
            m_mpGrafRebra = data;
            m_intStoryOfFathers.Add(1); // С 1 всё начинается

            // Предобработка графа
            NormalizationRebra();
            m_intStoryOfFathers.Clear(); //На всякий
            m_intStoryOfFathers.Add(1);

        }

        private void NormalizationRebra()
        {
            List<MyPair> NormalGraf = new List<MyPair>();
            int da = m_mpGrafRebra.Count();
            for (int i = 0; i < da; i++)
            {
                //Просто делаем ИДЕАЛЬНУЮ копию графа (заранее проходим его, но без выдачи ВЕРДИКТА)
                int father = m_intStoryOfFathers.Last();
                MyPair tmp = new MyPair();
                tmp = FindRebro1(father);
                int nextFather = tmp.second;
                m_intStoryOfFathers.Add(nextFather);
                m_mpGrafRebra.Remove(tmp);
                NormalGraf.Add(tmp);

                CleanFathers1(); // Для управления потоком (правильный поиск рёбер)
            }
            m_mpGrafRebra = NormalGraf;
        }

        private MyPair FindRebro1(int father)
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
            // Обычный переворот
            int frst = 0;
            frst = rebro.first;

            int scnd = 0;
            scnd = rebro.second;
            rebro.second = frst;
            rebro.first = scnd;

        }

        private void CleanFathers1()
        {
            foreach(var da in m_intStoryOfFathers.ToList())
            {
                MyPair tmp = FindRebro1(da);
                if(tmp == null)
                {
                    m_intStoryOfFathers.Reverse();
                    m_intStoryOfFathers.Remove(da);
                    m_intStoryOfFathers.Reverse();
                }
            }
        }

        public Flags NextStep()
        {
            m_mpNoBridgeRebra.Clear();
            m_mpBridgeRebra.Clear();

            if (m_mpGrafRebra.Count() == 0)
                return Flags.END;

            int father = m_intStoryOfFathers.Last();
            MyPair m_mpRebro = FindRebro2(father);
            m_mpThisRebro = m_mpRebro;

            int nextFather = m_mpRebro.second;
            m_intStoryOfFathers.Add(nextFather);
            m_mpStoryOfWay.Add(m_mpRebro);

            m_flgFlag = GetFlag(m_mpRebro);

            switch (m_flgFlag)
            {
                case Flags.NEXT:

                    return Flags.NEXT;

                case Flags.NO_BRIDGE:
                    GetResult(m_mpRebro);

                    return Flags.NO_BRIDGE;

                case Flags.IS_BRIDGE:
                    GetResult(m_mpRebro);

                    return Flags.IS_BRIDGE;


            }

            return Flags.END;


        }  

        private MyPair FindRebro2(int father)
        {
            MyPair result = m_mpGrafRebra.Find(rebro => rebro.first == father);

            return result;
        }

        private Flags GetFlag(MyPair rebro)
        {
            int nextFather = m_intStoryOfFathers.Last();
            MyPair nextRebro = FindRebro2(nextFather);

            if (IsNoBridge(rebro))
                return Flags.NO_BRIDGE;
            if (nextRebro == null)
                return Flags.IS_BRIDGE;
            if (nextRebro != null)
                return Flags.NEXT;

            return Flags.END;

        }

        private bool IsNoBridge(MyPair rebro)
        {
            int father = rebro.second;
            int counter = 0;
            foreach (var da in m_intStoryOfFathers)
            {
                if (da == father)
                    counter++;
            }

            if (counter > 1)
                return true;

            return false;

        }

        private void GetResult(MyPair rebro)
        {
            if (m_flgFlag == Flags.IS_BRIDGE)
            {
                //Удаляем его из списков
                m_mpGrafRebra.Remove(rebro);
                m_mpStoryOfWay.Remove(rebro);

                //Чистим вершины (должна удалиться только этого ребра)
                CleanFathers2(rebro);

                // ДОБАВЛЕНИЕ В СПИСОК - МОСТЫ
                m_mpBridgeRebra.Add(rebro); 

            }
            else //Для NO_BRIDGE
            {
                //ДОБАВЛЕНИЕ В СПИСОК И ЧИСТИМ ГРАФ - НЕ МОСТЫ
                AddData(rebro);

                //Чистим вершины
                CleanFathers2(rebro);

            }

        }

        private void CleanFathers2(MyPair rebro)
        {
            int lastFather = m_intStoryOfFathers.Last();
            m_intStoryOfFathers.Reverse();
            m_intStoryOfFathers.Remove(lastFather);//Просто, чтобы на первом же не остановилось
            foreach (var da in m_intStoryOfFathers.ToList())
            {
                if (!IsFork(da))
                    m_intStoryOfFathers.Remove(da);

            }
            m_intStoryOfFathers.Reverse();
        }

        private void AddData(MyPair rebro)
        {
            m_mpGrafRebra.Remove(rebro);
            m_mpStoryOfWay.Remove(rebro);

            m_mpNoBridgeRebra.Add(rebro);

            m_mpGrafRebra.Reverse();
            m_mpStoryOfWay.Reverse();
            foreach (var da in m_mpStoryOfWay.ToList())
            {
                if (da.second == rebro.second)
                    break;
                m_mpGrafRebra.Remove(da);
                m_mpStoryOfWay.Remove(da);

                m_mpNoBridgeRebra.Add(da); // ДОБАВЛЕНИЕ В СПИСОК - НЕ РЁБРА
            }
            m_mpGrafRebra.Reverse();
            m_mpStoryOfWay.Reverse();

        }

        private bool IsFork(int number)
        {
            int count = 0; //Колоичество рёбер от этого объекта
            foreach (var gr in m_mpGrafRebra)
            {
                if (number == gr.first)
                    count++;
            }
            if (count > 0)
                return true;

            return false;
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
