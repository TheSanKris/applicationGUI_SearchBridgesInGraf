using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FiindBrigeInGraf
{

    internal class Graf
    {
        public void Init(List<MyPair> data)
        {
            m_mpGrafPairs = data;
            m_mpCopyPairs.Clear();
            foreach (var obj in m_mpGrafPairs) //Пошёл в жопу эттот С# со своей передачей ссылок (или указателей), сука
            {
                m_mpCopyPairs.Add(obj);
            }
            m_NumberFathers.Add(1);
        }

        public Flags NextStep()
        {
            if ((m_mpGrafPairs.Count() == 0) && (m_mpBufferPairs.Count() != 0))
                Init(m_mpBufferPairs);

            if (m_mpGrafPairs.Count() == 0)
                return Flags.END;
            m_mpBridgeRebra.Clear();
            m_mpNoBridgeRebra.Clear();

            int search_number = m_NumberFathers.Last(); //Поиск ребра, где это число родитель
            MyPair pair = FindPair(search_number);
            m_mpThisRebro = pair;
            m_mpBufferPairs.Add(pair);
            if (IsFork(search_number))
                if ((m_NumbersForks.Count() == 0) || (m_NumbersForks.Last() != search_number))
                    m_NumbersForks.Add(search_number);
            RemovePair(m_RemovePair);
            Flags flag = GetStatus(pair);
            m_NumberFathers.Add(pair.second);
            switch (flag)
            {
                case Flags.NEXT:

                    //
                    return Flags.NEXT;
                    break;

                case Flags.NO_BRIDGE:
                    DeleteData(pair, flag);

                    //
                    return Flags.NO_BRIDGE;
                    break;

                case Flags.IS_BRIDGE:
                    DeleteData(pair, flag);

                    //
                    return Flags.IS_BRIDGE;
                    break;

                default:
                    break;
            }
            return Flags.END;


        }

        private Flags GetStatus(MyPair pair)
        {
            int search_number = pair.second;
            if (!IsFork(search_number))
                return Flags.IS_BRIDGE;
            if (IsRebroToFork(pair))
                return Flags.NO_BRIDGE;

            return Flags.NEXT;
        }

        private MyPair FindPair(int number)
        {
            MyPair result = m_mpGrafPairs.Find(parr => parr.first == number);
            if (result != null)
            {
                m_RemovePair = result;
                return result;
            }

            result = m_mpGrafPairs.Find(parr => parr.second == number);
            if (result != null)
            {
                m_RemovePair = result;
                MyPair pair = new MyPair();
                pair.Insert(result.second, result.first); //Переворот пары (тк идеалогия: (родитель, потомок)
                m_mpGrafPairs.Add(pair);
                m_mpGrafPairs.Remove(result);
                result = pair;

                return result;
            }

            return null;

        }

        private bool IsFork(int number)
        {
            int count = 0; //Колоичество рёбер от этого объекта
            foreach (var gr in m_mpCopyPairs)
            {
                if ((number == gr.first) || (number == gr.second))
                    count++;
            }
            if (count > 1)
                return true;

            return false;
        }

        private bool IsRebroToFork(MyPair pair)
        {
            foreach (var gr in m_NumbersForks)
            {
                if (pair.second == gr)
                    return true;
            }

            return false;
        }

        private void DeleteData(MyPair pair, Flags flag)
        {
            if (flag == Flags.NO_BRIDGE)
            {
                int number_fork = pair.second;
                int da = 0;
                while (true)
                {
                    if ((m_mpBufferPairs.Count() == 0) || (m_mpBufferPairs.Last().second == number_fork))
                        da++;
                    if (da > 1)
                        break;
                    m_mpGrafPairs.Remove(m_mpBufferPairs.Last());
                    m_mpNoBridgeRebra.Add(m_mpBufferPairs.Last());
                    m_mpBufferPairs.Remove(m_mpBufferPairs.Last());

                }
                while (true)
                {
                    if ((m_NumbersForks.Last() == number_fork) && (m_NumberFathers.Last() == number_fork))
                    {
                        if (m_NumbersForks.Last() == number_fork)
                            m_NumbersForks.Remove(m_NumbersForks.Last());
                        break;
                    }

                    if (m_NumbersForks.Last() != number_fork)
                        m_NumbersForks.Remove(m_NumbersForks.Last());

                }
                ClearFathers();

            }
            else
            {
                m_mpBridgeRebra.Add(m_mpBufferPairs.Last());
                m_mpGrafPairs.Remove(pair);
                m_mpBufferPairs.Remove(pair);
                int number_fork = pair.first;

                if (m_NumbersForks.Last() != number_fork)
                    m_NumbersForks.Remove(m_NumbersForks.Last());
                if (m_NumberFathers.Last() != number_fork)
                    m_NumberFathers.Remove(m_NumberFathers.Last());

            }
        }

        private void ClearFathers()
        {
            foreach(var obj in m_NumberFathers.ToList())
            {
                MyPair tmp = FindPair(obj);
                if (tmp == null)
                    m_NumberFathers.Remove(obj);
            }

        }

        private void RemovePair(MyPair pair)
        {
            m_mpGrafPairs.Remove(pair);
            MyPair tmp = new MyPair();
            tmp.Insert(pair.second, pair.first);
            m_mpGrafPairs.Remove(tmp);
        }

        public List<MyPair> m_mpNoBridgeRebra = new List<MyPair>();
        public List<MyPair> m_mpBridgeRebra = new List<MyPair>();
        public MyPair m_mpThisRebro = new MyPair();

        private List<MyPair> m_mpGrafPairs = new List<MyPair>();
        private List<MyPair> m_mpBufferPairs = new List<MyPair>();
        private MyPair m_RemovePair = new MyPair();
        private List<MyPair> m_mpCopyPairs = new List<MyPair>();

        private List<int> m_NumbersForks = new List<int>();
        private List<int> m_NumberFathers = new List<int>();
        
       
        

    }
}
