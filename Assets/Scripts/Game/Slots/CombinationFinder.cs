using System.Collections.Generic;
using System.Linq;

namespace Code.Game.Slots
{
    public class CombinationsFinder
    {
        private class Cell
        {
            public bool Visited { set; get; }
            public readonly Slot Slot;

            public Cell(Slot slot) => Slot = slot;

            public override bool Equals(object obj)
            {
                if (obj is Cell other)
                {
                    if (other.Slot.Data == Slot.Data)
                        return true;
                    else
                        return false;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Slot.Data.GetHashCode(); ;
            }
        }

        public HashSet<Slot> FindIn(List<Column> columns)
        {
            List<Cell> cells = new() { };
            LinkedList<List<Cell>> slotField = new();
            foreach (var column in columns)
            {
                foreach (var slot in column.GetSlotsInColumn())
                    cells.Add(new Cell(slot));

                slotField.AddLast(cells.ToList());
                cells.Clear();
            }

            List<Slot> found = new();
            List<Cell> first = slotField.First.Value;
            for (int i = 0; i < first.Count; i++)
            {
                var neighbours = SearchForNeighbours(slotField.First, i, false);
                if (neighbours.Count > 1)
                {
                    found.AddRange(neighbours.Select(n => n.Slot));
                    if (i - 1 >= 0)
                        if (first[i].Equals(first[i - 1]))
                            found.Add(first[i - 1].Slot);

                    if (i + 1 < first.Count)
                        if (first[i].Equals(first[i + 1]))
                            found.Add(first[i + 1].Slot);
                }
            }
            HashSet<Slot> matches = new(found);
            return matches;
        }

        private List<Cell> SearchForNeighbours(LinkedListNode<List<Cell>> node, int cellIndex, bool seachInVertOnFirst = true)
        {
            Cell cell = node.Value[cellIndex];
            List<Cell> result = new() { cell };
            if (cell.Visited)
                return result;
            cell.Visited = true;

            if (cellIndex - 1 >= 0 && node.Next != null)
            {
                if (node.Next.Value[cellIndex - 1].Equals(cell))
                    result.AddRange(SearchForNeighbours(node.Next, cellIndex - 1));
            }

            if (cellIndex + 1 < node.Value.Count && node.Next != null)
            {
                if (node.Next.Value[cellIndex + 1].Equals(cell))
                    result.AddRange(SearchForNeighbours(node.Next, cellIndex + 1));
            }

            if (seachInVertOnFirst)
            {
                if (cellIndex - 1 >= 0)
                    if (node.Value[cellIndex - 1].Equals(cell))
                        result.AddRange(SearchForNeighbours(node, cellIndex - 1));

                if (cellIndex + 1 < node.Value.Count)
                    if (node.Value[cellIndex + 1].Equals(cell))
                        result.AddRange(SearchForNeighbours(node, cellIndex + 1));
            }

            if (node.Next != null && node.Next.Value[cellIndex].Equals(cell))
                result.AddRange(SearchForNeighbours(node.Next, cellIndex));

            return result;
        }
    }
}