using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Technique
    {
        public delegate int LogicFunction(bool[,,] pencil);

        private bool active;
        private LogicFunction function;
        private int count;
        public string Name;

        public Technique(string name, LogicFunction logicFunction)
        {
            function = logicFunction;
            Name = name;
            active = false;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public bool GetActive()
        {
            return active;
        }

        public bool ApplyLogic(bool[,,] pencil)
        {
            int tempCount = 0;
            if(active)
            {
                tempCount = function(pencil);
            }
            count += tempCount;
            return tempCount > 0;
        }

        public int GetCount()
        {
            return count;
        }

        public void ClearCount()
        {
            count = 0;
        }
    }
}
