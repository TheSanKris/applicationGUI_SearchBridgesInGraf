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
                    AddNumberNoRebra();
                    foreach (var obj in m_mpBufferPairs)
                    {
                        m_mpGrafPairs.Insert(0, obj);
                    }
                    ClearFathers();
                    m_mpBufferPairs.Clear();
                    //
                    return Flags.NO_BRIDGE;
                    break;

                case Flags.IS_BRIDGE:
                    DeleteData(pair, flag);
                    foreach (var obj in m_mpBufferPairs)
                    {
                        m_mpGrafPairs.Insert(0, obj);
                    }
                    ClearFathers();
                    m_mpBufferPairs.Clear();
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
            MyPair tmp = FindPair(search_number);
            if (IsRebroToFork(pair))
                return Flags.NO_BRIDGE;
            if ((tmp == null) )
                return Flags.IS_BRIDGE;
            

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
                {
                    return true;
                }
                    
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
                    m_mpNoBridgeRebra.Add(m_mpBufferPairs.Last());
                    m_mpBufferPairs.Remove(m_mpBufferPairs.Last());

                }

                int tmp = m_NumberFathers.Last();
                m_NumberFathers.Reverse();
                m_NumberFathers.Remove(tmp);
                m_NumberFathers.Reverse();

            }
            else
            {
                m_mpBridgeRebra.Add(m_mpBufferPairs.Last());
                m_mpGrafPairs.Remove(pair);
                m_mpBufferPairs.Remove(pair);
                int number_fork = pair.first;
                m_NumbersForks.Remove(m_NumbersForks.Last());
                m_NumberFathers.Remove(m_NumberFathers.Last());

            }
        }

        private void ClearFathers()
        {
            MyPair tmp = new MyPair();
            List<MyPair> copy = new List<MyPair>();
            foreach (var g in m_mpGrafPairs)
            {
                copy.Add(g);
            }
            foreach (var obj in m_NumberFathers.ToList())
            {
                

                tmp = FindPairSecond(obj, copy);
                if (tmp != null)
                    RemovePaireSecond(tmp, copy);
                

                if (tmp == null)
                    m_NumberFathers.Remove(obj);
            }

        }

        private MyPair FindPairSecond(int number, List<MyPair> list)
        {
            MyPair result = list.Find(parr => parr.first == number);
            if (result != null)
            {
                return result;
            }

            result = list.Find(parr => parr.second == number);
            if (result != null)
            {
                MyPair pair = new MyPair();
                pair.Insert(result.second, result.first); //Переворот пары (тк идеалогия: (родитель, потомок)
                list.Add(pair);
                list.Remove(result);
                result = pair;

                return result;
            }

            return null;

        }

        private void RemovePair(MyPair pair)
        {
            m_mpGrafPairs.Remove(pair);
            MyPair tmp = new MyPair();
            tmp.Insert(pair.second, pair.first);
            m_mpGrafPairs.Remove(tmp);
        }

        private void RemovePaireSecond(MyPair pair, List<MyPair> list)
        {
            list.Remove(pair);
            MyPair tmp = new MyPair();
            tmp.Insert(pair.second, pair.first);
            list.Remove(tmp);
        }

        public static List<T> removeDuplicates<T>(List<T> list)
        {
            return new HashSet<T>(list).ToList();
        }
        private void AddNumberNoRebra()
        {
            foreach(var obj in m_mpNoBridgeRebra)
            {
                m_NumberNoRebra.Add(obj.first);
                m_NumberNoRebra.Add(obj.second);
            }
            m_NumberNoRebra = removeDuplicates(m_NumberNoRebra);

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
        private List<int> m_NumberNoRebra = new List<int>();




    }
}
