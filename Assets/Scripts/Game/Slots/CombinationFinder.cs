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
                return Slot.Data.GetHashCode();
            }
        }


        public (HashSet<Slot>, HashSet<Slot>) FindInColumns(List<Column> columns)
        {
            List<List<Cell>> columnCells = new();
            for (int i = 0; i < columns.Count; i++)
            {
                var slots = columns[i].GetSlotsInColumn();
                columnCells.Add(new());
                for (int j = 0; j < slots.Count; j++)
                    columnCells[columnCells.Count - 1].Add(new(slots[j]));

            }

            HashSet<Slot> mainMatches = FindInCell(columnCells);
            HashSet<Slot> otherMatches = new();

            Stack<List<Cell>> columnsSt = new(columnCells);
            for (int i = 1; i < columns.Count; i++)
            {
                otherMatches.UnionWith(FindInCell(columnsSt.ToList()));
                columnsSt.Pop();
            }

            otherMatches.ExceptWith(mainMatches);
            return (mainMatches, otherMatches);
        }

        private HashSet<Slot> FindInCell(List<List<Cell>> columns)
        {
            List<Cell> matches = new();
            var firstRowSlots = columns[0];
            List<Cell> matchesInRow = new();
            foreach (var slot in firstRowSlots)
            {
                if (slot.Visited)
                    continue;
                matchesInRow.Clear();
                bool found = false;
                for (int i = 1; i < columns.Count; i++)
                {
                    var slotsInRow = columns[i];
                    matchesInRow.AddRange(slotsInRow.Where(s => s.Equals(slot)).ToList());
                    if (matchesInRow.Count > 0)
                    {
                        matches.AddRange(matchesInRow);
                        matchesInRow.Clear();
                        found = true;
                    }
                    else
                        break;
                }
                if (found)
                    matches.AddRange(firstRowSlots.Where(s => s.Equals(slot)));
            }
            return new(matches.Select(m => m.Slot));
        }
    }
}