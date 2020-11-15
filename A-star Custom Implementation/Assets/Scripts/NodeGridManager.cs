using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class NodeGridManager
    {
        private List<Node[,]> _availableNodeGrids = new List<Node[,]>();

        public Node[,] GetNodeGridFromPool()
        {
            // TODO: Will this need a lock?
            if (_availableNodeGrids.Count == 0)
            {
                CreateNodeGrid();
                
            }

            return _availableNodeGrids.Take(1).First();
        }

        private void CreateNodeGrid()
        {
            
        }
    }
}